using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ActionMediator : MonoBehaviour
{
	public ViewWithDefaultAction viewWithDefaultAction;
	protected UnityAction[] unityActionArray;

	virtual protected void Start ()
	{
		CreateActions ();
		viewWithDefaultAction.send = (int buttonNumber) => {
			unityActionArray [buttonNumber] ();
		};
	}

	virtual protected void CreateActions ()
	{
		
	}

	virtual protected void OnDestroy ()
	{
		viewWithDefaultAction.send = null;
	}
}

