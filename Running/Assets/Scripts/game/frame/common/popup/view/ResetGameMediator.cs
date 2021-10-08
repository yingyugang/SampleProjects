using UnityEngine;
using System.Collections;

public class ResetGameMediator : PopupContentMediator
{
	protected override void YesButtonOnClickHandler ()
	{
		base.YesButtonOnClickHandler ();
		SystemConstant.ClearDeviceID ();
		NoticeManager.ClearAllNotice ();
		ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Splash);
	}
}
