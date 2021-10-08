using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GamePresentStartMediator : PopupContentActivityMediator
{
	private GamePresentStart gamePresentStart;
	public UnityAction unityAction;

	private void OnEnable ()
	{
		gamePresentStart = popupContent as GamePresentStart;
		int num = UpdateInformation.GetInstance.ad_bonus_num;
		if (num > 0) {
			gamePresentStart.hasTime.gameObject.SetActive (true);
			gamePresentStart.noTime.gameObject.SetActive (false);
			gamePresentStart.greyButton.SetActive (true);
			gamePresentStart.leftTime.text = string.Format (LanguageJP.PRESENT_LEFT_TIME, num);
		} else {
			gamePresentStart.hasTime.gameObject.SetActive (false);
			gamePresentStart.noTime.gameObject.SetActive (true);
			gamePresentStart.greyButton.SetActive (false);
		}
	}

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				if (unityAction != null) {
					unityAction ();
				}
			}
		};
	}
}
