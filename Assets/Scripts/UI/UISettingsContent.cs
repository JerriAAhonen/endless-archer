using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsContent : MonoBehaviour
{
	[SerializeField] private Slider sfxVolumeSlider;
	[SerializeField] private Slider musicVolumeSlider;
	[SerializeField] private Slider aimSensitivitySlider;

	public void Open()
	{
		sfxVolumeSlider.value = PlayerPrefsUtil.SFXVolume;
		musicVolumeSlider.value = PlayerPrefsUtil.MusicVolume;
		aimSensitivitySlider.value = PlayerPrefsUtil.AimSensitivity;
	}

	public void Close() 
	{
		PlayerPrefsUtil.SFXVolume = sfxVolumeSlider.value;
		PlayerPrefsUtil.MusicVolume = musicVolumeSlider.value;
		PlayerPrefsUtil.AimSensitivity = aimSensitivitySlider.value;
	}

	#region Unity Callbacks

	public void OnSFXVolume()
	{
		AudioManager.Instance.SetSFXVolume(sfxVolumeSlider.value);
	}

	public void OnMusicVolume()
	{
		AudioManager.Instance.SetMusicVolume(musicVolumeSlider.value);
	}

	public void OnAimSensitivity()
	{
	}

	#endregion
}
