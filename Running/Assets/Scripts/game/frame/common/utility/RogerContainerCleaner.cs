using UnityEngine;
using System.Collections;

public class RogerContainerCleaner
{
	public static void Clean (Transform transform)
	{
		int length = transform.childCount;
		for (int i = 0; i < length; i++) {
			GameObject.Destroy (transform.GetChild (i).gameObject);
		}
	}
}
