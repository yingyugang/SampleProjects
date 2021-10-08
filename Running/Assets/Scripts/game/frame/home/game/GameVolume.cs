using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GameVolume : Volume
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
