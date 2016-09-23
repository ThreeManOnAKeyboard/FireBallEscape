using UnityEngine;

public class StoneController : MonoBehaviour
{
	public Vector2 impulseForce;

	private Rigidbody2D thisRigidBody;
	private PlayerController playerController;
	private bool onStoneRain;

	// Use this for initialization
	void Awake()
	{
		playerController = FindObjectOfType<PlayerController>();
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
				if (!playerController.isUnderSuperShield)
				{
					playerController.Damage(false);
				}
			}
			else
			{
				playerController.Kill();
			}
		}
	}
}
