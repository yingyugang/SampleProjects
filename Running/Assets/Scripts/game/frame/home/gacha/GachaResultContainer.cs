using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GachaResultContainer : ViewWithDefaultAction
{
	public GameObject normalButtonGroup;
	public GameObject nextButtonGroup;
	public GameObject gachaButton1;
	public GameObject gachaButton2;
	public GameObject gachaButton3;
	public RectTransform contentRectTransform;
	public GameObject additionalButtonGroup;
	public GameObject additionNextButtonGroup;
	public GameObject originalButtonGroup;
	public GameObject originalNextButtonGroup;

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
				Send (8);
			}
		};
	}

}
