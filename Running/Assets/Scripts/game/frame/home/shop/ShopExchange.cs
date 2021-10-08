using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using home;

public class ShopExchange : ViewWithDefaultAction
{
	public Transform container;
	public ExchangeScrollItem instantiation;
	public HeaderMediator header;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			}
		};
	}
}
