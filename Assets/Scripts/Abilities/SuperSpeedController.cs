using System.Collections;
using Player;
using UnityEngine;
using _3rdParty;

namespace Abilities
{
	public class SuperSpeedController : AbilityController
	{
		public float duration;
		public float fadeDuration;
		[Range(0.1f, 1f)]
		public float timeScale;
		[Range(0.002f, 0.02f)]
		public float fixedDeltaTime;
		public float playerSpeedMultiplier;

		private GameObject player;

		private void Awake()
		{
			player = GameObject.FindWithTag(Tags.Player);
		}

		private void OnEnable()
		{
			StartCoroutine(Perform());
		}

		private void Update()
		{
			if (player == null)
			{
				gameObject.SetActive(false);
			}
		}

		private IEnumerator Perform()
		{
			float time = 0f;
			Movement.speedMultiplier = playerSpeedMultiplier / 4f;

			// Perform fade in
			while (time < fadeDuration)
			{
				Time.timeScale = 1f - time / fadeDuration * (1f - timeScale);
				Time.fixedDeltaTime = 0.02f - time / fadeDuration * (0.02f - fixedDeltaTime);
				//Movement.speedMultiplier = Mathf.Clamp(time / fadeDuration * playerSpeedMultiplier, 1f, playerSpeedMultiplier);

				time += Time.unscaledDeltaTime;
				yield return null;
			}

			Time.timeScale = timeScale;
			Time.fixedDeltaTime = fixedDeltaTime;
			Movement.speedMultiplier = playerSpeedMultiplier;

			yield return new WaitForSecondsRealtime(duration - 2f * fadeDuration);

			time = 0f;
			Movement.speedMultiplier = 1f;

			// Perform fade out
			while (time < fadeDuration)
			{
				Time.timeScale = 1f - (1f - time / fadeDuration) * (1f - timeScale);
				Time.fixedDeltaTime = 0.02f - (1f - time / fadeDuration) * (0.02f - fixedDeltaTime);
				//Movement.speedMultiplier = Mathf.Clamp((1f - time / fadeDuration) * playerSpeedMultiplier, 1f, playerSpeedMultiplier);

				time += Time.unscaledDeltaTime;
				yield return null;
			}

			Time.timeScale = 1f;
			Time.fixedDeltaTime = 0.02f;

			gameObject.SetActive(false);
		}
	}
}
