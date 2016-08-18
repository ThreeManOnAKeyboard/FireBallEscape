using System.Collections;
using UnityEngine;

public class LeftRightSpawner : Spawner
{
	// The script which contains the calculated positions of bands
	private LeftRightMovement LRMScript;

	// Use this for initialization
	new void Start()
	{
		base.Start();

		LRMScript = FindObjectOfType<LeftRightMovement>();

		// Start for each band it's own coroutine
		for (int i = 0; i < LRMScript.bandsPositions.Length; i++)
		{
			StartCoroutine(SpawnDrop(LRMScript.bandsPositions[i]));
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
			do
			{
				drop = ObjectPool.Instance.GetPooledObject(GetRandomDrop());
			} while (drop == null);

			drop.transform.position = new Vector3(spawnPosition, transform.position.y, transform.position.z);
			drop.SetActive(true);
		}
	}
}
