using UnityEngine;
using UnityEngine.EventSystems;

public class TouchFollowMovement : MonoBehaviour
{
	private Vector2 speed;
	public Vector2 minSpeed;
	public Vector2 maxSpeed;
	public float noTouchSpeed;
	//public float maxNoTouchSpeed;

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

		speed = new Vector2
		(
			Mathf.Clamp(PlayerController.health / PlayerController.maximumHealth * maxSpeed.x, minSpeed.x, maxSpeed.x),
			Mathf.Clamp(PlayerController.health / PlayerController.maximumHealth * maxSpeed.y, minSpeed.y, maxSpeed.y)
		);

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
			touchPosition.y += noTouchSpeed * Time.deltaTime;
		}
#elif UNITY_ANDROID
		if (Input.touchCount > 0)
		{
			if (Input.GetTouch(0).phase == TouchPhase.Began && TouchManager.Instance.IsPointerOverUIObject(0))
			{
				UITouchExited = false;
			}

			if (Input.GetTouch(0).phase == TouchPhase.Ended && UITouchExited == false)
			{
				UITouchExited = true;
			}
		}

		if (Input.touchCount > 0 && UITouchExited)
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
			touchPosition.y += noTouchSpeed * Time.deltaTime;
		}
#endif

		transform.position = new Vector3
		(
			Mathf.Clamp
			(
				transform.position.x + (touchPosition.x - transform.position.x) * speed.x * Time.deltaTime,
				CameraController.leftBorder + GameManager.Instance.bordersOffset,
				CameraController.rightBorder - GameManager.Instance.bordersOffset
			),
			transform.position.y + (touchPosition.y - transform.position.y) * speed.y * Time.deltaTime,
			transform.position.z
		);
	}
}
