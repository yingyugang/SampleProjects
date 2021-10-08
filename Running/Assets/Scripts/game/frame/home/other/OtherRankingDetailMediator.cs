using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class OtherRankingDetailMediator : ActivityMediator
{
	private OtherRankingDetail otherRankingDetail;
	private Coroutine currentEnumerator;
	public RankingScrollView rankingScrollView;
	private GameDetail gameDetail;

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow (2);
			}
		};
	}

	private void OnEnable ()
	{
		StartCoroutine (rankingScrollView.Init (gameDetail.id, 20));
	}

	public void SetWindow (GameDetail gameDetail, Sprite iconSprite, int rankOrder, int pointNumber, int rankNumber)
	{
		this.gameDetail = gameDetail;
		otherRankingDetail = viewWithDefaultAction as OtherRankingDetail;
		otherRankingDetail.Show (iconSprite, rankOrder, gameDetail.score, rankNumber, gameDetail.name);
	}

	public void SetWindowByID (int id)
	{
		GameDetail gameDetail = UpdateInformation.GetInstance.game_list[id - 1];
		Sprite iconSprite = AssetBundleResourcesLoader.gameIconDictionary[string.Format ("{0}{1}", LanguageJP.GAME_ICON_PREFIX, gameDetail.id)];
		int rankOrder = gameDetail.rank;
		int[] array = RankingTop.GetInstance[id - 1];
		int pointNumber = array[0];
		int rankNumber = array[1];
		SetWindow (gameDetail, iconSprite, rankOrder, pointNumber, rankNumber);
	}
}
