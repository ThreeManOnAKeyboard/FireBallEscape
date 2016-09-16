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

		thisRigidBody.AddForce(impulseForce * (Random.Range(0, 2) == 0 ? 1f : -1f), ForceMode2D.Impulse);
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == Tags.tags.Player.ToString())
		{
			if (onStoneRain)
			{
				col.gameObject.GetComponent<PlayerController>().Damage();
			}
			else
			{
				col.gameObject.GetComponent<PlayerController>().Kill();
			}
		}
	}
}
