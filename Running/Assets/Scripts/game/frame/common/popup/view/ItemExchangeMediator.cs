using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ItemExchangeMediator : PopupContentMediator
{
	private ItemExchange itemExchange;
	public UnityAction unityAction;

	private void OnEnable ()
	{
		itemExchange = popupContent as ItemExchange;
		itemExchange.coinField.text = objectList[0].ToString ();
		itemExchange.ticketField.text = objectList[1].ToString ();

		itemExchange.coinNumber.text = string.Format ("{0}{1}", objectList[2].ToString (), LanguageJP.M);
		itemExchange.ticketNumber.text = string.Format ("{0}{1}", objectList[3].ToString (), LanguageJP.M);
		itemExchange.icon.sprite = itemExchange.image.sprite = (Sprite)objectList[4];

		itemExchange.title.text = string.Format (LanguageJP.ITEM_EXCHANGE_TITLE, objectList[5].ToString ());
		itemExchange.description.text = string.Format (LanguageJP.ITEM_EXCHANGE_DESCRIPTION, objectList[5].ToString ());
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
