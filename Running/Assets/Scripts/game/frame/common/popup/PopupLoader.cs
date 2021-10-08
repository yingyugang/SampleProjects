using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class PopupLoader : MonoBehaviour
{
	public PopupContentMediator[] popupContentMediatorArray;
	private PopupContentMediator popupContentMediator;
	public Image bg;
	public UnityAction no;
	public UnityAction yes;
	public UnityAction ok;

	public PopupContentMediator Popup (PopupEnum popupEnum, PopupContentMediator.PopupAction popupAction = null, List<object> list = null)
	{
		if (popupContentMediator != null) {
			Close ();
		}

		popupContentMediator = popupContentMediatorArray [(int)popupEnum];
		popupContentMediator.popupAction = popupAction;
		popupContentMediator.objectList = list;
		popupContentMediator.close = () => {
			ShowOrHideBg (true);
			Close ();
		};

		popupContentMediator.no = () => {
			if (no != null) {
				no ();
			}
		};
		popupContentMediator.yes = () => {
			if (yes != null) {
				yes ();
			}
		};
		popupContentMediator.ok = () => {
			if (ok != null) {
				ok ();
			}
		};

		popupContentMediator.popup = (PopupEnum p) => {
			Popup (p, popupAction);
		};

		popupContentMediator.popupWithAction = (PopupEnum p, PopupContentMediator.PopupAction pa) => {
			Popup (p, pa);
		};

		popupContentMediator.showOrHideBg = (bool isShow) => {
			ShowOrHideBg (isShow);
		};

		popupContentMediator.gameObject.SetActive (true);
		gameObject.SetActive (true);

		return popupContentMediator;
	}

	public void ShowOrHideBg (bool isShow)
	{
		bg.enabled = isShow;
	}

	private void Close ()
	{
		gameObject.SetActive (false);
		popupContentMediator.gameObject.SetActive (false);

		popupContentMediator.close = null;
		popupContentMediator.no = null;
		popupContentMediator.yes = null;
		popupContentMediator.ok = null;
		popupContentMediator = null;
	}

	private void OnDestroy ()
	{
		if (popupContentMediator != null) {
			popupContentMediator.close = null;
			popupContentMediator.no = null;
			popupContentMediator.yes = null;
			popupContentMediator.ok = null;
			popupContentMediator.popup = null;
			popupContentMediator.popupWithAction = null;
			popupContentMediator.showOrHideBg = null;
		}
	}
}
