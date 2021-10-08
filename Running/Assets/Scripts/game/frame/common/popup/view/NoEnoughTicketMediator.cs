using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class NoEnoughTicketMediator : PopupContentActivityMediator
{
	private NoEnoughTicket noEnoughTicket;
	public UnityAction unityAction;

	private void OnEnable ()
	{
		noEnoughTicket = popupContent as NoEnoughTicket;
		noEnoughTicket.own.text = objectList [0].ToString ();
		noEnoughTicket.cost.text = objectList [1].ToString ();
	}

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				if (unityAction != null) {
					unityAction ();
				}
				ClosePopup ();
			}
		};
	}
}
