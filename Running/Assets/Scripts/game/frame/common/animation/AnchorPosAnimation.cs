using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AnchorPosAnimation : MonoBehaviour
{
	public Vector2 vector2;
	public float duration;
	public RectTransform rectTransform;

	public void Play ()
	{
		rectTransform.DOAnchorPos (vector2, duration);
	}
}
