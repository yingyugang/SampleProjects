using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GameManualTextMediator : GameManualMediator
{
	private List<HelpCSVStructure> contentList;
	private GameManualText gameManualText;

	override protected void GetResourcesList (string menuName)
	{
		gameManualText = gameManualCommon as GameManualText;
		contentList = MasterCSV.helpCSV.Where (result => result.menu_name == menuName).ToList ();
		totalOfGameObjects = contentList.Count;
		CheckAvailable ();
		ShowOrHideHandler (true);
		CheckIsAuto ();
		ComponentConstant.API_MANAGER.shortLoadingMediator.Hide ();
		GetResourcesListComplete ();
	}

	override protected void ShowOrHideHandler (bool isShow)
	{
		gameManualText.textField.text = contentList [currentIndex].description;
	}
}
