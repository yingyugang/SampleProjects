using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;

public class TutorialGachaMediator : ActivityMediator
{
	public TutorialGachaLogic tutorialGachaLogic;

	protected override void InitData ()
	{
		SendAPI ();
	}

	private void SendAPI ()
	{
		tutorialGachaLogic.m_gacha_id = 1;
		tutorialGachaLogic.mode = 999999;
		tutorialGachaLogic.cost_type = 0;
		tutorialGachaLogic.complete = () => {
			PlayerPrefs.SetInt (GameConstant.HasCompletedTutorial, 1);
			ComponentConstant.GACHA_PLAYER.unityAction = (int type, bool isChanged) => {
				ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Main);
			};
			ComponentConstant.GACHA_PLAYER.Play (2, true);
		};
		tutorialGachaLogic.error = (string status) => {
			MenuMediator.isTutorial = false;
			if (status == "1008") {
				PlayerPrefs.SetInt (GameConstant.HasCompletedTutorial, 1);
				ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Startup);
			}
		};
		tutorialGachaLogic.SendAPI ();
	}
}
