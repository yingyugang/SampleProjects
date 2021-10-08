using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class PageMediator : MonoBehaviour
{
	public Page page;
	public UnityAction<bool> setDockActive;

	protected ActivityMediator currentActivityMediator;
	protected ActivityMediator[] activityMediatorArray;
	public UnityAction<int,int> unityAction;
	public UnityAction<int,int> navigate;
	protected int pageNumber;

	virtual protected void OnEnable ()
	{
		CheckResources ();
	}

	virtual protected void CheckResources ()
	{
		
	}

	virtual protected IEnumerator GetResources ()
	{
		yield return null;
	}

	public void ShowWindow ()
	{
		ShowWindow (0);
	}

	virtual protected void Awake ()
	{
		activityMediatorArray = page.activityMediatorArray;
		if (activityMediatorArray.Length > 0) {
			currentActivityMediator = activityMediatorArray [0];
		}
		AddEventListeners ();
	}

	private void AddEventListeners ()
	{
		for (int i = 0; i < activityMediatorArray.Length; i++) {
			activityMediatorArray [i].page = page;
			activityMediatorArray [i].popup = (PopupEnum popupEnum) => {
				Popup (popupEnum);
			};

			activityMediatorArray [i].popupAction = (PopupEnum popupEnum, PopupContentMediator.PopupAction popupAction) => {
				PopupAction (popupEnum, popupAction);
			};

			activityMediatorArray [i].popupWithParameters = (PopupEnum popupEnum, List<object> list) => {
				Popup (popupEnum, list);
			};

			activityMediatorArray [i].popupActionWithParameters = (PopupEnum popupEnum, PopupContentMediator.PopupAction popupAction, List<object> list) => {
				PopupAction (popupEnum, popupAction, list);
			};

			activityMediatorArray [i].showWindow = (int windowNumber) => {
				ShowWindow (windowNumber);
			};

			activityMediatorArray [i].showAndHoldWindow = (int windowNumber) => {
				ShowWindow (windowNumber, true);
			};

			activityMediatorArray [i].setDockActive = (bool isActive) => {
				setDockActive (isActive);
			};

			activityMediatorArray [i].navigate = (int pageIndex, int windowIndex) => {
				if (navigate != null) {
					navigate (pageIndex, windowIndex);
				}
			};
			activityMediatorArray [i].gotoGame = () => {
				if (navigate != null) {
					navigate (0, 0);
				}
			};
			activityMediatorArray [i].gotoShop = () => {
				if (navigate != null) {
					navigate (3, 0);
				}
			};
			activityMediatorArray [i].gotoEvent = () => {
				if (navigate != null) {
					navigate (5, 11);
				}
			};
			activityMediatorArray[i].gotoRankingDetail = () =>
			{
				if (navigate != null)
				{
					navigate (5, 9);
				}
			};
			activityMediatorArray[i].gotoItemExchange = () => {
				if (navigate != null)
				{
					navigate (3, 4);
				}
			};
			activityMediatorArray[i].gotoSpecifiedGame = () => {
				if (navigate != null)
				{
					navigate (0, 1);
				}
			};
		}
	}

	private void RemoveEventListeners ()
	{
		for (int i = 0; i < activityMediatorArray.Length; i++) {
			activityMediatorArray [i].popup = null;
			activityMediatorArray [i].popupAction = null;
			activityMediatorArray [i].popupWithParameters = null;
			activityMediatorArray [i].popupActionWithParameters = null;
			activityMediatorArray [i].showWindow = null;
			activityMediatorArray [i].showAndHoldWindow = null;
			activityMediatorArray [i].setDockActive = null;
			activityMediatorArray [i].navigate = null;
			activityMediatorArray [i].gotoGame = null;
			activityMediatorArray [i].gotoShop = null;
			activityMediatorArray [i].gotoEvent = null;
			activityMediatorArray[i].gotoRankingDetail = null;
			activityMediatorArray[i].gotoItemExchange = null;
			activityMediatorArray[i].gotoSpecifiedGame = null;
		}
	}

	private void OnDestroy ()
	{
		RemoveEventListeners ();
	}

	virtual protected void Popup (PopupEnum popupEnum, List<object> list = null)
	{
		page.popupLoader.Popup (popupEnum, null, list);
	}

	virtual protected void PopupAction (PopupEnum popupEnum, PopupContentMediator.PopupAction popupAction, List<object> list = null)
	{
		page.popupLoader.Popup (popupEnum, popupAction, list);
	}

	virtual public void ShowWindow (int windowNumber, bool isHold = false)
	{
		if (currentActivityMediator != null) {
			currentActivityMediator.gameObject.SetActive (isHold);
			currentActivityMediator = activityMediatorArray [windowNumber];
			currentActivityMediator.gameObject.SetActive (true);
			if (unityAction != null) {
				unityAction (pageNumber, windowNumber);
			}
		}
	}
}
