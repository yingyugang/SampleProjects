using System.Collections.Generic;
using UnityEngine;

public class WarningTextView : MonoBehaviour
{

    public string textWarning { set; get; }
    public int typeWarning { set; get; }

    private static float timeView;
    private static float timeViewCount;
    private static bool isDenyAdd;
    private static List<WarningTextClass> queueWarning = new List<WarningTextClass>();
    private bool isUpdate;
    private static int countAtView;
    public UILabel LBWarningText;
    public GameObject anchorControl;

    public WarningTextView()
    {
    }
    public  WarningTextView(string textWarning, int typeWarning)
    {
        isDenyAdd = false;
        for (int i = 0; i < queueWarning.Count; i++)
        {

            if (queueWarning[i].type == typeWarning)
            {
                isDenyAdd = true;
                break;
            }
        }

        if (!isDenyAdd)
        {
            this.textWarning = textWarning;
            this.typeWarning = typeWarning;
            WarningTextClass warningTemp = new WarningTextClass(this.textWarning, this.typeWarning);
            queueWarning.Add(warningTemp);
            // print("co duoc add");
        }

        // print(queueWarning.Count);
        //  print("string : " + queueWarning[queueWarning.Count - 1].text + " ID: " + queueWarning[queueWarning.Count - 1].ID);
    }

    public void RemoveWarning(int Type)
    {
        // print("Type sua xong: " + Type);

        if (queueWarning.Count != 0)
        {
            //print("Type đang hiển thị Finish: " + queueWarning[countAtView].type);
            if (Type == queueWarning[countAtView].type)
            {
                //print(queueWarning.Count);
                timeViewCount = timeView;
            }
            else
            {
                print(queueWarning.Count);
                for (int i = queueWarning.Count - 1; i > 0; i--)
                {
                    if (queueWarning[i].type == Type)
                    {
                        queueWarning.RemoveAt(i);
                    }
                }
            }
        }
    }
    void Start()
    {
        isUpdate = true;
        timeView = 4.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (queueWarning.Count != 0)
        {
            SetStatus(true);
            //  print((int)timeViewCount + "-" + queueWarning[countAtView].ID);
            if (timeViewCount < timeView)
            {
                timeViewCount += Time.deltaTime;
                SetViewText();
            }
            else
            {
                queueWarning.RemoveAt(countAtView);
                isUpdate = true;
                timeViewCount = 0;
                countAtView = 0;
            }
        }
        else
        {
            LBWarningText.text = "";
            SetStatus(false);
        }
    }

    void SetViewText()
    {
        if (isUpdate)
        {
            foreach (WarningTextClass warningTemp in queueWarning)
            {
                if (warningTemp.type < queueWarning[countAtView].type)
                {
                    countAtView = queueWarning.IndexOf(warningTemp);
                }
            }
            // print("countAtView SetView : " + countAtView);
            // print("ID đang hiển thị SetView : " + queueWarning[countAtView].type);
            if (!queueWarning[countAtView].type.Equals(1))
                LBWarningText.text = queueWarning[countAtView].text;
            else
            {
                LBWarningText.text = "Bạn chỉ còn " + CommonObjectScript.maxTimeOfMission + " ngày.";
            }
            isUpdate = false;
        }
    }
    void SetStatus(bool status)
    {
        if (!anchorControl.activeInHierarchy.Equals(status))
        {
            anchorControl.SetActive(status);
        }
    }
}
