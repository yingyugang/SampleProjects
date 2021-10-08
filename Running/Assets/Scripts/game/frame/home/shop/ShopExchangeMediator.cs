using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class ShopExchangeMediator : ActivityMediator
{
	private ShopExchange shopExchange;
	public ShopPage shopPage;
	public ExchangeLogic exchangeLogic;
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

	protected override void InitData ()
	{
		shopExchange = viewWithDefaultAction as ShopExchange;
		IEnumerable<ExchangeCSVStructure> exchangeCSVStructureEnumerable = MasterCSV.exchangeCSV;

		foreach (var item in exchangeCSVStructureEnumerable)
		{
			ExchangeScrollItem exchangeScrollItem = Instantiator.GetInstance ().Instantiate<ExchangeScrollItem> (shopExchange.instantiation, Vector2.zero, Vector3.one, shopExchange.container);
			exchangeScrollItem.unityAction = (RogerScrollItem rogerScrollItem) =>
			{
				ExchangeScrollItem currentExchangeScrollItem = rogerScrollItem as ExchangeScrollItem;
				needCoin = currentExchangeScrollItem.coinNumber;
				Player player = Player.GetInstance;
				popupContentMediator = shopPage.popupLoader.Popup (PopupEnum.Exchange, null, new List<object> () {
					player.ticket_num,
					player.free_ticket_num,
					currentExchangeScrollItem.coinNumber,
					currentExchangeScrollItem.ticketNumber
				});
				(popupContentMediator as ExchangeMediator).unityAction = () =>
				{
					if (player.ticket_num < currentExchangeScrollItem.coinNumber)
					{
						PopupError (currentExchangeScrollItem.coinNumber);
					}
					else {
						SendAPI (currentExchangeScrollItem.m_exchange_id);
					}
				};
			};
			exchangeScrollItem.Show (item.id, item.coin_num, item.ticket_num);
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
		exchangeLogic.m_exchange_id = id;
		exchangeLogic.complete = () =>
		{
			PopupContentMediator popupContentMediator = shopPage.popupLoader.Popup (PopupEnum.ExchangeFinished);
			shopExchange.header.UpdateCoinAndMoney ();
		};
		exchangeLogic.error = (string status) =>
		{
			PopupError (needCoin);
		};
		exchangeLogic.SendAPI ();
	}
}
