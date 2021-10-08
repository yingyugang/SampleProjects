using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class OtherNotice : ViewWithDefaultAction
{
	public Transform container;
	public NoticeScrollItem instantiation;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			}
		};
	}
}
