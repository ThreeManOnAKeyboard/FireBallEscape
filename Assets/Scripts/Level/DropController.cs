using UnityEngine;
using UnityEngine.Events;

public class DropController : MonoBehaviour
{
	public UnityEvent methodToCall;

	public GameObject collisionEffect;

	public float healthAmount;
	public float fallSpeed;
	public float scoreAmount;

	private GameObject collidedObject;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		collidedObject = col.gameObject;
		methodToCall.Invoke();

		if (col.gameObject.tag != Tags.tags.Destroyer.ToString())
		{
			DoPostEffect();
		}
	}

	public void OnWaterDrop()
	{
		if (collidedObject.tag == Tags.tags.Player.ToString())
		{
			collidedObject.GetComponent<PlayerController>().Damage();
		}
	}

	public void OnFuelDrop()
	{
		if (collidedObject.tag == Tags.tags.Player.ToString())
		{
			collidedObject.GetComponent<PlayerController>().Heal();
		}

		PlayerController.score += scoreAmount;
	}

	public void OnShieldDrop()
	{
		if (collidedObject.tag == Tags.tags.Player.ToString())
		{
			collidedObject.GetComponent<PlayerController>().ActivateShield();
		}
	}

	public void OnWaveDrop()
	{
		if (collidedObject.tag == Tags.tags.Player.ToString())
		{
			collidedObject.GetComponent<PlayerController>().ActicateWave();
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
