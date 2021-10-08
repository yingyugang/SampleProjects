using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GameIntroductionMediator : ActivityMediator
{
	public GameManualMediator gameManualMediator;
	private GameDetail gameDetail;
	private GameIntroduction gameIntroduction;

	public void SetWindow (GameDetail gameDetail)
	{
		this.gameDetail = gameDetail;
		gameIntroduction = viewWithDefaultAction as GameIntroduction;
		gameIntroduction.Show (gameDetail.name);
		gameManualMediator.Init (LanguageJP.GAME_RULE_PREFIX + gameDetail.id);
	}

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow (1);
			}
		};
	}
}
