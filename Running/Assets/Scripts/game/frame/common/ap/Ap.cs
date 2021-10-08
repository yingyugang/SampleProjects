using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Ap : MonoBehaviour
{
	public Image[] imageArray;
	public Text time;

	public void SetAp (int count)
	{
		int length = imageArray.Length;
		for (int i = 0; i < count; i++) {
			imageArray [i].gameObject.SetActive (true);
		}

		for (int j = count; j < length; j++) {
			imageArray [j].gameObject.SetActive (false);
		}
	}

	public void ShowOrHideTime (bool isActive)
	{
		time.gameObject.SetActive (isActive);
	}

	public void SetRemainTime (int remainTime)
	{
		time.text = TimeUtil.TimeToString (remainTime);
	}

	public void SetDefaultTime ()
	{
		time.text = "00:00";
	}
}
