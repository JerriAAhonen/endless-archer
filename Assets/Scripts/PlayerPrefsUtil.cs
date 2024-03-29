using System;
using UnityEngine;

public static class PlayerPrefsUtil
{
	public static event Action PlayerPrefsUpdated;

	// Settings
	private const string SFXVolumeKey = "Settings_SFXVolume";
	private const string MusicVolumeKey = "Settings_MusicVolume";
	private const string AimSensitivityKey = "Settings_AimSensitivity";

	// Highscores
	private const string HighscoreScoreKey = "Highscore_Score";
	private const string HighscoreTimeKey = "Highscore_Time";

	#region Settings

	public static float SFXVolume
	{
		get => Get(SFXVolumeKey, 0.75f);
		set => Set(SFXVolumeKey, value);
	}

	public static float MusicVolume
	{
		get => Get(MusicVolumeKey, 0.75f);
		set => Set(MusicVolumeKey, value);
	}

	public static float AimSensitivity
	{
		get => Get(AimSensitivityKey, 0.5f);
		set => Set(AimSensitivityKey, value);
	}

	#endregion

	#region Highscore

	public static int Highscore
	{
		get => Get(HighscoreScoreKey, 0);
		set => Set(HighscoreScoreKey, value);
	}

	public static float HighscoreTime
	{
		get => Get(HighscoreTimeKey, 0f);
		set => Set(HighscoreTimeKey, value);
	}

	#endregion

	private static string Get(string key, string defaultValue) => PlayerPrefs.GetString(key, defaultValue);
	private static int Get(string key, int defaultValue) => PlayerPrefs.GetInt(key, defaultValue);
	private static float Get(string key, float defaultValue) => PlayerPrefs.GetFloat(key, defaultValue);

	private static void Set(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
		PlayerPrefsUpdated?.Invoke();
	}

	private static void Set(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
		PlayerPrefsUpdated?.Invoke();
	}

	private static void Set(string key, float value) 
	{
		PlayerPrefs.SetFloat(key, value);
		PlayerPrefsUpdated?.Invoke();
	}
}
