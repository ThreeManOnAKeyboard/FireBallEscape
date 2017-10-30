using UnityEngine;
using UnityEngine.UI;
using _3rdParty;

namespace Managers
{
	public class ScoreManager : MonoBehaviour
	{
		public static ScoreManager instance;

		public Text currentScoreText;
		public Text finalScoreText;
		public Text highScoreText;
		public Text multiplierText;
		public float scoreRate;

		private float score;
		private int scoreMultiplier;

		private Vector3 previousPosition;
		private Transform playerTransform;

		// Use this for initialization
		private void Awake()
		{
			scoreMultiplier = 1;

			if (instance == null)
			{
				instance = this;
			}

			playerTransform = GameObject.FindWithTag(Tags.Player).transform;
		}

		// Update is called once per frame
		private void Update()
		{
			if (playerTransform != null)
			{
				// Update the score
				score += (playerTransform.position.y - previousPosition.y) * scoreMultiplier * scoreRate;
				currentScoreText.text = ((int)score).ToString();

				// Save previous position for next frame calculations
				previousPosition = playerTransform.position;
			}
		}

		public void AddScore(float scoreAmount)
		{
			score += scoreAmount * scoreMultiplier * scoreRate;
		}

		public void AddMultiplier()
		{
			scoreMultiplier *= 2;
			multiplierText.text = scoreMultiplier.ToString();
		}

		public void SubstractMultiplier()
		{
			if (scoreMultiplier != 1)
			{
				scoreMultiplier /= 2;
				multiplierText.text = scoreMultiplier.ToString();
			}
		}

		public void ProcessScore()
		{
			float currentHighScore = 0;

			if (PlayerPrefs.HasKey(PlayerPrefsKeys.Highscore))
			{
				currentHighScore = PlayerPrefs.GetFloat(PlayerPrefsKeys.Highscore);
			}

			if (score > currentHighScore)
			{
				PlayerPrefs.SetFloat(PlayerPrefsKeys.Highscore, score);
				currentHighScore = score;
			}

			finalScoreText.text = ((int)score).ToString();
			highScoreText.text = ((int)currentHighScore).ToString();
		}
	}
}
