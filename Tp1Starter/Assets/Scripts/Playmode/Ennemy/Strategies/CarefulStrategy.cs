using Playmode.Ennemy.BodyParts;
using Playmode.Movement;
using Playmode.Entity.Senses;
using UnityEngine;
using Playmode.Entity.Status;

namespace Playmode.Ennemy.Strategies
{
    public class CarefulStrategy : IEnnemyStrategy
    {
        private readonly EnnemySensor ennemySensor;
        private readonly Mover mover;
        private readonly HandController handController;
        bool trackingEnnemy;
        EnnemyController target;
        float innerTimer;
        
        

        public CarefulStrategy(Mover mover, HandController handController, EnnemySensor ennemySensor)
        {
            this.mover = mover;
            this.handController = handController;
            this.ennemySensor = ennemySensor;
            trackingEnnemy = false;
            ennemySensor.OnEnnemySeen += OnEnnemySeen;
            ennemySensor.OnEnnemySightLost += OnEnnemySightLost;
            innerTimer = 0;
        }

        public void Act()
        {
            if(trackingEnnemy)
            {
                Vector3 spaceBetweenEnnemies = target.gameObject.transform.position - mover.gameObject.transform.position;
                
                float distance = spaceBetweenEnnemies.sqrMagnitude;
                float angle = Vector3.Angle(mover.gameObject.transform.up, spaceBetweenEnnemies);
              // handController.AimTowards(target.gameObject);
                if (distance > 30)
                {
                   mover.Move(Mover.Foward);
                }
               else if (distance < 30)
                {
                   mover.Move(Mover.Backward);
                }
                
                if(angle > 2.5)
                {

                    mover.Rotate(Mover.Clockwise);
                }
                else if(angle < 2.5)
                {
                    
                     mover.Rotate(Mover.CounterClockwise);
                }
                handController.Use();
            }
            else
            {
                if(mover.gameObject.transform.position.y * mover.gameObject.transform.position.y >= 7.8 * 7.8) //pour gérer en même temps le haut et le bas
                {
                    mover.Rotate(Mover.Clockwise);
                }
                else if(mover.gameObject.transform.position.x * mover.gameObject.transform.position.x >= 19 * 19)
                {
                    mover.Rotate(Mover.Clockwise);
                }
                mover.Move(Mover.Foward);
            }
        }
        private void OnEnnemySeen(EnnemyController ennemy)
        {
            
            trackingEnnemy = true;
            if (target == null)
            {
                target = ennemy;
            }
            target.GetComponent<Health>().OnDeath += OnTargetDied;
        }
        private void OnEnnemySightLost(EnnemyController ennemy)
        {
           
            if (ennemy == target)
            {
               if(ennemySensor.EnnemiesInSight.GetEnumerator().MoveNext())
                {
                    target = ennemySensor.EnnemiesInSight.GetEnumerator().Current;
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
            
            ennemySensor.LooseSightOf(target);
        }
    }
}
