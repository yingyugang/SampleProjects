using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

public class GachaRecycleDetailTopMediator : ActivityMediator
{
	private Gacha gacha;
	private GachaRecycleDetailTop gachaRecycleDetailTop;
	private GachaRecycleDetailData gachaRecycleDetailData;
	public GachaLogic gachaLogic;
	private GachaResultInfo gachaResultInfo;
	public GachaAnimationMediator gachaAnimationMediator;
	public GachaDetailMediator gachaDetailMediator;

	private readonly List<string> levelList = new List<string> () {
		"LG",
		"UR",
		"SR",
		"R",
		"N"
	};

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow (4);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				gachaDetailMediator.SetWindow (gacha, gachaRecycleDetailData.sprite);
				showWindow (2);
			}
		};
	}

	private void SendAPI (GachaRecycleItem gachaRecycleItem, int id, int mode, int costType, int own, int cost)
	{
		RecyclePoint recyclePoint = UpdateInformation.GetInstance.recycle_pt.Clone ();
		gachaLogic.m_gacha_id = id;
		gachaLogic.mode = mode;
		gachaLogic.cost_type = costType;
		gachaLogic.complete = () => {
                        int[] arr = new int[] { 4, 3, 2, 1, 0 };
                        recyclePoint[arr[costType - 1]] -= cost;
                        own -= cost;
                        gachaRecycleItem.UpdateOwnFiled (own);
                        gachaRecycleDetailTop.ad.SetActive (false);
			gachaResultInfo = GachaResultInfo.GetInstance;
			gachaAnimationMediator.SetWindow (gacha.gacha_type, mode);
			showWindow (3);
		};
		gachaLogic.error = (string status) => {
			PopupError (own, cost, costType);
		};
		gachaLogic.SendAPI ();
	}

	public void SetWindow (Gacha gacha, GachaRecycleDetailData gachaRecycleDetailData)
	{
		this.gacha = gacha;
		this.gachaRecycleDetailData = gachaRecycleDetailData;
		gachaRecycleDetailTop = viewWithDefaultAction as GachaRecycleDetailTop;
		string level = levelList [gachaRecycleDetailData.mode - GameConstant.GACHA_RECYCLE_START_ID];
		gachaRecycleDetailTop.Show (level + LanguageJP.RecycleTitle, gachaRecycleDetailData.sprite, gachaRecycleDetailData.card_desc);
		List<string> currentLevelList = null;
		if (gachaRecycleDetailData.mode == 6) {
			currentLevelList = levelList.GetRange (0, 1);
		} else if (gachaRecycleDetailData.mode == 7) {
			currentLevelList = levelList.GetRange (1, 1);
		} else {
			currentLevelList = levelList.GetRange (2, 3);
		}
		CreateGachaRecycleMenuItems (gachaRecycleDetailData.mode, currentLevelList);
	}

	private void CreateGachaRecycleMenuItems (int mode, List<string> currentLevelList)
	{
		RogerContainerCleaner.Clean (gachaRecycleDetailTop.container);
		int length = gachaRecycleDetailData.ownList.Count;
		Instantiator instantiator = Instantiator.GetInstance ();
		for (int i = length - 1, j = 1; i >= 0; i--,j++) {
			GachaRecycleItem gachaRecycleItem = instantiator.Instantiate (gachaRecycleDetailTop.instantiation, Vector2.zero, Vector3.one, gachaRecycleDetailTop.container);
			gachaRecycleItem.unityAction = RecycleHandler;
			gachaRecycleItem.SetData (mode, j, i, currentLevelList [i], gachaRecycleDetailData.ownList [i], gachaRecycleDetailData.costList [i]);
			gachaRecycleItem.coinList [i].gameObject.SetActive (true);
			gachaRecycleItem.gameObject.SetActive (true);
		}
	}

	private void RecycleHandler (GachaRecycleItem gachaRecycleItem, int mode, int type, int[] array)
	{
		int own = array [0];
		int cost = array [1];
		if (own < cost) {
			PopupError (own, cost, type);
		} else {
			SendAPI (gachaRecycleItem, gacha.id, mode, type, own, cost);
		}
	}

	private void PopupError (int own, int cost, int costType)
	{
		PopupContentMediator popupContentMediator = page.popupLoader.Popup (PopupEnum.NoEnoughPoint, null, new List<object> () {
			own,
			cost,
			costType
		});
		(popupContentMediator as NoEnoughPointMediator).unityAction = () => {
			gotoShop ();
		};
	}
}
