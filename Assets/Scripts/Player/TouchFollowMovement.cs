using UnityEngine;
using UnityEngine.EventSystems;

public class TouchFollowMovement : Movement
{
	// For test purposes in case if a mobile device is connected to Unity via Unity Remote 5
	public bool isUnityRemote;

	[Header("Aditional Speed Configuration")]
	public float xAxisLerpSpeed;
	[Range(0.01f, 2f)]
	public float noTouchSpeedRate;

	public float yOffset;

	[Range(0, 1)]
	public float yTouchLimitTreshold;

	private Vector3 touchPosition;

	private bool facingRight;

	private Vector2 touchFollowSpeed;

	private bool[] UITouchExited = { true, true };

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

		speed = GetCurrentSpeed();

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		if (isUnityRemote)
		{
			ProcessMobileInput();
		}
		else
		{
			ProcessComputerInput();
		}
#elif UNITY_ANDROID
		ProcessMobileInput();
#endif
		if (touchPosition != transform.position)
		{
			transform.position = new Vector3
			(
				Mathf.Clamp
				(
					Mathf.MoveTowards(transform.position.x, touchPosition.x, speed.x * Time.deltaTime),
					CameraController.Instance.leftBorder + GameManager.Instance.bordersOffset,
					CameraController.Instance.rightBorder - GameManager.Instance.bordersOffset
				),
				Mathf.Lerp(transform.position.y, touchPosition.y, speed.y * Time.deltaTime),
				transform.position.z
			);
		}
	}

	private void ProcessComputerInput()
	{
		// If player held the mouse button on UI element then don't let the character move
		if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())
		{
			UITouchExited[0] = false;
		}

		// If the mouse button is released after pressing the UI element then let the character move
		if (Input.GetMouseButtonUp(0) && !UITouchExited[0])
		{
			UITouchExited[0] = true;
		}

		if (Input.GetMouseButton(0) && UITouchExited[0])
		{
			touchPosition = Camera.main.ScreenToWorldPoint
			(
				new Vector3
				(
					Input.mousePosition.x,
					Mathf.Clamp(Input.mousePosition.y, 0f, Screen.height * yTouchLimitTreshold),
					Mathf.Abs(Camera.main.transform.position.z - transform.position.z)
				)
			);

			if ((transform.position.x < touchPosition.x && !facingRight) || (transform.position.x > touchPosition.x && facingRight))
			{
				touchFollowSpeed.x = 0f;
				facingRight = !facingRight;
			}

			touchFollowSpeed.x = Mathf.Lerp
			(
				touchFollowSpeed.x,
				speed.x,
				xAxisLerpSpeed * Time.deltaTime
			);

			touchPosition.y += yOffset;
		}
		else
		{
			MoveUp();
		}
	}

	private void ProcessMobileInput()
	{
		int iterationsCount = Mathf.Min(Input.touchCount, 2);
		if (iterationsCount > 0)
		{
			for (int touchIndex = 0; touchIndex < iterationsCount; touchIndex++)
			{
				if (Input.GetTouch(touchIndex).phase == TouchPhase.Began && TouchManager.Instance.IsPointerOverUIObject(touchIndex))
				{
					UITouchExited[touchIndex] = false;
				}

				if (Input.GetTouch(touchIndex).phase == TouchPhase.Ended && !UITouchExited[touchIndex])
				{
					UITouchExited[touchIndex] = true;
				}
			}

			for (int touchIndex = 0; touchIndex < iterationsCount; touchIndex++)
			{
				if (UITouchExited[touchIndex])
				{
					touchPosition = Camera.main.ScreenToWorldPoint
					(
						new Vector3
						(
							Input.GetTouch(touchIndex).position.x,
							Mathf.Clamp(Input.GetTouch(touchIndex).position.y, 0f, Screen.height * yTouchLimitTreshold),
							Mathf.Abs(Camera.main.transform.position.z - transform.position.z)
						)
					);

					if ((transform.position.x < touchPosition.x && !facingRight) || (transform.position.x > touchPosition.x && facingRight))
					{
						touchFollowSpeed.x = 0f;
						facingRight = !facingRight;
					}

					touchFollowSpeed.x = Mathf.Lerp
					(
						touchFollowSpeed.x,
						speed.x,
						xAxisLerpSpeed * Time.deltaTime
					);

					touchPosition.y += yOffset;
					break;
				}
				else if (touchIndex == (iterationsCount - 1))
				{
					MoveUp();
				}
			}
		}
		else
		{
			MoveUp();
		}
	}

	private void MoveUp()
	{
		touchFollowSpeed.x = 0f;
		touchPosition = transform.position;
		touchPosition.y += noTouchSpeedRate * speed.y * Time.deltaTime;
		transform.position = touchPosition;
	}
}
