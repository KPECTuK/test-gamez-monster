using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class CompToggle : MonoBehaviour
{
	private void Awake()
	{
		var toggle = GetComponent<Toggle>();

		UnityAction<bool> callback = null;

		if(name.Contains("low"))
		{
			if(callback != null)
			{
				throw new Exception("name parser fault: dif definition");
			}

			callback = OnToggledLow;
			toggle.isOn = MainState.GameMachine.IsLow;
		}

		if(name.Contains("med"))
		{
			if(callback != null)
			{
				throw new Exception("name parser fault: dif definition");
			}

			callback = OnToggledMedium;
			toggle.isOn = MainState.GameMachine.IsMedium;
		}

		if(name.Contains("high"))
		{
			if(callback != null)
			{
				throw new Exception("name parser fault: dif definition");
			}

			callback = OnToggledHigh;
			toggle.isOn = MainState.GameMachine.IsHigh;
		}

		if(toggle.onValueChanged.GetPersistentEventCount() != 0)
		{
			throw new Exception("name parser fault: lots of listeners");
		}

		toggle.onValueChanged.AddListener(callback);
	}

	private static void OnToggledLow(bool state)
	{
		if(state)
		{
			MainState.SetLogicLow();
		}
	}

	private static void OnToggledMedium(bool state)
	{
		if(state)
		{
			MainState.SetLogicMedium();
		}
	}

	private static void OnToggledHigh(bool state)
	{
		if(state)
		{
			MainState.SetLogicHigh();
		}
	}
}
