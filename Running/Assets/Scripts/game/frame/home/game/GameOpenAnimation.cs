using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using home;

public class GameOpenAnimation : ViewWithDefaultAction
{
	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			}
		};
	}
}
