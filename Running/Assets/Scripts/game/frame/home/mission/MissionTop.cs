using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class MissionTop : ViewWithDefaultAction
{
	public Text number;
	public GreyButton greyButton;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			},
			() => {
				Send (1);
			}
		};
	}
}
