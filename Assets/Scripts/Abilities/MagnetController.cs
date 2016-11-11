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
	private List<Transform> targets;
	private bool isActive;

	// Use this for initialization
	private void Awake()
	{
		playerTransform = FindObjectOfType<PlayerController>().transform;
	}

	private void OnEnable()
	{
		targets = new List<Transform>();
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
			Destroy(gameObject);
		}
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (isActive && (col.transform.position.y - transform.position.y) > maxYOffset)
		{
			col.gameObject.GetComponent<FuelDrop>().canMove = false;
			targets.Add(col.transform);
			StartCoroutine(MagnetizeDrop(col.transform));
		}
	}

	private IEnumerator MagnetizeDrop(Transform target)
	{
		// Initialize speed
		float speed = startSpeed;

		while (target.gameObject.activeInHierarchy)
		{
			if (playerTransform != null)
			{
				// Aproach target fuel drop to player
				target.position = Vector3.MoveTowards
				(
					target.position,
					playerTransform.position,
					speed * Time.unscaledDeltaTime
				);
			}
			else
			{
				Destroy(gameObject);
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
