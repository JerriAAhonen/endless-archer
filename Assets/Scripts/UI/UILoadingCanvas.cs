using System;
using UnityEngine;

/// <summary>
/// A very lightweight loading canvas that just smooths out the transitions between game states
/// </summary>
public class UILoadingCanvas : MonoBehaviour
{
	[SerializeField] private CanvasGroup cg;

	private int? tweenId;

	public void SetVisible(bool visible, Action onComplete)
	{
		var from = cg.alpha;
		var to = visible ? 1f : 0f;
		var transitionDur = 0.3f;

		cg.blocksRaycasts = visible;
		cg.interactable = visible;

		if (tweenId.HasValue)
			LeanTween.cancel(tweenId.Value, true);
		
		tweenId = LeanTween.value(from, to, transitionDur)
			.setOnUpdate(v => cg.alpha = v)
			.setOnComplete(() =>
			{
				tweenId = null;
				onComplete?.Invoke();
			})
			.uniqueId;
	}
}
