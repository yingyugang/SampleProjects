using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class PrintAgainMediator : PopupContentActivityMediator
{
	private PrintAgain printAgain;
	public UnityAction unityAction;

	private void OnEnable ()
	{
		printAgain = popupContent as PrintAgain;
		int own = (int)objectList [0];
		int cost = (int)objectList [1];
		string description = ((int)objectList [2] == 1) ? LanguageJP.PRINT_NAME_AGAIN_1 : LanguageJP.PRINT_NAME_AGAIN_2;
		printAgain.own.text = own.ToString ();
		printAgain.cost.text = cost.ToString ();
		printAgain.title.text = printAgain.text.text = description;
		printAgain.additionalText.SetActive ((int)objectList[2] == 2);
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
