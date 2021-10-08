using UnityEngine;
using System.Collections;

public class PanelComfirmExitController : MonoBehaviour {

	// Use this for initialization
    public UILabel[] arrayLabel;
    AudioControl audioControl;
    void OnEnable()
    {
        CommonObjectScript.isViewPoppup = true;
    }
    void Start()
    {
        audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        arrayLabel[0].text = FactoryScenesController.languageHungBV["EXIT"];
        arrayLabel[1].text = FactoryScenesController.languageHungBV["COMFIREXIT"];
        arrayLabel[2].text = FactoryScenesController.languageHungBV["CANCEL"];
        arrayLabel[3].text = FactoryScenesController.languageHungBV["AGREE"];

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || CommonObjectScript.isEndGame || CommonObjectScript.isViewComplete)
        {
            BtnCancel_Click();
        }
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            BtnCancel_Click();
        }
    }
    public void BtnOK_Click()
    {
        MissionControl.ResetAllItem();
        audioControl.PlaySound("Click 1");
        DataCache.SaveAchievementCache();
        Time.timeScale = 1;
        print("aaaaaaaaaaa");
        //Application.LoadLevel("Mission");
        LoadingScene.ShowLoadingScene("Mission", true);
        
      //  CommonObjectScript.nameScenes = "Mission";

        //Hungbv19/01
        //FactoryScenesController.isCreat = false;
        //TownScenesController.isCreat = false;
        //CreatAndControlPanelHelp.countClickHelpPanel = 0;

        CommonObjectScript.isViewPoppup = false;
        Destroy(gameObject);
    }

    public void BtnCancel_Click()
    {
        audioControl.PlaySound("Click 1");
        Time.timeScale = 1;
        print("aaaaaaaaaaa");
        this.gameObject.GetComponent<Animator>().Play("InVisible");
    }

    void Invisible()
    {
        this.gameObject.GetComponent<Animator>().Play("Nomal");
        this.transform.parent.GetComponent<Animator>().Play("Visible");
        this.gameObject.SetActive(false);
        CommonObjectScript.isViewPoppup = false;
        //this.gameObject.SetActive(false);
    }
    void StopTime()
    {
        Time.timeScale = 0;
        
    }
}
