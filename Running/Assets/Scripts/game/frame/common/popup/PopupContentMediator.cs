using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class PopupContentMediator : MonoBehaviour
{
	public delegate void PopupAction ();

	public PopupAction popupAction;

	public UnityAction close;
	public UnityAction<PopupEnum> popup;
	public UnityAction<PopupEnum, PopupAction> popupWithAction;
	public UnityAction<bool> showOrHideBg;
	public PopupContent popupContent;
	public UnityAction no;
	public UnityAction yes;
	public UnityAction ok;
	public List<object> objectList;

	protected UnityAction[] unityActionArray;

	virtual protected void Start ()
	{
		AddEventListeners ();
	}

	private void OnDestroy ()
	{
		RemoveEventListeners ();
	}

	private void AddEventListeners ()
	{
		if (popupContent.noButton != null) {
			popupContent.noButtonOnClicked = NoButtonOnClickHandler;
		}

		if (popupContent.yesButton != null) {
			popupContent.yesButtonOnClicked = YesButtonOnClickHandler;
		}

		if (popupContent.okButton != null) {
			popupContent.okButtonOnClicked = OKButtonOnClickHandler;
		}
	}

	private void RemoveEventListeners ()
	{
		popupContent.noButtonOnClicked = null;
		popupContent.yesButtonOnClicked = null;
		popupContent.okButtonOnClicked = null;
	}

	virtual protected void NoButtonOnClickHandler ()
	{
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
		if (no != null) {
			no ();
		}
		ClosePopup ();
	}

	virtual protected void YesButtonOnClickHandler ()
	{
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
		if (yes != null) {
			yes ();
		}
		ClosePopup ();
	}

	virtual protected void OKButtonOnClickHandler ()
	{
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
		if (ok != null) {
			ok ();
		}
		ClosePopup ();
	}

	protected void ClosePopup ()
	{
		if (close != null) {
			close ();
		}
	}
}
