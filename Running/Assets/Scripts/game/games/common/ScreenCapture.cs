using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class ScreenCapture : MonoBehaviour
{

	void OnEnable ()
	{
		ScreenshotManager.ScreenshotFinishedSaving += ScreenshotSaved;	
	}

	void OnDisable ()
	{
		ScreenshotManager.ScreenshotFinishedSaving -= ScreenshotSaved;	
	}

	public void SaveScreenshot()
	{
		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se01_ok);
		StartCoroutine(ScreenshotManager.Save("MyScreenshot", Application.productName, true));
	}

	void ScreenshotSaved()
	{
		Debug.Log ("screenshot finished saving");
	}
}