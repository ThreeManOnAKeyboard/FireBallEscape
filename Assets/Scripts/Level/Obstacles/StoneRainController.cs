using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Level.Drops;
using Level.Other;
using UnityEngine;
using _3rdParty;

namespace Level.Obstacles
{
	public class StoneRainController : MonoBehaviour
	{
		[Header("Camera Shake Parameters")]
		public float shakeDuration;
		public float shakeSpeed;
		public float shakeMagnitude;
		public float zoomDistance;

		[Header("Earth Shake Parameters")]
		public List<DropSpawnProperties> stoneRainDrops;
		public float duration;

		public static bool isActive;

		private Spawner.Spawner currentSpawner;

		// Use this for initialization
		private void Awake()
		{
			currentSpawner = GameObject.FindWithTag(Tags.Spawner).GetComponents<Spawner.Spawner>().Single(spawner => spawner.enabled);
		}

		private void OnEnable()
		{
			isActive = true;
			StartCoroutine(StartEarthshake());
		}

		private IEnumerator StartEarthshake()
		{
			// Wait until spawner can be changed
			while (!Spawner.Spawner.canChangeSpawnables)
			{
				yield return null;
			}

			Spawner.Spawner.canChangeSpawnables = false;

			CameraShake.instance.StartShake(shakeDuration, shakeSpeed, shakeMagnitude, zoomDistance);

			yield return new WaitForSeconds(shakeDuration);

			currentSpawner.ChangeSpawnables(stoneRainDrops);
			StartCoroutine(ResetDrops());
		}

		private IEnumerator ResetDrops()
		{
			yield return new WaitForSeconds(duration);

			currentSpawner.ResetSpawnables();
			Spawner.Spawner.canChangeSpawnables = true;
			isActive = false;
			gameObject.SetActive(false);
		}
	}
}
