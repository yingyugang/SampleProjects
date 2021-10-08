using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class OriginalExchangeMediator : PopupContentMediator
{
	private OriginalExchange originalExchange;
	public UnityAction unityAction;

	private void OnEnable ()
	{
		originalExchange = popupContent as OriginalExchange;
		originalExchange.originalField.text = objectList [0].ToString ();

		originalExchange.ticketNumber.text = string.Format ("{0}{1}", objectList [1].ToString (), LanguageJP.M);
		originalExchange.card.sprite = (Sprite)objectList [2];
		originalExchange.cardNameField.text = objectList [3].ToString ();
		originalExchange.mask.SetActive (!(bool)objectList [4]);
		originalExchange.yesButton.enabled = (bool)objectList [4];
	}

	protected override void OKButtonOnClickHandler ()
	{
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
		if (ok != null) {
			ok ();
		}
		ClosePopup ();
	}

	protected override void YesButtonOnClickHandler ()
	{
		base.YesButtonOnClickHandler ();
		if (unityAction != null) {
			unityAction ();
		}
	}
}
