using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Linq;

public class GameDescriptionMediator : PopupContentMediator
{
	private GameDescription gameDescription;
	public UnityAction<string,string> unityAction;
	public GameManualMediator gameManualMediator;

	private void OnEnable ()
	{
		gameDescription = popupContent as GameDescription;
		gameDescription.gameName.text = objectList [0].ToString ();
		gameManualMediator.Init (LanguageJP.GAME_RULE_PREFIX + (int)objectList [1]);
	}
}
