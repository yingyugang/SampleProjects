using UnityEngine;
using System.Collections;

public class LifeChargeeMediator : PopupContentMediator
{
	private LifeChargee lifeChargee;

	protected override void YesButtonOnClickHandler ()
	{
		base.YesButtonOnClickHandler ();
		popupAction ();
	}

	protected override void OKButtonOnClickHandler ()
	{
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
		if (ok != null) {
			ok ();
		}
		ClosePopup ();
	}

	private void OnEnable ()
	{
		lifeChargee = popupContent as LifeChargee;
		lifeChargee.content.text = (objectList [0]).ToString ();
		lifeChargee.coin.text = ((int)objectList [1]).ToString ();
	}
}
