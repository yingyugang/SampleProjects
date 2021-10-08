using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using home;

public class CardTop : ViewWithDefaultAction
{
	public Image selector;
	public Text content;
	public HeaderMediator header;
	public UnityAction<Button> unityAction;
	public GameObject select;
	public List<Text> textList;

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
