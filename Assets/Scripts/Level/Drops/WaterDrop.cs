using Abilities;
using UnityEngine;
using _3rdParty;

namespace Level.Drops
{
	public class WaterDrop : Drop
	{
		public override void OnTriggerEnter2D(Collider2D col)
		{
			switch (col.tag)
			{
				case Tags.Player:
					playerController.Damage(healthMultiplier);
					AbilitiesController.instance.UpdateCombination(Enumerations.DropType.Water);
					break;

				case Tags.Fireball:
					if (dropBooker != null)
					{
						if (dropBooker != col.gameObject)
						{
							return;
						}
					}
					else
					{
						return;
					}
					break;
			}

			dropBooker = null;

			// Remove this drop from targets list if Strike ability is activated
			if (StrikeController.targets != null)
			{
				StrikeController.targets.Remove(gameObject);
			}

			if (!col.CompareTag(Tags.Firespirits))
			{
				DoCollisionEffect(col.tag);
			}
		}
	}
}
