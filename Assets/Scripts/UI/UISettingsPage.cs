using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsPage : UIMenuPageBase
{
	[SerializeField] private Slider sfxVolumeSlider;
	[SerializeField] private Slider musicVolumeSlider;
	[SerializeField] private Slider aimSensitivitySlider;
	[SerializeField] private Button buttonReturn;

	private void Awake()
	{
		buttonReturn.onClick.AddListener(OnReturn);
	}

	#region Menu Page

	public override void Enter()
	{
		sfxVolumeSlider.value = PlayerPrefsUtil.SFXVolume;
		musicVolumeSlider.value = PlayerPrefsUtil.MusicVolume;
		aimSensitivitySlider.value = PlayerPrefsUtil.AimSensitivity;

		gameObject.SetActive(true);
	}

	public override void Exit()
	{
		gameObject.SetActive(false);

		PlayerPrefsUtil.SFXVolume = sfxVolumeSlider.value;
		PlayerPrefsUtil.MusicVolume = musicVolumeSlider.value;
		PlayerPrefsUtil.AimSensitivity = aimSensitivitySlider.value;
	}

	#endregion

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

	#region Button Callbacks

	private void OnReturn()
	{
		controller.SwitchPage(MenuPageType.Main);
	}

	#endregion
}
