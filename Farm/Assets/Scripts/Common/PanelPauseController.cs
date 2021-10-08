using UnityEngine;
using System.Collections;

public class PanelPauseController : MonoBehaviour
{

    // Use this for initialization
    private int IDButtonInPause;
    public GameObject PanelComfirmExitMenu;
    private Transform label;
    public Transform DialogConfirm;
    public Transform btn_Mussic;
    public Transform btn_Sound;
    public Texture[] texture;
    public GameObject[] textureFuture;
    AudioControl audioControl;
    void OnEnable()
    {
        if ((Random.Range(0, 1000) % 2) == 0)
        {
            textureFuture[0].SetActive(true);
            textureFuture[1].SetActive(false);
        }
        else
        {
            textureFuture[0].SetActive(false);
            textureFuture[1].SetActive(true);
        }
        if (AudioControl.soundEnable)
        {
            btn_Sound.GetComponent<UITexture>().mainTexture = texture[0];
        }
        else
        {
            btn_Sound.GetComponent<UITexture>().mainTexture = texture[1];
        }
        if (AudioControl.musicEnable)
        {
            btn_Mussic.GetComponent<UITexture>().mainTexture = texture[2];
        }
        else
        {
            btn_Mussic.GetComponent<UITexture>().mainTexture = texture[3];
        }
        GetComponent<AdmobControl>().ShowBanner();
    }
    void Start()
    {
        audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        label = transform.Find("PanelLeft").Find("Label");

        label.GetComponent<UILabel>().text = MissionControl.Language["MISSION"] + " " + VariableSystem.mission;
        //label.GetComponent<UILabel>().text = "MISSION 0";

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || CommonObjectScript.isEndGame || CommonObjectScript.isViewComplete)
        {
            Play_Click();
        }
    }

    void StopTime()
    {
        Time.timeScale = 0;
        CommonObjectScript.isViewPoppup = true;
    }

    void InVisible()
    {
        if (IDButtonInPause == 2)
        {
            this.gameObject.SetActive(false);
            CommonObjectScript.isViewPoppup = false;
        }
        else if (IDButtonInPause == 1)
        {
            PanelComfirmExitMenu.gameObject.SetActive(true);
            PanelComfirmExitMenu.GetComponent<Animator>().Play("Visible");
        }
        else if (IDButtonInPause == 3 || IDButtonInPause == 4)
        {
            this.gameObject.SetActive(false);
            Time.timeScale = 0;
        }
        GetComponent<AdmobControl>().HideBanner();

    }

    public void Menu_Click()
    {
        audioControl.PlaySound("Click 1");
        IDButtonInPause = 1;
        Time.timeScale = 1;
        print("aaaaaaaaaaa");
        this.gameObject.GetComponent<Animator>().Play("Invisible");
    }
    public void Play_Click()
    {
        audioControl.PlaySound("Click 1");
        IDButtonInPause = 2;
        Time.timeScale = 1;
        //print("aaaaaaaaaaa");
        this.gameObject.GetComponent<Animator>().Play("Invisible");
    }
    public void QC_Click()
    {
        audioControl.PlaySound("Click 1");
        Application.OpenURL("http://s.qplay.vn/");
    }
    public void RePlay_Click()
    {
        audioControl.PlaySound("Click 1");
        Play_Click();
        Transform confirm = Instantiate(DialogConfirm) as Transform;
        print(this.transform.parent.name + "------------------------------");
        confirm.parent = this.transform.parent;
        //confirm.parent = transform;
        Time.timeScale = 1;
        IDButtonInPause = 4;
        confirm.GetComponent<DialogConfirm>().ShowDialog(MissionControl.Language["Try_Again"], MissionControl.Language["try_again_detail"], () =>
        {
            //Hungbv 19/01
            //FactoryScenesController.isCreat = false;
            //TownScenesController.isCreat = false;
            //CreatAndControlPanelHelp.countClickHelpPanel = 0;
            MissionControl.ShowTryAgain();
            Destroy(gameObject);
            Time.timeScale = 1;
        }, () =>
        {
            //CancelButton();
           Time.timeScale = 1;
        });
        print("aaaaaaaaaaa");
    }
    public void Sound_Click()
    {
        audioControl.PlaySound("Click 1");

        if (AudioControl.soundEnable)
        {
            btn_Sound.GetComponent<UITexture>().mainTexture = texture[1];
            AudioControl.StopAllSound();
        }
        else
        {
            btn_Sound.GetComponent<UITexture>().mainTexture = texture[0];
            AudioControl.ResumeAllSound();
        }
    }

    public void Music_Click()
    {
        audioControl.PlaySound("Click 1");

        if (AudioControl.musicEnable)
        {
            btn_Mussic.GetComponent<UITexture>().mainTexture = texture[3];
            AudioControl.StopAllMusic();
        }
        else
        {
            btn_Mussic.GetComponent<UITexture>().mainTexture = texture[2];
            AudioControl.ResumeAllMusic();
        }
    }
    public void Help_Click()
    {
        audioControl.PlaySound("Click 1");
        IDButtonInPause = 3;
        Time.timeScale = 1;
        print("aaaaaaaaaaa");
        this.gameObject.GetComponent<Animator>().Play("Invisible");

        CommonObjectScript.isViewPoppup = false;
        this.transform.parent.Find("DialogHelp").GetComponent<DialogHelp>().ShowDialog();
    }
}
