using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class OtherTopMediator : ActivityMediator
{
	private OtherTop otherTop;

	private void OnEnable ()
	{
		otherTop = viewWithDefaultAction as OtherTop;
		List<EventInfo> eventInfoList = UpdateInformation.GetInstance.event_info_list;
		int length = eventInfoList.Count;
		bool isOpened = false;
		for (int i = 0; i < length; i++) {
			if (eventInfoList [i].eventStatusEnum == EventStatusEnum.OPENED) {
				isOpened = true;
				break;
			}
		}
		otherTop.SetEventButtonActive (GameConstant.eventTypeEnum != EventTypeEnum.NONE, isOpened);
	}

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				showWindow (1);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				showWindow (2);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				showWindow (3);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				showWindow (4);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				showWindow (5);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				showWindow (6);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				showWindow (7);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				showWindow (8);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				showWindow (9);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				showWindow (11);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				page.popupLoader.Popup (PopupEnum.QA);
			}
		};
	}
}
