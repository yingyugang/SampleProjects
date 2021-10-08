using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManualText : GameManual
{
	public Text textField;

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
