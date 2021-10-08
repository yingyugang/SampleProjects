using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	public GameObject endingPanel;
	private bool m_IsShowEnding = false;
	private static UIManager _instance;


	void Awake()
	{
		_instance = this;
	}


	public static UIManager Instance{
		get{ return _instance;}
	}
		

	public void ShowEnding()
	{
		if(!m_IsShowEnding)
		{
			Header.Instance.UpdateLife(0);
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.bgm15_title_back);
			if (GetOut.GameManager.Instance ().isClock) {
				GetOut.GameManager.Instance ().audioSourceBackground.pitch = 1;
//				GetOut.GameManager.Instance ().audioSourceBackground.volume = 1;
			}
			endingPanel.SetActive(true);
			m_IsShowEnding = true;
		}
	}

	public void CloseEnding()
	{
		ComponentConstant.SOUND_MANAGER.StopBGM();
		endingPanel.SetActive(false);
	}
}
