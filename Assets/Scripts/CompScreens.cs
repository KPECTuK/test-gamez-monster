using UnityEngine;

public sealed class CompScreens : MonoBehaviour
{
	public MonoBehaviour ScreenMenu;
	public MonoBehaviour ScreenScore;

	//private bool _screenStateMenu;
	//private bool _screenStateScore;

	//private void Awake()
	//{
	//	_screenStateScore = Main.MachineUI.IsScreenScore;
	//	ScreenScore.gameObject.SetActive(_screenStateScore);
	//	_screenStateMenu = Main.MachineUI.IsScreenMenu;
	//	ScreenMenu.gameObject.SetActive(_screenStateMenu);
	//}

	//private void LateUpdate()
	//{
	//	if(_screenStateScore != Main.MachineUI.IsScreenScore)
	//	{
	//		_screenStateScore = Main.MachineUI.IsScreenScore;
	//		ScreenScore.gameObject.SetActive(_screenStateScore);
	//	}

	//	if(_screenStateMenu != Main.MachineUI.IsScreenMenu)
	//	{
	//		_screenStateMenu = Main.MachineUI.IsScreenMenu;
	//		ScreenMenu.gameObject.SetActive(_screenStateMenu);
	//	}
	//}

	private void LateUpdate()
	{
		ScreenScore.gameObject.SetActive(Main.MachineUI.IsScreenScore);
		ScreenMenu.gameObject.SetActive(Main.MachineUI.IsScreenMenu);
	}
}
