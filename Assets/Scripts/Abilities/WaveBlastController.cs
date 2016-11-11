using System.Collections;
using UnityEngine;

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

	private float finalShapeRadius = 7f;
	private float finalColliderRadius = 8f;

	private GameObject player;
	private ParticleSystem thisParticleSystem;
	private ParticleSystem.ShapeModule thisShape;
	private CircleCollider2D circleCollider2D;

	private void Awake()
	{
		player = GameObject.FindWithTag(Tags.PLAYER);
		thisShape = GetComponent<ParticleSystem>().shape;
		circleCollider2D = GetComponent<CircleCollider2D>();
	}

	// Use this for initialization
	private void OnEnable()
	{
		transform.position = player.transform.position;

		CameraShake.Instance.StartShake(duration, shakeSpeed, magnitude, zoomDistance);

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
			thisShape.radius = time / finalRadiusDeltaTime * finalShapeRadius;
			circleCollider2D.radius = time / finalRadiusDeltaTime * finalColliderRadius;

			yield return null;

			if (Time.timeScale != 0)
			{
				time += Time.unscaledDeltaTime;
			}
		}
	}
}
