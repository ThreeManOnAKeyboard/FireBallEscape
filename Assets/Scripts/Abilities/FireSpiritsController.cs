using UnityEngine;

public class FireSpiritsController : MonoBehaviour
{
	private FireSpiritController[] fireSpirits;
	private Transform playerTransform;

	// Use this for initialization
	private void Awake()
	{
		playerTransform = GameObject.FindWithTag(Tags.PLAYER).transform;
		fireSpirits = GetComponentsInChildren<FireSpiritController>(true);
	}

	// OnEnable is called once the gameobject is enabled
	private void OnEnable()
	{
		// Enable fire spirits
		foreach (FireSpiritController fireSpirit in fireSpirits)
		{
			fireSpirit.gameObject.SetActive(true);
		}
	}

	// Update is called once per frame
	private void Update()
	{
		// Check if player is still alive
		if (playerTransform == null)
		{
			gameObject.SetActive(false);
			return;
		}

		transform.position = playerTransform.position;

		// Verify if each fire spirit reached their target
		foreach (FireSpiritController fireSpirit in fireSpirits)
		{
			if (fireSpirit.gameObject.activeInHierarchy)
			{
				// At least one fire spirit has not reached its target so return
				return;
			}
		}

		// Deactivate this ability
		gameObject.SetActive(false);
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		// Trigger an available fire spirit to destroy the target
		foreach (FireSpiritController fireSpirit in fireSpirits)
		{
			// Check availability
			if (fireSpirit.isAvailable)
			{
				fireSpirit.SetTarget(col.transform);
			}
		}
	}
}
