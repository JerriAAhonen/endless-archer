using System.Collections.Generic;
using UnityEngine;

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
