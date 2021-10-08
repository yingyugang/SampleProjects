using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class GameTopMediator : ActivityMediator
{
	public GameDetailTopMediator gameDetailTopMediator;
	private GameTop gameTop;
	public static List<GameDetail> gameDetailList;
	public GameOpenLogic gameOpenLogic;
	private int currentGameNumber;
	private int totalGameNumber;
	public GamePresent gamePresent;

	private void OnEnable ()
	{
		SendMessageUpwards (GameConstant.ClearNoticeManager, NoticeManager.GAME, SendMessageOptions.DontRequireReceiver);
		gameTop = viewWithDefaultAction as GameTop;
		gameTop.SetInformation (UpdateInformation.GetInstance.game_led_text, UpdateInformation.GetInstance.game_led_image);
	}

	private void CheckEventType ()
	{
		gameTop = viewWithDefaultAction as GameTop;
		List<EventInfo> eventInfoList = UpdateInformation.GetInstance.event_info_list;
		GetEventStatus ();
		if (eventInfoList.Count == 0) {
			GameConstant.eventTypeEnum = EventTypeEnum.NONE;
		} else if (eventInfoList.Count == 1) {
			if (eventInfoList [0].eventStatusEnum == EventStatusEnum.OPENED) {
				GetEventBanner (eventInfoList [0]);
			}
			GameConstant.eventTypeEnum = EventTypeEnum.ONE;
		} else if (eventInfoList.Count >= 2) {
			if (eventInfoList [1].eventStatusEnum == EventStatusEnum.OPENED) {
				GetEventBanner (eventInfoList [1]);
			} else if (eventInfoList [0].eventStatusEnum == EventStatusEnum.OPENED) {
				GetEventBanner (eventInfoList [0]);
			}
			GameConstant.eventTypeEnum = EventTypeEnum.BOTH;
		}
	}

	private void GetEventStatus ()
	{
		List<EventInfo> eventInfoList = UpdateInformation.GetInstance.event_info_list;
		int currentTime = SystemInformation.GetInstance.current_time;
		int length = eventInfoList.Count;
		for (int i = 0; i < length; i++) {
			EventInfo eventInfo = eventInfoList [i];
			if (eventInfo.end_at < currentTime) {
				eventInfo.eventStatusEnum = EventStatusEnum.CLOSED;
			} else if (eventInfo.start_at <= currentTime && eventInfo.end_at >= currentTime) {
				eventInfo.eventStatusEnum = EventStatusEnum.OPENED;
			} else if (eventInfo.display_at <= currentTime) {
				eventInfo.eventStatusEnum = EventStatusEnum.PUBLIC;
			} else {
				eventInfo.eventStatusEnum = EventStatusEnum.CLOSED;
			}
		}
	}

	private void GetEventBanner (EventInfo eventInfo)
	{
		GameConstant.currentEventGameID = eventInfo.m_game_id;
		SendMessageUpwards ("ShowEventBadge", SendMessageOptions.DontRequireReceiver);
		StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetResource<Texture2D> (AssetBundleName.event_image.ToString (), eventInfo.banner_image, (Texture2D texture2D) => {
			gameTop.eventButton.image.sprite = TextureToSpriteConverter.ConvertToSprite (texture2D);
			gameTop.eventButton.gameObject.SetActive (true);
		}, false));
	}

	protected override void InitData ()
	{
		CheckEventType ();
		List<GameCSVStructure> gameCSVStructureList = MasterCSV.gameCSV.ToList ();
		gameTop = viewWithDefaultAction as GameTop;
		gameDetailList = UpdateInformation.GetInstance.game_list;
		currentGameNumber = totalGameNumber = gameDetailList.Count;
		for (int i = 0; i < totalGameNumber; i++) {
			GameDetail gameDetail = gameDetailList [i];
			GameCSVStructure gameCSVStructure = gameCSVStructureList [i];
			CreateGameDetail (gameDetail, gameCSVStructure);
			bool isLock = gameDetail.islock == 1;
			gameTop.gameItemList [i].SetData (gameDetail.id, gameDetail.rank, gameDetail.isnew == 1, isLock);
			if (isLock) {
				currentGameNumber--;
			}
			gameTop.gameItemList [i].CreateAnimation ();
		}
		SetGameNumberText (currentGameNumber, totalGameNumber);
	}

	private void CreateGameDetail (GameDetail gameDetail, GameCSVStructure gameCSVStructure)
	{
		gameDetail.name = gameCSVStructure.name;
		gameDetail.description = gameCSVStructure.description;
		gameDetail.image_resource = gameCSVStructure.image_resource;
		gameDetail.ap = gameCSVStructure.ap;
		gameDetail.open_need_ticket = gameCSVStructure.open_need_ticket;
		gameDetail.open_need_free_ticket = gameCSVStructure.open_need_free_ticket;
		gameDetail.default_open = gameCSVStructure.default_open;
		gameDetail.open_start_time = gameCSVStructure.open_start_time;
	}

	private void SetGameNumberText (int num, int total)
	{
		gameTop.gameNumber.text = string.Format ("{0}{1}{2}{3}{4}{5}", LanguageJP.GameIsOpen, LanguageJP.PINK_COLOR_PREFIX, num, LanguageJP.COLOR_SUFFIX, LanguageJP.DEVIDE, total);
	}

	protected override void CreateActions ()
	{
		gameTop = viewWithDefaultAction as GameTop;
		unityActionArray = new UnityAction[] {
			() => {
				CheckIsLockBeforeEnter (0, gameDetailList [0].name, SceneEnum.Daruma);
			},
			() => {
				CheckIsLockBeforeEnter (1, gameDetailList [1].name, SceneEnum.GetOut);//TODO
			},
			() => {
				CheckIsLockBeforeEnter (2, gameDetailList [2].name, SceneEnum.Swimming);
			},
			() => {
				CheckIsLockBeforeEnter (3, gameDetailList [3].name, SceneEnum.BreakoutClone);
			},
			() => {
				CheckIsLockBeforeEnter (4, gameDetailList [4].name, SceneEnum.Shee);
			},
			() => {
				CheckIsLockBeforeEnter (5, gameDetailList [5].name, SceneEnum.Biking);
			},
			() => {
				CheckIsLockBeforeEnter (6, gameDetailList [6].name, SceneEnum.SixRun);
			},
			() => {
				CheckIsLockBeforeEnter (7, gameDetailList [7].name, SceneEnum.Guess);
			},
			() => {
				CheckIsLockBeforeEnter (8, gameDetailList [8].name, SceneEnum.OilKing);
			}
		};

		gameTop.eventUnityAction = () => {
			gotoEvent ();
		};

		gameTop.presentUnityAction = () => {
			ShowPresent ();
		};
	}

	private void ShowPresent()
	{
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
		gamePresent.Init (page.popupLoader);
	}

	private void CheckIsLockBeforeEnter (int num, string title, SceneEnum sceneEnum)
	{		
		GameDetail gameDetail = gameDetailList [num];
		if(CheatController.GetInstance()!=null){
			CheatController.GetInstance ().SetCurrentGame (gameDetail.id);
		}
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);

		if (gameDetail.islock == 1) {
			PopupContentMediator popupContentMediator = page.popupLoader.Popup (PopupEnum.Deblocking, null, new List<object> {
				string.Format ("{0}{1}", title, LanguageJP.GAME_DEBLOCK_ASK),
				Player.GetInstance.free_ticket_num,
				string.Format ("{0}{1}{2}{3}{4}{5}", LanguageJP.GAME_TICKET_NAME, LanguageJP.X, LanguageJP.PINK_COLOR_PREFIX, gameDetail.open_need_free_ticket, LanguageJP.COLOR_SUFFIX, LanguageJP.GAME_UNLOCK),
				Player.GetInstance.ticket_num,
				string.Format ("{0}{1}{2}{3}{4}{5}", LanguageJP.GAME_COIN_NAME, LanguageJP.X, LanguageJP.PINK_COLOR_PREFIX, gameDetail.open_need_ticket, LanguageJP.COLOR_SUFFIX, LanguageJP.GAME_UNLOCK)
			});
			(popupContentMediator as DeblockingMediator).unityAction = (int i) => {
				if (i > 0) {
					int[] array = GetOwnAndCost (i == 1, gameDetail);
					int own = array [0];
					int cost = array [1];
					if (own < cost) {
						PopupNoEnough (own, cost, i == 2);
					} else {
						SendAPI (i, num);
					}
				} else {
					gotoShop ();
				}
			};
		} else {
			gameDetailTopMediator.SetWindow (gameDetail, sceneEnum);
			showWindow (1);
		}
	}

	private void PopupNoEnough (int own, int cost, bool isTicket)
	{
		PopupContentMediator popupContentMediator = null;
		if (isTicket) {
			popupContentMediator = page.popupLoader.Popup (PopupEnum.NoEnoughTicket, null, new List<object> () {
				own,
				cost
			});
			(popupContentMediator as NoEnoughTicketMediator).unityAction = () => {
				gotoShop ();
			};
		} else {
			popupContentMediator = page.popupLoader.Popup (PopupEnum.NoEnoughCoin, null, new List<object> () {
				own,
				cost
			});
			(popupContentMediator as NoEnoughCoinMediator).unityAction = () => {
				gotoShop ();
			};
		}
	}

	private int[] GetOwnAndCost (bool isCoin, GameDetail gameDetail)
	{
		if (isCoin) {
			int coin_own = Player.GetInstance.ticket_num;
			int coin_cost = gameDetail.open_need_ticket;
			return new int[]{ coin_own, coin_cost };
		} else {
			int ticket_own = Player.GetInstance.free_ticket_num;
			int ticket_cost = gameDetail.open_need_free_ticket;
			return new int[]{ ticket_own, ticket_cost };
		}
	}

	private void SendAPI (int mode, int num)
	{
		gameOpenLogic.m_game_id = num + 1;
		gameOpenLogic.mode = mode;
		gameOpenLogic.complete = () => {
			gameTop.header.UpdateCoinAndMoney ();
			gameTop.gameItemList [num].SetLock (false);
			gameTop.gameItemList [num].SetNew (gameDetailList [num].isnew == 1);
			currentGameNumber++;
			SetGameNumberText (currentGameNumber, totalGameNumber);
		};
		gameOpenLogic.SendAPI ();
	}
}
