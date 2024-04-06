using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UIRainbowText : MonoBehaviour
{
	[SerializeField] private float speed = 1f;

	private TextMeshProUGUI text;
	private Color defaultColor;
	private float hue = 0f;
	private bool animate;

	private void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
		defaultColor = text.color;
	}

	public void Animate()
	{
		animate = true;
		StartCoroutine(RainbowAnimation());
	}

	public void Stop()
	{
		animate = false;
		text.color = defaultColor;
	}

	IEnumerator RainbowAnimation()
	{
		while (animate)
		{
			hue += speed * Time.deltaTime;

			// Convert the hue to a color
			Color color = Color.HSVToRGB(hue % 1f, 1f, 1f);

			text.color = color;

			yield return null;
		}
	}
}
