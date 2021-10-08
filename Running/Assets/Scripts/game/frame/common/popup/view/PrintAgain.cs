using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class PrintAgain : PopupContentWithDefaultAction
{
	public Text title;
	public Text own;
	public Text cost;
	public Text text;
	public GameObject additionalText;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			}
		};
	}
}
