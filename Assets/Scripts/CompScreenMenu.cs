using UnityEngine;
using UnityEngine.UI;

public class CompScreenMenu : MonoBehaviour
{
	private void Awake()
	{
		var button = GetComponentInChildren<Button>();
		button.onClick.AddListener(Main.GameStart);
	}
}
