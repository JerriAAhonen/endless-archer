using UnityEngine;

public class LevelController : MonoBehaviour
{
	[Header("Controllers")]
	[SerializeField] private LevelSegmentController segmentController;
	[SerializeField] private PlayerController playerController;
	[SerializeField] private UIFloatingTextController floatingTextController;
	[Header("UI")]
	[SerializeField] private UICoreController uiCoreController;
	[Header("SFX")]
	[SerializeField] private AudioEvent gameOverSFX;
	[SerializeField] private AudioEvent gainPointsSFX;
	[Header("Settings")]
	// Option to add challenge game modes where combos decay faster etc..
	[SerializeField] private float defaultComboDecayDuration = 4f;

	private ScoreController scoreController;

	private void Start()
	{
		GlobalGameState.SetGameOngoing(false);

		playerController.Init(this);
		segmentController.Init(this);
		uiCoreController.SetVisible(false);
	}

	private void Update()
	{
		if (!GlobalGameState.GameOngoing) return;
		if (GlobalGameState.Paused) return;

		scoreController?.Update(Time.deltaTime);
	}

	public void StartLevel()
	{
		if (GlobalGameState.GameOngoing)
			return;

		GlobalGameState.SetGameOngoing(true);

		scoreController = new ScoreController(defaultComboDecayDuration);
		scoreController.Score.AddListener(OnScoreChanged);
		scoreController.Combo.AddListener(OnComboChanged);

		segmentController.OnStartLevel();
		playerController.OnStartLevel();
		uiCoreController.OnStartLevel();

		void OnScoreChanged(int newScore)
		{
			uiCoreController.Score.SetScore(newScore);
		}

		void OnComboChanged(int newCombo)
		{
			uiCoreController.Score.SetCombo(newCombo);
		}
	}

	public void GameOver()
	{
		if (!GlobalGameState.GameOngoing) 
			return;

		GlobalGameState.SetGameOngoing(false);
		CursorController.OnGameOver();

		AudioManager.Instance.SetMusicLowpass(true);
		AudioManager.Instance.PlayOnce(gameOverSFX);
		
		EventBus<Event_LevelEnded>.Raise(new Event_LevelEnded());

		scoreController.OnGameOver(out var newHighscore);
		playerController.OnGameOver();

		GameManager.Instance.ShowLoadingCanvas(true, () =>
		{
			segmentController.OnGameOver();
			uiCoreController.OnGameOver();
			uiCoreController.GameOver.Show(scoreController.Score, PlayerPrefsUtil.Highscore, newHighscore, OnContinue);

			GameManager.Instance.ShowLoadingCanvas(false, null);
		});

		void OnContinue()
		{
			uiCoreController.SetVisible(false);
			AudioManager.Instance.SetMusicLowpass(false);
			GameManager.Instance.OnContinueAfterGameOver();
		}
	}

	public struct ScoreArgs
	{
		public int amount;
		public Vector3 targetPosition;
		public Vector3 floatingTextPos;

		/// <param name="amount">Score amount</param>
		/// <param name="targetPosition">World position of the shooting target</param>
		/// <param name="floatingTextPos">World position of the ref pos for floating text</param>
		public ScoreArgs(int amount, Vector3 targetPosition, Vector3 floatingTextPos)
		{
			this.amount = amount;
			this.targetPosition = targetPosition;
			this.floatingTextPos = floatingTextPos;
		}
	}

	public void AddScore(ScoreArgs args)
	{
		// Vector3.Distance can be slow, but here it's not used that often
		var distToPlayer = Vector3.Distance(playerController.transform.position, args.targetPosition);
		
		var multiplier = 1f;
		if (distToPlayer > 10)
			multiplier = distToPlayer / 10f;

		var finalScore = Mathf.RoundToInt(args.amount * multiplier);
		var floatingText = $"+ {finalScore}";

		//Debug.Log($"dist:{distToPlayer}, score:{args.amount}, final:{finalScore}");

		scoreController.Add(finalScore);
		uiCoreController.ShowFloatingText(args.floatingTextPos, floatingText);
		AudioManager.Instance.PlayOnce(gainPointsSFX);
	}
}
