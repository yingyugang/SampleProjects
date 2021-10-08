using UnityEngine;
using System.Collections;

public class TechnogyResultController : MonoBehaviour
{

    // Use this for initialization
    AudioControl audioControl;
    public UILabel lbInfor, lbButton;
    public UITexture icon;
    public int IDObject, levelObject;
    private Texture imageicon;
    void OnEnable()
    {
        icon.GetComponent<UITexture>().mainTexture = null;
        CommonObjectScript.isViewPoppup = true;

    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        if (audioControl == null)
            audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        SetImage(this.IDObject, this.levelObject);
        lbInfor.text = TownScenesController.languageTowns["UpgradeTechnogy"];
        lbButton.text = FactoryScenesController.languageHungBV["AGREE"];

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || CommonObjectScript.isEndGame || CommonObjectScript.isViewComplete)
        {
            if (!TownScenesController.isHelp)
                Close_Click();
        }
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            if (!TownScenesController.isHelp)
                Close_Click();
        }
    }
    public void Close_Click()
    {
        audioControl.PlaySound("Click 1");
        if (!TownScenesController.isHelp)
            this.gameObject.GetComponent<Animator>().Play("InVisible");
    }
    public void Destroy()
    {
        TownScenesController.townsBusy[5] = false;
        CommonObjectScript.isViewPoppup = false;
        Destroy(gameObject);
        CreatTownScenesController.isDenyContinue = false;
    }
    void SetImage(int ID, int level)
    {
        //icon.GetComponent<UITexture>().mainTexture = null;
        imageicon = Resources.Load<Texture>("Town/TechnogyItemResult/0" + ID + "_" + level);
        icon.GetComponent<UITexture>().mainTexture = imageicon;
    }
}
