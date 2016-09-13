using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallController : MonoBehaviour
{
	public float collisionTime;

	public float yScreenRatioLimit;

	public GameObject waterDropPrefab;
	public GameObject fireBallExplosion;

	// The water drop to follow
	private GameObject targetedWaterDrop;
	private Transform particleSystemTransform;
	private Transform playerTransform;
	private Animator animator;
	private Vector3 collisionPoint;
	private Vector3 startPosition;
	private float time;

	private void Awake()
	{
		particleSystemTransform = gameObject.GetComponentInChildren<Transform>();
		GameObject player = GameObject.FindWithTag(Tags.tags.Player.ToString());

		if (player != null)
		{
			playerTransform = player.transform;
		}

		animator = GetComponentInChildren<Animator>();
	}

	void OnEnable()
	{
		targetedWaterDrop = null;
		collisionPoint = Vector3.zero;
		startPosition = Vector3.zero;
		time = 0f;
		StartCoroutine(StrikeWaterDrop());
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (targetedWaterDrop != null && col.gameObject == targetedWaterDrop.gameObject)
		{
			// Explosion effect
			GameObject collisionEffect = ObjectPool.Instance.GetPooledObject(fireBallExplosion);
			collisionEffect.transform.position = transform.position;
			collisionEffect.SetActive(true);
			animator.SetBool("Animate", false);

			// Deactivate current fireball
			gameObject.SetActive(false);
		}
	}

	private GameObject GetWaterDrop()
	{
		List<GameObject> waterDrops = ObjectPool.Instance.GetPooledList(waterDropPrefab);

		GameObject nearestWaterDrop = null;

		foreach (GameObject waterDrop in waterDrops)
		{
			// Check if water drop is active in hierarchy and it is enough far away from player
			if (waterDrop.activeInHierarchy && Camera.main.WorldToScreenPoint(waterDrop.transform.position).y / Screen.height > yScreenRatioLimit && !StrikeController.targetedDrops.Contains(waterDrop))
			{
				if (nearestWaterDrop == null)
				{
					nearestWaterDrop = waterDrop;
				}
				else if (waterDrop.transform.position.y < nearestWaterDrop.transform.position.y)
				{
					nearestWaterDrop = waterDrop;
				}
			}
		}

		if (nearestWaterDrop != null)
		{
			DropController dropController = nearestWaterDrop.gameObject.GetComponent<DropController>();

			collisionPoint = new Vector3
			(
				nearestWaterDrop.transform.position.x,
				nearestWaterDrop.transform.position.y - dropController.fallSpeed * collisionTime,
				nearestWaterDrop.transform.position.z
			);

			startPosition = transform.position;

			time = 0f;

			StrikeController.targetedDrops.Add(nearestWaterDrop.gameObject);
		}

		return nearestWaterDrop;
	}

	private IEnumerator StrikeWaterDrop()
	{
		animator.SetBool("Animate", true);

		while (time < collisionTime)
		{
			if (targetedWaterDrop == null || !targetedWaterDrop.gameObject.activeInHierarchy)
			{
				animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, -1, 0f);

				targetedWaterDrop = GetWaterDrop();

				if (playerTransform == null)
				{
					gameObject.SetActive(false);
				}
				else
				{
					// Reset particle system position
					//particleSystemTransform.position = Vector3.zero;
					transform.position = playerTransform.position;
				}
			}
			else
			{
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
}
