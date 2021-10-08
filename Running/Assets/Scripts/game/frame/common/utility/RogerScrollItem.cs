using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class RogerScrollItem : MonoBehaviour
{
	public UnityAction<RogerScrollItem> unityAction;
	protected int currentIndex;

	virtual public void Init (int index)
	{
		currentIndex = index;
	}

	virtual public int CurrentIndex {
		get {
			return currentIndex;
		}
		set {
			currentIndex = value;
			gameObject.SetActive (true);
			UpdateData (currentIndex);
		}
	}

	virtual protected void UpdateData (int index)
	{
		
	}

	virtual protected void Start ()
	{
		
	}

	virtual protected void OnDestroy ()
	{
		unityAction = null;
	}
}
