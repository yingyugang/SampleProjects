using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndingGame8 : MonoBehaviour
{
	public GameObject Ending;

	public void ShowImg(){
		Ending.SetActive (true);
	}

	public void CloseEnding ()
	{
		GUI_8_Manager.Instance.CloseEnding ();

	}

}
