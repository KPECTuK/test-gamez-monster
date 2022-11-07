using System;
using UnityEngine;

public static class MainState
{
	private const string KEY_ATTEMPTS_S = "attempts";

	public static GameMachineBase GameMachine { get; private set; }
	public static UIMachine UIMachine { get; }

	public static TimeSpan TimeLastSpent { get; private set; }
	public static int Attempts { get; private set; }

	private static IActive _current;

	static MainState()
	{
		LoadState();
		UIMachine = new UIMachine();
		UIMachine.LobbyShow();
		SetLogicMedium();
	}

	public static void GameStart()
	{
		_current = GameMachine;
		UIMachine.GameStart();
		GameMachine.GameStart();
	}

	public static void GameStop()
	{
		TimeLastSpent = GameMachine.TimeSpent;
		Attempts += 1;
		_current = UIMachine;
		GameMachine.GameStop();
		UIMachine.GameStop();
	}

	public static void LobbyShow()
	{
		_current = UIMachine;
		UIMachine.LobbyShow();
	}

	public static void SetLogicLow()
	{
		GameMachine = new GameMachineLow();
	}

	public static void SetLogicMedium()
	{
		GameMachine = new GameMachineMedium();
	}

	public static void SetLogicHigh()
	{
		GameMachine = new GameMachineHigh();
	}

	public static bool IsInput()
	{
		return Input.anyKey;
	}

	public static void Update()
	{
		_current?.Update();
	}

	public static void SaveState()
	{
		PlayerPrefs.SetInt(KEY_ATTEMPTS_S, Attempts);
		PlayerPrefs.Save();
	}

	public static void LoadState()
	{
		Attempts = PlayerPrefs.GetInt(KEY_ATTEMPTS_S, 0);
	}

	public static Frustum ToFrustum(this Camera source, Plane on)
	{
		var ray1 = source.ViewportPointToRay(new Vector3(1f, 1f));
		if(!on.Raycast(ray1, out var dist1))
		{
			return new Frustum();
		}

		var ray2 = source.ViewportPointToRay(new Vector3(1f, 0f));
		if(!on.Raycast(ray2, out var dist2))
		{
			return new Frustum();
		}

		var ray3 = source.ViewportPointToRay(new Vector3(0f, 0f));
		if(!on.Raycast(ray3, out var dist3))
		{
			return new Frustum();
		}

		var ray4 = source.ViewportPointToRay(new Vector3(0f, 1f));
		if(!on.Raycast(ray4, out var dist4))
		{
			return new Frustum();
		}

		var vur = ray1.GetPoint(dist1);
		var vdr = ray2.GetPoint(dist2);
		var vdl = ray3.GetPoint(dist3);
		var vul = ray4.GetPoint(dist4);

		var camera = source.transform;
		var forward = camera.forward;

		return new Frustum
		{
			Source = camera.position,
			Forward = forward,

			VUR = vur,
			VDR = vdr,
			VDL = vdl,
			VUL = vul,

			// TODO: assure towards camera
			Screen = on,

			Left = new Plane(vdl, vul, vul + forward),
			Right = new Plane(vur, vdr, vur + forward),
			Up = new Plane(vul, vur, vur + forward),
			Down = new Plane(vdr, vdl, vdr + forward),
		};
	}

	public static Frustum Inflate(this Frustum source, float offset)
	{
		Vector3 Offset(Vector3 current, Vector3 previous, Vector3 next)
		{
			var planePrev = new Plane(previous, current, current + source.Screen.normal);
			var planeNext = new Plane(current, next, next + source.Screen.normal);
			var dirPrev = (current - previous).normalized;
			var dirNext = (current - next).normalized;
			var compNext = dirPrev * (offset / Vector3.Dot(planeNext.normal, dirPrev));
			var compPrev = dirNext * (offset / Vector3.Dot(planePrev.normal, dirNext));
			return current + compPrev + compNext;
		}

		var vur = Offset(source.VUR, source.VUL, source.VDR);
		var vdr = Offset(source.VDR, source.VUR, source.VDL);
		var vdl = Offset(source.VDL, source.VDR, source.VUL);
		var vul = Offset(source.VUL, source.VDL, source.VUR);

		return new Frustum
		{
			Source = source.Source,
			Forward = source.Forward,

			VUR = vur,
			VDR = vdr,
			VDL = vdl,
			VUL = vul,

			Screen = source.Screen,

			Left = new Plane(vdl, vul, vul + source.Forward),
			Right = new Plane(vur, vdr, vur + source.Forward),
			Up = new Plane(vul, vur, vur + source.Forward),
			Down = new Plane(vdr, vdl, vdr + source.Forward),
		};
	}

	public static Vector3 VectorTop(this Frustum source)
	{
		return source.VUR - source.VUL;
	}

	public static Vector3 VectorBottom(this Frustum source)
	{
		return source.VDR - source.VDL;
	}

	public static Vector3 SpawnPointBoundTop(this CompCameraGame source)
	{
		var ray = new Ray(
			source.RectScreen.VUL,
			source.RectScreen.VUR - source.RectScreen.VUL);
		source.RectOuter.Right.Raycast(ray, out var dist);
		return ray.GetPoint(dist);
	}

	public static Vector3 SpawnPointBoundBottom(this CompCameraGame source)
	{
		var ray = new Ray(
			source.RectScreen.VDL,
			source.RectScreen.VDR - source.RectScreen.VDL);
		source.RectOuter.Right.Raycast(ray, out var dist);
		return ray.GetPoint(dist);
	}

	public static Vector3 SpawnPointPawn(this CompCameraGame source)
	{
		var originU = Vector3.Lerp(source.RectInner.VUL, source.RectInner.VUR, .5f);
		var originD = Vector3.Lerp(source.RectInner.VDL, source.RectInner.VDR, .5f);
		var origin = (originU + originD) * .5f;
		return origin;
	}

	public static Vector3 SpawnPointObstacle(this CompCameraGame source, float ratio)
	{
		var originL = Vector3.Lerp(source.RectInner.VUL, source.RectInner.VDL, ratio);
		var originR = Vector3.Lerp(source.RectInner.VUR, source.RectInner.VDR, ratio);
		var ray = new Ray(originL, originR - originL);
		source.RectOuter.Right.Raycast(ray, out var dist);
		return ray.GetPoint(dist);
	}

	public static bool IsOut(this Frustum source, Vector3 pos)
	{
		return !source.Left.GetSide(pos);
	}

	public static void Draw(this Vector3 position, Quaternion rotation, Color color, float size = .1f)
	{
		const float DIM = 1.0f;
		const float AXIS_GAP = 0.7f;

		Debug.DrawLine(position + rotation * Vector3.up * size * AXIS_GAP, position + rotation * Vector3.up * size, Color.green * DIM);
		Debug.DrawLine(position, position + rotation * Vector3.up * size * AXIS_GAP, color * DIM);
		Debug.DrawLine(position, position - rotation * Vector3.up * size, color * DIM);

		Debug.DrawLine(position + rotation * Vector3.right * size * AXIS_GAP, position + rotation * Vector3.right * size, Color.red * DIM);
		Debug.DrawLine(position, position + rotation * Vector3.right * size * AXIS_GAP, color * DIM);
		Debug.DrawLine(position, position - rotation * Vector3.right * size, color * DIM);

		Debug.DrawLine(position + rotation * Vector3.forward * size * AXIS_GAP, position + rotation * Vector3.forward * size, Color.blue * DIM);
		Debug.DrawLine(position, position + rotation * Vector3.forward * size * AXIS_GAP, color * DIM);
		Debug.DrawLine(position, position - rotation * Vector3.forward * size, color * DIM);
	}

	public static void Draw(this Plane source, Color color)
	{
		var origin = source.normal * -source.distance;
		const float STEP = Mathf.PI / 12f;
		var unit = Quaternion.FromToRotation(Vector3.up, source.normal) * Vector3.left;
		for(var delta = 0f; delta < 2f * Mathf.PI; delta += Mathf.PI / 12f)
		{
			Debug.DrawLine(
				origin + Quaternion.AngleAxis(delta * Mathf.Rad2Deg, source.normal) * unit,
				origin + Quaternion.AngleAxis((delta + STEP) * Mathf.Rad2Deg, source.normal) * unit,
				color);
		}

		Debug.DrawLine(origin, origin + source.normal * 1.2f, Color.cyan);
		origin.Draw(Quaternion.LookRotation(source.normal), color);
	}

	public static void Draw(this Ray source, Color color)
	{
		Debug.DrawLine(source.origin, source.origin + source.direction, color);
	}

	public static void Draw(this Frustum source, Color color, bool extended = true)
	{
		Debug.DrawLine(source.VDL, source.VDR, color);
		Debug.DrawLine(source.VDR, source.VUR, color);
		Debug.DrawLine(source.VUR, source.VUL, color);
		Debug.DrawLine(source.VUL, source.VDL, color);

		if(extended)
		{
			source.Down.Draw(Color.green * .5f);
			source.Up.Draw(Color.green);
			source.Left.Draw(Color.red * .5f);
			source.Right.Draw(Color.red);

			source.Screen.Draw(Color.grey);
		}
	}

	public static void Draw(this Vector3[] source, Color color)
	{
		for(var index = 0; index < source.Length; index++)
		{
			var next = (index + 1) % source.Length;
			Debug.DrawLine(source[index], source[next], color);
		}
	}

	public static void ClearSet<T>(ref T[] set) where T : Component
	{
		if(set == null)
		{
			return;
		}

		for(var index = 0; index < set.Length; index++)
		{
			if(set[index] != null)
			{
				UnityEngine.Object.Destroy(set[index].gameObject);
			}
		}

		set = null;
	}

	public static void ClearObject<T>(ref T @object) where T : Component
	{
		if(@object)
		{
			UnityEngine.Object.Destroy(@object.gameObject);
			@object = null;
		}
	}
}
