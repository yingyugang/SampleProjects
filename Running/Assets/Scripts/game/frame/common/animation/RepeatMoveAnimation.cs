using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class RepeatMoveAnimation : MonoBehaviour
{
	public int Xpos = 1920;
	public int Ypos = 1920;
	public float duration = 20f;
	public bool isReverse;

	public Image image;

	private void Start ()
	{
		Move ();
	}

	private void Move ()
	{
		Tweener tweener = image.rectTransform.DOAnchorPos (new Vector2 (Xpos, Ypos), duration, false).SetEase (Ease.Linear);
		if (isReverse) {
			tweener.SetLoops (-1, LoopType.Yoyo);
		} else {
			tweener.SetLoops (-1, LoopType.Restart);
		}
	}
}
