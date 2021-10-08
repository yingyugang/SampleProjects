using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectFilterMediator : PopupContentMediator
{
	public UnityAction<List<int>, List<int>, List<int>> unityAction;

	private SelectFilter selectFilter;
	private bool hasSelected;

	public bool HasSelected
	{
		get
		{
			List<Toggle> toggleList = selectFilter.toggleList;
			int length = toggleList.Count;
			for (int i = 0; i < length; i++)
			{
				if (toggleList[i].isOn == true)
				{
					return true;
				}
			}
			return false;
		}
	}

	protected override void Start ()
	{
		base.Start ();
		unityActionArray = new UnityAction[] {
			() => {
				ToggleToggles ();
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				GetSelection ();
				ClosePopup ();
			},
			() => {
				ChangeToggleValue ();
			}
		};

		selectFilter = popupContent as SelectFilter;
		selectFilter.send = (int buttonNumber) =>
		{
			unityActionArray[buttonNumber] ();
		};
	}

	private void GetSelection ()
	{
		List<Toggle> toggleList1 = selectFilter.toggleList1;
		List<Toggle> toggleList2 = selectFilter.toggleList2;
		List<Toggle> toggleList3 = selectFilter.toggleList3;

		List<int> resultList1 = GetResult (toggleList1);
		List<int> resultList2 = GetResult (toggleList2);
		List<int> resultList3 = GetResult (toggleList3);

		if (unityAction != null)
		{
			unityAction (resultList1, resultList2, resultList3);
		}
	}

	private List<int> GetResult (List<Toggle> toggleList)
	{
		List<int> resultList = new List<int> ();
		int length = toggleList.Count;
		for (int i = 0; i < length; i++)
		{
			if (toggleList[i].isOn)
			{
				resultList.Add (i + 1);
			}
		}
		return resultList;
	}

	private void ChangeToggleValue ()
	{
		bool hasSelected = HasSelected;
		if (hasSelected)
		{
			ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
		}
		else {
			ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
		}
		selectFilter.text1.gameObject.SetActive (hasSelected);
		selectFilter.text2.gameObject.SetActive (!hasSelected);
	}

	private void ToggleToggles ()
	{
		bool hasSelected = HasSelected;
		List<Toggle> toggleList = selectFilter.toggleList;
		int length = toggleList.Count;
		selectFilter.RemoveEvents ();
		for (int i = 0; i < length; i++)
		{
			toggleList[i].isOn = !hasSelected;
		}

		selectFilter.AddEvents ();
		if (!hasSelected)
		{
			ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
		}
		else {
			ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
		}

		selectFilter.text1.gameObject.SetActive (!hasSelected);
		selectFilter.text2.gameObject.SetActive (hasSelected);
	}

	private void OnDestroy ()
	{
		selectFilter.send = null;
	}
}
