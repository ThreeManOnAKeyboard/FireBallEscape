using System.Collections;
using UnityEngine;

public class GeyserController : MonoBehaviour
{
	public float ejectDuration;
	public float ejectCooldown;
	public float angleRange = 40f;
	public float offset;

	public ParticleSystem inactiveParticleSystem;
	public ParticleSystem activeParticleSystem;
	private BoxCollider2D boxCollider2D;

	void Awake()
	{
		boxCollider2D = GetComponent<BoxCollider2D>();
	}

	// Use this for initialization
	void OnEnable()
	{
		Vector3 position = transform.position;
		Quaternion rotation = transform.rotation;

		if (Random.Range(0, 2) == 0)
		{
			// Right side
			position.x = CameraController.rightBorder - offset;
			rotation = Quaternion.Euler(Random.Range(0f, 359f), 0f, Random.Range(-angleRange, angleRange));
		}
		else
		{
			// Left side
			position.x = CameraController.leftBorder + offset;
			rotation = Quaternion.Euler(0f, 0f, 180f + Random.Range(-angleRange, angleRange));
		}

		// Asign values
		transform.position = position;
		transform.rotation = rotation;

		StartCoroutine(Eject());
	}

	void OnDisable()
	{
		activeParticleSystem.Stop();
		inactiveParticleSystem.Play();
		boxCollider2D.enabled = false;
	}

	IEnumerator Eject()
	{
		while (true)
		{
			// Eject the geyser
			activeParticleSystem.Play();
			inactiveParticleSystem.Stop();
			boxCollider2D.enabled = true;

			yield return new WaitForSeconds(ejectDuration);

			// Stop ejection
			activeParticleSystem.Stop();
			inactiveParticleSystem.Play();
			boxCollider2D.enabled = false;

			yield return new WaitForSeconds(ejectCooldown);
		}
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == Tags.PLAYER)
		{
			col.gameObject.GetComponent<PlayerController>().Damage(false);
		}
	}
}
