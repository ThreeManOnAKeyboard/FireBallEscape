using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuelRainController : MonoBehaviour
{
	public float speed;
	public List<DropHolder> fuelRainDrops;
	public float duration;

	private delegate void EnableMethod(List<DropHolder> fuelRainDrops);
	private EnableMethod enableMethod;

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
				enableMethod = FindObjectOfType<FreeControlSpawner>().ChangeCurrentDrops;
				resetMethod = FindObjectOfType<FreeControlSpawner>().ResetDrops;
				break;
			case GameManager.ControlType.SIDEWAYS:
				enableMethod = FindObjectOfType<LeftRightSpawner>().ChangeCurrentDrops;
				resetMethod = FindObjectOfType<LeftRightSpawner>().ResetDrops;
				break;
			case GameManager.ControlType.ZIGZAG:
				enableMethod = FindObjectOfType<ZigZagSpawner>().ChangeCurrentDrops;
				resetMethod = FindObjectOfType<ZigZagSpawner>().ResetDrops;
				break;
		}
	}

	void OnEnable()
	{
		StoneRainController SRC = FindObjectOfType<StoneRainController>();

		if (SRC != null)
		{
			SRC.gameObject.SetActive(false);
		}

		transform.position = playerTransform.position;

		enableMethod(fuelRainDrops);

		StartCoroutine(ResetDrops());
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

		if (thisParticleSystem.isPlaying)
		{
			thisParticleSystem.Stop();
		}
	}
}
