using UnityEngine;

public class StoneController : MonoBehaviour
{
	[Header("Shake Parameters")]
	public float duration;
	public float speed;
	public float magnitude;
	public float zoomDistance;

	[Header("Instantiation Parameters")]
	public float yOffset;
	public Vector2 impulseForce;

	public GameObject explosionEffect;

	private Transform[] stonePieces;
	private Rigidbody2D thisRigidBody;
	private PlayerController playerController;
	private bool onStoneRain;

	// Use this for initialization
	void Awake()
	{
		playerController = FindObjectOfType<PlayerController>();
		thisRigidBody = GetComponent<Rigidbody2D>();
		stonePieces = GetComponentsInChildren<Transform>(true);
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

	public void OnCollisionEnter2D(Collision2D col)
	{
		CameraShake.Instance.StartShake(duration, speed, magnitude, zoomDistance);
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == Tags.PLAYER)
		{
			if (onStoneRain)
			{
				if (!playerController.isUnderSuperShield)
				{
					playerController.Damage();
				}
			}
			else
			{
				playerController.Kill();
			}

			for (int i = 0; i < stonePieces.Length; i++)
			{
				stonePieces[i].gameObject.SetActive(true);
			}

			transform.DetachChildren();
			//explosionEffect.SetActive(true);
			gameObject.SetActive(false);
		}
	}
}
