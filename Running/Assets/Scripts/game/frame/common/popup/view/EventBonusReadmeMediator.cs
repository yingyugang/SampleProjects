using UnityEngine;
using System.Collections;

public class EventBonusReadmeMediator : PopupContentActivityMediator
{
	private EventBonusReadme eventBonusReadme;

	private void OnEnable ()
	{
		eventBonusReadme = popupContent as EventBonusReadme;
		eventBonusReadme.title.text = objectList [0].ToString ();
		eventBonusReadme.text.text = objectList [1].ToString ();
	}
}
