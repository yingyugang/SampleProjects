using UnityEngine;
using System.Collections;

public class LoadingMediator : MonoBehaviour, ILoadingMediator
{
	public void Show ()
	{
		gameObject.SetActive (true);
	}

	public void Hide ()
	{
		gameObject.SetActive (false);
	}
}
