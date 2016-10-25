using UnityEngine;

public class Cheats : MonoBehaviour
{
#if UNITY_EDITOR
	private PlayerController playerController;

	// Use this for initialization
	void Awake()
	{
		playerController = FindObjectOfType<PlayerController>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			playerController.Heal();
		}

		// Simulate camera shake
		if (Input.GetButtonUp("Submit"))
		{
			Camera.main.gameObject.GetComponent<PerlinShake>().PlayShake();
		}
	}
#endif
}
