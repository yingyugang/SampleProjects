using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class PrintAgainNoEnough : PopupContentWithDefaultAction
{
	public Text title;
	public Text own;
	public Text cost;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			}
		};
	}
}
