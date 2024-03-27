using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CachedWait
{
	private static readonly Dictionary<float, WaitForSeconds> yielders = new(100);

	public static WaitForSeconds ForSeconds(float time)
	{
		if (!yielders.ContainsKey(time))
			yielders.Add(time, new WaitForSeconds(time));

		return yielders[time];
	}
}

public static class WaitForUtil
{
	public static IEnumerator RealSeconds(float seconds)
	{
		float timeRemaining = seconds;

		while (timeRemaining > 0)
		{
			timeRemaining -= Time.unscaledDeltaTime;
			yield return null;
		}
	}
}