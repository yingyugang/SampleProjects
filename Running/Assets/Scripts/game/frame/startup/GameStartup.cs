using UnityEngine;
using System.Collections;

public class GameStartup : SceneInitializer
{
	protected override void Init ()
	{
		ComponentConstant.SOUND_MANAGER.StopBGM (false, true);
		ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Splash, false);
	}

	override protected IEnumerator CheckSceneResources ()
	{
		yield return null;
		Init ();
	}
}
