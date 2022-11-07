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
			toggle.isOn = Main.MachineGame.IsLow;
		}

		if(name.Contains("med"))
		{
			if(callback != null)
			{
				throw new Exception("name parser fault: dif definition");
			}

			callback = OnToggledMedium;
			toggle.isOn = Main.MachineGame.IsMedium;
		}

		if(name.Contains("high"))
		{
			if(callback != null)
			{
				throw new Exception("name parser fault: dif definition");
			}

			callback = OnToggledHigh;
			toggle.isOn = Main.MachineGame.IsHigh;
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
			Main.SetLogicLow();
		}
	}

	private static void OnToggledMedium(bool state)
	{
		if(state)
		{
			Main.SetLogicMedium();
		}
	}

	private static void OnToggledHigh(bool state)
	{
		if(state)
		{
			Main.SetLogicHigh();
		}
	}
}
