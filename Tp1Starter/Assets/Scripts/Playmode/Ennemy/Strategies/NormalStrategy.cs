
/*
 *  Made by Mika Gauthier
 */


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Playmode.Ennemy.Strategies;
using UnityEngine;
using Playmode.Ennemy.BodyParts;
using Playmode.Entity.Senses;
using Playmode.Movement;

namespace Playmode.Ennemy.Strategies
{
	public class NormalStrategy : IEnnemyStrategy
	{

		private readonly Mover mover;
		private readonly HandController handController;
		private readonly EnnemySensor ennemySensor;
		private EnnemyController currentEnnemyTarget;
		private float elaspedTimeInOneDirection = 0f;
		


		public NormalStrategy(Mover mover, HandController handController, EnnemySensor ennemySensor)
		{
			this.mover = mover;
			this.handController = handController;
			this.ennemySensor = ennemySensor;
			this.currentEnnemyTarget = null;

			this.ennemySensor.OnEnnemySeen += OnEnnemySeen;
			this.ennemySensor.OnEnnemySightLost += OnEnnemySightLost;
		}


		public void Act()
		{
			if (ennemySensor.EnnemiesInSight.Any())
				ShootTarget(ennemySensor.EnnemiesInSight.ElementAt(0));
			else
				FindNewTargetDirection();
		
				
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
			mover.Move(Mover.Foward);
		}

		private void ShootTarget(EnnemyController target)
		{
			
		}


		private void OnEnnemySeen(EnnemyController ennemy)
		{
			Debug.Log("I've seen an ennemy!! Ya so dead noob!!!");
		}

		private void OnEnnemySightLost(EnnemyController ennemy)
		{
			Debug.Log("I've lost sight of an ennemy...Yikes!!!");
		}
	}
}




