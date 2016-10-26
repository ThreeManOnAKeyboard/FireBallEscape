using UnityEngine;

public class StoneController : MonoBehaviour
{
	public float damageMultiplier = 1f;

	[Header("Shake Parameters")]
	public float duration;
	public float speed;
	public float magnitude;
	public float zoomDistance;

	[Header("Instantiation Parameters")]
	public float yOffset;
	public Vector2 impulseForce;

	private Rigidbody2D[] stonePieces;
	private Rigidbody2D thisRigidBody;
	private PlayerController playerController;
	private bool onStoneRain;

	// Use this for initialization
	void Awake()
	{
		playerController = FindObjectOfType<PlayerController>();
		thisRigidBody = GetComponent<Rigidbody2D>();
		stonePieces = GetComponentsInChildren<Rigidbody2D>(true);
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
		if (duration != 0f)
		{
			CameraShake.Instance.StartShake(duration, speed, magnitude, zoomDistance);
		}
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == Tags.PLAYER)
		{
			if (onStoneRain)
			{
				if (!playerController.isUnderSuperShield)
				{
					playerController.Damage(damageMultiplier);
					playerController.OnStoneCollision();
				}
			}
			else
			{
				playerController.Kill();
			}

			// Detach stone pieces if they are present
			transform.DetachChildren();

			// Add force to each child piece
			foreach (Rigidbody2D piece in stonePieces)
			{
				piece.gameObject.SetActive(true);
				piece.AddForce(new Vector2(8f * (Random.Range(0, 2) == 0 ? 1f : -1f), Random.Range(4f, 9f)), ForceMode2D.Impulse);
			}

			gameObject.SetActive(false);
		}
	}
}
