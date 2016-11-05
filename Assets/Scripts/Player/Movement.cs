using UnityEngine;

public class Movement : MonoBehaviour
{
	public static float speedMultiplier;

	protected void Awake()
	{
		speedMultiplier = 1f;
	}
}
