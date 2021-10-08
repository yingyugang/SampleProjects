using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoginBonusItem : MonoBehaviour
{
	public GameObject item;
	public GameObject face;
	public Text text;
	public Image icon;
	public UnityAction unityAction;
	private const float duration = 0.5f;

	public void ShowOrHideFace (bool isShow)
	{
		face.SetActive (isShow);
		item.SetActive (!isShow);
	}

	public void SetData (int number, Sprite sprite)
	{
		icon.sprite = sprite;
		text.text = string.Format ("{0}{1}", LanguageJP.X, number.ToString ());
	}

	public void ShowAnimation ()
	{
		face.SetActive (true);
		face.transform.DOScale (new Vector3 (5, 5, 1), duration).SetEase (Ease.InExpo).From (true);
		face.transform.DOLocalMoveX (250, duration).SetEase (Ease.InExpo).From (true).OnComplete (CompleteHandler);
	}

	private void CompleteHandler ()
	{
		ComponentConstant.SOUND_MANAGER.Play (SoundEnum.se03_stamp);
		if (unityAction != null) {
			unityAction ();
		}
	}
}
