using UnityEngine;
using UnityEngine.Events;

public class DropController : MonoBehaviour
{
	public UnityEvent onCollisionMethod;

	public GameObject collisionEffect;

	public float fallSpeed;
	public float scoreAmount;

	public bool isComplexDrop;

	private GameObject collidedObject;

	private PlayerController playerController;

	// Use this for initialization
	void Start()
	{
		playerController = FindObjectOfType<PlayerController>();
	}

	// Update is called once per frame
	void Update()
	{
		transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		collidedObject = col.gameObject;
		onCollisionMethod.Invoke();

		if (col.gameObject.tag != Tags.tags.Destroyer.ToString() && !isComplexDrop)
		{
			DoPostEffect();
		}
	}

	public void OnWaterDrop()
	{
		bool postEffectRequired = false;

		if (collidedObject.tag == Tags.tags.Player.ToString())
		{
			playerController.Damage();
			playerController.ResetMultiplier();
		}

		if (StrikeController.targetedDrops != null)
		{
			if (collidedObject.tag == Tags.tags.Fireball.ToString())
			{
				if (StrikeController.targetedDrops.Contains(gameObject))
				{
					postEffectRequired = true;
				}
			}
			else
			{
				postEffectRequired = true;
			}

			StrikeController.targetedDrops.Remove(gameObject);
		}
		else
		{
			postEffectRequired = true;
		}

		if (postEffectRequired)
		{
			DoPostEffect();
		}
	}

	public void OnFuelDrop()
	{
		if (collidedObject.tag == Tags.tags.Player.ToString())
		{
			playerController.Heal();
			playerController.AddScore(scoreAmount);
		}
	}

	public void OnSidesFlameDrop()
	{
		if (collidedObject.tag == Tags.tags.Player.ToString())
		{
			playerController.Kill();
		}
		else if (collidedObject.tag == Tags.tags.MaxPowerWave.ToString())
		{
			DoPostEffect();
		}
	}

	public void DoPostEffect()
	{
		GameObject collisionParticleSystem = ObjectPool.Instance.GetPooledObject(collisionEffect);
		collisionParticleSystem.transform.position = transform.position;
		collisionParticleSystem.SetActive(true);
		gameObject.SetActive(false);
	}
}
