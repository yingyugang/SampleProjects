using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class OtherNoticeMediator : ActivityMediator
{
	private OtherNotice otherNotice;
	public NoticeCreator noticeCreator;
	public InformationLogic informationLogic;
	public NoticeManager noticeManager;

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow (0);
			}
		};
	}

	private void OnEnable ()
	{
		otherNotice = viewWithDefaultAction as OtherNotice;
		RogerContainerCleaner.Clean (otherNotice.container);
		if (noticeManager.GetStatus (NoticeManager.INFO)) {
			informationLogic.complete = () => {
				GetData ();
			};
			informationLogic.SendAPI ();
		} else {
			GetData ();
		}
		SendMessageUpwards (GameConstant.ClearNoticeManager, NoticeManager.INFO, SendMessageOptions.DontRequireReceiver);
	}

	private void GetData ()
	{
		otherNotice = viewWithDefaultAction as OtherNotice;
		noticeCreator.Create (otherNotice.instantiation, otherNotice.container);
	}
}
