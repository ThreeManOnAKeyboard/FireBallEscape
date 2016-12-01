using UnityEngine;

public class CollisionEffectController : MonoBehaviour
{
	void OnEnable()
	{
		ParticleSystem particleSystem = GetComponent<ParticleSystem>();

		Invoke("Deactivate", particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);
	}

	void Deactivate()
	{
		gameObject.SetActive(false);
	}
}
