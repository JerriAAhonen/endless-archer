using System.Collections.Generic;

public static class LevelSegmentTypeUtil
{
	public static bool IsWall(LevelSegmentType type) => type 
		is LevelSegmentType.WallSingle 
		or LevelSegmentType.WallDouble 
		or LevelSegmentType.WallTriple;

	public static bool IsHole(LevelSegmentType type) => type 
		is LevelSegmentType.HoleSingle 
		or LevelSegmentType.WallDouble 
		or LevelSegmentType.HoleTriple;

	public static LevelSegmentType[] AllWalls() => new LevelSegmentType[] 
	{ 
		LevelSegmentType.WallSingle, 
		LevelSegmentType.WallDouble, 
		LevelSegmentType.WallTriple 
	};

	public static LevelSegmentType[] AllHoles() => new LevelSegmentType[]
	{
		LevelSegmentType.HoleSingle, 
		LevelSegmentType.HoleDouble, 
		LevelSegmentType.HoleTriple
	};

	public static List<LevelSegmentType> GetFilterForNextType(LevelSegmentType lastType)
	{
		var filter = new List<LevelSegmentType> { LevelSegmentType.None };

		if (IsWall(lastType))
		{
			filter.AddRange(AllWalls());
			filter.AddRange(AllHoles());
			return filter;
		}

		if (IsHole(lastType))
		{
			filter.AddRange(AllHoles());
			filter.AddRange(AllWalls());
			return filter;
		}	

		return filter;
	}
}
