using UnityEngine;
using System.Collections;

public class TutorialMediator : MonoBehaviour
{
	public TutorialPageMediator tutorialPageMediator;

	private void Awake ()
	{
		ComponentConstant.SCENE_COMBINE_MANAGER.unityAction = (LongLoadingMediator longLoadingMediator) => {
			ComponentConstant.SCENE_COMBINE_MANAGER.unityAction = null;
			tutorialPageMediator.unityAction = () => {
				tutorialPageMediator.unityAction = null;
				longLoadingMediator.Hide ();
			};
		};
	}
}
