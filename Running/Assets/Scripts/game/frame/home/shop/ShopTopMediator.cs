using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class ShopTopMediator : ActivityMediator
{
	private ShopTop shopTop;
	public ShopPage shopPage;
	public RecoverLogic recoverLogic;
	public ShopList shopList;
	public ShopListMediator shopListMediator;
	[HideInInspector]
	public int age_range;

	private void OnEnable ()
	{
		SendMessageUpwards (GameConstant.ClearNoticeManager, NoticeManager.SHOP, SendMessageOptions.DontRequireReceiver);
		shopTop = viewWithDefaultAction as ShopTop;
		CheckIfCanBeRecover ();
	}

	private void CheckIfCanBeRecover ()
	{
		shopTop.greyButton.SetActive (ApMediator.currentRecovery != ApMediator.MAX_RECOVERY);
	}

	protected override void CreateActions ()
	{
		shopTop = viewWithDefaultAction as ShopTop;
		List<ConventionCSVStructure> conventions = MasterCSV.conventionCSV.ToList ();

		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				PopupContentMediator popupContentMediator =	shopPage.popupLoader.Popup (PopupEnum.PurchaseAge, ShowShopList);
				(popupContentMediator as PurchaseAgeMediator).unityAction = (int i) => {
					age_range = i;
				};
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				Player player = Player.GetInstance;
				if (player.ticket_num < UpdateInformation.GetInstance.recover_ap_value) {
					PopupContentMediator popupContentMediator = page.popupLoader.Popup (PopupEnum.NoEnoughCoin, null, new List<object> () {
						player.ticket_num,
						UpdateInformation.GetInstance.recover_ap_value
					});
					(popupContentMediator as NoEnoughCoinMediator).unityAction = () => {
						gotoShop ();
					};
				} else {
					shopPage.popupLoader.Popup (PopupEnum.LifeChargee, SendAPI, new List<object> () {
						string.Format (LanguageJP.CHARGEE_CONFIRM, UpdateInformation.GetInstance.recover_ap_value),
						Player.GetInstance.ticket_num
					});
				}
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				showWindow (2);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				showWindow (3);
			},
			() => {
				shopPage.popupLoader.Popup (PopupEnum.ShopLegal, null, new System.Collections.Generic.List<object> () {
					conventions [1].title, conventions [1].description
				});
			},
			() => {
				shopPage.popupLoader.Popup (PopupEnum.ShopLegal, null, new System.Collections.Generic.List<object> () {
					conventions [2].title, conventions [2].description
				});
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				showWindow (4);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				showWindow (5);
			}
		};
	}

	private void SendAPI ()
	{
		recoverLogic.complete = () => {
			shopTop.header.UpdateCoinAndMoney ();
			shopTop.header.UpdateAp ();
			popup (PopupEnum.LifeChargeeFinished);
			CheckIfCanBeRecover ();
            UnityEngine.Analytics.Analytics.CustomEvent ("life_buy", new Dictionary<string, object> ());
		};
		recoverLogic.error = (string status) => {
			PopupContentMediator popupContentMediator = shopPage.popupLoader.Popup (PopupEnum.LifeChargeeError, null, new List<object> () {
				Player.GetInstance.ticket_num
			});
			(popupContentMediator as LifeChargeeErrorMediator).ok = () => {
				gotoShop ();
			};
		};
		recoverLogic.SendAPI ();
	}

	private void ShowShopList ()
	{
		shopListMediator.SetWindow (age_range);
		showWindow (1);
	}
}
