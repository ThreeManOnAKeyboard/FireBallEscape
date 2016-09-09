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
	private Transform waterDropTransform;

	private Vector3 collisionPoint;
	private Vector3 startPosition;
	private float time;

	void OnEnable()
	{
		StartCoroutine(StrikeWaterDrop());
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (waterDropTransform != null && col.gameObject == waterDropTransform.gameObject)
		{
			// Explosion effect
			GameObject collisionEffect = ObjectPool.Instance.GetPooledObject(fireBallExplosion);
			collisionEffect.transform.position = transform.position;
			collisionEffect.SetActive(true);

			// Deactivate current fireball
			gameObject.SetActive(false);
		}
	}

	private Transform GetWaterDrop()
	{
		List<GameObject> waterDrops = ObjectPool.Instance.GetPooledList(waterDropPrefab);

		Transform nearestWaterDrop = null;

		foreach (GameObject waterDrop in waterDrops)
		{
			// Check if water drop is active in hierarchy and it is enough far away from player
			if (waterDrop.activeInHierarchy && Camera.main.WorldToScreenPoint(waterDrop.transform.position).y / Screen.height > yScreenRatioLimit && !StrikeController.targetedDrops.Contains(waterDrop))
			{
				if (nearestWaterDrop == null)
				{
					nearestWaterDrop = waterDrop.transform;
				}
				else if (waterDrop.transform.position.y < nearestWaterDrop.position.y)
				{
					nearestWaterDrop = waterDrop.transform;
				}
			}
		}

		if (nearestWaterDrop != null)
		{
			DropController dropController = nearestWaterDrop.gameObject.GetComponent<DropController>();

			collisionPoint = new Vector3
			(
				nearestWaterDrop.position.x,
				nearestWaterDrop.position.y - dropController.fallSpeed * collisionDelay,
				nearestWaterDrop.position.z
			);

			startPosition = transform.position;

			StrikeController.targetedDrops.Add(nearestWaterDrop.gameObject);
		}

		time = 0f;

		return nearestWaterDrop;
	}

	private IEnumerator StrikeWaterDrop()
	{
		while (time < collisionDelay)
		{
			if (waterDropTransform == null || !waterDropTransform.gameObject.activeInHierarchy)
			{
				waterDropTransform = GetWaterDrop();
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
