using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOverView : UICoreViewBase
{
	[Space]
	[SerializeField] private GameObject title;
	[SerializeField] private GameObject score;
	[SerializeField] private TextMeshProUGUI scoreAmount;
	[Space]
	[SerializeField] private GameObject highscore;
	[SerializeField] private TextMeshProUGUI highscoreAmount;
	[SerializeField] private UIRainbowText highscoreRainbow;
	[Space]
	[SerializeField] private Button buttonContinue;
	[Space]
	[SerializeField] private float scoreAnimDur = 1f;
	[SerializeField] private float newHighscoreAnimDurMultiplier = 1.5f;

	private Action continueCallback;

	private void Awake()
	{
		buttonContinue.onClick.AddListener(OnContinueClicked);
		SetVisible(false);
	}

	public void Show(int score, int highscore, bool isNewHighscore, Action onContinue)
	{
		HideAll();
		SetVisible(true);

		scoreAmount.text = "0";
		highscoreAmount.text = "0";

		new Sequence.Builder()
			.AddElement(0, () => Show(title))
			.AddElement(100, () => Show(this.score))
			.AddElement(100, () => Show(scoreAmount.gameObject))
			.AddElement(100, () => Show(this.highscore))
			.AddElement(100, () => Show(highscoreAmount.gameObject))
			.AddElement(100, () => Show(buttonContinue.gameObject))
			.Start();

		UIAnimUtil.AnimateNumber(scoreAmount, 0, score, scoreAnimDur);
		var highscoreAnimDur = isNewHighscore ? scoreAnimDur * newHighscoreAnimDurMultiplier : scoreAnimDur;
		UIAnimUtil.AnimateNumber(highscoreAmount, 0, highscore, highscoreAnimDur, () =>
		{
			if (isNewHighscore)
				highscoreRainbow.Animate();
		});

		continueCallback = onContinue;

		void HideAll()
		{
			title.transform.localScale = Vector3.zero;
			this.score.transform.localScale = Vector3.zero;
			scoreAmount.transform.localScale = Vector3.zero;
			this.highscore.transform.localScale = Vector3.zero;
			highscoreAmount.transform.localScale = Vector3.zero;
			buttonContinue.transform.localScale = Vector3.zero;
		}

		void Show(GameObject go)
		{
			UIAnimUtil.AnimateScale(go, Vector3.one, LeanTweenType.easeOutBack, 0.1f);
		}
	}

	private void OnContinueClicked()
	{
		continueCallback?.Invoke();
		continueCallback = null;
		SetVisible(false);
		highscoreRainbow.Stop();
	}
}
