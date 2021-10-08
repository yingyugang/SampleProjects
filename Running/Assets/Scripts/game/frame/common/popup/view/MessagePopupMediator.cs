using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class MessagePopupMediator : PopupContentMediator
{
	private MessagePopup messagePopup;

	private void OnEnable ()
	{
		messagePopup = popupContent as MessagePopup;
		messagePopup.title.text = objectList [0].ToString ();
		messagePopup.message.text = objectList [1].ToString ();
	}
}
