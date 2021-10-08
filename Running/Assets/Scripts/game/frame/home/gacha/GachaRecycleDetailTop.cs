using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GachaRecycleDetailTop : ViewWithDefaultAction
{
	public Text titleField;
	public Image imageField;
	public Text probabilityField;
	public Transform container;
	public GachaRecycleItem instantiation;
	public GameObject ad;

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

	public void Show (string title, Sprite sprite, string probability)
	{
		titleField.text = title;
		imageField.sprite = sprite;
		probabilityField.text = probability;
	}
}
