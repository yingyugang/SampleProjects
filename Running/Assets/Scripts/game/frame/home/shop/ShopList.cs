using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using home;

public class ShopList : ViewWithDefaultAction
{
	public HeaderMediator header;
	public Transform container;
	public ProductScrollItem instantiation;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			}
		};
	}
}
