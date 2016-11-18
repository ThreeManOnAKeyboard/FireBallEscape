using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagnetController : AbilityController
{
	public float duration;
	public float startSpeed = 5f;
	public float acceleration = 10f;
	public float maxYOffset = -2f;

	private Transform playerTransform;
	private List<Drop> targets;
	private bool isActive;

	// Use this for initialization
	private void Awake()
	{
		playerTransform = FindObjectOfType<PlayerController>().transform;
	}

	private void OnEnable()
	{
		targets = new List<Drop>();
		isActive = true;
		StartCoroutine(DisableAfterDelay());
	}

	private void Update()
	{
		if (playerTransform != null)
		{
			transform.position = playerTransform.position;
		}
		else
		{
			targets.ForEach(target => target.canMove = true);
			gameObject.SetActive(false);
		}
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (isActive && (col.transform.position.y - transform.position.y) > maxYOffset)
		{
			StartCoroutine(MagnetizeDrop(col.gameObject.GetComponent<Drop>()));
		}
	}

	private IEnumerator MagnetizeDrop(Drop target)
	{
		// Initialize speed
		float speed = startSpeed;

		// Apply changes on target Drop
		target.canMove = false;
		targets.Add(target);

		while (target.gameObject.activeInHierarchy)
		{
			if (playerTransform != null)
			{
				// Aproach target fuel drop to player
				target.transform.position = Vector3.MoveTowards
				(
					target.transform.position,
					playerTransform.position,
					speed * Time.unscaledDeltaTime
				);
			}
			else
			{
				gameObject.SetActive(false);
			}

			// Increase speed each frame
			speed += acceleration * Time.unscaledDeltaTime;

			yield return null;
		}

		targets.Remove(target);
	}

	private IEnumerator DisableAfterDelay()
	{
		yield return new WaitForSeconds(duration);

		// Disable magnet to not magnetize any other fuel drops
		isActive = false;

		// Wait until all targets are magnetized
		while (targets.Count > 0)
		{
			yield return null;
		}

		gameObject.SetActive(false);
	}
}
