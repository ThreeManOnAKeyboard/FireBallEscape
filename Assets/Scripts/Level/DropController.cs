using UnityEngine;
using UnityEngine.Events;

public class DropController : MonoBehaviour
{
	public UnityEvent onCollisionMethod;
	public UnityEvent updateMethod;

	public GameObject collisionEffect;

	public float healthAmount;
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
		updateMethod.Invoke();
	}

	public void SimpleDropUpdate()
	{
		transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
	}

	public void SidesFlameDropUpdate()
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
		if (collidedObject.tag == Tags.tags.Player.ToString())
		{
			playerController.Damage();
			playerController.ResetMultiplier();
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

	public void OnShieldDrop()
	{
		if (collidedObject.tag == Tags.tags.Player.ToString())
		{
			playerController.ActivateShield();
		}
	}

	public void OnWaveDrop()
	{
		if (collidedObject.tag == Tags.tags.Player.ToString())
		{
			playerController.ActicateWave();
		}
	}

	public void OnFuelRainDrop()
	{
		if (collidedObject.tag == Tags.tags.Player.ToString())
		{
			switch (GameManager.Instance.controlType)
			{
				case GameManager.ControlType.FREE:
					FindObjectOfType<FreeControlSpawner>().ActivateFuelDropRain();
					break;
				case GameManager.ControlType.SIDEWAYS:
					FindObjectOfType<LeftRightSpawner>().ActivateFuelDropRain();
					break;
				case GameManager.ControlType.ZIGZAG:
					FindObjectOfType<ZigZagSpawner>().ActivateFuelDropRain();
					break;
			}
		}
	}

	public void OnSideFlamesDrop()
	{

	}

	public void DoPostEffect()
	{
		GameObject collisionParticleSystem = ObjectPool.Instance.GetPooledObject(collisionEffect);
		collisionParticleSystem.transform.position = transform.position;
		collisionParticleSystem.SetActive(true);
		gameObject.SetActive(false);
	}
}
