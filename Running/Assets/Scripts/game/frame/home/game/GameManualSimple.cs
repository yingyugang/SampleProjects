using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GameManualSimple : GameManualCommon
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
