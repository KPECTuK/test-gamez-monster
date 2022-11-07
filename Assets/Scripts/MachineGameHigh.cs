using UnityEngine;

public sealed class MachineGameHigh : MachineGameBase
{
	public override bool IsHigh => true;

	protected override void UpdateSpeed()
	{
		SpeedH = Vector3.right * 3f;
		var vector = Main.IsInput() ? Vector3.up : Vector3.down;
		SpeedV = vector * (2f + Mathf.Floor((float)TimeSpent.TotalSeconds / 15f));
	}

	protected override bool TrySpawnObstacle(out float positionRel)
	{
		const float SPAWN_EVERY_F = 2f;
		positionRel = Random.value;

		if(Distance < CompBlock.AABB_DIMENSIONS_F * SPAWN_EVERY_F)
		{
			return false;
		}

		Distance = 0f;
		return true;
	}
}
