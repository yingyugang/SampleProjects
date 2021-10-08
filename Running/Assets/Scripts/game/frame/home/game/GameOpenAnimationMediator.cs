using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Linq;
using System;

public class GameOpenAnimationMediator : ActivityMediator
{
	private int id;
	private SceneEnum sceneEnum;
	private GameOpenAnimation gameOpenAnimation;
	public GameManualSimpleMediator gameManualSimpleMediator;
	public GameReadyMediator gameReadyMediator;

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				GetReady ();
			}
		};
	}

	public void SetWindow (int id, SceneEnum sceneEnum)
	{
		this.id = id;
		this.sceneEnum = sceneEnum;
		gameOpenAnimation = viewWithDefaultAction as GameOpenAnimation;
		setDockActive (false);
		gameManualSimpleMediator.unityAction = () => {
			GetReady ();
		};
	}

	private void GetReady ()
	{
		gameReadyMediator.SetWindow (id, sceneEnum);
		showWindow (2);
	}

	private void OnEnable ()
	{
		gameManualSimpleMediator.Init (string.Format ("{0}{1}", LanguageJP.GAME_OPEN_ANIMATION_PREFIX, id));
	}
}