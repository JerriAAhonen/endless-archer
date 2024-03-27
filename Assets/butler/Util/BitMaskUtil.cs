using System;
using System.Collections.Generic;

public static class BitMaskUtil
{
	public static bool MaskContains(int mask, int value)
	{
		return ((value & mask) == value);
	}

	public static bool MaskContains<T>(T mask, T value)
		where T : IComparable, IFormattable, IConvertible
	{
		int intValue = Convert.ToInt32(value);
		int intMask = Convert.ToInt32(mask);
		return ((intValue & intMask) == intValue);
	}

	public static bool MaskContainsAny(int mask, int value)
	{
		return ((value & mask) != 0);
	}

	public static bool MaskContainsAny<T>(T mask, T value)
		where T : IComparable, IFormattable, IConvertible
	{
		int intValue = Convert.ToInt32(value);
		int intMask = Convert.ToInt32(mask);
		return ((intValue & intMask) != 0);
	}

	public static bool MaskContainsLayer(int mask, int layer)
	{
		return (mask & (1 << layer)) != 0;
	}

	public static bool MaskDoesNotContainLayer(int mask, int layer)
	{
		return (mask & (1 << layer)) == 0;
	}

	public static string ToString<T>(T mask)
		where T : IComparable, IFormattable, IConvertible
	{
		var found = new List<string>();
		foreach (T value in Enum.GetValues(typeof(T)))
		{
			if (MaskContains(mask, value))
			{
				found.Add(value.ToString());
			}
		}
		return $"({string.Join("|", found)})";
	}
}