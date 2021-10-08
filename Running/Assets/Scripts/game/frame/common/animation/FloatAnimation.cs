using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FloatAnimation : MonoBehaviour
{
	public Vector3 vector3;
	public Vector2 vector2;
	public RectTransform rectTransform;

	private void Start ()
	{
		rectTransform.DOLocalMove (vector3, Random.Range (vector2.x, vector2.y)).SetLoops (-1, LoopType.Yoyo).SetEase (Ease.Linear);
	}
}
