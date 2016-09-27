using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperFireBallController : MonoBehaviour
{
	//public float speed;
	//public float collisionTime;

	//public float yScreenRatioLimit;

	//public StrikeableObject[] strikeableObjects;
	//public GameObject fireBallExplosion;

	//// The target to follow
	//private GameObject target;
	//private Transform playerTransform;
	//private Animator animator;
	//private Vector3 collisionPoint;
	//private Vector3 startPosition;
	//private float time;
	//private bool constantMovingTarget;

	//private void Awake()
	//{
	//	GameObject player = GameObject.FindWithTag(Tags.PLAYER);

	//	if (player != null)
	//	{
	//		playerTransform = player.transform;
	//	}

	//	animator = GetComponentInChildren<Animator>();
	//}

	//void OnEnable()
	//{
	//	target = null;
	//	collisionPoint = Vector3.zero;
	//	startPosition = Vector3.zero;
	//	time = 0f;
	//	StartCoroutine(StrikeTarget());
	//}

	//public void OnTriggerEnter2D(Collider2D col)
	//{
	//	if (target != null && col.gameObject == target.gameObject)
	//	{
	//		// Explosion effect
	//		GameObject collisionEffect = ObjectPool.Instance.GetPooledObject(fireBallExplosion);
	//		collisionEffect.transform.position = transform.position;
	//		collisionEffect.SetActive(true);
	//		animator.Stop();

	//		if (!constantMovingTarget)
	//		{
	//			col.gameObject.SetActive(false);
	//		}

	//		// Deactivate current fireball
	//		gameObject.SetActive(false);
	//	}
	//}

	//private GameObject GetTarget()
	//{
	//	List<GameObject> targetOptions = ObjectPool.Instance.GetPooledList(strikeableObjects);

	//	GameObject nearestTarget = null;

	//	foreach (GameObject posibleTarget in targetOptions)
	//	{
	//		// Check if target is active in hierarchy and it is enough far away from player
	//		if (posibleTarget.activeInHierarchy && Camera.main.WorldToScreenPoint(posibleTarget.transform.position).y / Screen.height > yScreenRatioLimit && !StrikeController.targets.Contains(posibleTarget))
	//		{
	//			if ((nearestTarget == null) || posibleTarget.transform.position.y < nearestTarget.transform.position.y)
	//			{
	//				nearestTarget = posibleTarget;
	//			}
	//		}
	//	}

	//	if (nearestTarget != null)
	//	{
	//		for (int i = 0; i < strikeableObjects.Length; i++)
	//		{
	//			if (nearestTarget.name == strikeableObjects[i].theObject.name)
	//			{
	//				if (strikeableObjects[i].isMovingConstant)
	//				{
	//					constantMovingTarget = true;
	//					DropController dropController = nearestTarget.gameObject.GetComponent<DropController>();

	//					collisionPoint = new Vector3
	//					(
	//						nearestTarget.transform.position.x,
	//						nearestTarget.transform.position.y - dropController.fallSpeed * collisionTime,
	//						nearestTarget.transform.position.z
	//					);

	//					startPosition = transform.position;

	//					time = 0f;
	//				}
	//				else
	//				{
	//					constantMovingTarget = false;
	//					// Initialize on non constant moving object
	//				}

	//				transform.localScale = new Vector3
	//				(
	//					(Random.Range(0, 2) == 0 ? 1 : -1) * transform.localScale.x,
	//					transform.localScale.y,
	//					transform.localScale.z
	//				);

	//				StrikeController.targets.Add(nearestTarget.gameObject);
	//			}
	//		}
	//	}

	//	return nearestTarget;
	//}

	//private IEnumerator StrikeTarget()
	//{
	//	while (true)
	//	{
	//		if (target == null || !target.gameObject.activeInHierarchy)
	//		{
	//			target = GetTarget();

	//			if (constantMovingTarget)
	//			{
	//				animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, -1, 0f);
	//			}
	//			else
	//			{
	//				animator.Stop();
	//			}

	//			if (playerTransform == null)
	//			{
	//				gameObject.SetActive(false);
	//			}
	//			else
	//			{
	//				transform.position = playerTransform.position;
	//			}
	//		}
	//		else
	//		{
	//			if (constantMovingTarget)
	//			{
	//				transform.position = new Vector3
	//				(
	//					startPosition.x + (collisionPoint.x - startPosition.x) * (time / collisionTime),
	//					startPosition.y + (collisionPoint.y - startPosition.y) * (time / collisionTime),
	//					transform.position.z
	//				);

	//				time += Time.deltaTime;
	//			}
	//			else
	//			{
	//				transform.position = Vector3.MoveTowards
	//				(
	//					transform.position,
	//					target.transform.position,
	//					speed * Time.deltaTime
	//				);
	//			}
	//		}

	//		yield return null;
	//	}
	//}
}
