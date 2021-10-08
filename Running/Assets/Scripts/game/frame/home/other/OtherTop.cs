using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class OtherTop : ViewWithDefaultAction
{
	public GreyTipButton greyTipButton;

	public void SetEventButtonActive (bool isButtonActive, bool isTipActive)
	{
		greyTipButton.SetActive (isButtonActive, isTipActive);
	}

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			},
			() => {
				Send (1);
			},
			() => {
				Send (2);
			},
			() => {
				Send (3);
			},
			() => {
				Send (4);
			},
			() => {
				Send (5);
			},
			() => {
				Send (6);
			},
			() => {
				Send (7);
			},
			() => {
				Send (9);
			},
			() => {
				Send (10);
			}
		};
	}
}
