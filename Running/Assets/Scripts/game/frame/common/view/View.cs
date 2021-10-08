using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class View : MonoBehaviour
{
	public Button[] buttonArray;
	protected UnityAction[] unityActionArray;

	virtual protected void Start ()
	{
		AddActions ();
		AddEventListeners ();
	}

	virtual protected void AddActions ()
	{
		
	}

	virtual public void AddEventListeners ()
	{
		for (int i = 0; i < buttonArray.Length; i++) {
			buttonArray [i].onClick.AddListener (unityActionArray [i]);
		}
	}

	virtual public void RemoveEventListeners ()
	{
		for (int i = 0; i < buttonArray.Length; i++) {
			if (unityActionArray != null) {
				buttonArray [i].onClick.RemoveListener (unityActionArray [i]);
				buttonArray [i].onClick = null;
			}
		}

		unityActionArray = null;
	}

	virtual protected void OnDestroy ()
	{
		RemoveEventListeners ();
	}
}
