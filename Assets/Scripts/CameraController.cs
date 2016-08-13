﻿using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour
{
	public static float leftBorder;
	public static float rightBorder;

	public Tags.tags playerTag;
	public Tags.tags backgroundTag;

	private Transform target;

	public float followSpeed;
	public float yPositionOffset;

	void Awake()
	{
		// Set Quality settings for android build
#if UNITY_ANDROID && !UNITY_EDITOR
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
#endif
	}

	// Use this for initialization
	void Start()
	{
		// Calculate visible track borders positions in world space coordinates
		target = GameObject.FindGameObjectWithTag(playerTag.ToString()).transform;
		float distance = (target.position - Camera.main.transform.position).z;
		leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).x;
		rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance)).x;
	}

	// Update is called once per frame
	void Update()
	{
		if (target != null)
		{
			transform.position = new Vector3
			(
				transform.position.x,
				Mathf.Lerp(transform.position.y, target.position.y + yPositionOffset, Time.deltaTime * followSpeed),
				transform.position.z
			);
		}
	}

	public void EnableApplyRootMotion()
	{
		GetComponent<Animator>().applyRootMotion = true;
	}

	public void SetDownSample(int value)
	{
		GetComponent<BlurOptimized>().downsample = value;
	}
}
