using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogSetting : DialogAbs
{
    public Transform DilogConfirm;
    UIToggle btnLanguage;
    UILabel lbFb;

    void Awake()
    {
        btnLanguage = transform.Find("Language").GetComponent<UIToggle>();
        transform.Find("Sound").GetComponent<UIToggle>().value = AudioControl.soundEnable;
        transform.Find("Music").GetComponent<UIToggle>().value = AudioControl.musicEnable;
        lbFb = transform.Find("Fb").Find("Label").GetComponent<UILabel>();
    }

    void Update()
    {
        if (FB.IsLoggedIn)
        {
            lbFb.text = MissionControl.Language["Logout"];
        }
        else
        {
            lbFb.text = MissionControl.Language["Login"];
        }
    }

    public void ShowDialogSetting()
    {
        Show = true;
        gameObject.SetActive(true);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1f), 0.3f).setEase(LeanTweenType.easeOutBack);
        //Phai chay tu menu thi bien VariableSystem.language  moi co gia tri
        if ("Vietnamese".Equals(VariableSystem.language))
        {
            btnLanguage.value = true;
        }
        else
        {
            btnLanguage.value = false;
        }
        transform.Find("Music").GetComponent<UIToggle>().value = PlayerPrefs.GetInt("music_enable", 1) == 1;
        transform.Find("Sound").GetComponent<UIToggle>().value = PlayerPrefs.GetInt("sound_enable", 1) == 1;
        ChangeLanguage();
    }

    public void HideDialogSetting()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0f), 0.3f).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
        {
            //gameObject.SetActive(false);
            Show = false;
            transform.parent.Find("DialogLoading").GetComponent<DialogLoading>().HideLoading();
        });
    }
    public void MusicButton()
    {
        AudioControl.musicEnable = transform.Find("Music").GetComponent<UIToggle>().value;
        if (AudioControl.musicEnable == true)
        {
            AudioControl.ResumeAllMusic();
        }
        else
        {
            AudioControl.StopAllMusic();
        }

        if (AudioControl.musicEnable)
        {
            transform.Find("Music").Find("On").gameObject.SetActive(true);
            transform.Find("Music").Find("Off").gameObject.SetActive(false);
        }
        else
        {
            transform.Find("Music").Find("On").gameObject.SetActive(false);
            transform.Find("Music").Find("Off").gameObject.SetActive(true);
        }
    }

    public void SoundButton()
    {
        AudioControl.soundEnable = transform.Find("Sound").GetComponent<UIToggle>().value;
        if (AudioControl.soundEnable == true)
        {
            AudioControl.ResumeAllSound();
        }
        else
        {
            AudioControl.StopAllSound();
        }

        
        if (AudioControl.soundEnable)
        {
            transform.Find("Sound").Find("Off").gameObject.SetActive(false);
            transform.Find("Sound").Find("On").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("Sound").Find("Off").gameObject.SetActive(true);
            transform.Find("Sound").Find("On").gameObject.SetActive(false);
        }
    }

    public void LanguageButton()
    {
        if (btnLanguage.value)
        {
            VariableSystem.language = "Vietnamese";
        }
        else
        {
            VariableSystem.language = "English";
        }
        VariableSystem.SaveData();
        ChangeLanguage();

        // HungBV

        if ("Vietnamese".Equals(VariableSystem.language))
            FactoryScenesController.languageHungBV = FactoryScenesController.languageHungBVVI;
        else
            FactoryScenesController.languageHungBV = FactoryScenesController.languageHungBVEN;
    }

    public void ChangeLanguage()
    {
        if ("Vietnamese".Equals(VariableSystem.language))
        {
            MissionControl.Language = MissionControl.LanguageVI;
            // Debug.Log("TIENG VIET");
        }
        else
        {
            MissionControl.Language = MissionControl.LanguageEN;
            //Debug.Log("TIENG ANH");
        }
        transform.Find("Background").Find("Logo").Find("Name").GetComponent<UILabel>().text = MissionControl.Language["SETTING"];
        transform.Find("Background").Find("Music").Find("Name").GetComponent<UILabel>().text = MissionControl.Language["Music"];
        transform.Find("Background").Find("Sound").Find("Name").GetComponent<UILabel>().text = MissionControl.Language["Sound"];
        transform.Find("Help").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["Help"];
        transform.Find("Info").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["Info"];        
        transform.parent.parent.Find("Button").Find("ButtonLogin").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["LOGIN"];
        transform.Find("Sound").Find("On").GetComponent<UILabel>().text = MissionControl.Language["ON"];
        transform.Find("Music").Find("On").GetComponent<UILabel>().text = MissionControl.Language["ON"];
        transform.Find("Sound").Find("Off").GetComponent<UILabel>().text = MissionControl.Language["OFF"];
        transform.Find("Music").Find("Off").GetComponent<UILabel>().text = MissionControl.Language["OFF"];
        if(AudioControl.musicEnable)
        {
            transform.Find("Music").Find("On").gameObject.SetActive(true);
            transform.Find("Music").Find("Off").gameObject.SetActive(false);
        }
        else
        {
            transform.Find("Music").Find("On").gameObject.SetActive(false);
            transform.Find("Music").Find("Off").gameObject.SetActive(true);
        }
        if (AudioControl.soundEnable)
        {
            transform.Find("Sound").Find("Off").gameObject.SetActive(false);
            transform.Find("Sound").Find("On").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("Sound").Find("Off").gameObject.SetActive(true);
            transform.Find("Sound").Find("On").gameObject.SetActive(false);
        }
    }

    public override void ShowDialog(DialogAbs.CallBackShowDialog callback = null)
    {
        throw new System.NotImplementedException();
    }

    public override void HideDialog(DialogAbs.CallBackHideDialog callback = null)
    {
        throw new System.NotImplementedException();
    }

    public void ButtonHelp()
    {
        transform.parent.Find("DialogHelp").GetComponent<DialogHelp>().ShowDialog();
    }

    public void ButtonInfo()
    {
        Debug.Log("Show ButtonInfo");
        transform.parent.Find("DialogInfo").GetComponent<DialogInfo>().ShowDialog();
    }

    public void ButtonFB()
    {
        if(FB.IsLoggedIn)
        {
            DataCache.SaveAchievementCache(true);    
            Transform confirm = Instantiate(DilogConfirm) as Transform;
            confirm.GetComponent<DialogConfirm>().ShowDialog(MissionControl.Language["LOGOUT"], MissionControl.Language["Logout_confirm"], () =>
            {
                //Dang xuat .submit tat ca du lieu len server, sau do  xoa sach data
                DFB.FBLogout();
                transform.parent.parent.GetComponent<MissionControl>().LogoutButton();              
                DataCache.DeleteUserData();
                DataCache.GetAchievementCacheData();
                LoadingScene.ShowLoadingScene("Mission", true);
            });
        }
        else
        {
            transform.parent.parent.GetComponent<MissionControl>().LoginButton(false);
        }
        ChangeLanguage();
    }
}
