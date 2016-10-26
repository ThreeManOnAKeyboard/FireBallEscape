using UnityEngine;

public class CollisionEffectController : MonoBehaviour
{
	void OnEnable()
	{
		ParticleSystem particleSystem = GetComponent<ParticleSystem>();

		Invoke("Deactivate", particleSystem.duration + particleSystem.startLifetime);
	}

	void Deactivate()
	{
		gameObject.SetActive(false);
	}
}
