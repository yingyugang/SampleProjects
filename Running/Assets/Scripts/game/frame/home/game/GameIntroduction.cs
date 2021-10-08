using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using home;

public class GameIntroduction : ViewWithDefaultAction
{
	public Text title;

	public void Show (string title)
	{
		this.title.text = title;
	}

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			}
		};
	}
}