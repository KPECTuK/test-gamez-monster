using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public struct Frustum
{
	public Vector3 Source;
	public Vector3 Forward;

	public Vector3 VUR;
	public Vector3 VDR;
	public Vector3 VDL;
	public Vector3 VUL;

	public Plane Screen;
	public Plane Left;
	public Plane Right;
	public Plane Up;
	public Plane Down;
}
