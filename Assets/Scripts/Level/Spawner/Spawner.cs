using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Spawner : MonoBehaviour
{
	public List<DropSpawnProperties> spawnableObjects;

	public float minSpawnInterval;
	public float maxSpawnInterval;

	protected List<DropSpawnProperties> currentSpawnables;
	protected GameObject objectToSpawn;
	protected float priorityTotal;
	protected bool isCooldownDone = true;

	// Use this for initialization
	protected void Start()
	{
		currentSpawnables = spawnableObjects;
		OrderByPriority();
	}

	public void ChangeSpawnables(List<DropSpawnProperties> newSpawnables)
	{
		currentSpawnables = newSpawnables;
		OrderByPriority();
	}

	public void ResetDrops()
	{
		currentSpawnables = spawnableObjects;
		OrderByPriority();
	}

	public void OrderByPriorityEditor()
	{
		currentSpawnables = spawnableObjects;
		OrderByPriority();
		spawnableObjects = currentSpawnables;
	}

	public void OrderByPriority()
	{
		// Order drops by priority
		int minIndex;
		float minValue;
		for (int i = 0; i < currentSpawnables.Count - 1; i++)
		{
			minValue = currentSpawnables[i].priority;
			minIndex = i;
			for (int j = i + 1; j < currentSpawnables.Count; j++)
			{
				if (currentSpawnables[j].priority < minValue)
				{
					minIndex = j;
					minValue = currentSpawnables[j].priority;
				}
			}

			if (minIndex != i)
			{
				DropSpawnProperties tempDrop = currentSpawnables[i];
				currentSpawnables[i] = currentSpawnables[minIndex];
				currentSpawnables[minIndex] = tempDrop;
			}
		}

		priorityTotal = currentSpawnables.Sum(spawnable => spawnable.priority);
	}

	protected GameObject GetRandomSpawnable()
	{
		float randomResult = Random.Range(0f, priorityTotal);
		float prioritySubTotal = 0f;

		for (int i = 0; i < currentSpawnables.Count; i++)
		{
			prioritySubTotal += currentSpawnables[i].priority;
			if (randomResult <= prioritySubTotal)
			{
				if (currentSpawnables[i].cooldown != 0f)
				{
					if (isCooldownDone)
					{
						StartCoroutine(StartCooldown(currentSpawnables[i].cooldown));
						return currentSpawnables[i].spawnable;
					}
					else
					{
						// Retry until it will return an available spawnable
						return GetRandomSpawnable();
					}
				}
				else
				{
					return currentSpawnables[i].spawnable;
				}
			}
		}

		return null;
	}

	private IEnumerator StartCooldown(float cooldown)
	{
		isCooldownDone = false;
		yield return new WaitForSecondsRealtime(cooldown);
		isCooldownDone = true;
	}
}
