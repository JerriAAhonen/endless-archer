using UnityEditor;
using UnityEngine;

public static class EditorTools
{
	[MenuItem("Tools/UI/Anchors to Corners #a")]
	private static void AnchorsToCorners()
	{
		var t = Selection.activeTransform as RectTransform;
		var pt = Selection.activeTransform.parent as RectTransform;

		Undo.RecordObject(t, "Set anchors to corners");

		if (t == null || pt == null) 
			return;

		var newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
										t.anchorMin.y + t.offsetMin.y / pt.rect.height);
		var newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
										t.anchorMax.y + t.offsetMax.y / pt.rect.height);

		t.anchorMin = newAnchorsMin;
		t.anchorMax = newAnchorsMax;
		t.offsetMin = t.offsetMax = new Vector2(0f, 0f);
	}

	[MenuItem("Tools/UI/Corners to Anchors #s")]
	private static void CornersToAnchors()
	{
		var t = Selection.activeTransform as RectTransform;
		Undo.RecordObject(t, "Set corners to anchors");

		if (t == null) 
			return;

		t.offsetMin = t.offsetMax = new Vector2(0f, 0f);
	}

	[MenuItem("Tools/Cheats/Reset highscore")]
	public static void ResetHighscore()
	{
		PlayerPrefsUtil.Highscore = 0;
		PlayerPrefsUtil.HighscoreTime = 0;
	}
}
