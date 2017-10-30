using UnityEngine;

namespace Level.Obstacles
{
	public class StoneCollisionEffectController : MonoBehaviour
	{
		[Header("Dumb 3D modeler can't even make a ragdoll stone")]
		public float zAngleOffset = 90f;

		[Header("Random Ranges for forces applied for each piece")]
		public Vector2 minForce;
		public Vector2 maxForce;

		public float angularVelocity;

		private Rigidbody2D[] pieces;
		private Vector3[] piecesPositions;
		private Quaternion[] piecesRotations;

		// Use this for initialization
		private void Awake()
		{
			pieces = GetComponentsInChildren<Rigidbody2D>();
			piecesPositions = new Vector3[pieces.Length];
			piecesRotations = new Quaternion[pieces.Length];

			// Store default initial values
			for (int i = 0; i < pieces.Length; i++)
			{
				piecesPositions[i] = pieces[i].transform.localPosition;
				piecesRotations[i] = pieces[i].transform.rotation;
			}
		}

		private void OnEnable()
		{
			// Apply angle offset because of the dumb 3D modeler...
			transform.Rotate(Vector3.forward * zAngleOffset);

			// Set the default transform properties
			for (int i = 0; i < pieces.Length; i++)
			{
				pieces[i].transform.localPosition = piecesPositions[i];
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

				// Apply angular velocity
				pieces[i].angularVelocity = angularVelocity * (Random.Range(0, 2) == 0 ? 1f : -1f);
			}
		}
	}
}
