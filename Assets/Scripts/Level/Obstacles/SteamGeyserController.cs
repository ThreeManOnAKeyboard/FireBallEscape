﻿using System.Collections;
using UnityEngine;

public class SteamGeyserController : MonoBehaviour
{
	[Header("Configuration parameters")]
	public float ejectDuration;
	public float ejectCooldown;
	public float angleRange = 40f;
	public float offset;

	[Header("Particle Systems")]
	public ParticleSystem inactiveParticleSystem;
	public ParticleSystem activeParticleSystem;

	private BoxCollider2D thisCollider2D;
	private PlayerController playerController;
	private Transform meshTransform;

	private void Awake()
	{
		thisCollider2D = GetComponent<BoxCollider2D>();
		playerController = FindObjectOfType<PlayerController>();
		meshTransform = GetComponentInChildren<MeshFilter>().gameObject.transform;
	}

	// Use this for initialization
	private void OnEnable()
	{
		Vector3 position = transform.position;

		if (Random.Range(0, 2) == 0)
		{
			// Right side
			position.x = CameraController.Instance.rightBorder - offset;
			transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-angleRange, angleRange));
		}
		else
		{
			// Left side
			position.x = CameraController.Instance.leftBorder + offset;
			transform.rotation = Quaternion.Euler(0f, 0f, 180f + Random.Range(-angleRange, angleRange));
		}

		// Asign values
		transform.position = position;

		// Change the rotation of x axis of mesh object so it looks different
		meshTransform.Rotate(Vector3.right * Random.Range(0f, 359f));

		StartCoroutine(Eject());
	}

	private void OnDisable()
	{
		activeParticleSystem.Stop();
		inactiveParticleSystem.Play();
		thisCollider2D.enabled = false;
	}

	private IEnumerator Eject()
	{
		while (true)
		{
			// Eject the geyser
			activeParticleSystem.Play();
			inactiveParticleSystem.Stop();
			thisCollider2D.enabled = true;

			yield return new WaitForSeconds(ejectDuration);

			// Stop ejection
			activeParticleSystem.Stop();
			inactiveParticleSystem.Play();
			thisCollider2D.enabled = false;

			yield return new WaitForSeconds(ejectCooldown);
		}
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == Tags.PLAYER && !playerController.isUnderSuperShield)
		{
			playerController.Damage();
		}
	}
}
