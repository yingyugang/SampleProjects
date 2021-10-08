using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class OtherMigration : ViewWithDefaultAction
{
	public InputField mailInputField1;
	public InputField mailInputField2;
	public InputField pwInputField1;
	public InputField pwInputField2;
	public Text tip;
	public Text email;

	protected override void AddActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				Send (0);
			},
			() => {
				send (1);
			}
		};
	}
}
