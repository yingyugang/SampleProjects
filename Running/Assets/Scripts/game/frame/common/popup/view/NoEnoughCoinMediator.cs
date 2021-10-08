using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class NoEnoughCoinMediator : PopupContentActivityMediator
{
	private NoEnoughCoin noEnoughCoin;
	public UnityAction unityAction;

	private void OnEnable ()
	{
		noEnoughCoin = popupContent as NoEnoughCoin;
		noEnoughCoin.own.text = objectList [0].ToString ();
		noEnoughCoin.cost.text = objectList [1].ToString ();
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
