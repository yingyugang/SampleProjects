using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;

public class OtherRankingMediator : ActivityMediator
{
	private OtherRanking otherRanking;
	public OtherRankingDetailMediator otherRankingDetailMediator;

	private void OnEnable ()
	{
		otherRanking = viewWithDefaultAction as OtherRanking;
		RankingTop rankingTop = RankingTop.GetInstance;
		otherRanking.totalPoint.text = UpdateInformation.GetInstance.game_total_score.ToString ();
		otherRanking.totalRank.text = rankingTop.total_rank.ToString ();
		otherRanking.cardNumber.text = string.Format ("{0}{1}{2}{3}{4}{5}{6}{7}", LanguageJP.SIZE_54_PREFIX, LanguageJP.MAGENTA_COLOR_PREFIX, UpdateInformation.GetInstance.card_list.Count, LanguageJP.COLOR_SUFFIX, LanguageJP.SIZE_SUFFIX, LanguageJP.COIN, LanguageJP.DEVIDE, MasterCSV.cardCSV.Count ());
		otherRanking.cardRank.text = rankingTop.card_rank.ToString ();

		RogerContainerCleaner.Clean (otherRanking.container);

		List<GameDetail> list = UpdateInformation.GetInstance.game_list;

		int length = list.Count;
		Instantiator instantiator = Instantiator.GetInstance ();
		for (int i = 0; i < length; i++) {
			RankingGameScrollItem rankingGameScrollItem = instantiator.Instantiate (otherRanking.instantiation, Vector2.zero, Vector3.one, otherRanking.container);
			rankingGameScrollItem.unityAction = (RogerScrollItem rogerScrollItem) => {
				RankingGameScrollItem currentRankingGameScrollItem = rogerScrollItem as RankingGameScrollItem;
				otherRankingDetailMediator.SetWindow (currentRankingGameScrollItem.gameDetail, currentRankingGameScrollItem.iconField.sprite, currentRankingGameScrollItem.rankOrder, currentRankingGameScrollItem.pointNumber, currentRankingGameScrollItem.rankNumber);
				otherRankingDetailMediator.showWindow (9);
			};
			int[] array = rankingTop [i];
			rankingGameScrollItem.Show (list [i], array [0], array [1]);
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
