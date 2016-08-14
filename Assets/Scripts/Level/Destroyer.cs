using UnityEngine;

public class Destroyer : MonoBehaviour
{
	public Tags.tags whatToDestroyTag;

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == whatToDestroyTag.ToString())
		{
			Destroy(col.gameObject);
		}
	}
}
