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
		if (target != null && target.gameObject.activeInHierarchy)
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
		else
		{
			//timer += Time.deltaTime;
			//angle = timer;
			//transform.position = new Vector3
			//(
			//	(centerx + Mathf.Sin(angle) * rad),
			//	centery,
			//	((centerz + Mathf.Cos(angle) * rad))
			//);
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
			col.gameObject.GetComponent<Drop>().DoCollisionEffect(col.tag);
			gameObject.SetActive(false);
		}
	}
}
