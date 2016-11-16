using UnityEngine;

public class WaterDrop : Drop
{
	public override void OnTriggerEnter2D(Collider2D col)
	{
		switch (col.tag)
		{
			case Tags.PLAYER:
				playerController.Damage(healthMultiplier);
				AbilitiesController.Instance.UpdateCombination(Enumerations.DropType.Water);
				break;

			case Tags.FIREBALL:
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

		if (col.tag != Tags.FIRESPIRITS)
		{
			DoCollisionEffect(col.tag);
		}
	}
}
