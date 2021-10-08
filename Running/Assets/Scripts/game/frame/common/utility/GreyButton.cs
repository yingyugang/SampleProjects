using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GreyButton : SwitchButton
{
	public Button button;

	override public void SetActive (bool isActive)
	{
		base.SetActive (isActive);
		button.enabled = isActive;
	}
}
