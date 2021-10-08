using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class QA : PopupContentWithDefaultAction
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
