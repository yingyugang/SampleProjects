using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class QAMediator : PopupContentActivityMediator
{
	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				SendMail ();
			}
		};
	}

	private void SendMail ()
	{
		string address = GameConstant.EVENT_MAIL_ADDRESS;
		string subject = System.Uri.EscapeDataString (LanguageJP.QA_MAIL_SUBJECT);
		string originalBody = string.Format ("-------\n下記の情報は消さずにメール送信してください。 \n・ユーザー名:{0} \n・ユーザーID:{1} \n・アプリバージョン:ver. {2} \n・端末機種情報:{3} \n・OS情報:{4}", Player.GetInstance.name, PlayerPrefs.GetString (LanguageJP.P_CODE, string.Empty), SystemConstant.CLIENT_VERSION, SystemInfo.deviceModel, SystemInfo.operatingSystem);
		string body = System.Uri.EscapeDataString (originalBody);
		new SendMail ().Send (address, subject, body);
	}
}
