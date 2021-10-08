using UnityEngine;
using System.Collections;

public class GameSupportMediator : PopupContentMediator
{
	public GameManualMediator gameManualMediator;
	private GameSupport gameSupport;

	private void OnEnable ()
	{
		gameSupport = popupContent as GameSupport;
		string menuName = objectList [0].ToString ();
		gameSupport.title.text = menuName;

		gameManualMediator.Init (menuName);
	}
}
