using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
	public List<DropHolder> drops;
	[HideInInspector]
	public List<DropHolder> currentDrops;

	public float minSpawnInterval;
	public float maxSpawnInterval;

	protected GameObject drop;
	protected bool isCooldownDone = true;

	// Use this for initialization
	public void Start()
	{
		OrderDropsByProbability();
		currentDrops = drops;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void ActivateFuelDropRain(List<DropHolder> fuelRainDrops)
	{
		currentDrops = fuelRainDrops;
	}

	public void ResetDrops()
	{
		currentDrops = drops;
	}

	protected void OrderDropsByProbability()
	{
		// Order drops by probability
		int minIndex;
		float minValue;
		for (int i = 0; i < drops.Count - 1; i++)
		{
			minValue = drops[i].probability;
			minIndex = i;
			for (int j = i + 1; j < drops.Count; j++)
			{
				if (drops[j].probability < minValue)
				{
					minIndex = j;
					minValue = drops[j].probability;
				}
			}

			if (minIndex != i)
			{
				DropHolder tempDrop = drops[i];
				drops[i] = drops[minIndex];
				drops[minIndex] = tempDrop;
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
				if (currentDrops[i].needCooldown)
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
