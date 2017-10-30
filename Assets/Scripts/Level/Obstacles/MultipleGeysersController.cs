using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Level.Spawner;
using UnityEngine;
using _3rdParty;

namespace Level.Obstacles
{
	public class MultipleGeysersController : MonoBehaviour
	{
		public float duration;
		public List<SpawnableState> states;

		private Spawner.Spawner currentSpawner;

		// Use this for initialization
		private void Awake()
		{
			currentSpawner = GameObject.FindWithTag(Tags.Spawner).GetComponents<Spawner.Spawner>().Single(spawner => spawner.enabled);
		}

		private void OnEnable()
		{
			StartCoroutine(SpawnGeysers());
		}

		private IEnumerator SpawnGeysers()
		{
			// Wait until spawner can be changed
			while (!Spawner.Spawner.canChangeSpawnables)
			{
				yield return null;
			}
			Spawner.Spawner.canChangeSpawnables = false;

			currentSpawner.ChangeSpawnables(states[Random.Range(0, states.Count)].spawnables);

			yield return new WaitForSeconds(duration);

			Disable();
		}

		private void Disable()
		{
			currentSpawner.ResetSpawnables();
			Spawner.Spawner.canChangeSpawnables = true;
			gameObject.SetActive(false);
		}
	}
}
