using UnityEngine;

public class SteamGeyserController : GeyserController
{
	[Header("Damage parameters")]
	public float damageMultiplier = 1f;

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == Tags.PLAYER)
		{
			playerController.Damage(damageMultiplier);
		}
	}
}
