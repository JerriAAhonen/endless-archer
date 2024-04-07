using TMPro;
using UnityEngine;

public class UIFloatingText : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI label;
	[SerializeField] private RectTransform rt;
	[Header("Appear Animation")]
	[SerializeField] private float appearAnimDur = 0.3f;
	[SerializeField] private LeanTweenType appearAnimEase = LeanTweenType.easeOutBack;
	[Header("Disappear Animation")]
	[SerializeField] private float disappearAnimDur = 0.3f;
	[SerializeField] private LeanTweenType disappearAnimEase = LeanTweenType.easeOutCubic;

	public float ShowTime { get; private set; }

	private void OnEnable()
	{
		ShowTime = Time.time;
	}

	private void OnDisable()
	{
		ShowTime = 0f;
	}

	public void Show(Vector3 position, string text)
	{
		transform.position = position;
		label.text = text;

		transform.localScale = Vector3.zero;
		UIAnimUtil.AnimateScale(gameObject, Vector3.one, appearAnimEase, appearAnimDur, () =>
		{
			UIAnimUtil.AnimateScale(gameObject, Vector3.zero, disappearAnimEase, disappearAnimDur);
		});
	}
}
