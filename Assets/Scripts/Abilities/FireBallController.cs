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
	public GameObject trailEffect;
	public Transform trailParent;

	// The target to follow
	private GameObject target;
	private Transform playerTransform;
	private Animation thisAnimation;
	private Vector3 collisionPoint;
	private Vector3 startPosition;
	private TrailRenderer thisTrail;
	private float time;

	private void Awake()
	{
		playerTransform = GameObject.FindWithTag(Tags.PLAYER).transform;
		thisTrail = trailEffect.GetComponent<TrailRenderer>();

		thisAnimation = GetComponentInChildren<Animation>();
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
			return;
		}

		target = null;
		collisionPoint = Vector3.zero;
		startPosition = Vector3.zero;
		time = 0f;

		// Set up the new trail
		thisTrail = Instantiate(trailEffect).GetComponent<TrailRenderer>();
		thisTrail.transform.parent = trailParent;
		thisTrail.transform.localPosition = Vector3.zero;

		StartCoroutine(SetTarget());
	}

	private void OnDisable()
	{
		// Destroy old trail after it's fade to transparent
		if (thisTrail != null)
		{
			thisTrail.transform.parent = null;
			Destroy(thisTrail.gameObject, thisTrail.time);
		}

		fireballCount--;
		StopAllCoroutines();
	}

	private IEnumerator SetTarget()
	{
		List<GameObject> targetOptions = ObjectPool.Instance.GetPooledList(strikeableObject);

		while (target == null)
		{
			if (playerTransform != null)
			{
				transform.position = playerTransform.position;
			}
			else
			{
				gameObject.SetActive(false);
			}

			foreach (GameObject targetOption in targetOptions)
			{
				// Check if target is active in hierarchy and it is enough far away from player
				float targetScreenHeightRatio = Camera.main.WorldToScreenPoint(targetOption.transform.position).y / Screen.height;
				if (targetOption.activeInHierarchy && targetScreenHeightRatio > minScreenHeightRatio && !StrikeController.targets.Contains(targetOption))
				{
					if (target == null || targetOption.transform.position.y < target.transform.position.y)
					{
						target = targetOption;
					}
				}
			}

			yield return null;
		}

		Drop targetDropController = target.gameObject.GetComponent<Drop>();
		targetDropController.dropBooker = gameObject;
		collisionPoint = new Vector3
		(
			target.transform.position.x,
			target.transform.position.y - targetDropController.fallSpeed * collisionTime,
			target.transform.position.z
		);

		startPosition = transform.position;

		StrikeController.targets.Add(target.gameObject);

		time = 0f;

		transform.localScale = new Vector3
		(
			(Random.Range(0, 2) == 0 ? 1 : -1) * transform.localScale.x,
			transform.localScale.y,
			transform.localScale.z
		);

		StartCoroutine(StrikeTarget());
	}

	private IEnumerator StrikeTarget()
	{
		while (target != null && target.gameObject.activeInHierarchy && Time.timeScale == 1f)
		{
			thisAnimation.Play();

			// Translate fireball to target
			transform.position = new Vector3
			(
				startPosition.x + (collisionPoint.x - startPosition.x) * (time / collisionTime),
				startPosition.y + (collisionPoint.y - startPosition.y) * (time / collisionTime),
				transform.position.z
			);

			time += Time.deltaTime;

			yield return null;
		}

		gameObject.SetActive(false);
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject == target)
		{
			// Explosion effect
			GameObject collisionEffect = ObjectPool.Instance.GetPooledObject(fireBallExplosion);
			collisionEffect.transform.position = transform.position;
			collisionEffect.SetActive(true);
			thisAnimation.Stop();

			// Deactivate current fireball
			gameObject.SetActive(false);
		}
	}
}
