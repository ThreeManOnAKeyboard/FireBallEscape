using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
	public List<DropHolder> drops;
	public List<DropHolder> fuelRainDrops;
	[HideInInspector]
	public List<DropHolder> currentDrops;

	public float fuelRainDuration;

	public float minSpawnInterval;
	public float maxSpawnInterval;

	protected GameObject drop;

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

	public void ActivateFuelDropRain()
	{
		currentDrops = fuelRainDrops;
		StartCoroutine(ResetDrops());
	}

	public IEnumerator ResetDrops()
	{
		yield return new WaitForSeconds(fuelRainDuration);
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
				return currentDrops[i].drop;
			}
			else
			{
				randomResult -= currentDrops[i].probability;
			}
		}

		return null;
	}
}
