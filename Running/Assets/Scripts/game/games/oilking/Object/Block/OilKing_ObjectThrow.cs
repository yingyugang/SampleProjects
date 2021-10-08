using UnityEngine;
using System.Collections;

public class OilKing_ObjectThrow : MonoBehaviour
{


	void OnEnable()
	{
		StartCoroutine(watingExecuteObjectThrow());
	}
	IEnumerator watingExecuteObjectThrow()
	{
		yield return new WaitForEndOfFrame();
		this.transform.position = OilKing_BlockManager.Instance.GetPosBlock();
		OilKing_Animation.Instance.ExecuteObjectThrow(this.transform);
	}
}
