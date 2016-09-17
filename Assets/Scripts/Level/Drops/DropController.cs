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

		if (col.gameObject.tag != Tags.DESTROYER && !isComplexDrop)
		{
			DoPostEffect();
		}
	}

	public void OnWaterDrop()
	{
		bool postEffectRequired = false;

		if (collidedObject.tag == Tags.PLAYER)
		{
			playerController.Damage();
			playerController.ResetMultiplier();
		}

		if (collidedObject.tag == Tags.DESTROYER)
		{
			postEffectRequired = false;
		}
		else if (StrikeController.targetedDrops != null)
		{
			if (collidedObject.tag == Tags.FIREBALL)
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
		if (collidedObject.tag == Tags.PLAYER)
		{
			playerController.Heal();
			playerController.AddScore(scoreAmount);
		}
	}

	public void OnSidesFlameDrop()
	{
		if (collidedObject.tag == Tags.PLAYER)
		{
			playerController.Kill();
		}
		else if (collidedObject.tag == Tags.MAXPOWERWAVE)
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
