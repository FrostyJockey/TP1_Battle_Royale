using Playmode.Ennemy.BodyParts;
using Playmode.Movement;
using Playmode.Entity.Senses;
using UnityEngine;
using Playmode.Entity.Status;
using Playmode.Util.Values;
using Playmode.Pickups;

namespace Playmode.Ennemy.Strategies
{
    public class CarefulStrategy : IEnnemyStrategy
    {
        private readonly EnnemySensor ennemySensor;
        private readonly Mover mover;
        private readonly HandController handController;
        private readonly MedkitSensor medkitSensor;
        private readonly MedkitSensorCollision medkitSensorCollision;
        bool trackingEnnemy;
        GameObject target;
        float innerTimer;
        

        public CarefulStrategy(Mover mover, HandController handController, EnnemySensor ennemySensor, MedkitSensor medkitSensor, MedkitSensorCollision medkitSensorCollision)
        {
            this.mover = mover;
            this.handController = handController;
            this.ennemySensor = ennemySensor;
            this.medkitSensor = medkitSensor;
            trackingEnnemy = false;
            ennemySensor.OnEnnemySeen += OnEnnemySeen;
            ennemySensor.OnEnnemySightLost += OnEnnemySightLost;
            medkitSensor.OnMedkitSeen += OnMedkitSeen;
            medkitSensor.OnMedkitSightLost += OnMedkitSightLost;

            this.medkitSensorCollision = medkitSensorCollision;
            this.medkitSensorCollision.OnMedkitPickup += OnMedkitPickup;

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
                trackingEnnemy = true;
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
                    trackingEnnemy = false;
                }
            }
        }

        private void OnTargetDied()
        {
            
            ennemySensor.LooseSightOf(target.GetComponent<EnnemyController>());
        }

        private void OnMedkitSeen(MedkitController medkit)
        {
            target = medkit.transform.gameObject;
            trackingEnnemy = false;
        }

        private void OnMedkitSightLost(MedkitController medkit)
        {
            target = null;
        }


        private void OnMedkitPickup(MedkitController medkit)
        {
            medkitSensor.LooseSightOf(medkit);
        }

        private void MoveToTarget(GameObject targetedObject)
        {
            Vector3 spaceBetweenObjects = targetedObject.gameObject.transform.position - mover.gameObject.transform.position;

            float distance = spaceBetweenObjects.sqrMagnitude;
            float angle = Vector3.SignedAngle(mover.gameObject.transform.up, spaceBetweenObjects, Vector3.forward);
            if (!(trackingEnnemy && distance < 30))
            {
                mover.Move(Mover.Foward);
            }

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
