using System.Collections;
using System.Collections.Generic;
using Playmode.Weapon;
using UnityEngine;

namespace Playmode.Entity.Senses
{
    public delegate void WeaponSensorCollisionEventHandler(WeaponController weapon);

    public class WeaponSensorCollision : MonoBehaviour
    {
        public event WeaponSensorEventHandler OnWeaponPickup;

        private void NotifyWeaponPickup(WeaponController weapon)
        {
            if (OnWeaponPickup != null) OnWeaponPickup(weapon);
        }

        public void Pickup(WeaponController weapon)
        {
            NotifyWeaponPickup(weapon);
        }
    }
}


