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

	// Option to add challenge game modes where combos decay faster etc..
	private readonly float defaultComboDecayDuration = 2f;

	private ScoreController scoreController;

	public LevelState State { get; private set; }

	private void Start()
	{
		State = LevelState.Idle;

		playerController.Init(this);
		segmentController.Init(this);
		floatingTextController.Init(playerController.CameraTransform);
		uiCoreController.SetVisible(false);
	}

	private void Update()
	{
		if (State == LevelState.Idle)
			return;

		scoreController?.Update(Time.deltaTime);
	}

	public void StartLevel()
	{
		if (State == LevelState.OnGoing)
			return;

		State = LevelState.OnGoing;

		scoreController = new ScoreController(defaultComboDecayDuration);
		scoreController.Score.AddListener(OnScoreChanged);
		scoreController.Combo.AddListener(OnComboChanged);

		segmentController.OnStartLevel();
		playerController.OnStartLevel();
		uiCoreController.SetVisible(true);

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
		if (State == LevelState.Idle)
			return;

		State = LevelState.Idle;
		EventBus<Event_LevelEnded>.Raise(new Event_LevelEnded());

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

	public struct ScoreArgs
	{
		public int amount;
		public Vector3 targetPosition;

		public Vector3 floatingTextPos;
		public Color floatingTextColor;
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
		ShowFloatingText(args.floatingTextPos, floatingText, args.floatingTextColor);
	}

	private void ShowFloatingText(Vector3 pos, string text, Color color)
	{
		floatingTextController.ShowText(pos, text, color);
	}
}
