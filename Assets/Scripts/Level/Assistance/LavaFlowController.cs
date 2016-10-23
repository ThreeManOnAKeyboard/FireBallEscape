using UnityEngine;

public class LavaFlowController : MonoBehaviour
{
	public float borderOffset;
	public float leftBorderZAngle;
	public float rightBorderZAngle;

	private BoxCollider2D thisCollider2D;
	private PlayerController playerController;
	private ParticleSystem lavaParticleSystem;

	void Awake()
	{
		thisCollider2D = GetComponent<BoxCollider2D>();
		playerController = FindObjectOfType<PlayerController>();
		lavaParticleSystem = GetComponentInChildren<ParticleSystem>();
	}

	// Use this for initialization
	void OnEnable()
	{
		Vector3 position = transform.position;
		Quaternion rotation = transform.rotation;

		if (Random.Range(0, 2) == 0)
		{
			// Right side
			position.x = CameraController.rightBorder - borderOffset;
			//rotation = Quaternion.Euler(Random.Range(0f, 359f), 0f, Random.Range(-angleRange, angleRange));
			rotation = Quaternion.Euler(0f, 0f, rightBorderZAngle);
		}
		else
		{
			// Left side
			position.x = CameraController.leftBorder + borderOffset;
			//rotation = Quaternion.Euler(0f, 0f, 180f + Random.Range(-angleRange, angleRange));
			rotation = Quaternion.Euler(0f, 0f, leftBorderZAngle);
		}

		// Asign values
		transform.position = position;
		transform.rotation = rotation;

		// Enable the lava flow
		thisCollider2D.enabled = true;
	}

	void OnDisable()
	{
		thisCollider2D.enabled = false;
	}
}
