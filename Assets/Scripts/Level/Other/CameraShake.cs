using System.Collections;
using UnityEngine;
using _3rdParty;

namespace Level.Other
{
	public class CameraShake : MonoBehaviour
	{
		public static CameraShake instance;

		private float duration;
		private float speed;
		private float magnitude;
		private float zoomDistance;
		[HideInInspector]
		public float originZ;

		private void Awake()
		{
			instance = this;
		}

		public void StartShake(float _duration, float _speed, float _magnitude, float _zoomDistance)
		{
			StopCoroutine("Shake");

			duration = _duration;
			speed = _speed;
			magnitude = _magnitude;
			zoomDistance = _zoomDistance;

			StartCoroutine("Shake");
		}

		private IEnumerator Shake()
		{
			float elapsed = 0f;
			float x = 0f;
			float y = 0f;
			float zoom = 0f;

			float randomStart = Random.Range(-1000f, 1000f);

			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;

				float elapsedRate = elapsed / duration;

				// We want to reduce the shake from full power to 0 starting half way through
				float damper = 1f - Mathf.Clamp(2f * elapsedRate - 1f, 0f, 1f);

				// Calculate the noise parameter starting randomly and going as fast as speed allows
				float alpha = randomStart + speed * elapsedRate;

				// Map noise to [-1, 1]
				x = Noise.GetNoise(alpha, 0f, 0f) * 2f - 1f;
				y = Noise.GetNoise(0f, alpha, 0f) * 2f - 1f;

				x *= magnitude * damper;
				y *= magnitude * damper;

				if (elapsedRate < 0.25f && zoom < (zoomDistance * elapsedRate / 0.25f))
				{
					zoom = zoomDistance * elapsedRate / 0.25f;
				}
				else if (elapsedRate > 0.75f)
				{
					zoom = zoomDistance * (1 - elapsedRate) / 0.25f;
				}

				transform.position = new Vector3(x, transform.position.y + y, originZ + zoom);

				yield return null;
			}

			// Reset position
			transform.position = new Vector3(0, transform.position.y, originZ);
		}
	}
}
