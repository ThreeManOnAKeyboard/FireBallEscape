using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class SpawnableState
{
	public List<DropSpawnProperties> spawnables;
	public float nextDistance;
}

public class Spawner : MonoBehaviour
{
	public List<SpawnableState> spawnableStates;

	public float minSpawnInterval;
	public float maxSpawnInterval;
	public int currentState;
	public GameObject newSpawnable;

	protected List<DropSpawnProperties> currentSpawnables;
	protected GameObject objectToSpawn;
	protected float priorityTotal;
	public static bool canChangeSpawnables;
	protected bool isCooldownDone = true;
	protected Transform playerTransform;

	// Use this for initialization
	protected void Start()
	{
		canChangeSpawnables = true;
		playerTransform = GameObject.FindWithTag(Tags.PLAYER).transform;
		currentSpawnables = spawnableStates[currentState].spawnables;
		OrderByPriority();
		StartCoroutine(GoThroughSpawnableStates());
	}

#if UNITY_EDITOR
	public void AddNewSpawnable(int startFrom)
	{
		if (newSpawnable == null)
		{
			Debug.LogError("Select the prefab to add in New Spawnable field");
			return;
		}

		for (int i = startFrom; i < spawnableStates.Count; i++)
		{
			if (!spawnableStates[i].spawnables.Exists(s => s.spawnable == newSpawnable))
			{
				spawnableStates[i].spawnables.Add(new DropSpawnProperties() { spawnable = newSpawnable });
			}
		}

		newSpawnable = null;
	}

	public void OnValidate()
	{
		for (int i = 0; i < spawnableStates.Count; i++)
		{
			if (spawnableStates[i].spawnables.Count == 0)
			{
				for (int j = 0; j < i; j++)
				{
					for (int k = 0; k < spawnableStates[i].spawnables.Count; k++)
					{
						newSpawnable = spawnableStates[i].spawnables[k].spawnable;
						AddNewSpawnable(i);
					}
				}
			}
		}
	}

	public void OrderByPriorityEditor()
	{
		for (int i = 0; i < spawnableStates.Count; i++)
		{
			currentSpawnables = spawnableStates[i].spawnables;
			OrderByPriority();
			spawnableStates[i].spawnables = currentSpawnables;
		}
	}
#endif

	public void ChangeSpawnables(List<DropSpawnProperties> newSpawnables)
	{
		currentSpawnables = newSpawnables;
		OrderByPriority();
	}

	public void ResetSpawnables()
	{
		currentSpawnables = spawnableStates[currentState].spawnables;
		OrderByPriority();
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
		yield return new WaitForSeconds(cooldown);
		isCooldownDone = true;
	}

	private IEnumerator GoThroughSpawnableStates()
	{
		float targetDistance = 0;

		while (currentState < spawnableStates.Count - 1)
		{
			targetDistance += spawnableStates[currentState + 1].nextDistance;
			while (playerTransform.position.y < targetDistance)
			{
				yield return null;
			}

			currentState++;

			// In case spawner have to wait for some external spawn factors to end
			if (canChangeSpawnables)
			{
				ChangeSpawnables(spawnableStates[currentState].spawnables);
			}
		}
	}
}
