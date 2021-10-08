using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using home;

public class GameReady : ViewWithDefaultAction
{
	public SizeAnimation sizeAnimation;
	public Image mask;
	public GameObject blacks;
	public Image[] blackArray;
	public HeaderMediator header;
	public Image readme;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			}
		};
	}
}
