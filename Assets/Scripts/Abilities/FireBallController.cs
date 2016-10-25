using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallController : MonoBehaviour
{
	public float collisionTime;

	public float minScreenHeightRatio;

	public GameObject strikeableObject;
	public GameObject fireBallExplosion;

	// The target to follow
	private GameObject target;
	private Transform playerTransform;
	private Animator animator;
	private Vector3 collisionPoint;
	private Vector3 startPosition;
	private float time;

	private void Awake()
	{
		GameObject player = GameObject.FindWithTag(Tags.PLAYER);

		if (player != null)
		{
			playerTransform = player.transform;
		}

		animator = GetComponentInChildren<Animator>();
	}

	private void OnEnable()
	{
		if (playerTransform != null)
		{
			transform.position = playerTransform.position;
		}

		target = null;
		collisionPoint = Vector3.zero;
		startPosition = Vector3.zero;
		time = 0f;
		StartCoroutine(StrikeTarget());
	}

	private IEnumerator StrikeTarget()
	{
		while (true)
		{
			if (target == null || !target.gameObject.activeInHierarchy)
			{
				target = GetTarget();

				if (target == null)
				{
					animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, -1, 0f);
				}

				if (playerTransform == null)
				{
					gameObject.SetActive(false);
				}
				else
				{
					transform.position = playerTransform.position;
				}
			}
			else
			{
				// Translate fireball to target
				transform.position = new Vector3
				(
					startPosition.x + (collisionPoint.x - startPosition.x) * (time / collisionTime),
					startPosition.y + (collisionPoint.y - startPosition.y) * (time / collisionTime),
					transform.position.z
				);

				time += Time.deltaTime;
			}

			yield return null;
		}
	}

	private GameObject GetTarget()
	{
		List<GameObject> targetOptions = ObjectPool.Instance.GetPooledList(strikeableObject);

		GameObject nearestTarget = null;

		foreach (GameObject targetOption in targetOptions)
		{
			// Check if target is active in hierarchy and it is enough far away from player
			float targetScreenHeightRatio = Camera.main.WorldToScreenPoint(targetOption.transform.position).y / Screen.height;
			if (targetOption.activeInHierarchy && targetScreenHeightRatio > minScreenHeightRatio && !StrikeController.targets.Contains(targetOption))
			{
				if (nearestTarget == null || targetOption.transform.position.y < nearestTarget.transform.position.y)
				{
					nearestTarget = targetOption;
				}
			}
		}

		if (nearestTarget != null)
		{
			Drop targetDrop = nearestTarget.gameObject.GetComponent<Drop>();

			collisionPoint = new Vector3
			(
				nearestTarget.transform.position.x,
				nearestTarget.transform.position.y - targetDrop.fallSpeed * collisionTime,
				nearestTarget.transform.position.z
			);

			startPosition = transform.position;

			StrikeController.targets.Add(nearestTarget.gameObject);

			time = 0f;
		}

		transform.localScale = new Vector3
		(
			(Random.Range(0, 2) == 0 ? 1 : -1) * transform.localScale.x,
			transform.localScale.y,
			transform.localScale.z
		);

		return nearestTarget;
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (target != null && col.gameObject == target.gameObject)
		{
			// Explosion effect
			GameObject collisionEffect = ObjectPool.Instance.GetPooledObject(fireBallExplosion);
			collisionEffect.transform.position = transform.position;
			collisionEffect.SetActive(true);
			animator.Stop();

			// Deactivate current fireball
			gameObject.SetActive(false);
		}
	}
}
