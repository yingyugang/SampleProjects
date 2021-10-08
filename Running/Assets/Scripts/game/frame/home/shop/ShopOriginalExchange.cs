using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using home;

public class ShopOriginalExchange : ViewWithDefaultAction
{
	public Transform container;
	public OriginalExchangeScrollItem instantiation;
	public HeaderMediator header;
	public Text ticketNum;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			}
		};
	}
}
