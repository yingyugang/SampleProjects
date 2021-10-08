using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PopupContentWithDefaultAction : PopupContent
{
	public UnityAction<int> send;

	protected void Send (int buttonNumber)
	{
		if (send != null) {
			send (buttonNumber);
		}
	}
}
