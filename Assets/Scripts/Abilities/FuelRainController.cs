using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FuelRainController : MonoBehaviour
{
	public float speed;
	public List<DropSpawnProperties> fuelRainDrops;
	public float duration;

	private Image splashEffectImage;
	[Range(0.01f, 1f)]
	public float splashDuration;

	private delegate void EnableMethod(List<DropSpawnProperties> fuelRainDrops);
	private EnableMethod enableMethod;

	private delegate void ResetMethod();
	private ResetMethod resetMethod;
	private ParticleSystem thisParticleSystem;
	private Transform playerTransform;

	// Use this for initialization
	private void Awake()
	{
		thisParticleSystem = GetComponent<ParticleSystem>();
		playerTransform = GameObject.FindWithTag(Tags.PLAYER).transform;
		splashEffectImage = GameObject.Find("FuelRainSplashEffect").GetComponent<Image>();

		switch (GameManager.Instance.controlType)
		{
			case Enumerations.ControlType.Free:
				enableMethod = FindObjectOfType<FreeControlSpawner>().ChangeSpawnables;
				resetMethod = FindObjectOfType<FreeControlSpawner>().ResetDrops;
				break;
			case Enumerations.ControlType.Sideways:
				enableMethod = FindObjectOfType<LeftRightSpawner>().ChangeSpawnables;
				resetMethod = FindObjectOfType<LeftRightSpawner>().ResetDrops;
				break;
			case Enumerations.ControlType.ZigZag:
				enableMethod = FindObjectOfType<ZigZagSpawner>().ChangeSpawnables;
				resetMethod = FindObjectOfType<ZigZagSpawner>().ResetDrops;
				break;
		}
	}

	private void OnEnable()
	{
		StoneRainController SRC = FindObjectOfType<StoneRainController>();

		if (SRC != null)
		{
			SRC.gameObject.SetActive(false);
		}

		transform.position = playerTransform.position;

		StartCoroutine(Move());
	}

	private IEnumerator ResetDrops()
	{
		yield return new WaitForSeconds(duration);

		resetMethod();
		gameObject.SetActive(false);
	}

	private IEnumerator Move()
	{
		if (thisParticleSystem.isStopped)
		{
			thisParticleSystem.Play();
		}

		while (Camera.main.WorldToViewportPoint(transform.position).y <= 1.3f && Camera.main.WorldToViewportPoint(transform.position).y >= -0.3f)
		{
			transform.Translate(Vector2.up * speed * Time.deltaTime);

			yield return null;
		}

		// Activate splash effect after the fuel rain drop went out of the screen
		StartCoroutine(DoSplashEffect());
		enableMethod(fuelRainDrops);
		StartCoroutine(ResetDrops());

		if (thisParticleSystem.isPlaying)
		{
			thisParticleSystem.Stop();
		}
	}

	private IEnumerator DoSplashEffect()
	{
		float time = 0f;
		Color color = splashEffectImage.color;

		// Fade to maximum alpha
		while (time < (splashDuration / 3))
		{
			color.a = time / (splashDuration / 3);
			splashEffectImage.color = color;

			time += Time.deltaTime;
			yield return null;
		}

		time = 0f;
		// Fade to minimum alpha
		while (time <= (splashDuration * 2 / 3))
		{
			color.a = (1 - time / (splashDuration * 2 / 3));
			splashEffectImage.color = color;

			time += Time.deltaTime;
			yield return null;
		}

		color.a = 0f;
		splashEffectImage.color = color;
	}
}
