﻿using UnityEngine;
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

	public void DoPostEffect()
	{
		GameObject explosion = Instantiate(collisionEffect);
		explosion.transform.position = transform.position;
		Destroy(explosion, 2f);
		gameObject.SetActive(false);
	}
}
