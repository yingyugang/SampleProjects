using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using home;

public class ShopItemExchange : ViewWithDefaultAction
{
	public Transform container;
	public ItemExchangeScrollItem instantiation;
	public HeaderMediator header;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			},
			() => {
				Send (1);
			}
		};
	}
}
