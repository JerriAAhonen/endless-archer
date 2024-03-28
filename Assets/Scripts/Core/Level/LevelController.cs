using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	[SerializeField] private LevelSegmentController segmentController;

	private readonly float defaultComboDecayDuration = 2f;

	private ScoreController scoreController;

	public ScoreController Score => scoreController;

	public void StartLevel()
	{
		scoreController = new ScoreController(defaultComboDecayDuration);
		segmentController.StartLevel();
	}

	public void GameOver()
	{
		CheckHighscore();
		
		void CheckHighscore()
		{
			var oldHighscore = PlayerPrefsUtil.Highscore;
			if (scoreController.Score > oldHighscore)
			{
				PlayerPrefsUtil.Highscore = scoreController.Score;
				PlayerPrefsUtil.HighscoreTime = scoreController.TimeElapsed;
			}
		}
	}
}
