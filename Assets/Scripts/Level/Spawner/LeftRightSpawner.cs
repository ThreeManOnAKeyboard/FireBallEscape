using System.Collections;
using UnityEngine;

public class LeftRightSpawner : Spawner
{
	// The script which contains the calculated positions of bands
	private LeftRightMovement LRMScript;

	// Use this for initialization
	new private void Start()
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

	private IEnumerator SpawnDrop(float spawnPosition)
	{
		while (true)
		{
			// Wait the random amount of time till next drop instantiation
			yield return new WaitForSecondsRealtime(Random.Range(minSpawnInterval, maxSpawnInterval));

			// Pool new random drop
			do
			{
				objectToSpawn = ObjectPool.Instance.GetPooledObject(GetRandomSpawnable());
			} while (objectToSpawn == null);

			objectToSpawn.transform.position = new Vector3(spawnPosition, transform.position.y, transform.position.z);
			objectToSpawn.SetActive(true);
		}
	}
}
