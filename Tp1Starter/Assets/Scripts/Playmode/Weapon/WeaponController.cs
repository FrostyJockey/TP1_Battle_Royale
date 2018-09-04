using Playmode.Entity.Senses;
using System;
using UnityEngine;

namespace Playmode.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        [Header("Behaviour")] [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float fireDelayInSeconds = 1f;
        [SerializeField] private int weaponDamagePerBullet = 10;
        [SerializeField] private WeaponTypes typeOfWeapon = WeaponTypes.Shotgun;
        [SerializeField]private int ShotgunPalletsAmount = 5;

        private AudioSource shotFiredSound;

        private int spawnerNumber;
        private float lastTimeShotInSeconds;
        private PickupSpawnerController pickupSpawnerController;

        private bool CanShoot => Time.time - lastTimeShotInSeconds > fireDelayInSeconds;

        public WeaponTypes TypeOfWeapon
        {
            get { return typeOfWeapon; }
            set
            {
                if (typeOfWeapon != value)
                {
                    typeOfWeapon = value;
                }
            }
        }

        public PickupSpawnerController AssociatedSpawner
        {
            get { return pickupSpawnerController; }
            set
            {
                if (pickupSpawnerController != value)
                {
                    pickupSpawnerController = value;
                }
            }
        }

        private void Awake()
        {
            ValidateSerialisedFields();
            InitializeComponent();
        }

        private void ValidateSerialisedFields()
        {
            if (fireDelayInSeconds < 0)
                throw new ArgumentException("FireRate can't be lower than 0.");
            if (weaponDamagePerBullet < 0)
                throw new ArgumentException("Damage can't be lower than 0");
        }

        private void InitializeComponent()
        {
            lastTimeShotInSeconds = 0;

            shotFiredSound = transform.root.GetComponentInChildren<AudioSource>();
        }

        public void Shoot()
        {
            if (CanShoot)
            {
                PlayAudioCueShot();

                var angleSpread = 0;

                if (typeOfWeapon == WeaponTypes.Shotgun)
                {
                    for (var i = 0; i < ShotgunPalletsAmount; ++i)
                    {
                        angleSpread = UnityEngine.Random.Range(-15, 15);

                        var pallet = Instantiate(bulletPrefab, transform.position, transform.rotation);

                        pallet.GetComponentInChildren<HitStimulus>().BulletDamage = weaponDamagePerBullet;

                        pallet.transform.Rotate(0, 0, angleSpread);
                    }
                }
                else
                {
                    angleSpread = UnityEngine.Random.Range(-2, 2);

                    var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

                    bullet.GetComponentInChildren<HitStimulus>().BulletDamage = weaponDamagePerBullet;

                    bullet.transform.Rotate(0, 0, angleSpread);
                }

                lastTimeShotInSeconds = Time.time;
            }
        }

        public void AddWeaponStats(WeaponController weapon)
        {
            switch (weapon.TypeOfWeapon)
            {
                case (WeaponTypes.Shotgun):
                    weaponDamagePerBullet += 7;
                    typeOfWeapon = WeaponTypes.Shotgun;
                    break;

                case (WeaponTypes.Uzi):
                    fireDelayInSeconds -= 0.3f;
                    typeOfWeapon = WeaponTypes.Uzi;
                    break;
            }

            Hide(weapon);
        }

        private void Hide(WeaponController weapon)
        {

            gameObject.SetActive(false);
            AssociatedSpawner.IsSpawnerAvailable = true;
        }

        private void PlayAudioCueShot()
        {
            shotFiredSound.Play();
        }

        public void SetAssociatedSpawner(PickupSpawnerController spawner)
        {
            AssociatedSpawner = spawner;
        }
    }

}