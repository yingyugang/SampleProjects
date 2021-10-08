using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RankOrder : MonoBehaviour
{
	public List<GameObject> rankOrderList;
	public Text orderText;

	public void SetRankOrder (int num)
	{
		HideOrderImageAndText ();
		if (num < 4) {
			rankOrderList [num - 1].SetActive (true);
		} else {
			orderText.text = num.ToString ();
			orderText.gameObject.SetActive (true);
		}
	}

	private void HideOrderImageAndText ()
	{
		int length = rankOrderList.Count;
		for (int i = 0; i < length; i++) {
			rankOrderList [i].SetActive (false);
		}
		orderText.gameObject.SetActive (false);
	}
}
