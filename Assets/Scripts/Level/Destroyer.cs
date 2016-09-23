using UnityEngine;

public class Destroyer : MonoBehaviour
{
	public enum DestroyerType
	{
		DropsDestroyer,
		BackgroundDestroyer
	}

	public DestroyerType type;

	public void OnTriggerEnter2D(Collider2D col)
	{
		switch (type)
		{
			case DestroyerType.DropsDestroyer:
				if (col.tag == Tags.DROP)
				{
					col.gameObject.SetActive(false);
				}
				break;
			case DestroyerType.BackgroundDestroyer:
				if (col.tag == Tags.BACKGROUND)
				{
					col.gameObject.SetActive(false);
				}
				break;
		}
	}
}
