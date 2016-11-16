using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallController : MonoBehaviour
{
	public static int fireballCount;

	public float collisionTime;

	public float minScreenHeightRatio;

	public GameObject strikeableObject;
	public GameObject fireBallExplosion;

	// The target to follow
	private GameObject target;
	private Drop targetDropController;
	private Transform playerTransform;
	private Animator animator;
	private Vector3 collisionPoint;
	private Vector3 startPosition;
	private float time;

	private void Awake()
	{
		playerTransform = GameObject.FindWithTag(Tags.PLAYER).transform;

		animator = GetComponentInChildren<Animator>();
	}

	private void OnEnable()
	{
		fireballCount++;

		if (playerTransform != null)
		{
			transform.position = playerTransform.position;
		}
		else
		{
			gameObject.SetActive(false);
		}

		target = null;
		collisionPoint = Vector3.zero;
		startPosition = Vector3.zero;
		time = 0f;
		StartCoroutine(StrikeTarget());
	}

	private void OnDisable()
	{
		fireballCount--;
		StopAllCoroutines();
	}

	private IEnumerator StrikeTarget()
	{
		while (true)
		{
			// Disable fireball because it is not compatible with super speed ability
			if (Time.timeScale != 1f || playerTransform == null)
			{
				gameObject.SetActive(false);
			}

			if (target == null || !target.gameObject.activeInHierarchy)
			{
				animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, -1, 0f);
				transform.position = playerTransform.position;
				target = GetTarget();
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
			targetDropController = nearestTarget.gameObject.GetComponent<Drop>();
			targetDropController.dropBooker = gameObject;
			collisionPoint = new Vector3
			(
				nearestTarget.transform.position.x,
				nearestTarget.transform.position.y - targetDropController.fallSpeed * collisionTime,
				nearestTarget.transform.position.z
			);

			startPosition = transform.position;

			StrikeController.targets.Add(nearestTarget.gameObject);

			time = 0f;

			transform.localScale = new Vector3
			(
				(Random.Range(0, 2) == 0 ? 1 : -1) * transform.localScale.x,
				transform.localScale.y,
				transform.localScale.z
			);
		}

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
			targetDropController.dropBooker = null;

			// Deactivate current fireball
			gameObject.SetActive(false);
		}
	}

	public void ResetTarget()
	{
		target = null;
	}
}
