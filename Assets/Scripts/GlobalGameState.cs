using System.Diagnostics;

public static class GlobalGameState
{
	public static bool Paused { get; private set; }
	public static bool GameOngoing { get; private set; }

	public static void SetGamePaused(bool paused)
	{
		Paused = paused;
		EventBus<Event_PauseGame>.Raise(new Event_PauseGame { pause = paused });
	}

	public static void SetGameOngoing(bool gameOngoing)
	{
		Debug.Assert(GameOngoing != gameOngoing, "Game state already set!");
		GameOngoing = gameOngoing;
	}
}
