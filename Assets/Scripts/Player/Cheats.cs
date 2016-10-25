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
			Camera.main.gameObject.GetComponent<CameraShake>().StartShake
			(
				Random.Range(0.5f, 2f),
				Random.Range(10f, 15f),
				Random.Range(0.2f, 0.5f),
				Random.Range(1f, 2f)
			);
		}
	}
#endif
}
