using UnityEngine;

public class StoneController : MonoBehaviour
{
	public GameObject collisionEffectPrefab;
	public float damageMultiplier = 1f;

	[Header("Shake Parameters")]
	public float duration;
	public float speed;
	public float magnitude;
	public float zoomDistance;

	[Header("Instantiation Parameters")]
	public float yOffset;
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

			// If this type of stone has an collision effect attached to then instiantiate it
			if (collisionEffectPrefab != null)
			{
				GameObject collisionEffect = ObjectPool.Instance.GetPooledObject(collisionEffectPrefab);
				collisionEffect.transform.position = transform.position;
				collisionEffect.transform.rotation = transform.rotation;
				collisionEffect.SetActive(true);
			}

			gameObject.SetActive(false);
		}
	}
}
