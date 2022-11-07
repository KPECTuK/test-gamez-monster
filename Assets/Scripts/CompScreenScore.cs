using UnityEngine;
using UnityEngine.UI;

public class CompScreenScore : MonoBehaviour
{
	public Text TextDuration;
	public Text TextAttempts;

	private void Awake()
	{
		var buttons = GetComponentsInChildren<Button>();
		for(var index = 0; index < buttons.Length; index++)
		{
			var button = buttons[index];
			if(button.name.Contains("retry"))
			{
				button.onClick.AddListener(MainState.GameStart);
			}
			if(button.name.Contains("change"))
			{
				button.onClick.AddListener(MainState.LobbyShow);
			}
		}
	}

	public void OnEnable()
	{
		TextAttempts.text = $"Всего попыток: {MainState.Attempts}";
		TextDuration.text = $"Продолжительность попытки: {MainState.TimeLastSpent}";
	}
}
