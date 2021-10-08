using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OilKingAnimImage : MonoBehaviour {
	public Image imgTarget;
	public Sprite[] lstImage;
	public float timeRunOneFrame = 0.05f;
	public bool loopAnim = false;
	bool isRun = false;
	float timeCurrent = 0;

	public virtual void Start()
	{

	}

	void Update()
	{
		if (isRun && !Header.Instance.isPause)
		{
			timeCurrent += timeRunOneFrame * Time.deltaTime * 30;
			if ((int)timeCurrent >= lstImage.Length)
			{
				timeCurrent = 0;
				if (!loopAnim) StopAnim();
			}
			int index = (int)timeCurrent;
			imgTarget.sprite = lstImage[index];
		}
	}

	public virtual void RunAnim()
	{
		isRun = true;
		timeCurrent = 0;
		imgTarget.sprite = lstImage[0];
	}

	public virtual void StopAnim()
	{
		isRun = false;
		timeCurrent = 0;
		imgTarget.sprite = lstImage[0];
	}

	public virtual void PauseAnim()
	{
		isRun = false;
	}

	public virtual void ResumeAnim()
	{
		isRun = true;
	}

	public virtual void RestartAnim()
	{
		imgTarget.sprite = lstImage[0];
	}
}
