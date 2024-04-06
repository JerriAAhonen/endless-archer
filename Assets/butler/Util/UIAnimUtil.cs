using System;
using TMPro;
using UnityEngine;

public static class UIAnimUtil
{
	public static void AnimateNumber(TextMeshProUGUI label, int from, int to, float duration, Action onComplete = null)
	{
		LeanTween.value(0, 1, duration)
			.setOnUpdate(v =>
			{
				var val = Mathf.RoundToInt(Mathf.Lerp(from, to, v));
				label.text = val.ToCustomString();
			})
			.setOnComplete(() => onComplete?.Invoke());
	}

	public static void AnimateScale(Transform t, AnimationCurve curve, float duration, Action onComplete = null)
	{
		LeanTween.value(0, 1, duration)
			.setOnUpdate(v =>
			{
				var scale = curve.Evaluate(v);
				t.transform.localScale = Vector3.one * scale;
			})
			.setOnComplete(() => onComplete?.Invoke());
	}
}
