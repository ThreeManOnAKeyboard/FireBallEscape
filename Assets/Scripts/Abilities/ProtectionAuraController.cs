using System.Collections;
using UnityEngine;
using _3rdParty;

namespace Abilities
{
	public class ProtectionAuraController : AbilityController
	{
		public float duration;
		public float maxAngleRange;
		public float angleSensivity;
		public float angleGravity;

		private float previousX;
		private Transform playerTransform;

		private void Awake()
		{
			playerTransform = GameObject.FindWithTag(Tags.Player).transform;
			transform.parent = playerTransform;
			transform.localPosition = Vector3.zero;
		}

		private void OnEnable()
		{
			previousX = playerTransform.position.x;
			StartCoroutine(Disable());
		}

		private void Update()
		{
			// Rotate shield acording to player movement and input parameters
			if (playerTransform.position.x != previousX)
			{
				ApplyPlayerMotion();
			}
			else
			{
				ApplyAngleGravity();
			}

			// Save last player x position
			previousX = playerTransform.position.x;
		}

		private void ApplyPlayerMotion()
		{
			transform.rotation = Quaternion.Euler
			(
				transform.eulerAngles.x,
				transform.eulerAngles.y,
				Mathf.LerpAngle
				(
					transform.eulerAngles.z,
					playerTransform.position.x < previousX ? maxAngleRange : 360f - maxAngleRange,
					angleSensivity * Mathf.Abs(playerTransform.position.x - previousX) * Time.deltaTime
				)
			);
		}

		private void ApplyAngleGravity()
		{
			transform.rotation = Quaternion.Euler
			(
				transform.eulerAngles.x,
				transform.eulerAngles.y,
				Mathf.LerpAngle
				(
					transform.eulerAngles.z,
					transform.eulerAngles.z > 180 ? 360f : 0f,
					angleGravity * Time.deltaTime
				)
			);
		}

		private IEnumerator Disable()
		{
			yield return new WaitForSeconds(duration);

			gameObject.SetActive(false);
		}
	}
}
