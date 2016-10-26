using UnityEngine;
using System.Collections;

public class StoneCollisionEffectController : MonoBehaviour
{
	public float duration;
	public Vector2 minForce;
	public Vector2 maxForce;

	private Rigidbody2D[] pieces;
	private Vector3[] piecesPositions;
	private Quaternion[] piecesRotations;

	// Use this for initialization
	private void Awake()
	{
		pieces = GetComponentsInChildren<Rigidbody2D>(true);
		piecesPositions = new Vector3[pieces.Length];
		piecesRotations = new Quaternion[pieces.Length];

		// Store default initial values
		for (int i = 0; i < pieces.Length; i++)
		{
			piecesPositions[i] = pieces[i].transform.position;
			piecesRotations[i] = pieces[i].transform.rotation;
		}
	}

	private void OnEnable()
	{
		// Set the default transform properties
		for (int i = 0; i < pieces.Length; i++)
		{
			pieces[i].transform.position = piecesPositions[i];
			pieces[i].transform.rotation = piecesRotations[i];

			// Add force to each child piece
			pieces[i].AddForce
			(
				new Vector2
				(
					Random.Range(minForce.x, maxForce.x) * (Random.Range(0, 2) == 0 ? 1f : -1f),
					Random.Range(minForce.y, maxForce.y)
				),
				ForceMode2D.Impulse
			);
		}
	}

	private void OnDisable()
	{

	}

	private IEnumerator Disable()
	{
		yield return new WaitForSeconds(duration);

		// Disable GameObject
		gameObject.SetActive(false);
	}
}
