using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System;
using System.Collections.Generic;

public class DialogAchievement : DialogAbs
{
    Transform dialogMain;
    Transform bgBlack;
    public Transform ItemAchievement;
    UIGrid grid;
    Transform btnAchievement;
    UILabel lbCount;
    ArrayList arr;

    static int countachievement = 0;
    public static List<Achievement[]> arrGroupAchievement;
    void Awake()
    {
        if (arrGroupAchievement != null)
        {
            arrGroupAchievement.Clear();
            arrGroupAchievement = null;
        }
        arrGroupAchievement = new List<Achievement[]>();
        dialogMain = transform.Find("Main");
        bgBlack = transform.Find("BgBlack");
        grid = dialogMain.Find("Scroll View").Find("Grid").GetComponent<UIGrid>();

        //AddDataAchievement(3, 100);      
        //Read xml achievement in game
        TextAsset xml = Resources.Load<TextAsset>("Mission/Language/Achievement"); //Read File xml
        // Debug.Log(xml.text);
        XmlDocument xmlDoc = new XmlDocument();
        StringReader reader = new StringReader(xml.text);
        xmlDoc.Load(reader);
        XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;
        reader.Dispose();
        reader.Close();
        foreach (XmlNode group in xmlNodeList)
        {
            int groupId = Convert.ToInt16(group.Attributes["level"].Value);
            //Current achievement of group in xml data 
            int currentLevelAchievement = DataCache.dataAchievementCache[groupId - 1].Level;
            //Current value of mission group in xml data
            int currentValue = DataCache.dataAchievementCache[groupId - 1].Value; ;

            //Danh sach achievement tren cung 1 group
            Achievement[] achievements = getAchievements(group);
            //Them vao scrollview
            Transform item = Instantiate(ItemAchievement) as Transform;
            item.GetComponent<ItemAchievement>().SetData(groupId, achievements, currentLevelAchievement, currentValue);
            grid.AddChild(item);
            item.localScale = new Vector3(1, 1, 1);
            arrGroupAchievement.Add(achievements);
        }
        grid.Reposition();

        btnAchievement = transform.parent.parent.Find("Button").Find("ButtonAchievement");
        lbCount = btnAchievement.Find("Count").Find("Label").GetComponent<UILabel>();
        CountAchievementFinish();

        arr = new ArrayList();
        for (int i = 0; i < dialogMain.Find("Scroll View").Find("Grid").childCount; i++)
        {
            arr.Add(dialogMain.Find("Scroll View").Find("Grid").GetChild(i));
        }
        
    }

    public void CountAchievementFinish()
    {
        countachievement = 0;
        for (int i = 0; i < grid.GetChildList().Count; i++)
        {
            if (grid.GetChild(i).GetComponent<ItemAchievement>().finish)
            {
                countachievement++;
            }
        }
        Debug.Log("Dem nhiem vu hoan thanh " + countachievement);
        
    }

    //Lay danh sach achievement trong 1 group
    Achievement[] getAchievements(XmlNode group)
    {
        XmlNodeList xmlNodeList = group.ChildNodes;
        int length = xmlNodeList.Count;
        Achievement[] list = new Achievement[length];
        foreach (XmlNode node in xmlNodeList)
        {
            //Debug.Log("group level "+ group.Attributes["level"].Value + " node level " + node.Attributes["level"].Value);
            int groupId = Convert.ToInt32(group.Attributes["level"].Value) - 1;
            int levelId = Convert.ToInt32(node.Attributes["level"].Value) - 1;
            string title = node.Attributes["title"].Value;
            string detail = node.Attributes["detail1"].Value;//Tieng Anh
            string detail2 = node.Attributes["detail2"].Value;//Tieng Viet
            int reward = Convert.ToInt32(node.Attributes["reward"].Value);
            int target = Convert.ToInt32(node.Attributes["target"].Value);
            list[levelId] = new Achievement(groupId, levelId, title, detail, detail2, reward, target);
        }
        return list;
    }

    public override void ShowDialog(DialogAbs.CallBackShowDialog callback = null)
    {
        dialogMain.Find("Bg1").Find("Texture").Find("Title").GetComponent<UILabel>().text = MissionControl.Language["ACHIEVEMENTS"];
        grid.transform.parent.GetComponent<UIPanel>().clipOffset = new Vector2();
        grid.transform.parent.localPosition = new Vector2();
        //Cap nhat du lieu tu xml data cache
        for (int i = 0; i < DataCache.dataAchievementCache.Length; i++)
        {
            int id = DataCache.dataAchievementCache[i].Group - 1;
            //Current value of mission group in xml data
            int currentValue = DataCache.dataAchievementCache[id].Value;
            int currentLevel = DataCache.dataAchievementCache[id].Level;
            //Debug.Log("Update group " + DataCache.dataAchievementCache[i].Group + " level " + DataCache.dataAchievementCache[i].Level
            //    + " current " + DataCache.dataAchievementCache[id].Value);
            grid.GetChild(id).GetComponent<ItemAchievement>().UpdateData(currentLevel, currentValue);
        }
        Show = true;
        bgBlack.gameObject.SetActive(true);
        dialogMain.gameObject.SetActive(true);
        LeanTween.scale(dialogMain.gameObject, new Vector3(1, 1, 1), 0.4f).setEase(LeanTweenType.easeOutBack).setUseEstimatedTime(true).setOnComplete(() =>
        {
            dialogMain.Find("Scroll View").Find("Grid").gameObject.SetActive(true);
        });
        CountAchievementFinish();
    }

    public override void HideDialog(DialogAbs.CallBackHideDialog callback = null)
    {
        CountAchievementFinish();
        LeanTween.scale(dialogMain.gameObject, new Vector3(0, 0, 0), 0.4f).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
        {
            bgBlack.gameObject.SetActive(false);
            Show = false;
            dialogMain.Find("Scroll View").Find("Grid").gameObject.SetActive(false);
            dialogMain.gameObject.SetActive(false);
        });
        DataCache.SaveAchievementCache();    
    }

    public static void AddDataAchievement(int groupLevel, int addValue)
    {
        DataCache.AddAchievementCache(groupLevel, addValue);
        //Debug.Log("Cap nhat nhiem vu " + groupLevel);
        //Check nhiem vu hoan thanh
        int currentLevel = DataCache.dataAchievementCache[groupLevel - 1].Level;
        if (arrGroupAchievement[groupLevel - 1].Length >= currentLevel && DataCache.dataAchievementCache[groupLevel - 1].Value >= arrGroupAchievement[groupLevel - 1][currentLevel - 1].Target && DataCache.dataAchievementCache[groupLevel - 1].Notify == 0)
        {
            Debug.Log("-------------------CO NHIEM VU HOAN THANH 1---------------------");
            GoogleAnalytics.instance.LogScreen("Complete Achievement: " + arrGroupAchievement[groupLevel - 1][currentLevel - 1].Title);
            AchievementInGameControl.achiFinish.finish = true;
            AchievementInGameControl.achiFinish.title = arrGroupAchievement[groupLevel - 1][currentLevel - 1].Title;
            AchievementInGameControl.achiFinish.detail = arrGroupAchievement[groupLevel - 1][currentLevel - 1].Detail;
            if (VariableSystem.language != null && VariableSystem.language.Equals("Vietnamese"))
            {
                AchievementInGameControl.achiFinish.detail = arrGroupAchievement[groupLevel - 1][currentLevel - 1].Detail_Vi;
            }
            //Chi hien thi thong bao 1 lan
            DataCache.dataAchievementCache[groupLevel - 1].Notify = 1;
            countachievement++;
        }
    }

    public static void ReplaceDataAchievement(int groupLevel, int value)
    {
        DataCache.ReplaceAchievementCache(groupLevel, value);
        //Debug.Log(" Nhiem vu  groupLevel " + groupLevel + " Current level " + currentLevel + " " + DataCache.dataAchievementCache[groupLevel - 1].Value + " --- " + arrGroupAchievement[groupLevel - 1][currentLevel].Target);
        int currentLevel = DataCache.dataAchievementCache[groupLevel - 1].Level;
        if (DataCache.dataAchievementCache[groupLevel - 1].Value >= arrGroupAchievement[groupLevel - 1][currentLevel - 1].Target && DataCache.dataAchievementCache[groupLevel - 1].Notify == 0)
        {
            Debug.Log("-------------------CO NHIEM VU HOAN THANH " + groupLevel + "---------------------");
            AchievementInGameControl.achiFinish.finish = true;
            AchievementInGameControl.achiFinish.title = arrGroupAchievement[groupLevel - 1][currentLevel - 1].Title;
            AchievementInGameControl.achiFinish.detail = arrGroupAchievement[groupLevel - 1][currentLevel - 1].Detail;
            if (VariableSystem.language != null && VariableSystem.language.Equals("Vietnamese"))
            {
                AchievementInGameControl.achiFinish.detail = arrGroupAchievement[groupLevel - 1][currentLevel - 1].Detail_Vi;
            }
            //Chi hien thi thong bao 1 lan
            DataCache.dataAchievementCache[groupLevel - 1].Notify = 1;
            countachievement++;
        }
    }

    void Update()
    {
        if (Show)
        {
            //for (int i = 0; i < arr.Count; i++)
            //{
            //    Transform tf = arr[i] as Transform;
            //    if (tf.position.y > 1.5f || tf.position.y < -1.5f)
            //    {
            //        tf.gameObject.SetActive(false);
            //    }
            //    else
            //    {
            //        tf.gameObject.SetActive(true);
            //    }
            //}
            
        }
        if (countachievement > 0)
        {
            lbCount.transform.parent.gameObject.SetActive(true);
            lbCount.text = "" + countachievement;
            //Debug.Log("ASSSSSSSSSSSSSSSSSSSSSSSSSS");
        }
        else
        {
            lbCount.transform.parent.gameObject.SetActive(false);
            lbCount.text = "0";
        }
    }
}
