using UnityEngine;
using System.Collections;

public class PurchaseAgeAgreementMediator : PopupContentMediator
{
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
}
