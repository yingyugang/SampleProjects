using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rank : MonoBehaviour
{
	public List<GameObject> rankList;

	public void SetRank (int num)
	{
		int length = rankList.Count;
		for (int i = 0; i < length; i++) {
			rankList [i].SetActive (false);
		}
		rankList [num].SetActive (true);
	}
}
