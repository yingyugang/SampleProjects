using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class RogerImageSpriteCleaner : MonoBehaviour
{
	public static void Clean ()
	{
		GameObject go = GameObject.Find ("Home");
		if (go != null) {
			Transform trans = go.transform;
			Image[] array = trans.GetComponentsInChildren<Image> ();
			int length = array.Length;
			for (int i = 0; i < length; i++) {
				array [i].sprite = null;
				Destroy (array [i]);
			}
		}
	}
}
