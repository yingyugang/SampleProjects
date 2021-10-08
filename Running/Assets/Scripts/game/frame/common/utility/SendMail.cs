using UnityEngine;
using System.Collections;

public class SendMail
{
	public void Send (string address, string subject, string body)
	{
		Application.OpenURL ("mailto:" + address + "?subject=" + subject + "&body=" + body);
	}
}