using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightSpawner : MonoBehaviour
{
	public List<DropHolder> drops;

	public float minSpawnInterval = 0.3f;
	public float maxSpawnInterval = 1f;

	// The script which contains the calculated positions of bands
	private LeftRightMovement LRMScript;

	private GameObject drop;

	// Use this for initialization
	void Start()
	{
		LRMScript = FindObjectOfType<LeftRightMovement>();

		// Start for each band it's own coroutine
		for (int i = 0; i < LRMScript.bandsPositions.Length; i++)
		{
			StartCoroutine(SpawnDrop(LRMScript.bandsPositions[i]));
		}

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

	// Update is called once per frame
	void Update()
	{

	}

	IEnumerator SpawnDrop(float spawnPosition)
	{
		while (true)
		{
			// Wait the random amount of time till next drop instantiation
			yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

			// Pool new random drop
			while (drop == null)
			{
				drop = ObjectPool.Instance.GetPooledObject(GetRandomDrop());
			}
			print(drop.name);
			drop.transform.position = new Vector3(spawnPosition, transform.position.y, transform.position.z);
			drop.SetActive(true);
		}
	}

	public GameObject GetRandomDrop()
	{
		float randomResult = Random.Range(0, 1);

		for (int i = 0; i < drops.Count; i++)
		{
			if (drops[i].probability > randomResult)
			{
				return drops[i].drop;
			}
		}

		return null;
	}
}
