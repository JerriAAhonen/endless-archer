using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FloatingTextController : MonoBehaviour
{
	[SerializeField] private FloatingText floatingTextPrefab;
	[SerializeField] private float showTextForDuration = 2f;

	private readonly List<FloatingText> activeTexts = new();
	private IObjectPool<FloatingText> pool;
	private Transform cameraTransform;

	public void Init(Transform cameraTransform)
	{
		this.cameraTransform = cameraTransform;
	}

	private void Awake()
	{
		pool = new ObjectPool<FloatingText>(
			() => Instantiate(floatingTextPrefab),
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

	public void ShowText(Vector3 pos, string text, Color color)
	{
		var t = pool.Get();
		t.transform.position = pos;
		t.SetText(text, color, cameraTransform);
	}
}
