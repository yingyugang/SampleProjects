using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GameManualMediator : GameManualCommonMediator
{
	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ShowOrHide (true);
			},
			() => {
				ShowOrHide (false);
			}
		};
	}

	override protected void Reset ()
	{
		base.Reset ();
		ShowText ("", "", "");
		gameManualCommon.buttonArray [0].gameObject.SetActive (false);
		gameManualCommon.buttonArray [1].gameObject.SetActive (false);
	}

	override protected void GetResourcesListComplete ()
	{
		ShowText ((currentIndex + 1).ToString (), LanguageJP.DEVIDE, totalOfGameObjects.ToString ());
	}

	override protected void ShowOrHide (bool isBack, bool killAuto = false)
	{
		base.ShowOrHide (isBack, false);
		ShowText ((currentIndex + 1).ToString (), LanguageJP.DEVIDE, totalOfGameObjects.ToString ());
	}

	private void ShowText (string current, string symbol, string total)
	{
		(gameManualCommon as GameManual).text.text = string.Format ("{0}{1}{2}", current, symbol, total);
	}

	override protected void CheckAvailable ()
	{
		base.CheckAvailable ();
		gameManualCommon.buttonArray [0].gameObject.SetActive (currentIndex > 0);
		gameManualCommon.buttonArray [1].gameObject.SetActive (currentIndex < totalOfGameObjects - 1);
	}
}
