using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GachaDetailTopMediator : ActivityMediator
{
	private GachaDetailTop gachaDetailTop;
	public GachaDetailMediator gachaDetailMediator;
	private Gacha gacha;
	public GachaLogic gachaLogic;
	public GachaAnimationMediator gachaAnimationMediator;
	private GachaResultInfo gachaResultInfo;
	private Sprite sprite;
	private PopupContentMediator popupContentMediator;

	[HideInInspector]
	private int m_gacha_id;
	[HideInInspector]
	private int mode;
	[HideInInspector]
	private int cost_type;

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow (0);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				gachaDetailMediator.SetWindow (gacha, sprite);
				showWindow (2);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				CheckIfEnough (true);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				CheckIfEnough (false);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				CheckIfEnough (true);
			}
		};
	}

	private void CheckIfEnough (bool isSingle)
	{
		popupContentMediator = null;
		int[] array = GetOwnAndCost (isSingle);
		if (array != null)
		{
			if (array[0] < array[1])
			{
				PopupNoEnough (array[0], array[1]);
			}
			else {
				SendAPI (gacha.id, isSingle ? 1 : 2);
			}
		}
	}

	private int[] GetOwnAndCost (bool isSingle)
	{
		int own = 0;
		int cost = 0;
		if (gacha.gacha_type == 1)
		{
			own = Player.GetInstance.free_ticket_num;
			cost = isSingle ? int.Parse (gacha.single_cost) : gacha.multi_cost;
			return new int[] { own, cost };
		}
		else if (gacha.gacha_type == 2)
		{
			own = Player.GetInstance.ticket_num;
			cost = isSingle ? int.Parse (gacha.single_cost) : gacha.multi_cost;
			return new int[] { own, cost };
		}
		else if (gacha.gacha_type == 5)
		{
			own = PlayerItems.GetInstance.original_point;
			cost = isSingle ? int.Parse (gacha.single_cost) : gacha.multi_cost;
			return new int[] { own, cost };
		}
		else
		{
			return null;
		}
	}

	private void PopupNoEnough (int own, int cost)
	{
		popupContentMediator = null;
		if (gacha.gacha_type == 1)
		{
			popupContentMediator = page.popupLoader.Popup (PopupEnum.NoEnoughTicket, null, new List<object> () {
				own,
				cost
			});
			(popupContentMediator as NoEnoughTicketMediator).unityAction = () =>
			{
				gotoShop ();
			};
		}
		else if (gacha.gacha_type == 2)
		{
			popupContentMediator = page.popupLoader.Popup (PopupEnum.NoEnoughCoin, null, new List<object> () {
				own,
				cost
			});
			(popupContentMediator as NoEnoughCoinMediator).unityAction = () =>
			{
				gotoShop ();
			};
		}
		else if (gacha.gacha_type == 5)
		{
			popupContentMediator = page.popupLoader.Popup (PopupEnum.NoEnoughOriginalPoint, null, new List<object> () {
				own,
				cost
			});
			(popupContentMediator as NoEnoughOriginalPointMediator).unityAction = () =>
			{
				gotoShop ();
			};
		}
	}

	private void SendAPI (int id, int mode)
	{
		UpdateInformation.GetInstance.recycle_pt.Clone ();
		this.m_gacha_id = gachaLogic.m_gacha_id = id;
		this.mode = gachaLogic.mode = mode;
		this.cost_type = gachaLogic.cost_type = 0;
		gachaLogic.complete = () =>
		{
			GameConstant.CurrentGachaType = gacha.gacha_type;
			gachaDetailTop.ad.SetActive (false);
			gachaDetailTop.header.UpdateCoinAndMoney ();
			gachaResultInfo = GachaResultInfo.GetInstance;
			gachaAnimationMediator.SetWindow (gacha.gacha_type, mode);
			showWindow (3);
		};
		gachaLogic.error = (string status) =>
		{
			int[] array = GetOwnAndCost (mode == 1);
			PopupNoEnough (array[0], array[1]);
		};
		gachaLogic.SendAPI ();
	}

	public void SetWindow (Gacha gacha, Sprite sprite)
	{
		this.gacha = gacha;
		this.sprite = sprite;
		gachaDetailTop = viewWithDefaultAction as GachaDetailTop;
		if (gacha.gacha_type != 5)
		{
			gachaDetailTop.normalPoint.SetActive (true);
			gachaDetailTop.originalPoint.SetActive (false);
			gachaDetailTop.ShowNormal (gacha, sprite);
		}
		else
		{
			gachaDetailTop.normalPoint.SetActive (false);
			gachaDetailTop.originalPoint.SetActive (true);
			gachaDetailTop.ShowOriginal (gacha, sprite);
		}
		gachaDetailTop.buttonArray[1].gameObject.SetActive (!string.IsNullOrEmpty(gacha.detail_title) && !string.IsNullOrEmpty(gacha.detail_desc));
	}
}
