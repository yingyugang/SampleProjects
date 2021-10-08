using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PurchaseAgeMediator : PopupContentActivityMediator
{
	public UnityAction<int> unityAction;

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				popupWithAction (PopupEnum.PurchaseAgeAgreement, popupAction);
				unityAction (1);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				popupWithAction (PopupEnum.PurchaseAgeAgreement, popupAction);
				unityAction (2);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				unityAction (3);
				popupAction ();
				ClosePopup ();
			}
		};
	}
}
