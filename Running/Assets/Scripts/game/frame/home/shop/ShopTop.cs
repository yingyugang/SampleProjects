using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using home;
using UnityEngine.UI;

public class ShopTop : ViewWithDefaultAction
{
	public HeaderMediator header;
	public GreyButton greyButton;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			},
			() => {
				Send (1);
			},
			() => {
				Send (2);
			},
			() => {
				Send (3);
			},
            () => {
                Send (4);
            },
            () => {
                Send (5);
            },
			() => {
				Send (6);
			},
			() => {
				Send (7);
			}
		};
	}
}
