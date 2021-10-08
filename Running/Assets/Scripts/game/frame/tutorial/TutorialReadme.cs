using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialReadme : ViewWithDefaultAction
{
	public Image banner;
	public Image man;
	public Image arrow;
	public Image point_grey;
	public Image point_pink;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			}
		};
	}
}
