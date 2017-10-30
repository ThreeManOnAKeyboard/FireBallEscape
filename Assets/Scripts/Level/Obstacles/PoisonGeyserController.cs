using UnityEngine;
using _3rdParty;

namespace Level.Obstacles
{
	public class PoisonGeyserController : GeyserController
	{
		[Header("Damage parameters")]
		public float drainSpeed;
		public float drainAmount;

		public void OnTriggerEnter2D(Collider2D col)
		{
			if (col.gameObject.CompareTag(Tags.Player))
			{
				playerController.StartHealthDrain(drainAmount, drainSpeed);
			}
		}
	}
}
