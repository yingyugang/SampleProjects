using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ShowCoinMediator : PopupContentActivityMediator
{
	private ShowCoin showCoin;

	private void OnEnable ()
	{
		showCoin = popupContent as ShowCoin;
		showCoin.coin.text = Player.GetInstance.coin.ToString ();
		showCoin.freeCoin.text = Player.GetInstance.free_coin.ToString ();
	}
}
