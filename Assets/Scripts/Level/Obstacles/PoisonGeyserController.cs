using UnityEngine;

public class PoisonGeyserController : GeyserController
{
	[Header("Damage parameters")]
	public float drainSpeed;
	public float drainAmount;

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == Tags.PLAYER)
		{
			playerController.StartHealthDrain(drainAmount, drainSpeed);
		}
	}
}
