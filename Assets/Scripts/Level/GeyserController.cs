using System.Collections;
using UnityEngine;

public class GeyserController : MonoBehaviour
{
	public float ejectDuration;
	public float ejectCooldown;

	private ParticleSystem myParticleSystem;
	private BoxCollider2D boxCollider2D;

	void Awake()
	{
		myParticleSystem = GetComponentInChildren<ParticleSystem>();
		boxCollider2D = GetComponent<BoxCollider2D>();
	}

	// Use this for initialization
	void OnEnable()
	{
		Vector3 position = transform.position;
		Vector3 eulerAngles = transform.eulerAngles;

		if (Random.Range(0, 2) == 0)
		{
			// Right side
			position.x = CameraController.rightBorder;
			eulerAngles.z = 0f;
		}
		else
		{
			// Left side
			position.x = CameraController.leftBorder;
			eulerAngles.z = 180f;
		}

		// Asign values
		transform.position = position;
		transform.eulerAngles = eulerAngles;

		StartCoroutine(Eject());
	}

	void OnDisable()
	{
		myParticleSystem.Stop();
		boxCollider2D.enabled = false;
	}

	IEnumerator Eject()
	{
		while (true)
		{
			// Eject the geyser
			myParticleSystem.Play();
			boxCollider2D.enabled = true;

			yield return new WaitForSeconds(ejectDuration);

			// Stop ejection
			myParticleSystem.Stop();
			boxCollider2D.enabled = false;

			yield return new WaitForSeconds(ejectCooldown);
		}
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == Tags.tags.Player.ToString())
		{
			col.gameObject.GetComponent<PlayerController>().Damage();
		}
	}
}
