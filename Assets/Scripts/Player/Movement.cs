using UnityEngine;

public class Movement : MonoBehaviour
{
	[Header("Speed Parameters")]
	[Range(0f, 1f)]
	public float aditionalSpeedPercent;
	public Vector2 minSpeed;
	public Vector2 maxSpeed;
	public float maxSpeedDistance;
	public static float speedMultiplier;

	protected Vector2 speed;

	private Vector2 speedDifference;

	protected void Awake()
	{
		speedMultiplier = 1f;
		speedDifference = new Vector2
		(
			maxSpeed.x - minSpeed.x,
			maxSpeed.y - minSpeed.y
		);
	}

	// Return the current movement speed with applied multiplier for both axes
	protected Vector2 GetCurrentSpeed()
	{
		return new Vector2
		(
			Mathf.Clamp
			(
				(minSpeed.x + (transform.position.y / maxSpeedDistance) * speedDifference.x) -
				(1 - PlayerController.health / PlayerController.maximumHealth) * (aditionalSpeedPercent * speedDifference.x),
				minSpeed.x,
				maxSpeed.x
			),
			Mathf.Clamp
			(
				(minSpeed.y + (transform.position.y / maxSpeedDistance) * speedDifference.y) -
				(1 - PlayerController.health / PlayerController.maximumHealth) * (aditionalSpeedPercent * speedDifference.y),
				minSpeed.y,
				maxSpeed.y
			)
		) * speedMultiplier;
	}
}
