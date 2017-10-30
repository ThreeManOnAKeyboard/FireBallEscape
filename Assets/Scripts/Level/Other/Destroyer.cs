using UnityEngine;
using _3rdParty;

namespace Level.Other
{
	public class Destroyer : MonoBehaviour
	{
		public Enumerations.DestroyerType type;

		public void OnTriggerEnter2D(Collider2D col)
		{
			switch (type)
			{
				case Enumerations.DestroyerType.DropsDestroyer:
					if (col.tag == Tags.Drop)
					{
						col.gameObject.SetActive(false);
					}
					break;

				case Enumerations.DestroyerType.BackgroundDestroyer:
					if (col.tag == Tags.Background)
					{
						col.gameObject.SetActive(false);
					}
					break;
			}
		}
	}
}
