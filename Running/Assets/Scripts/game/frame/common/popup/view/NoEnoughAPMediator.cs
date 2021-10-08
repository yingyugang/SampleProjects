using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class NoEnoughAPMediator : PopupContentActivityMediator
{
	public UnityAction unityAction;

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				if (unityAction != null) {
					unityAction ();
				}
				ClosePopup ();
			}
		};
	}
}
