using UnityEngine;

public sealed class GameMachineLow : GameMachineBase
{
	public override bool IsLow => true;

	protected override void UpdateSpeed()
	{
		SpeedH = Vector3.right;
		var vector = MainState.IsInput() ? Vector3.up : Vector3.down;
		SpeedV = vector * (2f + Mathf.Floor((float)TimeSpent.TotalSeconds / 15f));
	}

	protected override bool TrySpawnObstacle(out float positionRel)
	{
		const float SPAWN_EVERY_F = 6f;
		positionRel = Random.value;

		if(Distance < CompBlock.AABB_DIMENSIONS_F * SPAWN_EVERY_F)
		{
			return false;
		}

		Distance = 0f;
		return true;
	}
}
