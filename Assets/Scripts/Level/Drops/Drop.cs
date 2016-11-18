using UnityEngine;

public abstract class Drop : MonoBehaviour
{
	public GameObject defaultCollisionEffect;
	public GameObject geyserCollisionEffect;
	public float fallSpeed;
	public float scoreAmount;
	public float healthMultiplier = 1f;

	[HideInInspector]
	public bool canMove;
	[HideInInspector]
	public GameObject dropBooker;

	protected PlayerController playerController;

	// Use this for initialization
	protected void Start()
	{
		playerController = FindObjectOfType<PlayerController>();
	}

	private void OnEnable()
	{
		canMove = true;
	}

	// Update is called once per frame
	protected void Update()
	{
		if (canMove)
		{
			transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
		}
	}

	// Perform a post effect for respective drop
	public void DoCollisionEffect(string colTag)
	{
		// Collision Effect
		GameObject collisionParticleSystem = null;

		// Choose collision effect
		switch (colTag)
		{
			case Tags.DESTROYER:
				break;

			case Tags.GEYSER:
				collisionParticleSystem = ObjectPool.Instance.GetPooledObject(geyserCollisionEffect);
				break;

			default:
				collisionParticleSystem = ObjectPool.Instance.GetPooledObject(defaultCollisionEffect);
				break;
		}

		if (collisionParticleSystem != null)
		{
			collisionParticleSystem.transform.position = transform.position;
			collisionParticleSystem.SetActive(true);
		}

		gameObject.SetActive(false);
	}

	// Override this for each type of drop
	public abstract void OnTriggerEnter2D(Collider2D col);
}
