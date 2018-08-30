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
        private GameObject target;
        private GameObject campingMedkit;
        [SerializeField] private int safetyHealthCap = 50;
        private EnnemyController ennemyController;

        public CamperStrategy(Mover mover, HandController handController, EnnemySensor ennemySensor, MedkitSensor medkitSensor, WeaponSensor weaponSensor)
        {
            target = null;
            campingMedkit = null;
            this.mover = mover;
            this.handController = handController;
            this.ennemySensor = ennemySensor;
            this.medkitSensor = medkitSensor;
            this.weaponSensor = weaponSensor;
            this.ennemySensor.OnEnnemySeen += OnEnnemySeen;
            this.ennemySensor.OnEnnemySightLost += OnEnnemySightLost;
            this.medkitSensor.OnMedkitSeen += OnMedkitSeen;
            this.medkitSensor.OnMedkitSightLost += OnMedkitSightLost;
            this.weaponSensor.OnWeaponSeen += OnWeaponSeen;
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
                        AimTowardsTarget(target);
                        handController.Use();
                        if (target.GetComponentInChildren<EnnemyController>().CalculateDistanceWithTarget(campingMedkit) <= 50)
                        {
                            AimTowardsTarget(campingMedkit);
                            mover.Move(Mover.Foward);
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
                mover.Move(Mover.Foward);
                if (mover.gameObject.transform.position.y * mover.gameObject.transform.position.y >= 7.8 * 7.8) //pour gérer en même temps le haut et le bas
                {
                    mover.Rotate(Mover.Clockwise);
                }
                else if (mover.gameObject.transform.position.x * mover.gameObject.transform.position.x >= 19 * 19)
                {
                    mover.Rotate(Mover.Clockwise);
                }
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
                if (ennemySensor.EnnemiesInSight.GetEnumerator().MoveNext())
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

        private void OnMedkitSeen(MedkitController medkit)
        {
            campingMedkit = medkit.transform.gameObject;
            
        }

        private void OnMedkitSightLost(MedkitController medkit)
        {
              
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

        }
        
    }
}
