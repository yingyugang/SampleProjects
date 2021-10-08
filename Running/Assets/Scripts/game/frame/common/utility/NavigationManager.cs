using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NavigationManager : MonoBehaviour
{
	public List<PageMediator> pageMediatorList;
	private Dictionary<string,PageMediator> dictionary;
	private PageMediator currentPageMediator;
	public Footer footer;
	private int currentPageIndex;
	private int currentWindowIndex;

	private void Start ()
	{
		dictionary = new Dictionary<string, PageMediator> ();
		currentPageMediator = pageMediatorList [0];
		currentPageMediator.gameObject.SetActive (true);
		int length = pageMediatorList.Count;
		for (int i = 0; i < length; i++) {
			dictionary.Add (pageMediatorList [i].name, pageMediatorList [i]);
			pageMediatorList [i].unityAction = (int pageIndex, int windowIndex) => {
				SetIndex (pageIndex, windowIndex);
			};

			pageMediatorList [i].navigate = (int pageIndex, int windowIndex) => {
				Navigate (pageIndex, windowIndex);
			};
		}

		footer.send = (int index) => {
			currentPageMediator.gameObject.SetActive (false);
			currentPageMediator = dictionary [Enum.GetName (typeof(PageEnum), index)];
			currentPageMediator.gameObject.SetActive (true);
			SetIndex (index, 0);
		};
	}

	private void Navigate (int pageIndex, int windowIndex)
	{
		SetIndex (pageIndex, windowIndex);
		SetFooter (pageIndex);
		currentPageMediator.ShowWindow (windowIndex);
	}

	private void SetFooter (int index)
	{
		footer.toggleArray [index].isOn = true;
	}

	public void SetIndex (int pageIndex, int windowIndex)
	{
		currentPageIndex = pageIndex;
		currentWindowIndex = windowIndex;
	}
}
