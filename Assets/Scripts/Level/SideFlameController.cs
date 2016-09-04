﻿using System.Collections;
using UnityEngine;

public class SideFlameController : MonoBehaviour
{
	public float switchDelay;

	private PlayerController playerController;

	// Use this for initialization
	void Start()
	{
		playerController = FindObjectOfType<PlayerController>();
	}

	void OnEnable()
	{
		// Reset position to center
		Vector3 parentPosition = transform.parent.position;
		parentPosition.x = 0f;
		transform.parent.position = parentPosition;

		StartCoroutine(SwitchFlameSide());
	}

	IEnumerator SwitchFlameSide()
	{
		while (true)
		{
			// Flip flame side
			transform.parent.Rotate(transform.up, 180f);

			//Vector3 parentEulerAngles = gameObject.transform.parent.transform.eulerAngles;
			//parentEulerAngles.x *= -1;
			//gameObject.transform.parent.transform.eulerAngles = parentEulerAngles;

			yield return new WaitForSeconds(switchDelay);
		}
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == Tags.tags.Player.ToString())
		{
			playerController.Damage();
		}
	}
}