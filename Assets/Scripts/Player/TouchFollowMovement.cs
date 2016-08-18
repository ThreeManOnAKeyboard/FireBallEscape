﻿using UnityEngine;

public class TouchFollowMovement : MonoBehaviour
{
	private float speed;
	public float minSpeed = 5f;
	public float maxSpeed = 10f;

	public float yOffset;

	[Range(0, 1)]
	public float yTouchLimitRatio;

	private Vector3 touchPosition;

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

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		if (Input.GetMouseButton(0))
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
		}
#endif

		speed = Mathf.Clamp(PlayerController.health / PlayerController.maximumHealth * maxSpeed, minSpeed, maxSpeed);

		transform.position = Vector3.Lerp
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
				touchPosition.y + yOffset,
				transform.position.z
			),
			Time.deltaTime * speed
		);
	}
}
