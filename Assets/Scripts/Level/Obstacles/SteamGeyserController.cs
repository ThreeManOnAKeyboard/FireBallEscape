using UnityEngine;
using _3rdParty;

namespace Level.Obstacles
{
	public class SteamGeyserController : GeyserController
	{
		[Header("Damage parameters")]
		public float damageMultiplier = 1f;

		public void OnTriggerEnter2D(Collider2D col)
		{
			if (col.gameObject.tag == Tags.Player)
			{
				playerController.Damage(damageMultiplier);
			}
		}
	}
}
