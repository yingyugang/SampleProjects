using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ActivityMediator : PopupMediator
{
	[HideInInspector]
	public Page page;
	public UnityAction<int> showWindow;
	public UnityAction<int> showAndHoldWindow;
	public UnityAction<bool> setDockActive;
	public UnityAction<int,int> navigate;
	public UnityAction gotoGame;
	public UnityAction gotoShop;
	public UnityAction gotoEvent;
	public UnityAction gotoRankingDetail;
	public UnityAction gotoItemExchange;
	public UnityAction gotoSpecifiedGame;

	protected override void Start ()
	{
		base.Start ();
		InitData ();
	}

	virtual protected void InitData ()
	{
		
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		showWindow = null;
		showAndHoldWindow = null;
		setDockActive = null;
		navigate = null;
		gotoGame = null;
		gotoShop = null;
		gotoEvent = null;
		gotoRankingDetail = null;
		gotoItemExchange = null;
		gotoSpecifiedGame = null;
	}
}

