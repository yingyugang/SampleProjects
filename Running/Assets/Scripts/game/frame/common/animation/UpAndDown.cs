using UnityEngine;
using System.Collections;
using DG.Tweening;

public class UpAndDown : MonoBehaviour
{
	public RectTransform rectTransform;

	public void Play ()
	{
		Vector3 vector3 = new Vector3 (rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y, 0);
		rectTransform.DOJumpAnchorPos (vector3, 15, 1, 0.2f);
	}
}
