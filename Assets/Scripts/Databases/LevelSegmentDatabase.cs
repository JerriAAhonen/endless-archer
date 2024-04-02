using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelSegmentType
{
	None = 0,
	Default = 1,
	WallSingle = 2,
	WallDouble = 3,
	WallTriple = 4,
	HoleSingle = 5,
	HoleDouble = 6,
	HoleTriple = 7,
}

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
}
