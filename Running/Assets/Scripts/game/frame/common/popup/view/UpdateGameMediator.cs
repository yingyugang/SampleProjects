using UnityEngine;
using System.Collections;

public class UpdateGameMediator : PopupContentMediator
{
	protected override void OKButtonOnClickHandler ()
	{
		base.OKButtonOnClickHandler ();
		ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Splash);
	}
}
