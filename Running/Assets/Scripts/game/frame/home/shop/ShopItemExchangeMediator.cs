using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class ShopItemExchangeMediator : ActivityMediator
{
	private ShopItemExchange shopItemExchange;
	public ShopPage shopPage;
	public ShopItemLimitLogic shopItemLimitLogic;
	public ItemExchangeLogic itemExchangeLogic;
	private PopupContentMediator popupContentMediator;
	private int needCoin;

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow (0);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				popup (PopupEnum.ItemExchangeDetail);
			}
		};
	}

	private void OnEnable ()
	{
		GetItemList ();
	}

	private void GetItemList ()
	{
		shopItemExchange = viewWithDefaultAction as ShopItemExchange;
		RogerContainerCleaner.Clean (shopItemExchange.container);
		SendShopItemAPI ();
	}

	private void SendShopItemAPI ()
	{
		shopItemLimitLogic.complete = () =>
		{
			InitItemExchange ();
		};
		shopItemLimitLogic.SendAPI ();
	}

	private void InitItemExchange ()
	{
		shopItemExchange = viewWithDefaultAction as ShopItemExchange;

		foreach (var item in UpdateInformation.GetInstance.limit_item_list)
		{
			ItemExchangeScrollItem itemExchangeScrollItem = Instantiator.GetInstance ().Instantiate<ItemExchangeScrollItem> (shopItemExchange.instantiation, Vector2.zero, Vector3.one, shopItemExchange.container);
			itemExchangeScrollItem.unityAction = (RogerScrollItem rogerScrollItem) =>
			{
				ItemExchangeScrollItem currentItemExchangeScrollItem = rogerScrollItem as ItemExchangeScrollItem;
				needCoin = currentItemExchangeScrollItem.coinNumber;
				Player player = Player.GetInstance;
				LimitItem limitItem = UpdateInformation.GetInstance.limit_item_list.FirstOrDefault (result => result.id == currentItemExchangeScrollItem.m_exchange_id);
				popupContentMediator = shopPage.popupLoader.Popup (PopupEnum.ItemExchange, null, new List<object> () {
					player.ticket_num,
					PlayerItems.GetInstance[limitItem.item_id],
					currentItemExchangeScrollItem.coinNumber,
					currentItemExchangeScrollItem.itemNumber,
					currentItemExchangeScrollItem.icon.sprite,
					MasterCSV.itemCSV.FirstOrDefault (result => result.id == limitItem.item_id).name
				});
				(popupContentMediator as ItemExchangeMediator).unityAction = () =>
				{
					if (player.ticket_num < currentItemExchangeScrollItem.coinNumber)
					{
						PopupError (currentItemExchangeScrollItem.coinNumber);
					}
					else {
						SendAPI (currentItemExchangeScrollItem.m_exchange_id);
					}
				};
			};
			itemExchangeScrollItem.Show (item);
		}
	}

	private void PopupError (int needCoin)
	{
		Player player = Player.GetInstance;
		popupContentMediator = page.popupLoader.Popup (PopupEnum.NoEnoughCoin, null, new List<object> () {
				player.ticket_num,
				needCoin
		});
		(popupContentMediator as NoEnoughCoinMediator).unityAction = () =>
		{
			gotoShop ();
		};
	}

	private void SendAPI (int id)
	{
		itemExchangeLogic.m_limit_item_id = id;
		itemExchangeLogic.complete = () =>
		{
			PopupContentMediator popupContentMediator = shopPage.popupLoader.Popup (PopupEnum.ExchangeFinished);
			popupContentMediator.ok = () =>
			{
				GetItemList ();
			};
			shopItemExchange.header.UpdateCoinAndMoney ();
		};
		itemExchangeLogic.error = (string status) =>
		{
			PopupError (needCoin);
		};
		itemExchangeLogic.SendAPI ();
	}
}
