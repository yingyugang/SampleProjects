using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using home;

public class MissionTopMediator : ActivityMediator
{
	private MissionTop missionTop;
	public MissionScrollView missionScrollView;
	private Coroutine currentEnumerator;
	public MissionReceiveLogic missionReceiveLogic;
	public HeaderMediator headerMediator;
	private List<MissionCSVStructure> missionCSVStructureList;

	private void OnEnable ()
	{
		SendMessageUpwards (GameConstant.ClearNoticeManager, NoticeManager.MISSION, SendMessageOptions.DontRequireReceiver);
		GetLastestData ();
		currentEnumerator = StartCoroutine (missionScrollView.Init (missionCSVStructureList, 10));
	}

	private void GetLastestData ()
	{
		if (missionCSVStructureList == null)
		{
			missionCSVStructureList = MasterCSV.missionCSV.ToList ();
		}
		List<Mission> list = UpdateInformation.GetInstance.mission_list;
		int length = list.Count;
		CheckButtonActive ();

		for (int i = 0; i < length; i++)
		{
			MissionCSVStructure missionCSVStructure = missionCSVStructureList.FirstOrDefault (result => result.id == list[i].m_mission_id);
			if (missionCSVStructure != null)
			{
				missionCSVStructure.mission = list[i];
			}
		}
	}

	private void CheckButtonActive ()
	{
		missionTop = viewWithDefaultAction as MissionTop;
		List<Mission> list = UpdateInformation.GetInstance.mission_list;
		if (list.Where (result => result.status == 1).Count () == 0)
		{
			missionTop.greyButton.SetActive (false);
		}
		else {
			missionTop.greyButton.SetActive (true);
		}
		missionTop.number.text = string.Format ("{0}{1}{2}{3}{4}{5}", LanguageJP.MISSION_COMPLETE_NUMBER, LanguageJP.PINK_COLOR_PREFIX, list.Count.ToString (LanguageJP.THREE_MASK), LanguageJP.COLOR_SUFFIX, LanguageJP.DEVIDE, missionCSVStructureList.Count.ToString (LanguageJP.THREE_MASK));
	}

	protected override void InitData ()
	{
		missionScrollView.unityAction = (MissionScrollItem missionScrollItem) =>
		{
			DealWithData (missionScrollItem);
		};
	}

	protected override void CreateActions ()
	{
		missionTop = viewWithDefaultAction as MissionTop;

		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				PopupContentMediator popupContentMediator = page.popupLoader.Popup (PopupEnum.MissionSelectFilter);
				(popupContentMediator as SelectFilterMediator).unityAction = (List<int> list1, List<int> list2, List<int> list3) => {
					IEnumerable<MissionCSVStructure> missionCSVStructureEnumerable = null;
					int length = list1.Count;
					for (int i = 0; i < length; i++) {
						if (list1 [i] == 1) {
							list1 [i] = 0;
						} else if (list1 [i] == 2) {
							list1 [i] = 50;
						} else {
							list1 [i] = list1 [i] - 2;
						}
					}
					missionCSVStructureEnumerable = missionCSVStructureList.Where (result => list1.Contains (result.mission_type));
					missionCSVStructureEnumerable = missionCSVStructureEnumerable.Where (result => list2.Contains (result.mission.status + 1));

					StopCoroutine (currentEnumerator);
					currentEnumerator = StartCoroutine (missionScrollView.Init (missionCSVStructureEnumerable, 10));
				};
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				missionReceiveLogic.m_mission_id = -1;
				missionReceiveLogic.complete = () => {
					missionTop.buttonArray [1].interactable = false;
					missionTop.greyButton.SetActive (false);
					int length = missionScrollView.missionScrollItemList.Count;
					for (int i = 0; i < length; i++) {
						MissionScrollItem missionScrollItem = missionScrollView.missionScrollItemList [i];
						if (missionScrollItem.missionCSVStructure.mission.status == 1) {
							missionScrollItem.ShowNormalCompletion ();
						}
					}
					List<MissionCSVStructure> currentMissionCSVStructureList = missionCSVStructureList.Where (result => result.mission.status == 1).ToList ();
					int count = currentMissionCSVStructureList.Count;
					for (int i = 0; i < count; i++) {
						currentMissionCSVStructureList [i].mission.status = currentMissionCSVStructureList [i].mission.status = 2;
					}
					UpdateData (true);
				};
				missionReceiveLogic.SendAPI ();
			}
		};
	}

	private void DealWithData (MissionScrollItem missionScrollItem)
	{
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
		missionReceiveLogic.m_mission_id = missionScrollItem.missionCSVStructure.id;
		missionReceiveLogic.complete = () =>
		{
			missionScrollItem.ShowNormalCompletion ();
			UpdateData (false);
		};
		missionReceiveLogic.SendAPI ();
	}

	private void UpdateData (bool isAll)
	{
		if (!isAll)
		{
			CheckButtonActive ();
		}
		headerMediator.UpdateCoinAndMoney ();
		popup (PopupEnum.MissionReceiveComplete);
	}
}
