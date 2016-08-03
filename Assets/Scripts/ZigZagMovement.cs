using UnityEngine;
using UnityEngine.UI;

public class ZigZagMovement : MonoBehaviour
{
	private enum Direction
	{
		Right,
		Left
	}

	private Direction currentDirection = Direction.Right;

	public Tags.tags speedometerTag;
	public float defaultSpeed = 5f;
	public float minSpeed = 1f;
	public float maxSpeed = 10f;
	[Range(0, 1)]
	public float speedXOnYRatio = 0.5f;

	public float borderOffset;

	private float currentSpeed;

	private Slider speedometer;

	private float lastTouchPosition;
	private float deltaTouchPosition;

	// Use this for initialization
	void Start()
	{
		currentSpeed = defaultSpeed;

		//speedometer = GameObject.FindGameObjectWithTag(speedometerTag.ToString()).GetComponent<Slider>();
	}

	// Update is called once per frame
	void Update()
	{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
		if (Input.GetMouseButton(0))
		{
			deltaTouchPosition = Input.mousePosition.y - lastTouchPosition;
		}
		else
		{
			deltaTouchPosition = 0;
		}

		lastTouchPosition = Input.mousePosition.y;
#elif UNITY_ANDROID
		if (Input.touchCount > 0)
		{
			Touch currentTouch = Input.GetTouch(0);

			if (currentTouch.phase == TouchPhase.Began)
			{
				lastTouchPosition = currentTouch.position.y;
			}
			else
			{
				deltaTouchPosition = currentTouch.position.y - lastTouchPosition;
				lastTouchPosition = currentTouch.position.y;
			}
		}
		else
		{
			deltaTouchPosition = 0;
		}
#endif

		currentSpeed = Mathf.Clamp
		(
			currentSpeed + deltaTouchPosition / Screen.width * maxSpeed,
			minSpeed,
			maxSpeed
		);

		if (transform.position.x > (CameraController.rightBorder - borderOffset))
		{
			currentDirection = Direction.Left;
		}
		else if (transform.position.x < (CameraController.leftBorder + borderOffset))
		{
			currentDirection = Direction.Right;
		}

		transform.Translate(Vector2.up * currentSpeed * Time.deltaTime * (1 - speedXOnYRatio));

		if (CameraController.rightBorder != 0f)
		{
			transform.Translate
			(
				(currentDirection == Direction.Right ? Vector2.right : Vector2.left) * Time.deltaTime * currentSpeed * speedXOnYRatio * Mathf.Clamp(1 - Mathf.Abs(transform.position.x) / CameraController.rightBorder, 0.35f, 1f)
			);
		}

		//speedometer.value = (currentSpeed - minSpeed) / (maxSpeed - minSpeed);
	}
}
