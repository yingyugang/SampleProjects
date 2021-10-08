using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PrintAgainNoEnoughMediator : PopupContentActivityMediator
{
	private PrintAgainNoEnough printAgainNoEnough;
	public UnityAction unityAction;

	private void OnEnable ()
	{
		printAgainNoEnough = popupContent as PrintAgainNoEnough;
		int own = (int)objectList [0];
		int cost = (int)objectList [1];
		string description = ((int)objectList [2] == 1) ? LanguageJP.PRINT_NAME_1 : LanguageJP.PRINT_NAME_2;
		printAgainNoEnough.title.text = description;
		printAgainNoEnough.own.text = own.ToString ();
		printAgainNoEnough.cost.text = cost.ToString ();
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
