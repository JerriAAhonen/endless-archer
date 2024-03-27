using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCanvas : MonoBehaviour
{
	[SerializeField] private CanvasGroup cg;
	[SerializeField] private Image fill;
	[SerializeField] private TextMeshProUGUI infoText;

	private void Awake() => SetVisible(false, 0f);

	public void SetVisible(bool visible, float animDur)
	{
		if (animDur <= 0f)
		{
			cg.alpha = visible ? 1f : 0f;
			cg.interactable = visible;
			cg.blocksRaycasts = visible;
			return;
		}

		LeanTween.value(visible ? 0f : 1f, visible ? 1f : 0f, animDur).setOnUpdate(v => cg.alpha = v)
			.setOnComplete(() =>
			{
				cg.interactable = visible;
				cg.blocksRaycasts = visible;
			});
	}

	public void SetFill(float amount)
	{
		fill.fillAmount = amount;
	}

	public void SetInfoText(string text)
	{
		infoText.text = text;
	}
}
