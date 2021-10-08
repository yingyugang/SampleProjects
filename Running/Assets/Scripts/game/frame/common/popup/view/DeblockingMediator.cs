using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class DeblockingMediator : PopupContentActivityMediator
{
	private Deblocking deblocking;
	public UnityAction<int> unityAction;

	private void OnEnable ()
	{
		deblocking = popupContent as Deblocking;
		deblocking.gameName.text = objectList [0].ToString ();
		deblocking.ticket_own.text = objectList [1].ToString ();
		deblocking.ticket_cost.text = objectList [2].ToString ();
		deblocking.coin_own.text = objectList [3].ToString ();
		deblocking.coin_cost.text = objectList [4].ToString ();
	}

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				if (unityAction != null) {
					unityAction (2);
				}
				ClosePopup ();
			},
			() => {
				if (unityAction != null) {
					unityAction (1);
				}
				ClosePopup ();
			},
			() => {
				if (unityAction != null) {
					unityAction (0);
				}
				ClosePopup ();
			}
		};
	}
}
