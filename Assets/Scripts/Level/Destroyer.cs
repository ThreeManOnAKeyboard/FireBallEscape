using UnityEngine;

public class Destroyer : MonoBehaviour
{
	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == Tags.DROP)
		{
			col.gameObject.SetActive(false);
		}
	}
}
