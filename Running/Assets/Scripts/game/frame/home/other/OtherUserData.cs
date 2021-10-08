using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

public class OtherUserData : ViewWithDefaultAction
{
	public Text coin;
	public Text freeCoin;
	public Text cardNumber;
	public Text missionNumber;
	public Text pointNumber;
	public OtherGameDataMediator otherGameDataMediator;
	public Transform container;
	public RectTransform iconContainer;
	public OtherGameIcon instantiation;
	public Text originalPoint;
	public Text originalTicket;
	public Text eventItem2;
	public Text eventItem3;
	public Text eventItem5;

	public List<Text> recycleList;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			}
		};
	}
}
