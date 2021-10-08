using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

public class GachaRecycleTop : ViewWithDefaultAction
{
	public List<ButtonDetector> buttonDetectorList;
	public List<Text> ownFieldList;
	public List<Text> costFieldList;
	public List<Text> descriptionFieldList;
	public UnityAction<int> unityAction;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			}
		};
	}

	public override void AddEventListeners ()
	{
		base.AddEventListeners ();
		int length = buttonDetectorList.Count;
		for (int i = 0; i < length; i++) {
			buttonDetectorList [i].unityAction = ButtonDetectorHandler;
		}
	}

	private void ButtonDetectorHandler (ButtonDetector buttonDetector)
	{
		if (unityAction != null) {
			unityAction (buttonDetectorList.IndexOf (buttonDetector));
		}
	}

	public override void RemoveEventListeners ()
	{
		base.RemoveEventListeners ();
		int length = buttonDetectorList.Count;
		for (int i = 0; i < length; i++) {
			buttonDetectorList [i].unityAction = null;
		}
	}

	public void Show (List<string> ownList, List<string> costList, string[] descriptionArray)
	{
		int length = costList.Count;
		for (int i = 0; i < length; i++) {
			ownFieldList [i].text = ownList [i];
			costFieldList [i].text = costList [i];
		}

		int count = descriptionArray.Length;
		for (int i = 0; i < count; i++) {
			descriptionFieldList [i].text = descriptionArray [i];
		}
	}
}
