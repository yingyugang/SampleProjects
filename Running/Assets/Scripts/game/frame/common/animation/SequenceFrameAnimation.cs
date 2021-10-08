using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SequenceFrameAnimation : BaseAnimation
{
	[HideInInspector]
	public Image[] imageArray;

	protected override void Init ()
	{
		total = imageArray.Length;
		HideAllPages ();
		Show ();
	}

	private void HideAllPages ()
	{
		for (int i = 0; i < total; i++) {
			imageArray [i].gameObject.SetActive (false);
		}
	}

	protected override IEnumerator ShowOneAfterAnother ()
	{
		GameObject go = imageArray [currentIndex].gameObject;
		go.SetActive (true);
		yield return new WaitForSeconds (speed);
		go.SetActive (false);
		currentIndex++;
		if (currentIndex >= total) {
			currentIndex = 0;
		}
		Show ();
	}
}
