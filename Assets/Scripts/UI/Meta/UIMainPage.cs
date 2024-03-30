using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainPage : UIMenuPageBase
{
	[Header("Buttons")]
	[SerializeField] private Button buttonPlay;
	[SerializeField] private Button buttonSettings;
	[SerializeField] private Button buttonQuit;

	private void Awake()
	{
		buttonPlay.onClick.AddListener(OnPlay);
		buttonSettings.onClick.AddListener(OnSettings);
		buttonQuit.onClick.AddListener(OnQuit);
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

	private void OnPlay()
	{
		GameManager.Instance.StartLevel();
	}

	private void OnSettings()
	{
		controller.SwitchPage(MenuPageType.Settings);
	}

	private void OnQuit()
	{
		GameManager.Instance.QuitGame();
	}
}
