using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu()]
public class LevelSegmentDatabase : ScriptableObject
{
	[SerializeField] private List<LevelSegment> levelSegments;

	public LevelSegment GetRandom()
	{
		var randIndex = UnityEngine.Random.Range(0, levelSegments.Count);
		return levelSegments[randIndex];
	}

	public LevelSegment Get(LevelSegmentType type)
	{
		return levelSegments.Find(x => x.Type == type);
	}

#if UNITY_EDITOR
	[Button("Refresh")]
	public void EDITOR_GetAllSegments()
	{
		levelSegments.Clear();

		var guids = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/Prefabs/LevelSegments" });

		foreach (var guid in guids)
		{
			var path = AssetDatabase.GUIDToAssetPath(guid);
			GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
			LevelSegment ls = go.GetComponent<LevelSegment>();
			levelSegments.Add(ls);
		}
	}
#endif
}
