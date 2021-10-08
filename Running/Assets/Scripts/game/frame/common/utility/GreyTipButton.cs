using UnityEngine;
using System.Collections;

public class GreyTipButton : GreyButton
{
	public GameObject tip;

	public void SetActive (bool isButtonActive, bool isTopActive)
	{
		base.SetActive (isButtonActive);
		tip.SetActive (isTopActive);
	}
}
