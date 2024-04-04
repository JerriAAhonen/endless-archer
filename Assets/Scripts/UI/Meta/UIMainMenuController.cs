using System.Collections.Generic;
using UnityEngine;

public enum MenuPageType
{
	None = 0,
	Main = 1,
	Settings = 2,
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

	public void SetActive(bool active)
	{
		root.SetActive(active);
	}

	/// <summary>
	/// Close all open pages, open page of newType
	/// </summary>
	/// <param name="newType">Type of page to open, None closes all pages</param>
	public void SwitchPage(MenuPageType newType)
	{
		while (openPages.Count > 0)
		{
			openPages.Peek().Exit();
			openPages.Peek().SetOpendedAdditively(false);
			openPages.Pop();
		}

		if (newType == MenuPageType.None)
			return;

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
		page.SetOpendedAdditively(true);
		page.Enter();
	}

	/// <summary>
	/// Closes the top page in the stack
	/// </summary>
	public void CloseTopPage()
	{
		if (openPages.Count == 0)
			return;

		openPages.Peek().Exit();
		openPages.Peek().SetOpendedAdditively(false);
		openPages.Pop();
	}

	private UIMenuPageBase GetPage(MenuPageType type)
	{
		return type switch
		{
			MenuPageType.None => null,
			MenuPageType.Main => mainPage,
			MenuPageType.Settings => settingsPage,
			_ => throw new System.NotImplementedException(),
		};
	}
}
