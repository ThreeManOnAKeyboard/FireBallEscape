using UnityEngine;

public class LavaFlowController : MonoBehaviour
{
	public float borderOffset;
	public float leftBorderZAngle;
	public float rightBorderZAngle;

	private PlayerController playerController;
	private Transform meshTransform;

	void Awake()
	{
		playerController = FindObjectOfType<PlayerController>();
		meshTransform = GetComponentInChildren<MeshFilter>().gameObject.transform;
	}

	// Use this for initialization
	void OnEnable()
	{
		Vector3 position = transform.position;
		Vector3 localScale = transform.localScale;

		// Set the Lava Flow gameobject on a random side of screen
		if (Random.Range(0, 2) == 0)
		{
			// Right side
			position.x = CameraController.Instance.rightBorder - borderOffset;
			transform.rotation = Quaternion.Euler(0f, 0f, rightBorderZAngle);
			localScale.x = 1f;
		}
		else
		{
			// Left side
			position.x = CameraController.Instance.leftBorder + borderOffset;
			transform.rotation = Quaternion.Euler(0f, 0f, leftBorderZAngle);
			localScale.x = -1f;
		}

		// Asign values
		transform.position = position;
		transform.localScale = localScale;

		// Change the rotation of x axis of mesh object so it looks different
		meshTransform.Rotate(Vector3.right * Random.Range(0f, 359f));
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == Tags.PLAYER && !playerController.isUnderSuperShield)
		{
			playerController.FullHeal();
		}
	}
}
