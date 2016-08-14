using UnityEngine;

public class ZigZagMovement : MonoBehaviour
{
	private enum Direction
	{
		Right,
		Left
	}
	private Direction currentDirection = Direction.Right;

	public float speed = 5f;
	[Range(0, 1)]
	public float speedXOnYRatio = 0.5f;

	public float borderOffset;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
		if (Input.GetMouseButtonDown(0))
#elif UNITY_ANDROID
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
#endif
		{
			if (currentDirection == Direction.Right)
			{
				currentDirection = Direction.Left;
			}
			else
			{
				currentDirection = Direction.Right;
			}
		}

		if (transform.position.x > (CameraController.rightBorder - borderOffset))
		{
			currentDirection = Direction.Left;
		}
		else if (transform.position.x < (CameraController.leftBorder + borderOffset))
		{
			currentDirection = Direction.Right;
		}

		transform.Translate(Vector2.up * speed * Time.deltaTime * (1 - speedXOnYRatio));

		if (CameraController.rightBorder != 0f)
		{
			transform.Translate
			(
				(currentDirection == Direction.Right ? Vector2.right : Vector2.left) * Time.deltaTime * speed * speedXOnYRatio * Mathf.Clamp(1 - Mathf.Abs(transform.position.x) / CameraController.rightBorder, 0.35f, 1f)
			);
		}
	}
}
