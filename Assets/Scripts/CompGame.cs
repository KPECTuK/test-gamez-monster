using System;
using UnityEngine;

public class CompGame : MonoBehaviour
{
	public CompBlock ProtoBlock;
	public CompPawn ProtoPawn;

	private CompBlock[] _boundTop;
	private CompBlock[] _boundBottom;
	private CompBlock[] _obstacles;
	private CompPawn _pawn;

	private CompCameraGame _camGame;

	private void Awake()
	{
		_camGame = GetComponentInChildren<CompCameraGame>();
	}

	public void LoadScene()
	{
		{
			//! spawns top bound
			var size = Mathf.CeilToInt(_camGame.RectOuter.VectorTop().magnitude / CompBlock.AABB_DIMENSIONS_F);
			_boundTop = new CompBlock[size];
			var offsetCurrent = _camGame.RectScreen.VUL;
			var offsetStep = _camGame.RectOuter.VectorTop().normalized * CompBlock.AABB_DIMENSIONS_F;
			for(var index = 0; index < size; index++)
			{
				// TODO: align to vector
				var instance = Instantiate(ProtoBlock, offsetCurrent, Quaternion.identity, transform);
				instance.name = $"bound_top_{index:00}";
				instance.FrustumScreenVerticalPosNormalized = 1f;
				_boundTop[index] = instance;
				offsetCurrent += offsetStep;
			}
		}

		{
			//! spawns bottom bound
			var size = Mathf.CeilToInt(_camGame.RectOuter.VectorBottom().magnitude / CompBlock.AABB_DIMENSIONS_F);
			_boundBottom = new CompBlock[size];
			var offsetCurrent = _camGame.RectScreen.VDL;
			var offsetStep = _camGame.RectOuter.VectorBottom().normalized * CompBlock.AABB_DIMENSIONS_F;
			for(var index = 0; index < size; index++)
			{
				// TODO: align to vector
				var instance = Instantiate(ProtoBlock, offsetCurrent, Quaternion.identity, transform);
				instance.name = $"bound_bottom_{index:00}";
				instance.FrustumScreenVerticalPosNormalized = 0f;
				_boundBottom[index] = instance;
				offsetCurrent += offsetStep;
			}
		}

		//! create obstacles pool
		var max = _boundTop.Length > _boundBottom.Length
			? _boundBottom.Length
			: _boundTop.Length;
		_obstacles = new CompBlock[max];

		Debug.Log($"lines: (top: {_boundTop.Length}, bottom: {_boundBottom.Length})");

		//! create pawn
		_pawn = Instantiate(ProtoPawn, _camGame.SpawnPointPawn(), Quaternion.identity, transform);
	}

	public void UnloadScene()
	{
		MainState.ClearObject(ref _pawn);
		MainState.ClearSet(ref _boundTop);
		MainState.ClearSet(ref _boundBottom);
		MainState.ClearSet(ref _obstacles);
	}

	public void SpawnObstacle(float ratio)
	{
		CompBlock obstacle = null;
		for(var index = 0; index < _obstacles.Length; index++)
		{
			if(_obstacles[index] == null)
			{
				obstacle = Instantiate(ProtoBlock);
				_obstacles[index] = obstacle;
				break;
			}

			if(!_obstacles[index].gameObject.activeSelf)
			{
				obstacle = _obstacles[index];
				obstacle.gameObject.SetActive(true);
				break;
			}
		}

		if(obstacle == null)
		{
			throw new Exception("pool");
		}

		obstacle.transform.position = _camGame.SpawnPointObstacle(ratio);
		obstacle.FrustumScreenVerticalPosNormalized = ratio;
	}

	public void RollBoard(Vector3 speed)
	{
		var input = speed * Time.deltaTime;

		// move top bound
		var offsetTop = Vector3.ProjectOnPlane(input, _camGame.RectScreen.Up.normal);
		for(var index = 0; index < _boundTop.Length; index++)
		{
			_boundTop[index].transform.position -= offsetTop;
			if(_camGame.RectOuter.IsOut(_boundTop[index].transform.position))
			{
				// TODO: align to vector, use ratio stored in block
				_boundTop[index].transform.position = _camGame.SpawnPointBoundTop();
			}
		}

		// move bottom bound
		var offsetBottom = Vector3.ProjectOnPlane(input, _camGame.RectScreen.Down.normal);
		for(var index = 0; index < _boundBottom.Length; index++)
		{
			_boundBottom[index].transform.position -= offsetBottom;
			if(_camGame.RectOuter.IsOut(_boundBottom[index].transform.position))
			{
				// TODO: align to vector, use ratio stored in block
				_boundBottom[index].transform.position = _camGame.SpawnPointBoundBottom();
			}
		}

		// move obstacle
		for(var index = 0; index < _obstacles.Length; index++)
		{
			if(_obstacles[index] != null && _obstacles[index].gameObject.activeSelf)
			{
				_obstacles[index].transform.position -= Vector3.Lerp(
					offsetTop,
					offsetBottom,
					_obstacles[index].FrustumScreenVerticalPosNormalized);
				// TODO: align to vector
				if(_camGame.RectOuter.IsOut(_obstacles[index].transform.position))
				{
					_obstacles[index].gameObject.SetActive(false);
					//? sort by disabled and nulls
				}
			}
		}
	}

	public void RollPawn(Vector3 speed)
	{
		var offset = speed * Time.deltaTime;
		_pawn.transform.position += offset;
	}
}
