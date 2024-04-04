using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
	[SerializeField] private UIMainMenuController mainMenuController;
	[SerializeField] private LevelController levelController;

	public LevelController LevelController => levelController;

	public void OpenMainMenu()
	{
		mainMenuController.SwitchPage(MenuPageType.Main);

		CursorController.OnOpenMenu();
	}

	public void StartLevel()
	{
		mainMenuController.Close();
		levelController.StartLevel();

		CursorController.OnStartLevel();
	}

	public void OnGameOver()
	{
		OpenMainMenu();
	}

	public void QuitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
		Application.Quit();
	}
}
