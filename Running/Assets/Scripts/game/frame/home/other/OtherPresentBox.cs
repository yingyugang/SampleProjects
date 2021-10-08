using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class OtherPresentBox : ViewWithDefaultAction
{
	public Transform container;
	public PresentScrollItem instantiation;
	public GreyButton greyButton;

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
