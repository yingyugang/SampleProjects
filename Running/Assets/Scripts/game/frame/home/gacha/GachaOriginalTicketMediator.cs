using UnityEngine;
using System.Collections;
using System.Linq;

public class GachaOriginalTicketMediator : ActivityMediator
{
	private GachaOriginalTicket gachaOriginalTicket;

	private void OnEnable ()
	{
		gachaOriginalTicket = viewWithDefaultAction as GachaOriginalTicket;
		gachaOriginalTicket.own.text = string.Format (LanguageJP.ORIGINAL_TICKET_GET_TEXT, GachaResultInfo.GetInstance.original_ticket);
		gachaOriginalTicket.total.text = PlayerItems.GetInstance.original_ticket.ToString ();
	}

	public void SetTitle (string title)
	{
		gachaOriginalTicket = viewWithDefaultAction as GachaOriginalTicket;
		gachaOriginalTicket.title.text = title;
	}
}
