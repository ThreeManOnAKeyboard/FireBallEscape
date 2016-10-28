using UnityEngine;

public class Destroyer : MonoBehaviour
{
	public Enumerations.DestroyerType type;

	public void OnTriggerEnter2D(Collider2D col)
	{
		switch (type)
		{
			case Enumerations.DestroyerType.DropsDestroyer:
				if (col.tag == Tags.DROP)
				{
					col.gameObject.SetActive(false);
				}
				break;

			case Enumerations.DestroyerType.BackgroundDestroyer:
				if (col.tag == Tags.BACKGROUND)
				{
					col.gameObject.SetActive(false);
				}
				break;
		}
	}
}
