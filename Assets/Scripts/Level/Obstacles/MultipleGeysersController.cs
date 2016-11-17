using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class MultipleGeysersController : MonoBehaviour
{
	public float duration;
	public List<SpawnableState> states;

	private Spawner currentSpawner;

	// Use this for initialization
	private void Awake()
	{
		currentSpawner = GameObject.FindWithTag(Tags.SPAWNER).GetComponents<Spawner>().Single(spawner => spawner.enabled);
	}

	private void OnEnable()
	{
		StartCoroutine(SpawnGeysers());
	}

	private IEnumerator SpawnGeysers()
	{
		// Wait until spawner can be changed
		while (!Spawner.canChangeSpawnables)
		{
			yield return null;
		}
		Spawner.canChangeSpawnables = false;

		currentSpawner.ChangeSpawnables(states[Random.Range(0, states.Count)].spawnables);

		yield return new WaitForSeconds(duration);

		Disable();
	}

	private void Disable()
	{
		currentSpawner.ResetSpawnables();
		Spawner.canChangeSpawnables = true;
		gameObject.SetActive(false);
	}
}
