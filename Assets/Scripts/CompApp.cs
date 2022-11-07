using UnityEngine;

public class CompApp : MonoBehaviour
{
	private void Update()
	{
		MainState.Update();
	}

	private void OnDestroy()
	{
		MainState.SaveState();
	}
}
