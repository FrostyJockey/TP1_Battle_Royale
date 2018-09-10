﻿using System.Collections;
using System.Collections.Generic;
using Playmode.Ennemy.Strategies;
using Playmode.Movement;
using UnityEngine;

//BEN_CORRECTION : Namespace.

public class BaseStrategy : IEnnemyStrategy {

	public virtual void Act() { }

	protected void RotateFromBorders(Mover mover)
	{
		//BEN_REVIEW : Lire le warning : « Possible loss of fraction at "Screen.height / 2". »
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