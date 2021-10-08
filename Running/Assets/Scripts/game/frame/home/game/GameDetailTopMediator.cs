using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Linq;
using System;

public class GameDetailTopMediator : ActivityMediator
{
	private GameDetail gameDetail;
	private SceneEnum sceneEnum;
	public GameStartLogic gameStartLogic;
	public GameIntroductionMediator gameIntroductionMediator;
	private GameDetailTop gameDetailTop;
	public GameOpenAnimationMediator gameOpenAnimationMediator;
	public GameReadyMediator gameReadyMediator;
	public ApMediator apMediator;
	public OtherRankingDetailMediator otherRankingDetailMediator;
	public GameObject playerItem;

	public void SetWindow (GameDetail gameDetail, SceneEnum sceneEnum)
	{
		this.gameDetail = gameDetail;
		this.sceneEnum = sceneEnum;
		gameDetailTop = viewWithDefaultAction as GameDetailTop;

		int id = gameDetail.id;

		//test hoantt
//		if (id==9) {
//			id = 8;
//		}

		Sprite sprite = GamePageMediator.gameIntroductionResourcesDirectory[id][LanguageJP.GAME_BANNER];
//		foreach (var item in AssetBundleResourcesLoader.gameIconDictionary.Keys) {
//			Debug.Log ("item="+item.ToString ());
//		}
		Sprite gameicon = AssetBundleResourcesLoader.gameIconDictionary [string.Format ("{0}{1}", LanguageJP.GAME_ICON_PREFIX, id)];
		gameDetailTop.Show (gameDetail.name, gameDetail.score, gameDetail.rank, gameDetail.event_bonus, gameDetail.next_clear_gacha, sprite, gameicon, CardRate.GetTotal (2, id));

		if (GameConstant.currentEventGameID == gameDetail.id)
		{
			InitGameEventItemList ();
			gameDetailTop.leaf.SetActive (true);
			gameDetailTop.warningText.gameObject.SetActive (false);
			playerItem.SetActive (true);
		}
		else
		{
			if (GameConstant.currentEventGameID != 0) {
				InitGameEventItemList ();
				gameDetailTop.warningText.gameObject.SetActive (true);
				playerItem.SetActive (true);
			} else {
				playerItem.SetActive (false);
			}
			gameDetailTop.leaf.SetActive (false);
		}
	}

	private void InitGameEventItemList ()
	{
		gameDetailTop = viewWithDefaultAction as GameDetailTop;
		int length = gameDetailTop.gameEventItemList.Count;
		for (int i = 0; i < length; i++)
		{
			gameDetailTop.gameEventItemList[i].Init (i);
		}
	}

	public void SetWindowByID (int gameID)
	{
		SetWindow (GameTopMediator.gameDetailList[gameID - 1], GetSceneEnumByGameId (gameID));
	}

	private SceneEnum GetSceneEnumByGameId (int gameId)
	{
		switch (gameId)
		{
			case 1:
				return SceneEnum.Daruma;
			case 2:
				return SceneEnum.GetOut;
			case 3:
				return SceneEnum.Swimming;
			case 5:
				return SceneEnum.Shee;
			case 4:
				return SceneEnum.BreakoutClone;
			case 6:
				return SceneEnum.Biking;
			case 7:
				return SceneEnum.SixRun;
			case 8:
				return SceneEnum.Guess;
			case 9:
				return SceneEnum.OilKing;
			default:
				return SceneEnum.Daruma;
		}
	}

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				CheatController.ResetCheats();
				showWindow (0);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);

				//test hoantt
				int idgame=gameDetail.id;
//				if (idgame==9) {
//					idgame=8;
//				}
				GameCSVStructure gameCSVStructure = MasterCSV.gameCSV.FirstOrDefault (result => result.id == idgame);
				if (ApMediator.currentRecovery < gameCSVStructure.ap) {
					CheatController.ResetCheats();
					PopupError ();
				} else {
					SendAPI ();
				}
			},
			() => {
				CheatController.ResetCheats();
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				showWindow (3);
				gameIntroductionMediator.SetWindow (gameDetail);
			},
			() => {
				CheatController.ResetCheats();
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				showWindow (4);
			},
			() => {
				CheatController.ResetCheats();
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				gotoItemExchange ();
			},
			()=>
			{
				if (gotoRankingDetail != null)
				{
					GameCSVStructure gameCSVStructure = MasterCSV.gameCSV.FirstOrDefault (result => result.id == gameDetail.id);
					otherRankingDetailMediator.SetWindowByID (gameCSVStructure.id);
					gotoRankingDetail ();
				}
			}
		};
	}

	private void PopupError ()
	{
		PopupContentMediator popupContentMediator = page.popupLoader.Popup (PopupEnum.NoEnoughAP);
		(popupContentMediator as NoEnoughAPMediator).unityAction = () =>
		{
			gotoShop ();
		};
	}

	private void SendAPI ()
	{
		gameStartLogic.m_game_id = gameDetail.id;
		int itemID = GetItemID ();
		gameStartLogic.m_item_id = itemID;
		CheatData cheatData = CheatController.GetLastMatchCheat ();
		gameStartLogic.is_cheat = (cheatData==null) ? 0 : 1;
		Debug.Log ("SendAPI Start logic");
		GameConstant.GameRate = PlayerItems.GetGameRate (itemID);
		GameConstant.GameEventBonus = gameDetail.event_bonus;
		gameStartLogic.error = (string status) =>
		{
			PopupError ();
		};
		gameStartLogic.complete = () =>
		{
			ComponentConstant.SOUND_MANAGER.StopBGM ();
			gameDetailTop.header.UpdateAp ();

			//test hoantt
			int id=gameDetail.id;
//			if (id==9) {
//				id=8;
//			}
			gameOpenAnimationMediator.SetWindow (gameDetail.id, sceneEnum);
			GameCSVStructure gameCSVStructure = MasterCSV.gameCSV.FirstOrDefault (result => result.id == id);
			if (gameCSVStructure.open_animation == 1)
			{
				showWindow (5);
			}
			else {
				gameReadyMediator.SetWindow (gameDetail.id, sceneEnum);
				showWindow (2);
			}
		};
		gameStartLogic.SendAPI ();
	}

	private int GetItemID ()
	{
		gameDetailTop = viewWithDefaultAction as GameDetailTop;
		int length = gameDetailTop.gameEventItemList.Count;
		for (int i = 0; i < length; i++)
		{
			int id = gameDetailTop.gameEventItemList[i].GetItemID ();
			if (id != 0)
			{
				return id;
			}
		}
		return 0;
	}
}
