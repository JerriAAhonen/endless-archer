using TMPro;
using UnityEngine;

public class UIScoreView : UICoreViewBase
{
	[Space]
	[SerializeField] private TextMeshProUGUI scoreLabel;
	[SerializeField] private TextMeshProUGUI comboLabel;
	[SerializeField] private float scoreAnimDur;
	[SerializeField] private float scaleAnimDur;
	[SerializeField] private AnimationCurve scaleAnimCurve;

	private int currentScore;

	private void Awake()
	{
		scoreLabel.text = "0";
		comboLabel.text = "x0";
	}

	public void SetScore(int score)
	{
		UIAnimUtil.AnimateNumber(scoreLabel, currentScore, score, scoreAnimDur);
		UIAnimUtil.AnimateScale(scoreLabel.transform, scaleAnimCurve, scaleAnimDur);
		currentScore = score;
	}

	public void SetCombo(int combo)
	{
		comboLabel.text = $"x{combo}";
		UIAnimUtil.AnimateScale(comboLabel.transform, scaleAnimCurve, scaleAnimDur);
	}
}
