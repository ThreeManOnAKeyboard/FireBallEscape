using UnityEngine;

public class PoisonDrop : Drop
{
	public float healthDrainSpeed;
	public float amount;

	public override void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == Tags.PLAYER)
		{
			playerController.StartHealthDrain(amount, healthDrainSpeed);
			AbilitiesController.Instance.UpdateCombination(Enumerations.DropType.Poison);
		}

		DoCollisionEffect(col.tag);
	}
}
