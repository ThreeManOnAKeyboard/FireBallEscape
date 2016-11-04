using UnityEngine;

public class FireSpiritController : MonoBehaviour
{
	public float speed;

	public bool isAvailable { get; private set; }

	private Transform target;

	private void OnEnable()
	{
		// Reset fire spirit
		target = null;
		isAvailable = true;
	}

	// Update is called once per frame
	private void Update()
	{
		if (target != null)
		{
			// Move towards a terget
			transform.position = Vector3.Lerp
			(
				transform.position,
				target.position,
				speed * Time.deltaTime
			);
		}
		else if (!isAvailable)
		{
			// target was destroyed before fire spirit reached it
			gameObject.SetActive(false);
		}
	}

	public void SetTarget(Transform _target)
	{
		isAvailable = false;
		target = _target;
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (!isAvailable && col.transform == target)
		{
			// Destroy target
			Debug.Log("Boom dat target!");

			// Disable fire spirit
			gameObject.SetActive(false);
		}
	}
}
