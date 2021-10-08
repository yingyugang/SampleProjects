using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Events;

public class SizeAnimation : MonoBehaviour
{
	public RectTransform rectTransform;
	public Vector2 vector2;
	public float duration;
	public UnityAction unityAction;

	public void Play (UnityAction unityAction)
	{
		this.unityAction = unityAction;
		rectTransform.DOSizeDelta (vector2, duration).OnComplete (OnCompleteHandler);
	}

	private void OnCompleteHandler ()
	{
		unityAction ();
	}
}
