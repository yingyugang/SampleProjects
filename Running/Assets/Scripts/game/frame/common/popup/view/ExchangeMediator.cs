using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ExchangeMediator : PopupContentMediator
{
	private Exchange exchange;
	public UnityAction unityAction;

	private void OnEnable ()
	{
		exchange = popupContent as Exchange;
		exchange.coinField.text = objectList[0].ToString ();
		exchange.ticketField.text = objectList[1].ToString ();

		exchange.coinNumber.text = string.Format ("{0}{1}", objectList[2].ToString (), LanguageJP.M);
		exchange.ticketNumber.text = string.Format ("{0}{1}", objectList[3].ToString (), LanguageJP.M);
	}

	protected override void OKButtonOnClickHandler ()
	{
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
		if (ok != null)
		{
			ok ();
		}
		ClosePopup ();
	}

	protected override void YesButtonOnClickHandler ()
	{
		base.YesButtonOnClickHandler ();
		if (unityAction != null)
		{
			unityAction ();
		}
	}
}
