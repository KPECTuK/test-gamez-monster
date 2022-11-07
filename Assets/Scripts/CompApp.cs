using UnityEngine;

public class CompApp : MonoBehaviour
{
	private void Update()
	{
		Main.Update();
	}

	private void OnDestroy()
	{
		Main.SaveState();
	}
}
