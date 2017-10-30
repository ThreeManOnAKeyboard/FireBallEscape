using Abilities;
using Managers;
using UnityEngine;
using _3rdParty;

namespace Level.Drops
{
	public class FuelDrop : Drop
	{
		public override void OnTriggerEnter2D(Collider2D col)
		{
			if (col.tag == Tags.Player)
			{
				playerController.Heal(healthMultiplier);
				AbilitiesController.instance.UpdateCombination(Enumerations.DropType.Fuel);
				ScoreManager.instance.AddScore(scoreAmount);
			}

			if (col.tag != Tags.ElementIgnore)
			{
				DoCollisionEffect(col.tag);
			}
		}
	}
}
