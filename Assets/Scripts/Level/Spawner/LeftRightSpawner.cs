using System.Collections;
using Player;
using UnityEngine;

namespace Level.Spawner
{
	public class LeftRightSpawner : Spawner
	{
		// The script which contains the calculated positions of bands
		private LeftRightMovement lrmScript;

		// Use this for initialization
		private new void Start()
		{
			base.Start();

			lrmScript = FindObjectOfType<LeftRightMovement>();

			// Start for each band it's own coroutine
			foreach (float bandPosition in lrmScript.bandsPositions)
			{
				StartCoroutine(SpawnDrop(bandPosition));
			}
		}

		private IEnumerator SpawnDrop(float spawnPosition)
		{
			while (true)
			{
				// Wait the random amount of time till next drop instantiation
				yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

				// Pool new random drop
				do
				{
					objectToSpawn = ObjectPool.instance.GetPooledObject(GetRandomSpawnable());
				} while (objectToSpawn == null);

				objectToSpawn.transform.position = new Vector3(spawnPosition, transform.position.y, transform.position.z);
				objectToSpawn.SetActive(true);
			}
		}
	}
}
