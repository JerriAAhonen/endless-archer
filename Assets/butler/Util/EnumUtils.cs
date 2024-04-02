using System;
using System.Linq;

public static class EnumUtils
{
	public static T GetRandom<T>(T[] exclude) where T : Enum
	{
		if (exclude == null || exclude.Length == 0)
			return GetRandom<T>();

		T result;
		do
		{
			var values = Enum.GetValues(typeof(T));
			var index = UnityEngine.Random.Range(0, values.Length);
			result = (T)values.GetValue(index);
		} while (exclude.Contains(result));
		
		return result;
	}

	public static T GetRandom<T>() where T : Enum
	{
		var values = Enum.GetValues(typeof(T));
		var index = UnityEngine.Random.Range(0, values.Length);
		return (T)values.GetValue(index);
	}
}