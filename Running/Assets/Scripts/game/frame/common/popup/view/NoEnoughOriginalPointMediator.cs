using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class NoEnoughOriginalPointMediator : PopupContentActivityMediator
{
	private NoEnoughOriginalPoint noEnoughOriginalPoint;
	public UnityAction unityAction;

	private void OnEnable ()
	{
		noEnoughOriginalPoint = popupContent as NoEnoughOriginalPoint;
		noEnoughOriginalPoint.own.text = objectList [0].ToString ();
		noEnoughOriginalPoint.cost.text = objectList [1].ToString ();
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
