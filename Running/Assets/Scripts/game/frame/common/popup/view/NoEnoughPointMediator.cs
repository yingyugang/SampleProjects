using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class NoEnoughPointMediator : PopupContentActivityMediator
{
	private NoEnoughPoint noEnoughPoint;
	public UnityAction unityAction;

	private void OnEnable ()
	{
		noEnoughPoint = popupContent as NoEnoughPoint;
		noEnoughPoint.own.text = objectList [0].ToString ();
		noEnoughPoint.cost.text = objectList [1].ToString ();
		noEnoughPoint.ShowIcon ((int)objectList [2] - 1);
	}

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
