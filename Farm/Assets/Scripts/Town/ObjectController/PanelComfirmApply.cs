using UnityEngine;
using System.Collections;

public class PanelComfirmApply : MonoBehaviour
{

    // Use this for initialization
    public UILabel[] labelComfirm;
    void OnEnable()
    {
        CommonObjectScript.isViewPoppup = true;


    }
    void Start()
    {
        labelComfirm[0].text = TownScenesController.languageTowns["WARNING"];
        labelComfirm[1].text = TownScenesController.languageTowns["NoteWarning"];
        labelComfirm[2].text = FactoryScenesController.languageHungBV["AGREE"];
        labelComfirm[3].text = FactoryScenesController.languageHungBV["CANCEL"];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || CommonObjectScript.isEndGame || CommonObjectScript.isViewComplete)
        {
            if (!TownScenesController.isHelp)
                this.gameObject.GetComponent<Animator>().Play("InVisible");
        }
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            if (!TownScenesController.isHelp)
                this.gameObject.GetComponent<Animator>().Play("InVisible");
        }
    }
    void EndAnimationInvisible()
    {
        CommonObjectScript.isViewPoppup = false;
        this.gameObject.transform.parent.GetComponent<MaketResearchController>().EndAnimationComrfirm();
        this.gameObject.SetActive(false);
        this.transform.parent.gameObject.SetActive(false);
    }
}
