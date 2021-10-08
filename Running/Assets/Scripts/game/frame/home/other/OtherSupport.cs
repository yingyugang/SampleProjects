using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class OtherSupport : ViewWithDefaultAction
{
	public Transform container;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			}
		};
	}
}
