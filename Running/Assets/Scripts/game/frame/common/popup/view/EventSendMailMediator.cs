using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Scripts.ThirdParty;

public class EventSendMailMediator : PopupContentMediator
{
    private EventSendMail eventSendMail;
    [HideInInspector]
    public string address;
    [HideInInspector]
    public string subject;
    [HideInInspector]
    public string body;
    private string originalBody;

    private void OnEnable ()
    {
        eventSendMail = popupContent as EventSendMail;
        eventSendMail.userID.text = objectList[0].ToString ();
        eventSendMail.userName.text = objectList[1].ToString ();
        address = GameConstant.EVENT_MAIL_ADDRESS;
        originalBody = string.Format ("{0}{1}{2}{3}{4}{5}{6}{7}{8}", address, LanguageJP.BREAK_LINE_HARD, LanguageJP.EVENT_MAIL_SUBJECT, LanguageJP.BREAK_LINE_HARD, eventSendMail.content.text, LanguageJP.BREAK_LINE_HARD, eventSendMail.userID.text, LanguageJP.BREAK_LINE_HARD, eventSendMail.userName.text);
        body = System.Uri.EscapeDataString (originalBody);
        subject = System.Uri.EscapeDataString (LanguageJP.EVENT_MAIL_SUBJECT);
    }

    protected override void YesButtonOnClickHandler ()
    {
        new SendMail ().Send (address, subject, body);
    }

    protected override void NoButtonOnClickHandler ()
    {
        ClipboardUtil.ClipboardSet (originalBody);
    }
}
