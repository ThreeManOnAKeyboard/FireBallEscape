﻿using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	enum Direction
	{
		Right,
		Left
	}

	Direction currentDirection = Direction.Right;

	public Tags.tags speedometerTag;
	public float defaultSpeed = 5f;
	public float xAxisMinSpeed = 1f;
	public float xAxisMaxSpeed = 10f;
	[Range(0, 1)]
	public float speedXOnYRatio = 0.5f;

	public float borderOffset;

	private float currentSpeed;

	// Borders position in 3D space
	private float distance;
	private float leftBorder;
	private float rightBorder;

	private Slider speedometer;

	// TEMP TRASH
	private float lastMousePosition;
	private float deltaMousePosition;

	// Use this for initialization
	void Start()
	{
		distance = (transform.position - Camera.main.transform.position).z;
		leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).x;
		rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance)).x;

		currentSpeed = defaultSpeed;

		speedometer = GameObject.FindGameObjectWithTag(speedometerTag.ToString()).GetComponent<Slider>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			deltaMousePosition = Input.mousePosition.x - lastMousePosition;
		}
		else
		{
			deltaMousePosition = 0;
		}

		lastMousePosition = Input.mousePosition.x;

		currentSpeed = Mathf.Clamp
		(
			currentSpeed + deltaMousePosition / Screen.width * xAxisMaxSpeed,
			xAxisMinSpeed,
			xAxisMaxSpeed
		);

		if (transform.position.x > (rightBorder - borderOffset))
		{
			currentDirection = Direction.Left;
		}
		else if (transform.position.x < (leftBorder + borderOffset))
		{
			currentDirection = Direction.Right;
		}

		transform.Translate(Vector3.up * currentSpeed * Time.deltaTime * (1 - speedXOnYRatio));

		transform.Translate
		(
			(currentDirection == Direction.Right ? Vector3.right : Vector3.left) * Time.deltaTime * currentSpeed * speedXOnYRatio
		);

		speedometer.value = (currentSpeed - xAxisMinSpeed) / (xAxisMaxSpeed - xAxisMinSpeed);
	}
}
