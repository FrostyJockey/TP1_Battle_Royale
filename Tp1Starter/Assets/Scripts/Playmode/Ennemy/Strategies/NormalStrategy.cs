
using UnityEngine;
using Playmode.Ennemy.BodyParts;
using Playmode.Entity.Senses;
using Playmode.Entity.Status;
using Playmode.Movement;

namespace Playmode.Ennemy.Strategies
{

	public class NormalStrategy : BaseStrategy
	{

		private readonly Mover mover;
		private readonly HandController handController;
		private readonly EnnemySensor ennemySensor;
		private EnnemyController currentEnnemyTarget;
		private EnnemyController ennemyController;
		private float elaspedTimeInOneDirection = 0f;
        [SerializeField] private float walkingTime = 5;
        [SerializeField] private float rotatingTime = 3.5f;
        [SerializeField] private float minimumDistanceBetweenEnnemies = 10;
		


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


		public override void Act()
		{
			if (currentEnnemyTarget != null)
			{
				float angleOffset = ennemyController.CalculateAngleWithTarget(currentEnnemyTarget.gameObject);

                mover.Rotate(angleOffset < 0 ? Mover.Clockwise : Mover.CounterClockwise);

				ShootTarget();
			}

			else
			{
                RotateFromBorders(mover);

				FindNewTargetDirection();
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
			if (elaspedTimeInOneDirection >= walkingTime)
			{
				if (elaspedTimeInOneDirection >= walkingTime + rotatingTime)
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
            if (IsFarFromTarget())
            {
                AdvanceForward();
            }

            handController.Use();
        }

        private bool IsFarFromTarget()
        {
            return ennemyController.CalculateDistanceWithTarget(currentEnnemyTarget.gameObject) > minimumDistanceBetweenEnnemies;
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




