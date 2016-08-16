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

			GameObject explosion = Instantiate(dropExplosion);
			explosion.transform.position = transform.position;
			Destroy(explosion, 2f);
			gameObject.SetActive(false);
		}
	}

	// Update is called once per frame
	void Update()
	{
		transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
	}
}
