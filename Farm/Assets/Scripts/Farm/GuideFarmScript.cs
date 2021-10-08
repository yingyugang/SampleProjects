using Assets.Scripts.Common;
using System;
using System.Xml;
using UnityEngine;

public class GuideFarmScript : MonoBehaviour
{
    private int level, ranger;//current level to determine guide
    public int indexGuide;// index of guide (start in 1)
    ReadXML textGuideXML;

    PanelHelpController panelHelp;
    CommonObjectScript common;
    GameObject helpGirlPrefabs, PanelHelp, cirClePrefabs, circle, handPrefabs, hander;
    string tempStr;
    bool isVN, isHideByNew;

    float countTimeShow = 0f;
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
        else if (level == 1 || level == 3 || (level == 4 && CommonObjectScript.indexGuide == 9) ||
            level == 7 || level == 8 || level == 11 || level == 15 || level == 18 || level == 21 ||
            level == 26 || level == 38 || level == 50)
        {
            LoadingAssets();
        }
        else if (MissionData.targetCommon.startScene == 1)
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
        print("Hướng dẫn");
        if (VariableSystem.language.Equals("Vietnamese")) isVN = true;
        else isVN = false;
        isHideByNew = false;
        textGuideXML = new ReadXML("Farm/XMLFile/TextGuide" + level);
        cirClePrefabs = (GameObject)Resources.Load("Help/CircleHelp");
        handPrefabs = (GameObject)Resources.Load("Help/HandHelp");
        helpGirlPrefabs = (GameObject)Resources.Load("Common/PanelHelp");

        //create instance
        PanelHelp = (GameObject)Instantiate(helpGirlPrefabs);
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
                    panelHelp.SetStatusHelp(false);
                    isHideByNew = true;
                }
            }
            else if (isHideByNew)
            {
                panelHelp.SetStatusHelp(true);
                isHideByNew = false;
            }
        }
        if (level == 1 || level == 3 || level == 4 || level == 7 || level == 11 || level == 15 ||
            level == 18 || level == 26 || level == 8 || level == 21 || level == 38 || level == 50)
        {
            if (indexGuide == 9 && panelHelp.countClick == 0) panelHelp.countClick = 8;//level 1
            if (indexGuide == 16 && panelHelp.countClick == 0) panelHelp.countClick = 16;//level 4
            if (panelHelp.countClick != indexGuide - 1) NextGuideText(true);
        }
        if (VariableSystem.mission == 1)
        {
            if (indexGuide == 19 || indexGuide == 22)
            {
                if (countTimeShow > 0)
                {
                    countTimeShow -= Time.deltaTime;
                    if (countTimeShow <= 0)
                    {
                        countTimeShow = 0;
                        circle.SetActive(true);
                        hander.SetActive(true);
                    }
                }
            }
        }
    }
    public void NextGuideText(bool bytext = false)
    {
        if (!bytext) { panelHelp.countClick++; return; }
        indexGuide++;
        //print(indexGuide);
        if (level == 1)
        {
            #region control in mission 1
            if (indexGuide == 16)
            {
                common.setActiveButton(false, true, false, false, false, false, false, false);
                CommonObjectScript.indexGuide = 16;
            }
            else if (indexGuide == 18)
            {
                common.setActiveButton(false, false, false, false, true, false, false, false);
            }
            else if (indexGuide == 21)
            {
                common.setActiveButton(false, false, false, false, false, true, false, false);
            }
            else if (indexGuide == 23)//end guide
            {
                Transform[] tranforms = transform.Find("Circle").GetComponentsInChildren<Transform>();
                if (tranforms.Length > 1)
                {
                    for (int i = 1; i < tranforms.Length; i++) GameObject.Destroy(tranforms[i].gameObject);
                }
                CommonObjectScript.CompleteGuide();
                DialogResult.ShowResult();
                return;
            }
            #endregion
        }
        else if ((level == 3 && indexGuide == 8)||(level == 38 && indexGuide == 4))
        {
            CommonObjectScript.CompleteGuide();
            common.setActiveButton(true, true, true, true, true, true, true, true);
            GameObject.Destroy(this.gameObject);
            return;
        }
        else if ((level == 4 && indexGuide == 13) ||
            (level == 8 && indexGuide == 5))
        {
            common.setActiveButton(false, true, false, false, false, false, false, false);//go to factory
        }
        else if ((level == 7 && indexGuide == 2) || (level == 11 && indexGuide == 2) ||
            (level == 15 && indexGuide == 2) || (level == 18 && indexGuide == 2) ||
            (level == 26 && indexGuide == 2) || (level == 21 && indexGuide == 2) ||
            (level == 50 && indexGuide == 2))
        {
            common.setActiveButton(false, false, false, true, false, false, false, false);//go to city
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
        if (VariableSystem.mission == 1)
        {
            if (indexGuide == 19 || indexGuide == 22) countTimeShow = 1;
        }
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
                if (VariableSystem.mission == 1)
                {
                    if (indexGuide == 19 || indexGuide == 22)
                    {
                        circle.SetActive(false);
                        hander.SetActive(false);
                    }
                }
            }
        }
    }
}
