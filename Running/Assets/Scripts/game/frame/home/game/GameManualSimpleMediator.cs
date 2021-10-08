using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class GameManualSimpleMediator : GameManualCommonMediator
{
	protected override void CreateActions ()
	{
		unityActionArray = new UnityAction[] {
			() => {
				if (unityAction != null) {
					unityAction ();
				}
			}
		};
	}

	override protected void ShowOrHideHandler (bool isShow)
	{
		Image image = imageList [currentIndex];
		Image previousImage = null;
		if (currentIndex != 0) {
			previousImage = imageList [currentIndex - 1];
		}
		image.color = Color.white;
		if (isShow) {
			image.color = new Color (1, 1, 1, 0);
			image.gameObject.SetActive (true);
			image.DOFade (1, currentIndex >= totalOfGameObjects - 1 ? INTERVAL : INTERVAL / 2).OnComplete (() => {
				if (previousImage != null) {
					previousImage.gameObject.SetActive (false);
				}
				if (currentIndex >= totalOfGameObjects - 1) {
					StartCoroutine (GetReady ());
				}
			});
		}
	}

	private IEnumerator GetReady ()
	{
		yield return new WaitForSeconds (INTERVAL * 2);
		if (unityAction != null) {
			unityAction ();
		}
	}
}
