using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class GachaRecycleItem : MonoBehaviour
{
	public UnityAction<GachaRecycleItem,int,int,int[]> unityAction;
	public List<Image> coinList;
	public Text typeField;
	public Text ownField;
	public Text costField;
	[HideInInspector]
	public int mode;
	[HideInInspector]
	public int index;
	[HideInInspector]
	public int level;
	public Button button;
	[HideInInspector]
	public int own;
	[HideInInspector]
	public int cost;
	[HideInInspector]
	private int iconID;

	private void Start ()
	{
		AddEventListeners ();
	}

	private void OnDestroy ()
	{
		RemoveEventListeners ();
		unityAction = null;
	}

	private void AddEventListeners ()
	{
		button.onClick.AddListener (ButtonOnClickHandler);
	}

	private void RemoveEventListeners ()
	{
		button.onClick.RemoveListener (ButtonOnClickHandler);
		button.onClick = null;
	}

	private void ButtonOnClickHandler ()
	{
		if (unityAction != null) {
			int[] array = new int[]{ own, cost };
			if (mode == 6) {
				unityAction (this, mode, 5, array);
			} else if (mode == 7) {
				unityAction (this, mode, 4, array);
			} else {
				unityAction (this, mode, index, array);
			}
		}
	}

	public void UpdateOwnFiled (int own)
	{
		this.own = own;
		ownField.text = own.ToString ();
	}

	public void SetData (int mode, int index, int iconID, string level, string own, string cost)
	{
		this.mode = mode;
		this.index = index;
		this.iconID = iconID;
		this.own = int.Parse (own);
		this.cost = int.Parse (cost);
		typeField.text = string.Format ("{0}{1}", level, LanguageJP.RecycleItemTitle);
		ownField.text = own;
		costField.text = string.Format ("{0}{1}{2}{3}{4}{5}", LanguageJP.X, LanguageJP.PINK_COLOR_PREFIX, cost, LanguageJP.COIN, LanguageJP.COLOR_SUFFIX, LanguageJP.RecycleItem1Time);
		SetIcon ();
	}

	private void SetIcon ()
	{
		if (mode == 6) {
			coinList [0].gameObject.SetActive (true);
		} else if (mode == 7) {
			coinList [1].gameObject.SetActive (true);
		} else {
			coinList.GetRange (2, 3) [iconID].gameObject.SetActive (true);
		}
	}
}
