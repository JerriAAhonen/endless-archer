using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
	[SerializeField] private UIMainMenuController mainMenuController;
	[SerializeField] private LevelController levelController;

	public void OpenMenu()
	{
		mainMenuController.SetVisible(true);
		mainMenuController.SwitchPage(MenuPageType.Main);
	}

	public void StartLevel()
	{
		mainMenuController.SetVisible(false);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
