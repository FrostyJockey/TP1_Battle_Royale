/*
 * 
 * Made by Benjamin Lemelin
 * Tampered by Charles-David Thibodeau
 * 
 */

using System;
using Playmode.Bullet;
using Playmode.Ennemy.BodyParts;
using Playmode.Ennemy.Strategies;
using Playmode.Entity.Destruction;
using Playmode.Entity.Senses;
using Playmode.Entity.Status;
using Playmode.Movement;
using Playmode.Pickups;
using Playmode.Weapon;
using UnityEngine;

namespace Playmode.Ennemy
{
    public class EnnemyController : MonoBehaviour
    {
        [Header("Body Parts")] [SerializeField] private GameObject body;
        [SerializeField] private GameObject hand;
        [SerializeField] private GameObject sight;
        [SerializeField] private GameObject typeSign;
        [Header("Type Images")] [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite carefulSprite;
        [SerializeField] private Sprite cowboySprite;
        [SerializeField] private Sprite camperSprite;
        [Header("Behaviour")] [SerializeField] private GameObject startingWeaponPrefab;
		


        private Health health;
        private Mover mover;
        private Destroyer destroyer;
        private EnnemySensor ennemySensor;
        private HitSensor hitSensor;
        private HandController handController;
        private WeaponSensor weaponSensor;
        private MedkitSensor medkitSensor;
        private MedkitSensorCollision medkitSensorCollision;
        private WeaponSensorCollision weaponSensorCollision;

        private IEnnemyStrategy strategy;

        private void Awake()
        {
            ValidateSerialisedFields();
            InitializeComponent();
            CreateStartingWeapon();
        }

        private void ValidateSerialisedFields()
        {
            if (body == null)
                throw new ArgumentException("Body parts must be provided. Body is missing.");
            if (hand == null)
                throw new ArgumentException("Body parts must be provided. Hand is missing.");
            if (sight == null)
                throw new ArgumentException("Body parts must be provided. Sight is missing.");
            if (typeSign == null)
                throw new ArgumentException("Body parts must be provided. TypeSign is missing.");
            if (normalSprite == null)
                throw new ArgumentException("Type sprites must be provided. Normal is missing.");
            if (carefulSprite == null)
                throw new ArgumentException("Type sprites must be provided. Careful is missing.");
            if (cowboySprite == null)
                throw new ArgumentException("Type sprites must be provided. Cowboy is missing.");
            if (camperSprite == null)
                throw new ArgumentException("Type sprites must be provided. Camper is missing.");
            if (startingWeaponPrefab == null)
                throw new ArgumentException("StartingWeapon prefab must be provided.");
        }

        private void InitializeComponent()
        {
            health = GetComponent<Health>();
            mover = GetComponent<RootMover>();
            destroyer = GetComponent<RootDestroyer>();

            var rootTransform = transform.root;

            ennemySensor = rootTransform.GetComponentInChildren<EnnemySensor>();
            handController = hand.GetComponent<HandController>();

            hitSensor = rootTransform.GetComponentInChildren<HitSensor>();

            medkitSensor = rootTransform.GetComponentInChildren<MedkitSensor>();
            medkitSensorCollision = rootTransform.GetComponentInChildren<MedkitSensorCollision>();

            weaponSensor = rootTransform.GetComponentInChildren<WeaponSensor>();
            weaponSensorCollision = rootTransform.GetComponentInChildren<WeaponSensorCollision>();

        }

        private void CreateStartingWeapon()
        {
            handController.Hold(Instantiate(
                startingWeaponPrefab,
                Vector3.zero,
                Quaternion.identity
            ));
        }

        private void OnEnable()
        {
            hitSensor.OnHit += OnHit;
            health.OnDeath += OnDeath;
            medkitSensorCollision.OnMedkitPickup += OnMedkitPickup;
            weaponSensorCollision.OnWeaponPickup += OnWeaponPickup;
        }

        private void Update()
        {
            strategy.Act();
        }

        private void OnDisable()
        {
            hitSensor.OnHit -= OnHit;
            health.OnDeath -= OnDeath;
            medkitSensorCollision.OnMedkitPickup -= OnMedkitPickup;
            weaponSensorCollision.OnWeaponPickup -= OnWeaponPickup;
        }

        public void Configure(EnnemyStrategy strategy, Color color)
        {
            body.GetComponent<SpriteRenderer>().color = color;
            sight.GetComponent<SpriteRenderer>().color = color;
            
            switch (strategy)
            {
                case EnnemyStrategy.Careful:
                    typeSign.GetComponent<SpriteRenderer>().sprite = carefulSprite;
                    //BEN_CORRECTION : Je sais ce que cela veut dire, mais si vous lisez ce commentaire pour la première fois,
                    //                 comment pouvez vous comprendre ce que cela veut dire ?
                    // Think of better way for that, possibly
                    this.strategy = new CarefulStrategy(mover, 
                        handController, 
                        ennemySensor, 
                        medkitSensor, 
                        medkitSensorCollision, 
                        weaponSensor, 
                        weaponSensorCollision
                        );
                    break;

                case EnnemyStrategy.Cowboy:
                    typeSign.GetComponent<SpriteRenderer>().sprite = cowboySprite;
                    // Think of better way for that, possibly
                    this.strategy = new CowboyStrategy(mover, handController, ennemySensor, weaponSensor, weaponSensorCollision);
                    break;

                case EnnemyStrategy.Camper:
                    typeSign.GetComponent<SpriteRenderer>().sprite = camperSprite;
                    this.strategy = new CamperStrategy(mover, handController, ennemySensor, medkitSensor, weaponSensor, medkitSensorCollision, weaponSensorCollision);
                    break;

                default:
	                this.strategy = new NormalStrategy(mover, handController, ennemySensor);
					typeSign.GetComponent<SpriteRenderer>().sprite = normalSprite;
                    break;
            }
        }

        private void OnHit(int hitPoints, BulletController bullet)
        {
            health.Hit(hitPoints);
            //BEN_CORRECTION : La bullet devrait se détruire tout seule lorsqu'elle rencontre un ennemi.
            //
            //                 L'ennemi devrait pas avoir à gérer cela.
            bullet.DestroyBullet();
        }

        private void OnDeath()
        {
            destroyer.Destroy();
        }

        private void OnMedkitPickup(MedkitController medkit)
        {
            health.Heal(medkit.HealthValue);

            var currentMedkit = transform.root.GetComponentInChildren<MedkitController>(); //BEN_CORRECION : Ligne Inutilisé.
            //BEN_CORRECTION : Intruision dans les responsabilités du "Medkit". Le "Medkit" devrait activer lui même son
            //                 "Spawner". Ce n'est pas la responsabilité de l'ennemi.
            medkit.ActivateAssociatedSpawner(medkit); 
        }

        private void OnWeaponPickup(WeaponController weapon)
        {
            var currentWeapon = transform.root.GetComponentInChildren<WeaponController>();
            currentWeapon.AddWeaponStats(weapon);
        }

        //BEN_REVIEW : GetDistanceToTarget ? C'est pas un peu long "Calculate" ?
        //             Aussi, aurait pu être statique.
        public float CalculateDistanceWithTarget(GameObject targetedObject)
        {
            Vector3 spaceBetweenObjects = targetedObject.gameObject.transform.position - mover.gameObject.transform.position;
            float distance = spaceBetweenObjects.sqrMagnitude;
			Debug.Log(distance);
            return distance;
        }

        //BEN_REVIEW : Idem.
        public float CalculateAngleWithTarget(GameObject targetedObject)
        {
            Vector3 spaceBetweenObjects = targetedObject.gameObject.transform.position - mover.gameObject.transform.position;
            float angle = Vector3.SignedAngle(mover.gameObject.transform.up, spaceBetweenObjects, Vector3.forward);
            return angle;
        }
	}
}