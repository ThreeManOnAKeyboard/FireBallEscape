using System.Collections;
using Level.Other;
using Managers;
using UnityEngine;

namespace Level.Spawner
{
	public class ZigZagSpawner : Spawner
	{
		public float additionalOffset;
		private float totalBorderOffset;

		// Use this for initialization
		private new void Start()
		{
			base.Start();

			totalBorderOffset = GameManager.Instance.bordersOffset + additionalOffset;
			StartCoroutine(SpawnDrop());
		}

		private IEnumerator SpawnDrop()
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

				objectToSpawn.transform.position = new Vector3
				(
					Random.Range
					(
						CameraController.instance.LeftBorder + totalBorderOffset,
						CameraController.instance.RightBorder - totalBorderOffset
					),
					transform.position.y,
					transform.position.z
				);
				objectToSpawn.SetActive(true);
			}
		}
	}
}
