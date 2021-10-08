using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GameManualCommonMediator : ActionMediator
{
	protected GameManualCommon gameManualCommon;
	protected int currentIndex;
	protected int totalOfGameObjects;
	protected List<Image> imageList;
	public UnityAction unityAction;
	public const float INTERVAL = 0.75f;
	public bool isAuto;
	private Coroutine coroutine;

	virtual public void Init (string assetBundleName)
	{
		gameManualCommon = viewWithDefaultAction as GameManualCommon;
		Reset ();
		RogerContainerCleaner.Clean (gameManualCommon.container);
		ComponentConstant.API_MANAGER.shortLoadingMediator.Show ();
		GetResourcesList (assetBundleName);
	}

	virtual protected void Reset ()
	{
		currentIndex = 0;
	}

	virtual protected void GetResourcesList (string assetBundleName)
	{
		StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> (assetBundleName, (List<Texture2D> list) => {
			totalOfGameObjects = list.Count;
			Instantiator instantiator = Instantiator.GetInstance ();
			imageList = new List<Image> ();
			for (int i = 0; i < totalOfGameObjects; i++) {
				imageList.Add (instantiator.Instantiate<Image> (gameManualCommon.instantiation, Vector2.zero, Vector3.one, gameManualCommon.container));
				imageList [i].sprite = TextureToSpriteConverter.ConvertToSprite (list [i] as Texture2D);
			}
			CheckAvailable ();
			ShowOrHideHandler (true);
			CheckIsAuto ();
			ComponentConstant.API_MANAGER.shortLoadingMediator.Hide ();

			GetResourcesListComplete ();
		}, false));
	}

	virtual protected void GetResourcesListComplete ()
	{
		
	}

	private IEnumerator AutoScroll ()
	{
		for (int i = 0; i < totalOfGameObjects; i++) {
			yield return new WaitForSeconds (INTERVAL);
			ShowOrHide (false);
		}
	}

	protected void CheckIsAuto ()
	{
		if (isAuto) {
			coroutine = StartCoroutine (AutoScroll ());
		}
	}

	protected void StopAuto ()
	{
		isAuto = false;
		if (coroutine != null) {
			StopCoroutine (coroutine);
		}
	}

	virtual protected void ShowOrHideHandler (bool isShow)
	{
		imageList [currentIndex].gameObject.SetActive (isShow);
	}

	virtual protected void ShowOrHide (bool isBack, bool killAuto = false)
	{
		ShowOrHideHandler (false);
		currentIndex = isBack ? currentIndex - 1 : currentIndex + 1;
		ShowOrHideHandler (true);
		CheckAvailable ();
		if (isAuto && killAuto) {
			StopAuto ();
		}
	}

	virtual protected void CheckAvailable ()
	{
		if (currentIndex >= totalOfGameObjects - 1) {
			if (isAuto) {
				StopAuto ();
			}
			PlayComplete ();
		}
	}

	virtual protected void PlayComplete ()
	{
		
	}
}
