using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(CanvasRenderer))]
public sealed class GradientGraphic : MaskableGraphic
{
	[SerializeField] private Gradient gradient = new();
	[SerializeField] private bool flip;
	[SerializeField] private bool vertical;
	[SerializeField] private Sprite whiteTexture;

	private List<float> segments;

	public override Texture mainTexture => whiteTexture != null ? whiteTexture.texture : base.mainTexture;

	protected override void OnDestroy()
	{
		PoolUtil.Release(ref segments);
		base.OnDestroy();
	}

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		if (segments == null || segments.Count == 0)
			CalculateSegments();

		vh.Clear();

		if (segments.Count < 2)
		{
			Debug.LogWarning("Gradient does not have enough keys.");
			return;
		}

		CreateVertices(vh);
	}

	public void SetDirty()
	{
		CalculateSegments();
		base.SetVerticesDirty();
	}

	public Gradient Gradient
	{
		get => gradient;
		set
		{
			gradient = value;
			SetDirty();
		}
	}

	public void SetColors(IReadOnlyList<Color> colors)
	{
		if (colors == null || colors.Count == 0)
		{
			Debug.LogError("Invalid colors.");
			return;
		}

		var colorKeys = new GradientColorKey[colors.Count];
		var alphaKeys = new GradientAlphaKey[colors.Count];
		for (int i = 0; i < colors.Count; i++)
		{
			float t = i / (float) (colors.Count - 1);
			Color c = colors[i];
			colorKeys[i] = new GradientColorKey { color = c, time = t };
			alphaKeys[i] = new GradientAlphaKey { alpha = c.a, time = t };
		}

		gradient = new Gradient();
		gradient.SetKeys(colorKeys, alphaKeys);
		SetDirty();
	}

	public void SetColors(IReadOnlyList<(float, Color)> colors)
	{
		if (colors == null || colors.Count == 0)
		{
			Debug.LogError("Invalid colors.");
			return;
		}

		var colorKeys = new GradientColorKey[colors.Count];
		var alphaKeys = new GradientAlphaKey[colors.Count];
		for (int i = 0; i < colors.Count; i++)
		{
			float t = colors[i].Item1;
			Color c = colors[i].Item2;
			colorKeys[i] = new GradientColorKey { color = c, time = t };
			alphaKeys[i] = new GradientAlphaKey { alpha = c.a, time = t };
		}

		gradient = new Gradient();
		gradient.SetKeys(colorKeys, alphaKeys);
		SetDirty();
	}

	private void CalculateSegments()
	{
		GradientColorKey[] colorKeys = gradient.colorKeys;
		GradientAlphaKey[] alphaKeys = gradient.alphaKeys;

		PoolUtil.Get(ref segments);
		var minCapacity = Mathf.Max(colorKeys.Length, alphaKeys.Length);
		if (segments.Capacity < minCapacity)
			segments.Capacity = minCapacity;

		for (int i = 0; i < colorKeys.Length; ++i)
			TryAdd(colorKeys[i].time);
		for (int i = 0; i < alphaKeys.Length; ++i)
			TryAdd(alphaKeys[i].time);

		segments.Sort();

		void TryAdd(float time)
		{
			foreach (float t in segments)
				if (Mathf.Approximately(t, time))
					return;
			segments.Add(time);
		}
	}

	private void CreateVertices(VertexHelper vh)
	{
		Rect r = GetPixelAdjustedRect();
		int prevVertexIndex = 0;

		float refMin = vertical ? r.yMin : r.xMin;
		float refMax = vertical ? r.yMax : r.xMax;
		float staticMin = vertical ? r.xMin : r.yMin;
		float staticMax = vertical ? r.xMax : r.yMax;

		for (int i = 0; i < segments.Count; ++i)
		{
			float phase = segments[i];
			float v = refMin + (refMax - refMin) * phase;
			Color c = gradient.Evaluate(flip ? 1 - phase : phase) * color;

			Vector3 vec = vertical ? new Vector3(staticMin, v) : new Vector3(v, staticMin);
			Vector2 uv = vertical ? new Vector2(0, phase) : new Vector2(phase, 0);
			vh.AddVert(vec, c, uv);
			int nextVertexIndex = vh.currentVertCount - 1;

			vec = vertical ? new Vector3(staticMax, v) : new Vector3(v, staticMax);
			uv = vertical ? new Vector2(1, phase) : new Vector2(phase, 1);
			vh.AddVert(vec, c, uv);

			if (i > 0)
			{
				if (vertical)
				{
					vh.AddTriangle(prevVertexIndex, prevVertexIndex + 2, prevVertexIndex + 1);
					vh.AddTriangle(prevVertexIndex + 1, prevVertexIndex + 2, prevVertexIndex + 3);
				}
				else
				{
					vh.AddTriangle(prevVertexIndex, prevVertexIndex + 1, prevVertexIndex + 3);
					vh.AddTriangle(prevVertexIndex + 3, prevVertexIndex + 2, prevVertexIndex);
				}

				prevVertexIndex = nextVertexIndex;
			}
		}
	}
}
