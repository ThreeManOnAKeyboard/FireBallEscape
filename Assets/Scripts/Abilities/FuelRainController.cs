using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuelRainController : MonoBehaviour
{
	public float speed;
	public List<DropHolder> fuelRainDrops;
	public float fuelRainDuration;

	private delegate void OnEnableMethod(List<DropHolder> fuelRainDrops);
	private OnEnableMethod onEnableMethod;

	private delegate void ResetMethod();
	private ResetMethod resetMethod;
	private ParticleSystem thisParticleSystem;
	private Transform playerTransform;

	// Use this for initialization
	void Awake()
	{
		thisParticleSystem = GetComponent<ParticleSystem>();
		playerTransform = GameObject.FindWithTag(Tags.tags.Player.ToString()).transform;

		switch (GameManager.Instance.controlType)
		{
			case GameManager.ControlType.FREE:
				onEnableMethod = FindObjectOfType<FreeControlSpawner>().ActivateFuelDropRain;
				resetMethod = FindObjectOfType<FreeControlSpawner>().ResetDrops;
				break;
			case GameManager.ControlType.SIDEWAYS:
				onEnableMethod = FindObjectOfType<LeftRightSpawner>().ActivateFuelDropRain;
				resetMethod = FindObjectOfType<LeftRightSpawner>().ResetDrops;
				break;
			case GameManager.ControlType.ZIGZAG:
				onEnableMethod = FindObjectOfType<ZigZagSpawner>().ActivateFuelDropRain;
				resetMethod = FindObjectOfType<ZigZagSpawner>().ResetDrops;
				break;
		}
	}

	void OnEnable()
	{
		transform.position = playerTransform.position;

		onEnableMethod(fuelRainDrops);

		StartCoroutine(ResetDrops());
		StartCoroutine(Move());
	}

	private IEnumerator ResetDrops()
	{
		yield return new WaitForSeconds(fuelRainDuration);

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

		if (thisParticleSystem.isPlaying)
		{
			thisParticleSystem.Stop();
		}
	}
}
