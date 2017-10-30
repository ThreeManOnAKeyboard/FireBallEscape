using Abilities;
using UnityEngine;
using _3rdParty;

namespace Level.Drops
{
	public class AbilityDrop : Drop
	{
		[SerializeField] private Ability thisAbility;

		public override void OnTriggerEnter2D(Collider2D col)
		{
			switch (col.tag)
			{
				case Tags.Player:
					AbilitiesController.instance.UpdateCurrentAbility(thisAbility);

					break;

				case Tags.ElementIgnore:
					DoCollisionEffect(col.tag);

					break;
			}
		}
	}
}
