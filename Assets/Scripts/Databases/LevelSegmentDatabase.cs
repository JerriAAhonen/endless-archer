using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelSegmentType
{
	None,
	Default,
	Wall,
	Hole,
}

[CreateAssetMenu()]
public class LevelSegmentDatabase : ScriptableObject
{
	[Serializable]
	public class LevelSegmentMap
	{
		public LevelSegmentType type;
		public GameObject prefab;
	}

	[SerializeField] private List<LevelSegmentMap> levelSegments;

	public GameObject GetRandom()
	{
		var randIndex = UnityEngine.Random.Range(0, levelSegments.Count);
		return levelSegments[randIndex].prefab;
	}

	public GameObject Get(LevelSegmentType type)
	{
		return levelSegments.Find(x => x.type == type).prefab;
	}
}
