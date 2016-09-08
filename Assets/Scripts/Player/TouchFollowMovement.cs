using UnityEngine;
using UnityEngine.EventSystems;

public class TouchFollowMovement : MonoBehaviour
{
	private float speed;
	public float minSpeed = 5f;
	public float maxSpeed = 10f;
	[Range(0.001f, 0.01f)]
	public float noTouchSpeedRatio = 0.2f;

	public float yOffset;

	[Range(0, 1)]
	public float yTouchLimitRatio;

	private Vector3 touchPosition;

	private bool UITouchExited = true;

	// Use this for initialization
	void Start()
	{
		touchPosition = transform.position;
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
			touchPosition = transform.position;
			return;
		}

		speed = Mathf.Clamp(PlayerController.health / PlayerController.maximumHealth * maxSpeed, minSpeed, maxSpeed);

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		// If player held the mouse button on UI element then don't let the character move
		if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())
		{
			UITouchExited = false;
		}

		// If the mouse button is released after pressing the UI element then let the character move
		if (Input.GetMouseButtonUp(0) && UITouchExited == false)
		{
			UITouchExited = true;
		}

		if (Input.GetMouseButton(0) && UITouchExited == true)
		{
			touchPosition = Camera.main.ScreenToWorldPoint
			(
				new Vector3
				(
					Input.mousePosition.x,
					Mathf.Clamp(Input.mousePosition.y, 0f, Screen.height * yTouchLimitRatio),
					Mathf.Abs(Camera.main.transform.position.z - transform.position.z)
				)
			);

			touchPosition.y += yOffset;
		}
		else
		{
			touchPosition = transform.position;
			touchPosition.y += noTouchSpeedRatio * speed;
		}
#elif UNITY_ANDROID
		if (Input.touchCount > 0)
		{
			touchPosition = Camera.main.ScreenToWorldPoint
			(
				new Vector3
				(
					Input.GetTouch(0).position.x,
					Mathf.Clamp(Input.GetTouch(0).position.y, 0f, Screen.height * yTouchLimitRatio),
					Mathf.Abs(Camera.main.transform.position.z - transform.position.z)
				)
			);

			touchPosition.y += yOffset;
		}
		else
		{
			touchPosition = transform.position;
			touchPosition.y += noTouchSpeedRatio * speed;
		}
#endif

		transform.position = Vector3.MoveTowards
		(
			transform.position,
			new Vector3
			(
				Mathf.Clamp
				(
					touchPosition.x,
					CameraController.leftBorder + GameManager.Instance.bordersOffset,
					CameraController.rightBorder - GameManager.Instance.bordersOffset
				),
				touchPosition.y,
				transform.position.z
			),
			Time.deltaTime * speed
		);
	}
}
