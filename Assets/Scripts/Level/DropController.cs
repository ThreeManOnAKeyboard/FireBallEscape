using UnityEngine;
using UnityEngine.Events;

public class DropController : MonoBehaviour
{
	public UnityEvent methodToCall;

	public GameObject collisionEffect;

	public float healthAmount;
	public float fallSpeed;
	public float scoreAmount;

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
		methodToCall.Invoke();
		DoPostEffect();
	}

	public void OnWaterDrop()
	{

	}

	public void OnFuelDrop()
	{

	}

	public void OnShieldDrop()
	{

	}

	public void DoPostEffect()
	{
		GameObject explosion = Instantiate(collisionEffect);
		explosion.transform.position = transform.position;
		Destroy(explosion, 2f);
		gameObject.SetActive(false);
	}
}
