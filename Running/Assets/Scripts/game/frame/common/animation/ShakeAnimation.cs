using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ShakeAnimation : MonoBehaviour
{
	public int duration;
	public int strength;
	public int vibrato;
	public int randomness;

	public RectTransform rectTransform;

	private void OnEnable ()
	{
		rectTransform.DOShakePosition (duration, strength, vibrato, randomness);
	}
}
