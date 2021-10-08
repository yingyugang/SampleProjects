using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class ShopOriginalExchangeMediator : ActivityMediator
{
	private ShopOriginalExchange shopOriginalExchange;
	public ShopPage shopPage;
	public ShopOriginalLimitLogic shopOriginalLimitLogic;
	public OriginalExchangeLogic originalExchangeLogic;
	private PopupContentMediator popupContentMediator;
	private int needCoin;

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow (0);
			}
		};
	}

	private void OnEnable ()
	{
		GetItemList ();
	}

	private void GetItemList ()
	{
		shopOriginalExchange = viewWithDefaultAction as ShopOriginalExchange;
		RogerContainerCleaner.Clean (shopOriginalExchange.container);
		shopOriginalExchange.ticketNum.text = PlayerItems.GetInstance.original_ticket.ToString ();
		SendShopItemAPI ();
	}

	private void SendShopItemAPI ()
	{
		shopOriginalLimitLogic.complete = () => {
			InitItemExchange ();
		};
		shopOriginalLimitLogic.SendAPI ();
	}

	private void InitItemExchange ()
	{
		shopOriginalExchange = viewWithDefaultAction as ShopOriginalExchange;

		foreach (var item in UpdateInformation.GetInstance.ticket_exchange_list) {
			OriginalExchangeScrollItem originalExchangeScrollItem = Instantiator.GetInstance ().Instantiate<OriginalExchangeScrollItem> (shopOriginalExchange.instantiation, Vector2.zero, Vector3.one, shopOriginalExchange.container);
			originalExchangeScrollItem.unityAction = (RogerScrollItem rogerScrollItem) => {
				OriginalExchangeScrollItem currentOriginalExchangeScrollItem = rogerScrollItem as OriginalExchangeScrollItem;
				needCoin = currentOriginalExchangeScrollItem.ticketNumber;
				PlayerItems playerItems = PlayerItems.GetInstance;
				TicketExchange ticketExchange = UpdateInformation.GetInstance.ticket_exchange_list.FirstOrDefault (result => result.id == currentOriginalExchangeScrollItem.m_exchange_id);
				popupContentMediator = shopPage.popupLoader.Popup (PopupEnum.OriginalExchange, null, new List<object> () {
					playerItems.original_ticket,
					currentOriginalExchangeScrollItem.ticketNumber,
					currentOriginalExchangeScrollItem.icon.sprite,
					currentOriginalExchangeScrollItem.cardName,
					playerItems.original_ticket >= currentOriginalExchangeScrollItem.ticketNumber
				});
				(popupContentMediator as OriginalExchangeMediator).unityAction = () => {
					SendAPI (currentOriginalExchangeScrollItem.m_exchange_id);
				};
			};
			originalExchangeScrollItem.Show (item);
		}
	}

	private void SendAPI (int id)
	{
		originalExchangeLogic.ticket_id = id;
		originalExchangeLogic.complete = () => {
			PopupContentMediator popupContentMediator = shopPage.popupLoader.Popup (PopupEnum.ExchangeFinished);
			popupContentMediator.ok = () => {
				GetItemList ();
			};
			shopOriginalExchange.header.UpdateCoinAndMoney ();
		};
		originalExchangeLogic.SendAPI ();
	}
}
