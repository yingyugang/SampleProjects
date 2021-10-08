using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class SelectFilter : PopupContentWithDefaultAction
{
	public List<Toggle> toggleList1;
	public List<Toggle> toggleList2;
	public List<Toggle> toggleList3;
	[HideInInspector]
	public List<Toggle> toggleList;
	public Text text1;
	public Text text2;

	protected override void AddActions ()
	{
		toggleList.AddRange (toggleList1);
		toggleList.AddRange (toggleList2);
		toggleList.AddRange (toggleList3);
		AddEvents ();
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			},
			() => {
				Send (1);
			}
		};
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		RemoveEvents ();
	}

	public void AddEvents ()
	{
		int length = toggleList.Count;
		for (int i = 0; i < length; i++) {
			toggleList [i].onValueChanged.AddListener (ToggleArrayOnValueChangedHandler);
		}
	}

	public void RemoveEvents ()
	{
		int length = toggleList.Count;
		for (int i = 0; i < length; i++) {
			toggleList [i].onValueChanged.RemoveListener (ToggleArrayOnValueChangedHandler);
		}
	}


	private void ToggleArrayOnValueChangedHandler (bool isSelected)
	{
		Send (2);
	}
}
