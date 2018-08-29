﻿using System;
using Playmode.Movement;
using Playmode.Weapon;
using UnityEngine;

namespace Playmode.Ennemy.BodyParts
{
    public class HandController : MonoBehaviour
    {
        private Mover mover;
        private WeaponController weapon;

        private void Awake()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            mover = GetComponent<AnchoredMover>();
        }
        
        public void Hold(GameObject gameObject)
        {
            if (gameObject != null)
            {
                gameObject.transform.parent = transform;
                gameObject.transform.localPosition = Vector3.zero;
                
                weapon = gameObject.GetComponentInChildren<WeaponController>();
            }
            else
            {
                weapon = null;
            }
        }

		/*
        public void AimTowards(GameObject target)
        {
            Vector3 distanceBetweenTargetAndMover = (target.transform.position - mover.transform.position).normalized;
            Vector3 currentDirection = mover.transform.forward;
            float angle = Vector3.Angle(distanceBetweenTargetAndMover, currentDirection);

			mover.transform.parent.Rotate(0,0,angle);
        }
		*/

        public void Use()
        {
			if (weapon != null) weapon.Shoot();
        }
    }
}