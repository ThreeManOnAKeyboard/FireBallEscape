using Level.Other;
using UnityEngine;
using UnityEngine.EventSystems;
using _3rdParty;

namespace Player
{
	public class ZigZagMovement : Movement
	{
		public float borderOffset;

		[HideInInspector]
		public Enumerations.Direction currentDirection = Enumerations.Direction.Right;

		// Update is called once per frame
		private void Update()
		{
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
			if (transform.position.x > (CameraController.instance.RightBorder - borderOffset))
			{
				currentDirection = Enumerations.Direction.Left;
			}
			else if (transform.position.x < (CameraController.instance.LeftBorder + borderOffset))
			{
				currentDirection = Enumerations.Direction.Right;
			}

			// Calculate player base speed
			speed = GetCurrentSpeed();

			// Move player up
			transform.Translate(Vector2.up * speed.y * Time.deltaTime);

			// Move player sideways
			if (CameraController.instance.RightBorder != 0f)
			{
				transform.Translate
				(
					(currentDirection == Enumerations.Direction.Right ? Vector2.right : Vector2.left) * Time.deltaTime * speed.x
				);
			}
		}
	}
}
