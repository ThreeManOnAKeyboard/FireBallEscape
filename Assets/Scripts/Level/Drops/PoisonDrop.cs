using UnityEngine;

public class PoisonDrop : Drop
{
	public float healthDrainSpeed;
	public float duration;

	public override void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == Tags.PLAYER)
		{
			playerController.StartHealthDrain(duration, healthDrainSpeed);
		}

		DoCollisionEffect(col.tag);
	}
}
