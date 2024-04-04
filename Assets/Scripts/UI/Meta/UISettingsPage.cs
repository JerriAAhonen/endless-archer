using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsPage : UIMenuPageBase
{
	[SerializeField] private UISettingsContent content;
	[SerializeField] private Button buttonReturn;

	private void Awake()
	{
		buttonReturn.onClick.AddListener(OnReturn);
	}

	#region Menu Page

	public override void Enter()
	{
		content.Open();
		gameObject.SetActive(true);
	}

	public override void Exit()
	{
		content.Close();
		gameObject.SetActive(false);
	}

	#endregion

	private void OnReturn()
	{
		controller.SwitchPage(MenuPageType.Main);
	}
}
