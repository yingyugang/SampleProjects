using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class PrintShow : PopupContentWithDefaultAction
{
	public GameObject codelImage;
	public GameObject code2lImage;
	public Text buttonLText;
	public Text button2LText;
	public Text codel;
	public Text code2l;
	public GameObject codelObject;
	public GameObject code2lObject;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			},
			() => {
				Send (1);
			},
			() => {
				Send (2);
			}
		};
	}
}
