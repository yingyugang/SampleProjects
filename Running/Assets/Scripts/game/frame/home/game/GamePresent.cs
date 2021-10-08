using UnityEngine;
using System.Collections;
using home;

public class GamePresent : MonoBehaviour
{
	private PopupLoader popupLoader;
	public AdPresentLogic adPresentLogic;
	public HeaderMediator header;
	private GamePresentSelectMediator gamePresentSelectMediator;

	public void Init (PopupLoader popupLoader)
	{
		this.popupLoader = popupLoader;
		ShowStart ();
	}

	private void SendAPI ()
	{
		adPresentLogic.complete = () => {
			header.UpdateCoinAndMoney ();
			gamePresentSelectMediator.PlayAnimation	();
		};
		adPresentLogic.SendAPI ();
	}

	private void ShowStart ()
	{
		PopupContentMediator popupContentMediator = popupLoader.Popup (PopupEnum.PresentStart);
		(popupContentMediator as GamePresentStartMediator).unityAction = () => {
			PlayAd ();
		};
	}

	private void PlayAd ()
	{
		#if UNITY_EDITOR
			ShowSelect ();
		#else
			AdsManager.ShowVedioBanner (delegate(bool isSuccess) {
				if (isSuccess) {
					ShowSelect ();
				}else{
					popupLoader.Popup (PopupEnum.PresentPrepare);
				}
			}, 1);
		#endif
	}

	private void ShowSelect ()
	{
		gamePresentSelectMediator = popupLoader.Popup (PopupEnum.PresentSelect) as GamePresentSelectMediator;
		gamePresentSelectMediator.unityAction = () => {
			SendAPI	();
		};
	}
}
