using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class OtherSettingMediator : ActivityMediator
{
	private OtherSetting otherSetting;

	public UpdateUserNameLogic updateUserNameLogic;

	protected override void CreateActions ()
	{
		otherSetting = viewWithDefaultAction as OtherSetting;

		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow (0);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				PopupContentMediator popupContentMediator = page.popupLoader.Popup (PopupEnum.ChangeUserName);
				(popupContentMediator as EnterUserNameMediator).unityAction = (bool isSuccessful, string name) => {
					if (isSuccessful) {
						updateUserNameLogic.userName = name;
						updateUserNameLogic.complete = () => {
							GameConstant.UserName = name;
							otherSetting.header.UpdateName ();
							popup (PopupEnum.ChangeUserNameComplete);
						};
						updateUserNameLogic.SendAPI ();
					} else {
						ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () {
							string.Empty,
							LanguageJP.NAME_ERROR
						});
					}
				};
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				showWindow (10);
			}
		};
	}
}
