using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ViewWithDefaultAction : View
{
	public UnityAction<int> send;

	protected void Send (int buttonNumber)
	{
		if (send != null) {
			send (buttonNumber);
		}
	}
}
