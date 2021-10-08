using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;
using System.Text;
using System.Collections.Generic;

public class OtherMigrationMediator : ActivityMediator
{
	private OtherMigration otherMigration;
	public UpdateMigrationLogic updateMigrationLogic;

	private void OnEnable ()
	{
		otherMigration = viewWithDefaultAction as OtherMigration;
		SetMail ();
	}

	private void SetMail ()
	{
		string mail = Player.GetInstance.migration_mail;
		otherMigration.tip.gameObject.SetActive (!string.IsNullOrEmpty (mail));
		otherMigration.email.text = mail;
	}

	protected override void CreateActions ()
	{
		otherMigration = viewWithDefaultAction as OtherMigration;

		unityActionArray = new UnityAction[] {
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se02_ng);
				showWindow (0);
			},
			() => {
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se01_ok);
				if (CheckOutInputField ()) {
					string mail = otherMigration.mailInputField1.text;
					string password = otherMigration.pwInputField1.text;

					string mailCode = mail;
					string passwordCode = Convert.ToBase64String (Encoding.Default.GetBytes (password));

					updateMigrationLogic.mail = mailCode;
					updateMigrationLogic.pw = passwordCode;

					updateMigrationLogic.complete = () => {
						if (APIInformation.GetInstance.result_code == 0) {
							ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () {
								LanguageJP.MIGRATION_REPEAT_TITLE,
								LanguageJP.MIGRATION_REPEAT_CONTENT
							});
						} else {
							ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () {
								LanguageJP.MIGRATION_COMPLETE_TITLE,
								LanguageJP.MIGRATION_COMPLETE_CONTENT
							});
							SetMail ();
						}
						Debug.Log ("updateMigration complete");
					};
					updateMigrationLogic.SendAPI ();
				} else {
					ComponentConstant.POPUP_LOADER.Popup (PopupEnum.MessagePopup, null, new List<object> () {
						LanguageJP.MIGRATION_WRONG_TITLE,
						LanguageJP.MIGRATION_WRONG_CONTENT
					});
				}
			}
		};
	}

	private bool CheckOutInputField ()
	{
		return MailChecker.IsEmail (otherMigration.mailInputField1.text) && !string.IsNullOrEmpty (otherMigration.mailInputField1.text) && !string.IsNullOrEmpty (otherMigration.mailInputField2.text) && (otherMigration.mailInputField1.text == otherMigration.mailInputField2.text) && (otherMigration.pwInputField1.text == otherMigration.pwInputField2.text);
	}
}
