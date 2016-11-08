using UnityEngine;

public class Disabler : MonoBehaviour
{
	public float delay;

	private void OnEnable()
	{
		Invoke("Disable", delay);
	}

	private void Disable()
	{
		gameObject.SetActive(false);
	}
}
