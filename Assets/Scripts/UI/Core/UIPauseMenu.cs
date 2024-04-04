using UnityEngine;
using UnityEngine.UI;

public class UIPauseMenu : MonoBehaviour
{
	[SerializeField] private GameObject hideContainer;
	[SerializeField] private Button buttonCorner;
	[SerializeField] private Button buttonResume;
	[SerializeField] private Button buttonMainMenu;
	[SerializeField] private UISettingsContent settingsContent;

	public static bool GamePaused { get; private set; }

	private void Awake()
	{
		buttonCorner.onClick.AddListener(OnPause);
		buttonResume.onClick.AddListener(OnPause);
		buttonMainMenu.onClick.AddListener(OnMainMenu);
		hideContainer.SetActive(false);
	}

	private void Update()
	{
		// TODO Switch to input manager events
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			OnPause();
		}
	}

	private void OnPause()
	{
		GamePaused = !GamePaused;
		CursorController.OnPause(GamePaused);
		EventBus<Event_PauseGame>.Raise(new Event_PauseGame { pause = GamePaused });

		hideContainer.SetActive(GamePaused);

		if (GamePaused)
			settingsContent.Open();
		else
			settingsContent.Close();
	}

	private void OnMainMenu()
	{
		OnPause();
		GameManager.Instance.LevelController.GameOver();
	}
}
