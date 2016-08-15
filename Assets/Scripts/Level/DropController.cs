using UnityEngine;

public class DropController : MonoBehaviour
{
	public Tags.tags playerTag;
	public GameObject dropExplosion;
	public bool isHealing;
	public float fallSpeed;

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == playerTag.ToString())
		{
			if (isHealing)
			{
				col.gameObject.GetComponent<PlayerController>().Heal();
			}
			else
			{
				col.gameObject.GetComponent<PlayerController>().Damage();
			}

			dropExplosion = Instantiate(dropExplosion);
			dropExplosion.transform.position = transform.position;
			Destroy(dropExplosion, 2f);
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update()
	{
		transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
	}
}
