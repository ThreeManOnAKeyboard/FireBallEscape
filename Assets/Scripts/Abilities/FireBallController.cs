using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallController : MonoBehaviour
{
	public float collisionDelay;

	public float yScreenRatioLimit;

	public GameObject waterDropPrefab;
	public GameObject fireBallExplosion;

	// The water drop to follow
	private GameObject targetedWaterDrop;

	private Vector3 collisionPoint;
	private Vector3 startPosition;
	private float time;

	void OnEnable()
	{
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
				nearestWaterDrop.transform.position.y - dropController.fallSpeed * collisionDelay,
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
		while (time < collisionDelay)
		{
			if (targetedWaterDrop == null || !targetedWaterDrop.gameObject.activeInHierarchy)
			{
				targetedWaterDrop = GetWaterDrop();
			}
			else
			{
				transform.position = new Vector3
				(
					startPosition.x + (collisionPoint.x - startPosition.x) * (time / collisionDelay),
					startPosition.y + (collisionPoint.y - startPosition.y) * (time / collisionDelay),
					transform.position.z
				);

				time += Time.deltaTime;
			}

			yield return null;
		}
	}
}
