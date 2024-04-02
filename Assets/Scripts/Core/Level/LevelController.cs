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

	private ScoreController scoreController;

	public LevelState State { get; private set; }
	public ScoreController Score => scoreController;
	public FloatingTextController FloatingText => floatingTextController;

	private void Start()
	{
		State = LevelState.Idle;

		playerController.Init(this);
		segmentController.Init(this);
		floatingTextController.Init(playerController.CameraTransform);
		uiCoreController.SetVisible(false);
	}

	public void StartLevel()
	{
		if (State == LevelState.OnGoing)
			return;

		State = LevelState.OnGoing;

		scoreController = new ScoreController(defaultComboDecayDuration);
		scoreController.Score.AddListener(OnAddScore);

		segmentController.OnStartLevel();
		playerController.OnStartLevel();
		uiCoreController.SetVisible(true);

		void OnAddScore(int newScore)
		{
			uiCoreController.Score.SetScore(newScore);
		}
	}

	public void GameOver()
	{
		if (State == LevelState.Idle)
			return;

		State = LevelState.Idle;

		playerController.OnGameOver();
		segmentController.OnGameOver();
		uiCoreController.SetVisible(false);
		CheckHighscore();
		GameManager.Instance.OnGameOver();
		
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
