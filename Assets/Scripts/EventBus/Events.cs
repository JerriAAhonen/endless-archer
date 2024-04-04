public struct Event_PauseGame : IEvent
{
	public bool pause;
}

public struct Event_RotateLevel : IEvent
{
	public bool clockwise;
}