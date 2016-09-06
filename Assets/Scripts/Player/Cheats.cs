using UnityEngine;

public class Cheats : MonoBehaviour
{
	private PlayerController playerController;

	// Use this for initialization
	void Awake()
	{
		playerController = FindObjectOfType<PlayerController>();
	}

	// Update is called once per frame
	void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			playerController.Heal();
		}
	}
#endif
}
