using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class PerformanceManager : MonoBehaviour
{
	[Header("Minimum performance index")]
	public float performanceFirstTestDelay;
	public float performanceTestDelay;

	private Bloom bloom;
	private BloomOptimized bloomOptimized;

	private static int lastFrameCount = 0;
	private static float lastUnscaledTime = 0f;

	// Use this for initialization
	private void Awake()
	{
		// Get the bloom references
		bloom = Camera.main.GetComponent<Bloom>();
		bloomOptimized = Camera.main.GetComponent<BloomOptimized>();

		// Enable the supported bloom
		if (bloom.enabled && !bloom.CheckResources())
		{
			bloom.enabled = false;
			bloomOptimized.enabled = true;
		}

		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
	}

	private void Start()
	{
		StartCoroutine(StartAutoFrameRate());
	}

	private IEnumerator StartAutoFrameRate()
	{
		yield return new WaitForSeconds(performanceFirstTestDelay);

		SetTargetFrameRate();
	}

	private void SetTargetFrameRate()
	{
		int averageFPS = (int)((Time.frameCount - lastFrameCount) / (Time.unscaledTime - lastUnscaledTime));
		Application.targetFrameRate = averageFPS > 45 ? 60 : 30;

		//int targetFPS;

		//targetFPS = (averageFPS / 10 + 1) * 10;
		//targetFPS = (averageFPS % 10 > 5) ? ((averageFPS / 10 + 1) * 10) : ((averageFPS / 10) * 10);

		//Application.targetFrameRate = Mathf.Clamp
		//(
		//	targetFPS,
		//	30,
		//	60
		//);
	}
}
