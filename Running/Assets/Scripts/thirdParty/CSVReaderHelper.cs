using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class CSVReaderHelper : MonoBehaviour
{
	public UnityAction<string> complete;

	public void Read (string file)
	{
		StartCoroutine (ReadCSV (file));
	}

	private IEnumerator ReadCSV (string file)
	{
		WWW www = new WWW (PathConstant.CLIENT_RESOURCES_PATH + file);
		yield return www;
		if (www.isDone && string.IsNullOrEmpty (www.error)) {
			if (complete != null) {
				complete (www.text);
			}
		} else {
			ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () { 
				"ERROR",
				"Can not find " + file
			});
		}
	}
}
