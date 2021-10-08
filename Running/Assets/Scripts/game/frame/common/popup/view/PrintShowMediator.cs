using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PrintShowMediator : PopupContentActivityMediator
{
	private PrintShow printShow;
	public UnityAction<int> unityAction;

	private void OnEnable ()
	{
		printShow = popupContent as PrintShow;
		Card card = (Card)objectList [0];
		string codel = card.codel;
		string code2l = card.code2l;
		printShow.codelObject.SetActive (!string.IsNullOrEmpty (codel));
		printShow.code2lObject.SetActive (!string.IsNullOrEmpty (code2l));
		printShow.codelImage.SetActive (!string.IsNullOrEmpty (codel));
		printShow.code2lImage.SetActive (!string.IsNullOrEmpty (code2l));
		printShow.codel.text = card.codel;
		printShow.code2l.text = card.code2l;
		printShow.buttonLText.text = string.IsNullOrEmpty (codel) ? LanguageJP.PRINT_NAME_1 : LanguageJP.PRINT_NAME_AGAIN_1;
		printShow.button2LText.text = string.IsNullOrEmpty (code2l) ? LanguageJP.PRINT_NAME_2 : LanguageJP.PRINT_NAME_AGAIN_2;
	}

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				if (unityAction != null) {
					unityAction (1);
				}
				ClosePopup ();
			},
			() => {
				if (unityAction != null) {
					unityAction (2);
				}
				ClosePopup ();
			},
			() => {
				if (unityAction != null) {
					unityAction (3);
				}
				ClosePopup ();
			}
		};
	}
}
