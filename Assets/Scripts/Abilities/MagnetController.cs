using System.Collections;
using UnityEngine;

public class MagnetController : MonoBehaviour
{
	public float duration;
	public float startSpeed = 5f;
	public float acceleration = 10f;
	public float maxYOffset = -2f;

	private Transform playerTransform;

	// Use this for initialization
	private void Awake()
	{
		playerTransform = FindObjectOfType<PlayerController>().transform;
	}

	private void OnEnable()
	{
		StartCoroutine(DisableAfterDelay());
	}

	private void Update()
	{
		transform.position = playerTransform.position;
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if ((col.transform.position.y - transform.position.y) > maxYOffset)
		{
			col.gameObject.GetComponent<FuelDrop>().canMove = false;
			StartCoroutine(MagnetizeDrop(col.transform));
		}
	}

	private IEnumerator MagnetizeDrop(Transform target)
	{
		// Initialize speed
		float speed = startSpeed;

		while (target.gameObject.activeInHierarchy)
		{
			// Aproach target fuel drop to player
			target.position = Vector3.MoveTowards
			(
				target.position,
				playerTransform.position,
				speed * Time.deltaTime
			);

			// Increase speed each frame
			speed += acceleration * Time.deltaTime;

			yield return null;
		}
	}

	private IEnumerator DisableAfterDelay()
	{
		yield return new WaitForSeconds(duration);

		gameObject.SetActive(false);
	}
}
