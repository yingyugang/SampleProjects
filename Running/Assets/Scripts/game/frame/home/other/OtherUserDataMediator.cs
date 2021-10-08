using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public class OtherUserDataMediator : ActivityMediator
{
	private OtherUserData otherUserData;
	private readonly List<string> rankNameList = new List<string> () {
		"未",
		"E",
		"D",
		"C",
		"B",
		"A",
		"S",
		"SS",
		"SSS"
	};

	private void OnEnable ()
	{
		otherUserData = viewWithDefaultAction as OtherUserData;
		otherUserData.coin.text = string.Format ("{0}{1}",Player.GetInstance.coin, LanguageJP.COIN);
		otherUserData.freeCoin.text = string.Format ("{0}{1}",Player.GetInstance.free_coin, LanguageJP.COIN);
		otherUserData.cardNumber.text = string.Format ("{0}{1}{2}{3}{4}{5}", LanguageJP.RED_COLOR_PREFIX, UpdateInformation.GetInstance.card_list.Count, LanguageJP.COLOR_SUFFIX, LanguageJP.DEVIDE, MasterCSV.cardCSV.Count (), LanguageJP.COIN);
		otherUserData.missionNumber.text = string.Format ("{0}{1}{2}{3}{4}{5}", LanguageJP.RED_COLOR_PREFIX, UpdateInformation.GetInstance.mission_list.Count, LanguageJP.COLOR_SUFFIX, LanguageJP.DEVIDE, MasterCSV.missionCSV.Count (), LanguageJP.GE);
		otherUserData.pointNumber.text = string.Format ("{0}{1}", UpdateInformation.GetInstance.game_total_score, LanguageJP.POINT);

		int length = otherUserData.recycleList.Count;
		for (int i = 0; i < length; i++) {
			otherUserData.recycleList [i].text = string.Format ("{0}{1}", UpdateInformation.GetInstance.recycle_pt [i].ToString (LanguageJP.THREE_MASK), LanguageJP.COIN);
		}
		PlayerItems playerItems = PlayerItems.GetInstance;
		otherUserData.originalPoint.text = string.Format ("{0}{1}", playerItems.original_point.ToString (LanguageJP.THREE_MASK), LanguageJP.COIN);
		otherUserData.originalTicket.text = string.Format ("{0}{1}", playerItems.original_ticket.ToString (LanguageJP.THREE_MASK), LanguageJP.COIN);
		otherUserData.eventItem2.text = string.Format ("{0}{1}", playerItems.event_item2.ToString (LanguageJP.THREE_MASK), LanguageJP.COIN);
		otherUserData.eventItem3.text = string.Format ("{0}{1}", playerItems.event_item5.ToString (LanguageJP.THREE_MASK), LanguageJP.COIN);
		otherUserData.eventItem5.text = string.Format ("{0}{1}", playerItems.event_item10.ToString (LanguageJP.THREE_MASK), LanguageJP.COIN);

		CreateIconData ();
		//CreateGameData ();
	}

	private void CreateIconData ()
	{
		RogerContainerCleaner.Clean (otherUserData.iconContainer);
		List<GameDetail> gameDetailList = UpdateInformation.GetInstance.game_list;
		RankingTop rankingTop = RankingTop.GetInstance;
		int height = 0;
		int length = gameDetailList.Count + 1;
		for (int i = 0; i < length; i++) {
			OtherGameIcon otherGameIcon = Instantiator.GetInstance ().Instantiate (otherUserData.instantiation, Vector2.zero, Vector3.one, otherUserData.iconContainer);
			otherGameIcon.SetData (i == 0 ? 1 : 2, i);
			otherGameIcon.gameObject.SetActive (true);
			height = 156 * (i + 1);
		}
	}

	private void CreateGameData ()
	{
		List<GameDetail> gameDetailList = UpdateInformation.GetInstance.game_list;
		RankingTop rankingTop = RankingTop.GetInstance;
		int length = gameDetailList.Count;
		for (int i = 0; i < length; i++) {
			OtherGameDataMediator otherGameDataMediator = Instantiator.GetInstance ().Instantiate (otherUserData.otherGameDataMediator, Vector2.zero, Vector3.one, otherUserData.container);
			int[] array = rankingTop [i];
			otherGameDataMediator.SetData (gameDetailList [i].name, rankNameList [gameDetailList [i].rank], array [0], array [1]);
			otherGameDataMediator.gameObject.SetActive (true);
		}
	}

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow (0);
			}
		};
	}
}
