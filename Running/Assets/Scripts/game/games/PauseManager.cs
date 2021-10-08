using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour {
    public Image imgBanner;
    public Image imgInfo;
    public GameObject listGameName;
    public Sprite[] listResourceBanner;
    public GameObject panelExitConfrim;
    public GameObject panelUIButtonPause;
    public GameObject childPanel;
    private AudioSource m_BgAudioGame;
    public GameObject panelSoundSetting;

    private int m_IdGame = 1;
    private string m_TitleRuleExplanation = "Daruma";

    public delegate void OnClose();
    public delegate void OnExit();

    public delegate void OnBackToGame();
    public delegate void OnSettingSound();
    public delegate void OnRuleExplanation();
    public delegate void OnQuitGame();

    public event OnClose onClose;
    public event OnExit onExit;
    public event OnBackToGame onBackToGame;
    public event OnSettingSound onSettingSound;
    public event OnRuleExplanation onRuleExplanation;
    public event OnQuitGame onQuitGame;

    public static PauseManager s_Instance;
    private void SetImageGameName(int id)
    {
        imgBanner.sprite = listResourceBanner[id - 1];
        int count = listGameName.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            listGameName.transform.GetChild(i).gameObject.SetActive(false);
        }
        listGameName.transform.GetChild(id - 1).gameObject.SetActive(true);
    }

    public void Init(int id, AudioSource audio = null)
    {
        SetImageGameName(id);
        m_BgAudioGame = audio;
        m_IdGame = id;
        m_TitleRuleExplanation = "";
        if (UpdateInformation.GetInstance == null)
        {
            return;
        }
        List<GameDetail> gameDetailList = UpdateInformation.GetInstance.game_list;
        int length = gameDetailList.Count;
        for (int i = 0; i < length; i++)
        {
            if (gameDetailList[i].id == m_IdGame)
            {
                m_TitleRuleExplanation = gameDetailList[i].name;
                return;
            }
        }
    }

    private void Awake()
    {
        s_Instance = this;
    }

    private void Start()
    {
        panelExitConfrim.SetActive(false);
        panelUIButtonPause.SetActive(true);
        childPanel.SetActive(false);
        panelSoundSetting.SetActive(false);
    }

    public void SetVisible(bool isVisible)
    {
        childPanel.SetActive(isVisible);
		if(m_BgAudioGame != null)
		{
	        if (isVisible )
	        {
	            m_BgAudioGame.Pause();
	        }
	        else
	        {
	            m_BgAudioGame.UnPause();
	        }
		}
    }

    #region Exit confirm panel
    public void BtnClose() // Exit confirm
    {
        if (ComponentConstant.SOUND_MANAGER != null)
        {
            ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se02_ng);
        }
        panelUIButtonPause.SetActive(true);
        panelExitConfrim.SetActive(false);
    }

    // Exit to Home scene
    public void BtnExit()
    {
        if (ComponentConstant.SOUND_MANAGER != null)
        {
            ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se01_ok);
        }
        if (onExit != null)
        {
            onExit();
        }

        // Back to home scene.
        ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene(SceneEnum.Home);
        
    }
    #endregion

    #region UIButtonsPause panel

    public void BtnBackToGame()
    {
        if (ComponentConstant.SOUND_MANAGER != null)
        {
            ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se02_ng);
        }

        SetVisible(false);
        if (onBackToGame != null)
        {
            onBackToGame();
        }
    }

    public void BtnSettingSound()
    {
        if (ComponentConstant.SOUND_MANAGER != null)
        {
            ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se01_ok);
        }
        panelSoundSetting.SetActive(true);
        if (onSettingSound != null)
        {
            onSettingSound();
        }
    }

    public void BtnQuitGame()
    {
        if (ComponentConstant.SOUND_MANAGER != null)
        {
            ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se01_ok);
        }
        panelExitConfrim.SetActive(true);
        panelUIButtonPause.SetActive(false);
    }

    public void BtnRuleExplanation()
    {
        if (ComponentConstant.SOUND_MANAGER != null)
        {
            ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se01_ok);
        }

        if (onRuleExplanation != null)
        {
            onRuleExplanation();
        }

        // Show ruleExplanation
        if (ComponentConstant.POPUP_LOADER != null)
        {
            List<object> param = new List<object>() { m_TitleRuleExplanation, m_IdGame };
            ComponentConstant.POPUP_LOADER.Popup(PopupEnum.GameDescription, null, param);
        }
    }

    #endregion

    public void SetOnExitCallback(OnExit callback)
    {
        onExit += callback;
    }
    public void RemoveOnExitCallback(OnExit callback)
    {
        onExit -= callback;
    }
    public void SetOnBackToGameCallback(OnBackToGame callback)
    {
        onBackToGame += callback;
    }
    public void RemoveOnBackToGameCallback(OnBackToGame callback)
    {
        onBackToGame -= callback;
    }
    public void SetOnSettingSoundCallback(OnSettingSound callback)
    {
        onSettingSound += callback;
    }
    public void RemoveOnSettingSoundCallback(OnSettingSound callback)
    {
        onSettingSound -= callback;
    }
    public void SetOnRuleExplanationCallback(OnRuleExplanation callback)
    {
        onRuleExplanation += callback;
    }
    public void RemoveOnRuleExplanationCallback(OnRuleExplanation callback)
    {
        onRuleExplanation -= callback;
    }
}
