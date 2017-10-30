using Abilities;
using UnityEngine;
using _3rdParty;

namespace Level.Drops
{
	public class PoisonDrop : Drop
	{
		public float healthDrainSpeed;
		public float amount;

		public override void OnTriggerEnter2D(Collider2D col)
		{
			if (col.tag == Tags.Player)
			{
				playerController.StartHealthDrain(amount, healthDrainSpeed);
				AbilitiesController.instance.UpdateCombination(Enumerations.DropType.Poison);
			}

			if (col.tag != Tags.Firespirits)
			{
				DoCollisionEffect(col.tag);
			}
		}
	}
}
