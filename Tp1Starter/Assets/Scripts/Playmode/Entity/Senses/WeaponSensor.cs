using System.Collections;
using System.Collections.Generic;
using Playmode.Weapon;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public delegate void WeaponSensorEventHandler(WeaponController weapon);

    public class WeaponSensor : MonoBehaviour
    {
        private ICollection<WeaponController> weaponsInSight;

        public event WeaponSensorEventHandler OnWeaponSeen;
        public event WeaponSensorEventHandler OnWeaponSightLost;

        public IEnumerable<WeaponController> WeaponsInSight => weaponsInSight;

        private void Awake()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            weaponsInSight = new HashSet<WeaponController>();
        }

        public void See(WeaponController weapon)
        {
            Debug.Log("I see the weapon!");

            weaponsInSight.Add(weapon);

            NotifyWeaponSeen(weapon);
        }

        public void LooseSightOf(WeaponController weapon)
        {
            Debug.Log("Where's the weapon??");

            weaponsInSight.Remove(weapon);

            NotifyWeaponSightLost(weapon);
        }

        private void NotifyWeaponSeen(WeaponController weapon)
        {
            if (OnWeaponSeen != null) OnWeaponSeen(weapon);
        }

        private void NotifyWeaponSightLost(WeaponController weapon)
        {
            if (OnWeaponSightLost != null) OnWeaponSightLost(weapon);
        }
    }
}
