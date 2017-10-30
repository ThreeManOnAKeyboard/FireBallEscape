using System.Collections;
using System.Collections.Generic;
using Level.Spawner;
using UnityEngine;

namespace Abilities
{
	public class StrikeController : AbilityController
	{
		public GameObject fireBall;
		public int fireballsAmount = 1;
		public float fireRate = 1f;

		public static List<GameObject> targets;

		// Use this for initialization
		private void Awake()
		{
			targets = new List<GameObject>();
		}

		private void OnEnable()
		{
			StartCoroutine(SpawnFireBalls());
		}

		private IEnumerator SpawnFireBalls()
		{
			for (int i = 0; i < fireballsAmount; i++)
			{
				// Get instance
				GameObject fireBallInstance = ObjectPool.instance.GetPooledObject(fireBall);

				// Activate fireball
				fireBallInstance.SetActive(true);

				yield return new WaitForSeconds(1f / fireRate);
			}

			// Deactivate ability when all fireballs reached their target
			while (FireBallController.fireballCount != 0)
			{
				yield return new WaitForSecondsRealtime(0.2f);
			}

			gameObject.SetActive(false);
		}
	}
}
