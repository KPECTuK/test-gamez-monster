using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CompCameraGame : MonoBehaviour
{
	[NonReorderable]
	public Frustum RectInner;
	[NonReorderable]
	public Frustum RectScreen;
	[NonReorderable]
	public Frustum RectOuter;

	private void Awake()
	{
		var camera = GetComponent<Camera>();
		var plane = new Plane(Vector3.right, Vector3.zero, Vector3.up);
		RectScreen = camera.ToFrustum(plane);
		RectInner = RectScreen.Inflate(-1f);
		RectOuter = RectScreen.Inflate(1f);
	}

	#if UNITY_EDITOR
	public void OnDrawGizmos()
	{
		if(Application.isPlaying)
		{
			RectInner.Draw(Color.blue, false);
			RectScreen.Draw(Color.magenta, false);
			RectOuter.Draw(Color.blue, false);
		}
	}
	#endif
}
