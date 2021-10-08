using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using home;

public class OtherSetting : ViewWithDefaultAction
{
	public HeaderMediator header;

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
				send (2);
			}
		};
	}
}
