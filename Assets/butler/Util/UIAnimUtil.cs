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

	/// <summary>
	/// Animate the local scale of the transform 't'
	/// </summary>
	/// <param name="t">Transform to scale</param>
	/// <param name="curve">Evaluated from 0...1, determines the scale</param>
	/// <param name="duration"></param>
	/// <param name="onComplete"></param>
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

	/// <summary>
	/// Animate the go from current scale to scale 'to'
	/// </summary>
	/// <param name="go"></param>
	/// <param name="to"></param>
	/// <param name="ease"></param>
	/// <param name="duration"></param>
	/// <param name="onComplete"></param>
	public static void AnimateScale(GameObject go, Vector3 to, LeanTweenType ease, float duration, Action onComplete = null)
	{
		LeanTween.scale(go, to, duration)
			.setEase(ease)
			.setOnComplete(() => onComplete?.Invoke());
	}
}
