using System;
using UnityEngine;

public abstract class MachineGameBase : IModelActive
{
	public virtual bool IsLow => false;
	public virtual bool IsMedium => false;
	public virtual bool IsHigh => false;

	public TimeSpan TimeSpent { get; private set; }

	protected float Distance;
	protected Vector3 SpeedV;
	protected Vector3 SpeedH;

	private CompGame _game;

	protected abstract void UpdateSpeed();
	protected abstract bool TrySpawnObstacle(out float positionRel);

	public void GameStart()
	{
		_game = UnityEngine.Object.FindObjectOfType<CompGame>();
		_game.LoadScene();
	}

	public void GameStop()
	{
		if(_game)
		{
			_game.UnloadScene();
		}

		_game = null;
	}

	public void Update()
	{
		// TODO: get camera plane desc

		TimeSpent += TimeSpan.FromSeconds(Time.deltaTime);

		UpdateSpeed();
		Distance += SpeedH.magnitude * Time.deltaTime;

		if(TrySpawnObstacle(out var positionRel))
		{
			_game.SpawnObstacle(positionRel);
		}

		_game.RollBoard(SpeedH);
		_game.RollPawn(SpeedV);
	}
}
