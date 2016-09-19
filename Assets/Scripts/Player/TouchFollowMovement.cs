using UnityEngine;
using UnityEngine.EventSystems;

public class TouchFollowMovement : MonoBehaviour
{
	private Vector2 speed;
	public Vector2 minSpeed;
	public Vector2 maxSpeed;
	public float noTouchSpeed;

	public float yOffset;

	[Range(0, 1)]
	public float yTouchLimitRatio;

	private Vector3 touchPosition;

	private bool[] UITouchExited = new bool[2];

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
			UITouchExited[0] = false;
		}

		// If the mouse button is released after pressing the UI element then let the character move
		if (Input.GetMouseButtonUp(0) && UITouchExited[0] == false)
		{
			UITouchExited[0] = true;
		}

		if (Input.GetMouseButton(0) && UITouchExited[0] == true)
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
		//#elif UNITY_ANDROID
		int iterationsCount = Mathf.Min(Input.touchCount, 2);
		if (iterationsCount > 0)
		{
			for (int touchIndex = 0; touchIndex < iterationsCount; touchIndex++)
			{
				if (Input.GetTouch(touchIndex)
					.phase == TouchPhase.Began && TouchManager.Instance.IsPointerOverUIObject(touchIndex))
				{
					UITouchExited[touchIndex] = false;
				}

				if (Input.GetTouch(0).phase == TouchPhase.Ended && UITouchExited[touchIndex] == false)
				{
					UITouchExited[touchIndex] = true;
				}
			}
		}

		if (iterationsCount > 0)
		{
			for (int touchIndex = 0; touchIndex < iterationsCount; touchIndex++)
			{
				if (UITouchExited[touchIndex])
				{
					touchPosition = Camera.main.ScreenToWorldPoint
					(
						new Vector3
						(
							Input.GetTouch(touchIndex).position.x,
							Mathf.Clamp(Input.GetTouch(touchIndex).position.y, 0f, Screen.height * yTouchLimitRatio),
							Mathf.Abs(Camera.main.transform.position.z - transform.position.z)
						)
					);

					touchPosition.y += yOffset;
					break;
				}
				else
				{
					touchPosition = transform.position;
					touchPosition.y += noTouchSpeed * Time.deltaTime;
				}
			}
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
