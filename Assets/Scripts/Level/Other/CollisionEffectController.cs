using UnityEngine;

namespace Level.Other
{
	public class CollisionEffectController : MonoBehaviour
	{
		private void OnEnable()
		{
			ParticleSystem particleSystem = GetComponent<ParticleSystem>();

			Invoke("Deactivate", particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);
		}

		private void Deactivate()
		{
			gameObject.SetActive(false);
		}
	}
}
