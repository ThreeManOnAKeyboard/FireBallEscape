using UnityEngine;

public class StoneController : MonoBehaviour
{
	public Vector2 impulseForce;

	private Rigidbody2D thisRigidBody;

	// Use this for initialization
	void Awake()
	{
		thisRigidBody = GetComponent<Rigidbody2D>();
	}

	void OnEnable()
	{
		Debug.Break();
		thisRigidBody.AddForce(impulseForce * Random.Range(-1, 2), ForceMode2D.Impulse);
	}

	public void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == Tags.tags.Player.ToString())
		{

		}
	}
}
