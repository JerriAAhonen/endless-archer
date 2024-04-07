using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UIFloatingTextController : MonoBehaviour
{
	[SerializeField] private UIFloatingText floatingTextPrefab;
	[SerializeField] private float showTextForDuration = 2f;

	private readonly List<UIFloatingText> activeTexts = new();
	private IObjectPool<UIFloatingText> pool;

	private void Awake()
	{
		pool = new ObjectPool<UIFloatingText>(
			() => Instantiate(floatingTextPrefab, transform),
			t =>
			{
				t.gameObject.SetActive(true);
				activeTexts.Add(t);
			},
			t => 
			{
				t.gameObject.SetActive(false);
				activeTexts.Remove(t);
			},
			t => Destroy(t.gameObject));
	}

	private void LateUpdate()
	{
		foreach (var t in activeTexts)
		{
			if (Time.time - t.ShowTime >= showTextForDuration)
			{
				pool.Release(t);
				return;
			}
		}
	}

	public void ShowText(Vector3 position, string text)
	{
		var t = pool.Get();
		var screenPos = GameManager.Instance.MainCamera.WorldToScreenPoint(position);
		t.Show(screenPos, text);
	}
}
