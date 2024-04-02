using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelState
{
	Idle,
	OnGoing
}

public class LevelController : MonoBehaviour
{
	[SerializeField] private LevelSegmentController segmentController;
	[SerializeField] private PlayerController playerController;
	[SerializeField] private FloatingTextController floatingTextController;
	[SerializeField] private UICoreController uiCoreController;

	private readonly float defaultComboDecayDuration = 2f;

	private LevelState state;

	private ScoreController scoreController;

	public ScoreController Score => scoreController;
	public FloatingTextController FloatingText => floatingTextController;

	private void Start()
	{
		state = LevelState.Idle;

		playerController.Init(this);
		floatingTextController.Init(playerController.CameraTransform);
		uiCoreController.SetVisible(false);
	}

	public void StartLevel()
	{
		if (state == LevelState.OnGoing)
			return;

		state = LevelState.OnGoing;

		scoreController = new ScoreController(defaultComboDecayDuration);
		scoreController.Score.AddListener(OnAddScore);

		segmentController.StartLevel();
		playerController.OnStartLevel();
		uiCoreController.SetVisible(true);

		void OnAddScore(int newScore)
		{
			uiCoreController.Score.SetScore(newScore);
		}
	}

	public void GameOver()
	{
		if (state == LevelState.Idle)
			return;

		state = LevelState.Idle;

		playerController.OnLevelEnded();
		uiCoreController.SetVisible(false);
		CheckHighscore();
		GameManager.Instance.OnLevelEnded();
		
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
