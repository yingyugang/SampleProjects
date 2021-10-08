using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class OtherVolume : Volume
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
