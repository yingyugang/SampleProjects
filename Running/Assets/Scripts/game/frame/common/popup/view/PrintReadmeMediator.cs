using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PrintReadmeMediator : PopupContentActivityMediator
{
	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Application.OpenURL ("https://conveniprint.com/guide/");
			}
		};
	}
}
