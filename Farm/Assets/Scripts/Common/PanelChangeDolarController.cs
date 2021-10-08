using UnityEngine;
using System.Collections;

public class PanelChangeDolarController : MonoBehaviour
{
    CommonObjectScript commonObject;
    public UILabel[] arrayLabel;
    public int coinNeed;
    private int diamondChange, coinAdd;
    AudioControl audioControl;
    void OnEnable()
    {
        CommonObjectScript.isViewPoppup = true;
        diamondChange = coinNeed / 201 + 1;
        coinAdd = diamondChange * 200;
        arrayLabel[4].text = coinAdd.ToString();
        arrayLabel[5].text = diamondChange.ToString();
    }
    void Start()
    {

        audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        commonObject = GetComponentInParent<CommonObjectScript>();
        arrayLabel[0].text = FactoryScenesController.languageHungBV["CHANGEDOLAR"];
        arrayLabel[1].text = FactoryScenesController.languageHungBV["COMFIRCHANGEDOLAR"];
        arrayLabel[2].text = FactoryScenesController.languageHungBV["AGREE"];
        arrayLabel[3].text = FactoryScenesController.languageHungBV["CANCEL"];

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || CommonObjectScript.isEndGame || CommonObjectScript.isViewComplete)
        {
            CancelButton_Click();
        }
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            CancelButton_Click();
        }
    }

    public void OKButton_Click()
    {
        audioControl.PlaySound("Click 1");
        if (VariableSystem.diamond >= diamondChange)
        {
            //print("hieu ưng kim cuong va tien");
            AddCommonObject(coinAdd, -diamondChange);
        }
        else
        {
            // DialogInapp.ShowDialogInapp();
            DialogInapp.ShowInapp();
        }
        CancelButton_Click();
    }

    public void CancelButton_Click()
    {
        audioControl.PlaySound("Click 1");
        GetComponent<Animator>().Play("Invisible");
        commonObject.isOpennew = false;
    }

    void DestroyGameObj()
    {
        gameObject.SetActive(false);
        CommonObjectScript.isViewPoppup = false;
    }
    void AddCommonObject(int dollar, int diamond)
    {
        if (commonObject != null)
        {
            commonObject.AddDollar(dollar);
            commonObject.AddDiamond(diamond);
        }
    }
}
