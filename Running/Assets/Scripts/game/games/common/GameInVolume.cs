using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GameInVolume : Volume
{
	override protected void BackHandler ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			}
		};
	}
}
