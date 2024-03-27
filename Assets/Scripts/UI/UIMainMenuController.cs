using System.Collections.Generic;
using UnityEngine;

public enum MenuPageType
{
	Main,
	Settings
}

public class UIMainMenuController : MonoBehaviour
{
	[SerializeField] private GameObject root;
	[Header("Pages")]
	[SerializeField] private UIMainPage mainPage;
	[SerializeField] private UISettingsPage settingsPage;

	private readonly Stack<UIMenuPageBase> openPages = new();

	private void Awake()
	{
		mainPage.Init(this);
		settingsPage.Init(this);
	}

	/// <summary>
	/// Toggles main menu completely on or off
	/// </summary>
	public void SetVisible(bool visible)
	{
		root.SetActive(visible);
	}

	/// <summary>
	/// Close all open pages, open page of newType
	/// </summary>
	/// <param name="newType">Type of page to open</param>
	public void SwitchPage(MenuPageType newType)
	{
		while (openPages.Count > 0)
		{
			openPages.Peek().Exit();
			openPages.Peek().SetOpendedAdditively(false);
			openPages.Pop();
		}

		var page = GetPage(newType);
		page.Enter();
		openPages.Push(page);
	}

	/// <summary>
	/// Open a page additively, leaving already open pages open
	/// </summary>
	/// <param name="additiveType">Type of page to open additively</param>
	public void OpenAdditivePage(MenuPageType additiveType)
	{
		var page = GetPage(additiveType);
		openPages.Push(page);
		page.Enter();
		page.SetOpendedAdditively(true);
	}

	private UIMenuPageBase GetPage(MenuPageType type)
	{
		return type switch
		{
			MenuPageType.Main => mainPage,
			MenuPageType.Settings => settingsPage,
			_ => throw new System.NotImplementedException(),
		};
	}
}
