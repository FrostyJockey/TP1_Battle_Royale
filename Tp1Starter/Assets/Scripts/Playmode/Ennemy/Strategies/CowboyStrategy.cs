﻿using Playmode.Ennemy.BodyParts;
using Playmode.Movement;
using Playmode.Entity.Senses;
using UnityEngine;
using Playmode.Entity.Status;
using Playmode.Util.Values;
using Playmode.Weapon;

namespace Playmode.Ennemy.Strategies
{
    public class CowboyStrategy : IEnnemyStrategy
    {
        private readonly EnnemySensor ennemySensor;
        private readonly Mover mover;
        private readonly HandController handController;
        private readonly WeaponSensor weaponSensor;
        private readonly WeaponSensorCollision weaponSensorCollision;
        GameObject target;
        float innerTimer;
        

        public CowboyStrategy
            (Mover mover, HandController handController, EnnemySensor ennemySensor, WeaponSensor weaponSensor, WeaponSensorCollision weaponSensorCollision)
        {
            this.mover = mover;
            this.handController = handController;
            this.ennemySensor = ennemySensor;
            this.weaponSensor = weaponSensor;
            this.weaponSensorCollision = weaponSensorCollision;
            ennemySensor.OnEnnemySeen += OnEnnemySeen;
            ennemySensor.OnEnnemySightLost += OnEnnemySightLost;
            weaponSensor.OnWeaponSeen += OnWeaponSeen;
            weaponSensor.OnWeaponSightLost += OnWeaponSightLost;
            weaponSensorCollision.OnWeaponPickup += OnWeaponPickup;
            innerTimer = 0;
        }

        public void Act()
        {
            if (target != null)
            {
                MoveToTarget(target.gameObject);

                if (target.transform.root.CompareTag(Tags.Ennemy))
                {
                    handController.Use();
                }
            }
            else
            {
                // hardcodded borders
                if (mover.gameObject.transform.position.y * mover.gameObject.transform.position.y >= 7.8 * 7.8) //pour gérer en même temps le haut et le bas
                {
                    mover.Rotate(Mover.Clockwise);
                }
                else if (mover.gameObject.transform.position.x * mover.gameObject.transform.position.x >= 19 * 19)
                {
                    mover.Rotate(Mover.Clockwise);
                }

                mover.Move(Mover.Foward);
            }
        }

        private void OnEnnemySeen(EnnemyController ennemy)
        {
            if (target == null)
            {
                target = ennemy.gameObject;
                target.GetComponent<Health>().OnDeath += OnTargetDied;
            }
        }

        private void OnEnnemySightLost(EnnemyController ennemy)
        {
           
            if (ennemy == target)
            {
               if(ennemySensor.EnnemiesInSight.GetEnumerator().MoveNext())
                {
                    target = ennemySensor.EnnemiesInSight.GetEnumerator().Current.gameObject;
                }
               else
                {
                    target = null;
                }
            }
        }

        private void OnTargetDied()
        {
            ennemySensor.LooseSightOf(target.GetComponent<EnnemyController>());
        }

        private void OnWeaponSeen(WeaponController weapon)
        {
            target = weapon.transform.gameObject;
        }

        private void OnWeaponSightLost(WeaponController weapon)
        {
            target = null;
        }

        private void OnWeaponPickup(WeaponController weapon)
        {
            weaponSensor.LooseSightOf(weapon);
        }

        private void MoveToTarget(GameObject targetedObject)
        {
            Vector3 spaceBetweenObjects = targetedObject.gameObject.transform.position - mover.gameObject.transform.position;

            float distance = spaceBetweenObjects.sqrMagnitude;
            float angle = Vector3.SignedAngle(mover.gameObject.transform.up, spaceBetweenObjects, Vector3.forward);

            mover.Move(Mover.Foward);

            if (angle < 0)
            {
                mover.Rotate(Mover.Clockwise);
            }
            else if (angle > 0)
            {
                mover.Rotate(Mover.CounterClockwise);
            }
        }
    }
}
