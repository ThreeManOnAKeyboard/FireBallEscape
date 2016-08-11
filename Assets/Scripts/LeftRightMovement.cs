using UnityEngine;

public class LeftRightMovement : MonoBehaviour
{
	// The number of divisions in which the screen will be devided
	public int bandsCount;
	// The speed for each axis
	public Vector2 speed;
	// x position for each band
	private float[] bandsPositions;
	// Current band index
	private int bandIndex;

	// Use this for initialization
	void Start()
	{
		bandsPositions = new float[bandsCount];
		bandIndex = bandsCount / 2;

		// The width of visible track in units
		float trackWidth = CameraController.rightBorder * 2f - GameManager.Instance.bordersOffset;
		// Initialise bands x positions
		for (int i = 0; i < bandsCount; i++)
		{
			bandsPositions[i] = CameraController.leftBorder + GameManager.Instance.bordersOffset + (i == 0 ? 0 : ((trackWidth / (float)bandsCount) * i));
		}
	}

	// Update is called once per frame
	void Update()
	{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		if (Input.GetMouseButtonDown(0))
#elif UNITY_ANDROID
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
#endif
		{
			Leap();
		}

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
		if (Input.mousePosition.x > Screen.width / 2)
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
