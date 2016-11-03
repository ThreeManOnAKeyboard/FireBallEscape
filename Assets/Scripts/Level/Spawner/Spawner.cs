using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
	public List<DropSpawnProperties> spawnableObjects;

	public float minSpawnInterval;
	public float maxSpawnInterval;

	protected List<DropSpawnProperties> currentDrops;
	protected GameObject drop;
	protected bool isCooldownDone = true;

	// Use this for initialization
	protected void Start()
	{
		OrderByProbability();
		currentDrops = spawnableObjects;
	}

	protected void OnValidate()
	{
		// Order spawnable objects by their spawn probability
		OrderByProbability();
	}

	public void ChangeCurrentDrops(List<DropSpawnProperties> fuelRainDrops)
	{
		currentDrops = fuelRainDrops;
	}

	public void ResetDrops()
	{
		currentDrops = spawnableObjects;
	}

	protected void OrderByProbability()
	{
		// Order drops by probability
		int minIndex;
		float minValue;
		for (int i = 0; i < spawnableObjects.Count - 1; i++)
		{
			minValue = spawnableObjects[i].probability;
			minIndex = i;
			for (int j = i + 1; j < spawnableObjects.Count; j++)
			{
				if (spawnableObjects[j].probability < minValue)
				{
					minIndex = j;
					minValue = spawnableObjects[j].probability;
				}
			}

			if (minIndex != i)
			{
				DropSpawnProperties tempDrop = spawnableObjects[i];
				spawnableObjects[i] = spawnableObjects[minIndex];
				spawnableObjects[minIndex] = tempDrop;
			}
		}
	}

	protected GameObject GetRandomDrop()
	{
		float randomResult = Random.Range(0f, 1f);

		for (int i = 0; i < currentDrops.Count; i++)
		{
			if (randomResult <= currentDrops[i].probability)
			{
				if (currentDrops[i].cooldownDuration != 0f)
				{
					if (isCooldownDone)
					{
						StartCoroutine(StartCooldown(currentDrops[i].cooldownDuration));
						return currentDrops[i].drop;
					}
					else
					{
						// If a drop that requiries cooldown is not ready then try to get another drop type
						return GetRandomDrop();
					}
				}
				else
				{
					return currentDrops[i].drop;
				}
			}
		}

		return null;
	}

	private IEnumerator StartCooldown(float cooldownDuration)
	{
		isCooldownDone = false;
		yield return new WaitForSeconds(cooldownDuration);
		isCooldownDone = true;
	}
}
