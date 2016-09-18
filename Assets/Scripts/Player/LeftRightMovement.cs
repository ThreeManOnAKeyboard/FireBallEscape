using UnityEngine;
using UnityEngine.EventSystems;

public class LeftRightMovement : MonoBehaviour
{
	// The number of divisions in which the screen will be devided
	public int bandsCount;

	// The speed for each axis
	public Vector2 minSpeed;
	public Vector2 maxSpeed;
	private Vector2 speed;

	// x position for each band
	public float[] bandsPositions;

	// Current band index
	private int bandIndex;

	// Use this for initialization
	void Start()
	{
		bandsPositions = new float[bandsCount];
		bandIndex = (bandsCount - 1) / 2;

		// The width of visible track in units
		float segmentLength = (CameraController.rightBorder * 2f - GameManager.Instance.bordersOffset * 2f) / (bandsCount - 1);

		bandsPositions[0] = CameraController.leftBorder + GameManager.Instance.bordersOffset;

		// Initialise bands x positions
		for (int i = 1; i < bandsCount; i++)
		{
			bandsPositions[i] = bandsPositions[i - 1] + segmentLength;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Time.timeScale == 0)
		{
			return;
		}

		if (PlayerController.isInvincible)
		{
			bandIndex = (bandsCount - 1) / 2;
			return;
		}

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		if (!EventSystem.current.IsPointerOverGameObject() && Input.GetButtonDown("Horizontal"))
#elif UNITY_ANDROID
		if (Input.touchCount > 0 && !TouchManager.Instance.IsPointerOverUIObject(0) && Input.GetTouch(0).phase == TouchPhase.Began)
#endif
		{
			Leap();
		}

		// Calculate speed based on health amount
		speed = new Vector2
		(
			Mathf.Clamp(PlayerController.health / PlayerController.maximumHealth * maxSpeed.x, minSpeed.x, maxSpeed.x),
				Mathf.Clamp(PlayerController.health / PlayerController.maximumHealth * maxSpeed.y, minSpeed.y, maxSpeed.y)
			);

		// Move sideways
		transform.position = Vector3.MoveTowards
		(
			transform.position,
			new Vector3
			(
				bandsPositions[bandIndex],
				transform.position.y,
				transform.position.z
			),
			speed.x * Time.deltaTime
		);

		// Move up
		transform.Translate(Vector2.up * speed.y * Time.deltaTime);
	}

	// Perform leap on next / previous band
	public void Leap()
	{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		if (Input.GetAxisRaw("Horizontal") > 0f)
#elif UNITY_ANDROID
		if (Input.GetTouch(0).position.x > Screen.width / 2)
#endif
		{
			if (bandIndex < bandsCount - 1)
			{
				bandIndex++;
			}
		}
		else
		{
			if (bandIndex > 0)
			{
				bandIndex--;
			}
		}
	}
}
