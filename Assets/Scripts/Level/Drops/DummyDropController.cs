using UnityEngine;

public class DummyDropController : MonoBehaviour
{
	// Disable object when it is enabled, because it is used only to decrease spawn probability of other spawnable objects
	private void OnEnable()
	{
		gameObject.SetActive(false);
	}
}
