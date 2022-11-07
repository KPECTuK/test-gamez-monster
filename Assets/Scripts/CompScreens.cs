using UnityEngine;

public sealed class CompScreens : MonoBehaviour
{
	public MonoBehaviour ScreenMenu;
	public MonoBehaviour ScreenScore;

	//private bool _screenStateMenu;
	//private bool _screenStateScore;

	//private void Awake()
	//{
	//	_screenStateScore = MainState.UIMachine.IsScreenScore;
	//	ScreenScore.gameObject.SetActive(_screenStateScore);
	//	_screenStateMenu = MainState.UIMachine.IsScreenMenu;
	//	ScreenMenu.gameObject.SetActive(_screenStateMenu);
	//}

	//private void LateUpdate()
	//{
	//	if(_screenStateScore != MainState.UIMachine.IsScreenScore)
	//	{
	//		_screenStateScore = MainState.UIMachine.IsScreenScore;
	//		ScreenScore.gameObject.SetActive(_screenStateScore);
	//	}

	//	if(_screenStateMenu != MainState.UIMachine.IsScreenMenu)
	//	{
	//		_screenStateMenu = MainState.UIMachine.IsScreenMenu;
	//		ScreenMenu.gameObject.SetActive(_screenStateMenu);
	//	}
	//}

	private void LateUpdate()
	{
		ScreenScore.gameObject.SetActive(MainState.UIMachine.IsScreenScore);
		ScreenMenu.gameObject.SetActive(MainState.UIMachine.IsScreenMenu);
	}
}
