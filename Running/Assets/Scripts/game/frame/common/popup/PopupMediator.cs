using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class PopupMediator : ActionMediator
{
	public UnityAction<PopupEnum> popup;
	public UnityAction<PopupEnum,PopupContentMediator.PopupAction> popupAction;
	public UnityAction<PopupEnum,List<object>> popupWithParameters;
	public UnityAction<PopupEnum,PopupContentMediator.PopupAction,List<object>> popupActionWithParameters;
}
