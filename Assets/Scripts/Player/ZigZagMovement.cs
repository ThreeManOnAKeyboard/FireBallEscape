using UnityEngine;
using UnityEngine.EventSystems;

public class ZigZagMovement : MonoBehaviour
{
	[HideInInspector]
	public Enumerations.Direction currentDirection = Enumerations.Direction.Right;

	private float speed;
	public float minSpeed = 5f;
	public float maxSpeed = 10f;

	[Range(0, 1)]
	public float speedXOnYRatio = 0.5f;

	public float borderOffset;

	// Update is called once per frame
	void Update()
	{
		if (PlayerController.isInvincible)
		{
			return;
		}

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
#elif UNITY_ANDROID
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !TouchManager.Instance.IsPointerOverUIObject(0))
#endif
		{
			// Change the player movement direction on screen touch
			switch (currentDirection)
			{
				case Enumerations.Direction.Right:
					currentDirection = Enumerations.Direction.Left;
					break;

				case Enumerations.Direction.Left:
					currentDirection = Enumerations.Direction.Right;
					break;
			}
		}

		// Also change player direction when it reaches sreen borders
		if (transform.position.x > (CameraController.Instance.rightBorder - borderOffset))
		{
			currentDirection = Enumerations.Direction.Left;
		}
		else if (transform.position.x < (CameraController.Instance.leftBorder + borderOffset))
		{
			currentDirection = Enumerations.Direction.Right;
		}

		// Calculate player base speed
		speed = Mathf.Clamp(PlayerController.health / PlayerController.maximumHealth * maxSpeed, minSpeed, maxSpeed);

		// Move player up
		transform.Translate(Vector2.up * speed * Time.deltaTime * (1 - speedXOnYRatio));

		// Move player sideways
		if (CameraController.Instance.rightBorder != 0f)
		{
			transform.Translate
			(
				(currentDirection == Enumerations.Direction.Right ? Vector2.right : Vector2.left) * Time.deltaTime * speed * speedXOnYRatio
			);
		}
	}
}
