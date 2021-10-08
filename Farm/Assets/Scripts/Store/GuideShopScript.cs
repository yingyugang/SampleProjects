using Assets.Scripts.Common;
using System;
using System.Xml;
using UnityEngine;

public class GuideShopScript : MonoBehaviour
{
    private int level, ranger;//current level to determine guide
    public int indexGuide;// index of guide (start in 1)
    ReadXML textGuideXML;

    PanelHelpController panelHelp;
    CommonObjectScript common;
    GameObject helpGirlPrefabs, cirClePrefabs, circle, handPrefabs, hander;
    bool isVN, isHideByNew;
    string tempStr;
    // Use this for initialization
    void Start()
    {
        level = VariableSystem.mission;
        indexGuide = CommonObjectScript.indexGuide;
        common = GameObject.FindGameObjectWithTag("CommonObject").GetComponent<CommonObjectScript>();
        if (indexGuide < 0 || !CommonObjectScript.isGuide)//complete guide
        {
            GameObject.Destroy(this.gameObject);
        }
        else if (level == 2 || level == 4 || level == 6 || level == 9)
        {
            LoadingAssets();
        }
        else if (MissionData.targetCommon.startScene == 3)
        {
            CommonObjectScript.CompleteGuide();
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    void LoadingAssets()
    {
        if (VariableSystem.language.Equals("Vietnamese")) isVN = true;
        else isVN = false;
        isHideByNew = false;

        textGuideXML = new ReadXML("Shop/TextGuide" + level);
        cirClePrefabs = (GameObject)Resources.Load("Help/CircleHelp");
        handPrefabs = (GameObject)Resources.Load("Help/HandHelp");
        helpGirlPrefabs = (GameObject)Resources.Load("Common/PanelHelp");

        //create instance
        GameObject PanelHelp = (GameObject)Instantiate(helpGirlPrefabs);
        PanelHelp.name = "PanelHelp";
        PanelHelp.transform.parent = this.transform;
        PanelHelp.transform.localPosition = Vector3.zero;
        PanelHelp.transform.localScale = Vector3.one;

        panelHelp = PanelHelp.GetComponent<PanelHelpController>();
        common.setActiveButton(false, false, false, false, false, false, false, false);

        if (isVN)
            tempStr = textGuideXML.getDataByValue("id", indexGuide.ToString()).Attributes["textV"].Value;
        else
            tempStr = textGuideXML.getDataByValue("id", indexGuide.ToString()).Attributes["textE"].Value;
        panelHelp.setTextView(tempStr);
        //print("complete loading assests");
    }

    // Update is called once per frame
    void Update()
    {
        if (CommonObjectScript.isGuide && indexGuide == 1)
        {
            if (common.isOpennew)
            {
                if (!isHideByNew)
                {
                    print("Exits new items");
                    isHideByNew = true;
                    panelHelp.SetStatusHelp(false);
                }
            }
            else if (isHideByNew)
            {
                panelHelp.SetStatusHelp(true);
                isHideByNew = false;
            }
        }
        if (level == 2 || level == 4 || level == 6 || level == 9)
        {
            if (indexGuide == 3 && panelHelp.countClick == 0) panelHelp.countClick = 2;
            if (panelHelp.countClick != indexGuide - 1) NextGuideText(true);
        }
    }
    public void NextGuideText(bool bytext = false)
    {
        if (!bytext) { panelHelp.countClick++; return; }
        indexGuide++;
        if (level == 2)
        {
            #region controll in mission 2
            if (indexGuide == 2)
            {
                common.setActiveButton(false, false, false, true, false, false, false, false);
                CommonObjectScript.indexGuide = 3;
            }
            else if (indexGuide >= 8)
            {
                CommonObjectScript.CompleteGuide();
                print("hết mission 2");
                common.setActiveButton(true, true, true, true, true, true, true, true);
                GameObject.Destroy(this.gameObject);
                return;
            }
            #endregion
        }
        else if (level == 4 && indexGuide == 8)
        {
            common.setActiveButton(true, false, false, false, false, false, false, false);
            CommonObjectScript.indexGuide = 9;
        }
        else if ((level == 6 && indexGuide == 6) ||
            (level == 9 && indexGuide == 8))
        {
            CommonObjectScript.CompleteGuide();
            common.setActiveButton(true, true, true, true, true, true, true, true);
            GameObject.Destroy(this.gameObject);
            return;
        }
        getTextGuide();
        getControlGuide();
    }
    private void getTextGuide()
    {
        if (isVN)
            tempStr = textGuideXML.getDataByValue("id", indexGuide.ToString()).Attributes["textV"].Value;
        else
            tempStr = textGuideXML.getDataByValue("id", indexGuide.ToString()).Attributes["textE"].Value;

        if (tempStr.Equals("xxx"))//control
        {
            panelHelp.SetStatusHelp(false);
        }
        else // show text
        {
            panelHelp.SetStatusHelp(true);
            panelHelp.setTextView(tempStr);
        }
    }
    private void getControlGuide()
    {
        //delete old circles
        Transform[] tranforms = transform.Find("Circle").GetComponentsInChildren<Transform>();
        if (tranforms.Length > 1)
        {
            for (int i = 1; i < tranforms.Length; i++) GameObject.Destroy(tranforms[i].gameObject);
        }

        if (textGuideXML.getDataByValue("id", indexGuide.ToString()).ChildNodes.Count > 0) getCircleAndHand();
    }
    private void getCircleAndHand()
    {
        foreach (XmlNode child in textGuideXML.getDataByValue("id", indexGuide.ToString()).ChildNodes)
        {
            string[] position = child.Attributes[0].Value.Split(';');
            ranger = Convert.ToInt16(child.Attributes[1].Value);

            circle = (GameObject)Instantiate(cirClePrefabs);
            circle.name = "Circle";
            circle.transform.parent = this.transform.Find("Circle");
            circle.GetComponent<Transform>().localScale = Vector3.one * ranger;
            circle.GetComponent<Transform>().localPosition = new Vector3(Convert.ToInt16(position[0]), Convert.ToInt16(position[1]), 1);
            Transform[] trans = circle.GetComponentsInChildren<Transform>();
            for (int i = 0; i < trans.Length; i++)
            {
                trans[i].gameObject.layer = 5;
            }

            if (child.Name.Equals("hand"))
            {
                hander = (GameObject)Instantiate(handPrefabs);
                hander.name = "Hand";
                hander.transform.parent = this.transform.Find("Circle");
                if (ranger > 0)
                    hander.GetComponent<Transform>().localScale = Vector3.one * ranger * 2;
                else
                    hander.GetComponent<Transform>().localScale = new Vector3(1, -1, 1) * ranger * 2;
                hander.GetComponent<Transform>().localPosition = new Vector3(Convert.ToInt16(position[0]), Convert.ToInt16(position[1]), 1);
                Transform[] trans1 = hander.GetComponentsInChildren<Transform>();
                for (int i = 0; i < trans1.Length; i++)
                {
                    trans1[i].gameObject.layer = 5;
                }
            }
        }
    }
}
