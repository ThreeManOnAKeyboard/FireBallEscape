﻿using UnityEngine;

public class FuelDrop : Drop
{
	public override void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == Tags.PLAYER)
		{
			playerController.Heal();
			AbilitiesController.Instance.UpdateCombination(Enumerations.DropType.Fuel);
			ScoreManager.Instance.AddScore(scoreAmount);
		}

		DoCollisionEffect(col.tag);
	}
}
