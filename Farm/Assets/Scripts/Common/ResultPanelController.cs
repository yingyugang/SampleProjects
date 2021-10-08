using UnityEngine;
using System.Collections;

public class ResultPanelController : MonoBehaviour {

	// Use this for initialization
    public UILabel[] label;
	void Start () {
        if (VariableSystem.mission == 1)
        {
            label[0].text = "Controll Guide.";
            label[1].text = "1000";
        }
        else if (VariableSystem.mission == 2)
        {
            label[0].text = "Sell 10 Bread.";
            label[1].text = "4000";
        }
        else
        {
            label[0].text = "Miss Level";
            label[1].text = "0000";
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void ButtonOkClick()
    {
        if (VariableSystem.mission == 1)
        {
            AddCommonObject(0,2);
        }
        else if (VariableSystem.mission == 2)
        {
            AddCommonObject(0,2);
        }
        //Application.LoadLevel("Mission");
        LoadingScene.ShowLoadingScene("Mission", true);

       // CommonObjectScript.nameScenes = "Mission";

        //HungBV 19/01
        //FactoryScenesController.isCreat = false;
        //TownScenesController.isCreat = false;
        //CreatAndControlPanelHelp.countClickHelpPanel = 0;
    }
    void AddCommonObject(int dollar, int diamond)
    {
        GameObject commonObject = GameObject.Find("CommonObject");
        if (dollar < 0)
        {
            if (CommonObjectScript.dollar >= dollar)
                commonObject.GetComponent<CommonObjectScript>().AddDollar(dollar);
        }
        else
        {
            commonObject.GetComponent<CommonObjectScript>().AddDollar(dollar);
        }
        if (diamond < 0)
        {
            if (VariableSystem.diamond >= diamond)
                commonObject.GetComponent<CommonObjectScript>().AddDiamond(diamond);
        }
        else
        {
            commonObject.GetComponent<CommonObjectScript>().AddDiamond(diamond);
        }
    }
}
