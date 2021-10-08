using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class OilkingEnding : MonoBehaviour
{
	//public OilKingAnimImage anim;
	public CanvasGroup canvasGroup;

	bool isCanCloseEnding = false;

	public Image imgLogo;

	public void Show ()
	{
		imgLogo.sprite = OilKingAssetLoader.s_Instance.getEndingAnimation (1);
		gameObject.SetActive (true);
//		StartCoroutine (watingRunAlpha (true, 0.1f));
	}

	//	IEnumerator watingRunAlpha (bool isShow, float speed)
	//	{
	//		yield return new WaitForEndOfFrame ();
	//		if (isShow) {
	//			canvasGroup.alpha = 0;
	//			for (float i = 0; i <= 1; i += speed) {
	//				canvasGroup.alpha = i;
	//				yield return new WaitForEndOfFrame ();
	//			}
	//			canvasGroup.alpha = 1;
	//			anim.RunAnim ();
	//
	//		} else {
	//			canvasGroup.alpha = 1;
	//			for (float i = 1; i >= 0; i -= speed) {
	//				canvasGroup.alpha = i;
	//				yield return new WaitForEndOfFrame ();
	//			}
	//			canvasGroup.alpha = 0;
	//			gameObject.SetActive (false);
	//			OilKingManager.s_Instance.SendGameEndingAPI ();
	//		}
	//	}

	public void SetCanCloseEnding ()
	{
		isCanCloseEnding = true;
	}

	public void ButtonClose ()
	{
		if (isCanCloseEnding) {
			gameObject.SetActive (false);
			OilKingManager.s_Instance.SendGameEndingAPI ();
		}
	}
	public void SetSprite1(){
		imgLogo.sprite = OilKingAssetLoader.s_Instance.getEndingAnimation (1);
	}
	public void SetSprite2(){
		imgLogo.sprite = OilKingAssetLoader.s_Instance.getEndingAnimation (2);
	}
	public void SetSprite3(){
		imgLogo.sprite = OilKingAssetLoader.s_Instance.getEndingAnimation (3);
	}
	public void SetSprite4(){
		imgLogo.sprite = OilKingAssetLoader.s_Instance.getEndingAnimation (4);
	}
	public void SetSprite5(){
		imgLogo.sprite = OilKingAssetLoader.s_Instance.getEndingAnimation (5);
	}
}
