using System.Collections;
using Level.Other;
using UnityEngine;
using _3rdParty;

namespace Abilities
{
	public class WaveBlastController : AbilityController
	{
		[Header("Ability Parameters")]
		public float speed;
		public float finalRadiusDeltaTime;

		[Header("Shake Parameters")]
		public float duration;
		public float shakeSpeed;
		public float magnitude;
		public float zoomDistance;

		private const float FinalShapeRadius = 7f;
		private const float FinalColliderRadius = 8f;

		private GameObject player;
		private ParticleSystem thisParticleSystem;
		private ParticleSystem.ShapeModule thisShape;
		private CircleCollider2D circleCollider2D;

		private void Awake()
		{
			player = GameObject.FindWithTag(Tags.Player);
			thisShape = GetComponent<ParticleSystem>().shape;
			circleCollider2D = GetComponent<CircleCollider2D>();
		}

		// Use this for initialization
		private void OnEnable()
		{
			transform.position = player.transform.position;

			CameraShake.instance.StartShake(duration, shakeSpeed, magnitude, zoomDistance);

			StartCoroutine(SelfDestroy());
			StartCoroutine(ChangeRadiusSmoothly());
		}

		private IEnumerator SelfDestroy()
		{
			while (Camera.main.WorldToViewportPoint(transform.position).y <= 1.3f && Camera.main.WorldToViewportPoint(transform.position).y >= -0.3f)
			{
				if (Time.timeScale != 0)
				{
					transform.Translate(Vector2.up * speed * Time.unscaledDeltaTime);
				}

				yield return null;
			}

			gameObject.SetActive(false);
		}

		private IEnumerator ChangeRadiusSmoothly()
		{
			float time = 0f;

			while (time <= finalRadiusDeltaTime)
			{
				thisShape.radius = time / finalRadiusDeltaTime * FinalShapeRadius;
				circleCollider2D.radius = time / finalRadiusDeltaTime * FinalColliderRadius;

				yield return null;

				if (Time.timeScale != 0)
				{
					time += Time.unscaledDeltaTime;
				}
			}
		}
	}
}
