using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GUI_8_Manager : MonoBehaviour
{

	public GameObject Warning;
	public GameObject EndingObj;
	private static GUI_8_Manager _instance;

	public static GUI_8_Manager Instance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<GUI_8_Manager> ();
			return _instance;
		}
	}

	void Start ()
	{
		EndingObj.SetActive (false);
	}		

	public void OpenEnding ()
	{
		
		EndingObj.SetActive (true);
		Debug.Log ("ending on");


	}

	public void CloseEnding ()
	{
		if (ComponentConstant.SOUND_MANAGER != null) {
			ComponentConstant.SOUND_MANAGER.StopBGM ();
		}
		//Game8_Manager.instance.ResetVolume();
		Game8_Manager.instance.GameOver ();
		EndingObj.SetActive (false);

	}

	public void WarningBoss ()
	{
		Game8_Manager.instance.PlaySound (SoundEnum.SE26_oden_bossalert);
		Warning.SetActive (true);
		StartCoroutine (Game8_Manager.instance.DelayAction (2.0f, () =>ShowBoss()));
	}

	void ShowBoss ()
	{
		Game8_Manager.instance.StartBall = false;	
		Warning.SetActive (false);

	}

}
