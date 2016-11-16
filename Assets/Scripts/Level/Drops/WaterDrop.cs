using UnityEngine;

public class WaterDrop : Drop
{
	public override void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == Tags.PLAYER)
		{
			playerController.Damage(healthMultiplier);
			AbilitiesController.Instance.UpdateCombination(Enumerations.DropType.Water);
		}

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

	private void OnDisable()
	{
		if (dropBooker != null)
		{
			dropBooker.GetComponent<FireBallController>().ResetTarget();
			dropBooker = null;
		}
	}
}
