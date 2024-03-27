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
		gameObject.SetActive(true);
	}

	public override void Exit()
	{
		gameObject.SetActive(false);
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

	private void SetSFXVolume()
	{

	}

	private void SetMusicVolume()
	{

	}

	private void SetAimSensitivity()
	{

	}

	private float ConvertToLog(float sliderValue) => Mathf.Log10(sliderValue) * 20f;
}
