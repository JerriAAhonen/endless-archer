using UnityEngine;

public static class PlayerPrefsUtil
{
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
		get => PlayerPrefs.GetFloat(SFXVolumeKey, 0.75f);
		set => PlayerPrefs.SetFloat(SFXVolumeKey, value);
	}

	public static float MusicVolume
	{
		get => PlayerPrefs.GetFloat(MusicVolumeKey, 0.75f);
		set => PlayerPrefs.SetFloat(MusicVolumeKey, value);
	}

	public static float AimSensitivity
	{
		get => PlayerPrefs.GetFloat(AimSensitivityKey, 0.5f);
		set => PlayerPrefs.SetFloat(AimSensitivityKey, value);
	}

	#endregion

	#region Highscore

	public static int Highscore
	{
		get => PlayerPrefs.GetInt(HighscoreScoreKey, 0);
		set => PlayerPrefs.SetInt(HighscoreScoreKey, value);
	}

	public static float HighscoreTime
	{
		get => PlayerPrefs.GetFloat(HighscoreTimeKey, 0f);
		set => PlayerPrefs.SetFloat(HighscoreTimeKey, value);
	}

	#endregion
}
