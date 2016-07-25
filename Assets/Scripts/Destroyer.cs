using UnityEngine;

public class Destroyer : MonoBehaviour
{
	public void OnTriggerEnter2D(Collider2D col)
	{
		if (transform.parent != null)
		{
			Destroy(col.transform.parent.gameObject);
		}
		else
		{
			Destroy(col.gameObject);
		}
	}
}
