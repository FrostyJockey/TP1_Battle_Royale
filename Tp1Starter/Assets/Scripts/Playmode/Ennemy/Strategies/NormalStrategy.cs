
using UnityEngine;
using Playmode.Ennemy.BodyParts;
using Playmode.Entity.Senses;
using Playmode.Entity.Status;
using Playmode.Movement;

namespace Playmode.Ennemy.Strategies
{
	public class NormalStrategy : IEnnemyStrategy
	{

		private readonly Mover mover;
		private readonly HandController handController;
		private readonly EnnemySensor ennemySensor;
		private EnnemyController currentEnnemyTarget;
		private EnnemyController ennemyController;
		private float elaspedTimeInOneDirection = 0f;
		


		public NormalStrategy(Mover mover, HandController handController, EnnemySensor ennemySensor)
		{
			this.mover = mover;
			this.handController = handController;
			this.ennemySensor = ennemySensor;
			this.currentEnnemyTarget = null;

			this.ennemyController = mover.GetComponent<EnnemyController>();

			this.ennemySensor.OnEnnemySeen += OnEnnemySeen;
			this.ennemySensor.OnEnnemySightLost += OnEnnemySightLost;
		}


		public void Act()
		{
			if (currentEnnemyTarget != null)
			{
				float angleOffset = ennemyController.CalculateAngleWithTarget(currentEnnemyTarget.gameObject);

				if (angleOffset < 0)			
					mover.Rotate(Mover.Clockwise);				
				else if (angleOffset > 0)
					mover.Rotate(Mover.CounterClockwise);

				ShootTarget();
			}

			else
			{
				FindNewTargetDirection();
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

        private void OnDestroy()
        {
            ennemySensor.OnEnnemySeen -= OnEnnemySeen;
            ennemySensor.OnEnnemySightLost -= OnEnnemySightLost;
            currentEnnemyTarget.GetComponent<Health>().OnDeath -= OnTargetDied;
        }

        private void FindNewTargetDirection()
		{
			elaspedTimeInOneDirection += Time.deltaTime;
			if (elaspedTimeInOneDirection >= 7)
			{
				if (elaspedTimeInOneDirection >= 12)
				{
					elaspedTimeInOneDirection = 0;
				}
				mover.Rotate(Mover.Clockwise);
			}
			else
				AdvanceForward();
		}

		private void ShootTarget()
		{
			if (ennemyController.CalculateDistanceWithTarget(currentEnnemyTarget.gameObject) > ennemyController.MinimalDistanceBeforeCollision(currentEnnemyTarget.gameObject))
			{
				AdvanceForward();				
			}

			handController.Use();
		}

		private void AdvanceForward()
		{
			mover.Move(Mover.Foward);
		}

		private void OnEnnemySeen(EnnemyController ennemy)
		{
			if(currentEnnemyTarget == null)
				currentEnnemyTarget = ennemy;
			currentEnnemyTarget.GetComponent<Health>().OnDeath += OnTargetDied;
		}

		private void OnEnnemySightLost(EnnemyController ennemy)
		{
            if (ennemy.gameObject == currentEnnemyTarget)
            {
                currentEnnemyTarget = FindNextTargetEnnemy();
            }
		}

		private void OnTargetDied()
		{
            if (currentEnnemyTarget != null)
            {
                ennemySensor.LooseSightOf(currentEnnemyTarget);
            }
        }

        private EnnemyController FindNextTargetEnnemy()
        {
            EnnemyController nextTarget = null;

            var ennemiesInSight = ennemySensor.EnnemiesInSight;

            foreach (EnnemyController ennemy in ennemiesInSight)
            {
                if (ennemy != null)
                {
                    nextTarget = ennemy;
                }
            }

            return nextTarget;
        }
    }
}




