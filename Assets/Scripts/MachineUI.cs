public sealed class MachineUI : IModelActive
{
	public bool IsScreenMenu { get; private set; }
	public bool IsScreenScore { get; private set; }

	public void GameStart()
	{
		IsScreenMenu = false;
		IsScreenScore = false;
	}

	public void GameStop()
	{
		IsScreenMenu = false;
		IsScreenScore = true;
	}

	public void LobbyShow()
	{
		IsScreenMenu = true;
		IsScreenScore = false;
	}

	public void Update() { }
}
