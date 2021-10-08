using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using home;
using UnityEngine.UI;

public class OtherPresentBoxMediator : ActivityMediator
{
	private OtherPresentBox otherPresentBox;
	public PresentReceiveLogic presentReceiveLogic;
	public HeaderMediator headerMediator;
	public PresentLogic presentLogic;
	public NoticeManager noticeManager;

	private void GetData ()
	{
		otherPresentBox = viewWithDefaultAction as OtherPresentBox;
		List<MessageBox> messageList = UpdateInformation.GetInstance.message_box_list;
		int length = messageList.Count;
		CheckButtonActive ();
		for (int i = 0; i < length; i++) {
			PresentScrollItem presentScrollItem = Instantiator.GetInstance ().Instantiate (otherPresentBox.instantiation, Vector2.zero, Vector3.one, otherPresentBox.container);
			presentScrollItem.unityAction = (RogerScrollItem rogerScrollItem) => {
				presentReceiveLogic.id = (rogerScrollItem as PresentScrollItem).id;
				presentReceiveLogic.complete = () => {
					Destroy (rogerScrollItem.gameObject);
					UpdateData ();
				};
				presentReceiveLogic.SendAPI ();
			};
			MessageBox messageBox = messageList [i];
			presentScrollItem.Show (messageBox);
		}
	}

	private void OnEnable ()
	{
		otherPresentBox = viewWithDefaultAction as OtherPresentBox;
		RogerContainerCleaner.Clean (otherPresentBox.container);
		if (noticeManager.GetStatus (NoticeManager.PRESENT)) {
			presentLogic.complete = () => {
				GetData ();	
			};
			presentLogic.SendAPI ();
		} else {
			GetData ();
		}
		SendMessageUpwards (GameConstant.ClearNoticeManager, NoticeManager.PRESENT, SendMessageOptions.DontRequireReceiver);
	}

	private void CheckButtonActive ()
	{
		List<MessageBox> list = UpdateInformation.GetInstance.message_box_list;
		if (list.Count == 0) {
			otherPresentBox.greyButton.SetActive (false);
		} else {
			otherPresentBox.greyButton.SetActive (true);
		}
	}

	private void UpdateData ()
	{
		CheckButtonActive ();
		headerMediator.UpdateCoinAndMoney ();
		popup (PopupEnum.PresentReceiveComplete);
	}

	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow (0);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				presentReceiveLogic.id = -1;
				presentReceiveLogic.complete = () => {
					RogerContainerCleaner.Clean (otherPresentBox.container);
					UpdateData ();
				};
				presentReceiveLogic.SendAPI ();
			}
		};
	}
}
