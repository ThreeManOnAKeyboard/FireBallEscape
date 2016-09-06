using UnityEngine;

public class FireBallController : MonoBehaviour
{
	// x and y follow speed
	public Vector2 speed;
	public LayerMask waterDropLayer;

	// The water drop to follow
	private Transform waterDrop;
	private Transform playerTransform;

	// Use this for initialization
	void Awake()
	{
		playerTransform = GameObject.FindWithTag(Tags.tags.Player.ToString()).transform;
	}

	void OnEnable()
	{
		waterDrop = GetClosestWaterDrop();
		transform.position = playerTransform.position;
	}

	// Update is called once per frame
	void Update()
	{
		if (waterDrop.gameObject.activeInHierarchy)
		{
			transform.position = new Vector3
			(
				Mathf.Lerp(transform.position.x, waterDrop.position.x, Time.deltaTime * speed.x),
				Mathf.Lerp(transform.position.y, waterDrop.position.y, Time.deltaTime * speed.y),
				transform.position.z
			);
		}
		else
		{
			waterDrop = GetClosestWaterDrop();
		}
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject == waterDrop.gameObject)
		{
			// Explosion

			gameObject.SetActive(false);
		}
	}

	private Transform GetClosestWaterDrop()
	{
		return Physics2D.OverlapCircle(transform.position, Mathf.Infinity, waterDropLayer).transform;
	}
}
