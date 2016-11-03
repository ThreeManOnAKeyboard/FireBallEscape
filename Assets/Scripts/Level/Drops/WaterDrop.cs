using UnityEngine;

public class WaterDrop : Drop
{
	public override void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == Tags.PLAYER)
		{
			playerController.Damage(healthMultiplier);
			AbilitiesController.Instance.UpdateCombination(Enumerations.DropType.Water);
			ScoreManager.Instance.ResetMultiplier();
		}

		// Remove this drop from targets list if Strike ability is activated
		if (StrikeController.targets != null)
		{
			StrikeController.targets.Remove(gameObject);
		}

		//if (col.tag != Tags.ELEMENT_IGNORE)
		//{
		DoCollisionEffect(col.tag);
		//}
		//else
		//{
		//	gameObject.SetActive(false);
		//}
	}
}
