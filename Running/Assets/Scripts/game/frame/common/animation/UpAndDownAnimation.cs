using UnityEngine;
using System.Collections;

public class UpAndDownAnimation : BaseAnimation
{
	public UpAndDown[] upAndDownArray;

	protected override void Init ()
	{
		total = upAndDownArray.Length;
		Show ();
	}

	protected override IEnumerator ShowOneAfterAnother ()
	{
		yield return new WaitForSeconds (speed);
		UpAndDown upAndDown = upAndDownArray [currentIndex];
		upAndDown.Play ();
		yield return new WaitForSeconds (speed * 2);
		currentIndex++;
		if (currentIndex >= total) {
			currentIndex = 0;
		}
		Show ();
	}
}
