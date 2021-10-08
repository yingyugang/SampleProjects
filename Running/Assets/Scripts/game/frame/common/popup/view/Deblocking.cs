using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class Deblocking : PopupContentWithDefaultAction
{
	public Text gameName;
	public Text ticket_own;
	public Text ticket_cost;
	public Text coin_own;
	public Text coin_cost;

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
			}
		};
	}
}
