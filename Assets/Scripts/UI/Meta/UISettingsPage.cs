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
		HideAll();
		content.Open();
		gameObject.SetActive(true);

		new Sequence.Builder()
			.AddElement(0, () => Show(content.gameObject))
			.AddElement(100, () => Show(buttonReturn.gameObject))
			.Start();

		void HideAll()
		{
			content.transform.localScale = Vector3.zero;
			buttonReturn.transform.localScale = Vector3.zero;
		}

		void Show(GameObject go)
		{
			UIAnimUtil.AnimateScale(go, Vector3.one, LeanTweenType.easeOutBack, 0.1f);
		}
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
