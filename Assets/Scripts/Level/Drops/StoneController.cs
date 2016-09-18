using UnityEngine;

public class StoneController : MonoBehaviour
{
	public Vector2 impulseForce;

	private Rigidbody2D thisRigidBody;
	private bool onStoneRain;

	// Use this for initialization
	void Awake()
	{
		thisRigidBody = GetComponent<Rigidbody2D>();
	}

	void OnEnable()
	{
		if (StoneRainController.isActive)
		{
			onStoneRain = true;
		}
		else
		{
			onStoneRain = false;
		}

		// Center position
		transform.position = new Vector3
		(
			0f,
			transform.position.y,
			transform.position.z
		);

		thisRigidBody.AddForce(impulseForce * (Random.Range(0, 2) == 0 ? 1f : -1f), ForceMode2D.Impulse);
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == Tags.PLAYER)
		{
			if (onStoneRain)
			{
				col.gameObject.GetComponent<PlayerController>().Damage(false);
			}
			else
			{
				col.gameObject.GetComponent<PlayerController>().Kill();
			}
		}
	}
}
