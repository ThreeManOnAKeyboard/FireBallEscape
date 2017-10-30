using Level.Other;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player
{
	public class LeftRightMovement : Movement
	{
		// The number of divisions in which the screen will be devided
		public int bandsCount;

		// x position for each band
		[HideInInspector]
		public float[] bandsPositions;

		// Current band index
		private int bandIndex;

		// Use this for initialization
		private void Start()
		{
			bandsPositions = new float[bandsCount];
			bandIndex = (bandsCount - 1) / 2;

			// The width of visible track in units
			float segmentLength = (CameraController.instance.RightBorder * 2f - GameManager.Instance.bordersOffset * 2f) / (bandsCount - 1);

			bandsPositions[0] = CameraController.instance.LeftBorder + GameManager.Instance.bordersOffset;

			// Initialise bands x positions
			for (int i = 1; i < bandsCount; i++)
			{
				bandsPositions[i] = bandsPositions[i - 1] + segmentLength;
			}
		}

		// Update is called once per frame
		private void Update()
		{
			if (Time.timeScale == 0)
			{
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

			// Get the current speed
			speed = GetCurrentSpeed();

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
}
