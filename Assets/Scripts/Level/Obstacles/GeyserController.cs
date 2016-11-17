using UnityEngine;
using System.Collections;

public class GeyserController : MonoBehaviour
{
	[Header("Configuration parameters")]
	public float firstEjectDelay;
	public float ejectDuration;
	public float ejectCooldown;
	public float angleRange = 40f;
	public float offset;

	[Header("Particle Systems")]
	public ParticleSystem inactiveParticleSystem;
	public ParticleSystem activeParticleSystem;

	protected BoxCollider2D thisCollider2D;
	protected PlayerController playerController;
	protected Transform meshTransform;

	protected void Awake()
	{
		thisCollider2D = GetComponent<BoxCollider2D>();
		playerController = FindObjectOfType<PlayerController>();
		meshTransform = GetComponentInChildren<MeshFilter>().gameObject.transform;
	}

	// Use this for initialization
	protected void OnEnable()
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

		StartCoroutine(EjectProcess());
	}

	protected void OnDisable()
	{
		Stop();
	}

	protected IEnumerator EjectProcess()
	{
		yield return new WaitForSeconds(Random.Range(0f, firstEjectDelay));

		float worldToViewPortRatio;

		do
		{
			Start();
			yield return new WaitForSeconds(ejectDuration);

			Stop();

			yield return new WaitForSeconds(ejectCooldown);

			worldToViewPortRatio = Camera.main.WorldToViewportPoint(transform.position).y;
		} while (worldToViewPortRatio >= -0.25f);

		gameObject.SetActive(false);
	}

	private void Start()
	{
		// Eject
		activeParticleSystem.Play();
		inactiveParticleSystem.Stop();
		thisCollider2D.enabled = true;
	}

	private void Stop()
	{
		// Stop ejection
		activeParticleSystem.Stop();
		inactiveParticleSystem.Play();
		thisCollider2D.enabled = false;
	}
}
