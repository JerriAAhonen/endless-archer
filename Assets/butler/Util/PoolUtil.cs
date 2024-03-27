using System.Collections.Generic;
using UnityEngine.Pool;

public static class PoolUtil
{
	public static void Get<T>(ref List<T> list)
	{
		if (list == null)
			list = ListPool<T>.Get();
		else
			list.Clear();
	}

	public static void Release<T>(ref List<T> list)
	{
		if (list != null)
		{
			ListPool<T>.Release(list);
			list = null;
		}
	}

	public static void Get<K, V>(ref Dictionary<K, V> dict)
	{
		if (dict == null)
			dict = DictionaryPool<K, V>.Get();
		else
			dict.Clear();
	}

	public static void Release<K, V>(ref Dictionary<K, V> dict)
	{
		if (dict != null)
		{
			DictionaryPool<K, V>.Release(dict);
			dict = null;
		}
	}
}