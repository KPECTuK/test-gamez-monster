using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CompPawn : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		MainState.GameStop();
	}
}
