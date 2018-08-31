using Playmode.Ennemy.BodyParts;
using Playmode.Movement;
using Playmode.Entity.Senses;
using UnityEngine;
using Playmode.Entity.Status;
using Playmode.Util.Values;
using Playmode.Pickups;
using Playmode.Weapon;

using Playmode.Ennemy.BodyParts;
using Playmode.Movement;

namespace Playmode.Ennemy.Strategies
{
    public class CamperStrategy : IEnnemyStrategy
    {
        private readonly Mover mover;
        private readonly HandController handController;
        private readonly EnnemySensor ennemySensor;
        private readonly MedkitSensor medkitSensor;
        private readonly WeaponSensor weaponSensor;
        private readonly WeaponSensorCollision weaponSensorCollision;
        private readonly MedkitSensorCollision medkitSensorCollision;
        private GameObject target;
        private GameObject campingMedkit;
        [SerializeField] private int safetyHealthCap = 50;
        private EnnemyController ennemyController;

        public CamperStrategy(Mover mover, HandController handController, EnnemySensor ennemySensor, MedkitSensor medkitSensor, WeaponSensor weaponSensor, MedkitSensorCollision medkitSensorCollision, WeaponSensorCollision weaponSensorCollision)
        {
            target = null;
            campingMedkit = null;
            this.mover = mover;
            this.handController = handController;
            this.ennemySensor = ennemySensor;
            this.medkitSensor = medkitSensor;
            this.weaponSensor = weaponSensor;
            this.medkitSensorCollision = medkitSensorCollision;
            this.weaponSensorCollision = weaponSensorCollision;
            this.ennemySensor.OnEnnemySeen += OnEnnemySeen;
            this.ennemySensor.OnEnnemySightLost += OnEnnemySightLost;
            this.medkitSensor.OnMedkitSeen += OnMedkitSeen;
            this.medkitSensor.OnMedkitSightLost += OnMedkitSightLost;
            this.weaponSensor.OnWeaponSeen += OnWeaponSeen;
            weaponSensorCollision.OnWeaponPickup += OnWeaponPickup;
            medkitSensorCollision.OnMedkitPickup += OnMedkitPickup;
            ennemyController = mover.transform.root.GetComponentInChildren<EnnemyController>();
        }

        public void Act()
        {
            int currentHealth = mover.gameObject.GetComponent<Health>().HealthPoints;

            if (currentHealth <= 50 && campingMedkit != null)
            {
                AimTowardsTarget(campingMedkit);
                mover.Move(Mover.Foward);
            }
            else if (currentHealth >= 50 && campingMedkit != null)
            {
                if (ennemyController.CalculateDistanceWithTarget(campingMedkit) > 15)
                {
                    AimTowardsTarget(campingMedkit);
                    mover.Move(Mover.Foward);
                }
                else
                {
                    if(target == null)
                    {
                        mover.Rotate(Mover.Clockwise);
                    }
                    else
                    {
                        if (target.GetComponentInChildren<EnnemyController>().CalculateDistanceWithTarget(campingMedkit) <= 50)
                        {
                            AimTowardsTarget(campingMedkit);
                            mover.Move(Mover.Foward);
                        }
                        else
                        {
                            AimTowardsTarget(target);
                            handController.Use();
                        }
                    }
                }
            }
            else if (campingMedkit == null)
            {
                if(target != null && !target.transform.root.CompareTag(Tags.Ennemy))
                {
                    AimTowardsTarget(target);
                }

                RotateFromBorders();

                mover.Move(Mover.Foward);
            }

        }

        private void OnDestroy()
        {
            ennemySensor.OnEnnemySeen -= OnEnnemySeen;
            ennemySensor.OnEnnemySightLost -= OnEnnemySightLost;
            weaponSensor.OnWeaponSeen -= OnWeaponSeen;
            weaponSensor.OnWeaponSightLost -= OnWeaponSightLost;
            weaponSensorCollision.OnWeaponPickup -= OnWeaponPickup;
            target.GetComponent<Health>().OnDeath -= OnTargetDied;
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

            if (ennemy.gameObject == target || target == null)
            {
                target = FindNextTarget();
            }
        }

        private void OnTargetDied()
        {
            if (target != null)
            {
                ennemySensor.LooseSightOf(target.GetComponent<EnnemyController>());
            }
            else
            {
                target = FindNextTarget();
            }
        }

        private void OnMedkitSeen(MedkitController medkit)
        {
            campingMedkit = medkit.transform.gameObject;
            target = null;
        }

        private void OnMedkitSightLost(MedkitController medkit)
        {
              if (target == medkit.gameObject)
              {
                target = null;
              }
        }

        private void AimTowardsTarget(GameObject targetedObject)
        {
            float angle = ennemyController.CalculateAngleWithTarget(targetedObject);   
            if (angle < 0)
            {
                mover.Rotate(Mover.Clockwise);
            }
            else if (angle > 0)
            {

                mover.Rotate(Mover.CounterClockwise);
            }
        }

        private void OnWeaponSeen(WeaponController weapon)
        {
            if (campingMedkit == null)
            {
                target = weapon.gameObject;
            }
        }

        private void OnWeaponSightLost(WeaponController weapon)
        {
            if(target == weapon.gameObject)
            {
                target = null;
            }
        }

        private void OnWeaponPickup(WeaponController weapon)
        {
            weaponSensor.LooseSightOf(weapon);
            target = null;
        }

        private void OnMedkitPickup(MedkitController medkit)
        {
            medkitSensor.LooseSightOf(medkit);
            campingMedkit = null;
        }

        private GameObject FindNextTarget()
        {
            GameObject nextTarget = null;

            var ennemiesInSight = ennemySensor.EnnemiesInSight;

            foreach (EnnemyController ennemy in ennemiesInSight)
            {
                if (ennemy != null)
                {
                    nextTarget = ennemy.gameObject;
                }
            }

            return nextTarget;
        }

        private void RotateFromBorders()
        {
            if (mover.gameObject.transform.position.y * mover.gameObject.transform.position.y >= Screen.height / 2) //pour gérer en même temps le haut et le bas
            {
                mover.Rotate(Mover.Clockwise);
            }
            else if (mover.gameObject.transform.position.x * mover.gameObject.transform.position.x >= Screen.width / 2)
            {
                mover.Rotate(Mover.Clockwise);
            }
        }
    }
}
