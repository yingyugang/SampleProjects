using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Facebook.MiniJSON;

public class DialogMessage : DialogAbs
{

    public Transform ItemReceive;
    Transform gridViewReceive;
    Transform gridViewRequest;
    Transform dialogLoading;
    string fb_result = "Chua co gi";
    UILabel lbInbox;
    bool isInbox;

    void Start()
    {
        isInbox = true;
        lbInbox = transform.Find("StateInbox").GetComponent<UILabel>();
        lbInbox.gameObject.SetActive(true);
        gridViewReceive = transform.Find("ScrollView").Find("GridReceive");
        gridViewRequest = transform.Find("ScrollView").Find("GridRequest");
        dialogLoading = transform.parent.Find("DialogLoading");
        gridViewRequest.gameObject.SetActive(false);
        gameObject.SetActive(false);
        transform.Find("TabReceiveOff").gameObject.SetActive(false);
        transform.Find("TabRequestOff").gameObject.SetActive(true);
    }

    void Update()
    {

    }

    private void RemoveAllItem()
    {
        List<Transform> arrItem = gridViewReceive.gameObject.GetComponent<UIGrid>().GetChildList();
        for (int i = 0; i < arrItem.Count; i++)
        {
            Destroy(arrItem[i].gameObject);
            gridViewReceive.gameObject.GetComponent<UIGrid>().RemoveChild(arrItem[i]);
        }
        List<Transform> arrItemRequest = gridViewRequest.gameObject.GetComponent<UIGrid>().GetChildList();
        for (int i = 0; i < arrItemRequest.Count; i++)
        {
            Destroy(arrItemRequest[i].gameObject);
            gridViewRequest.gameObject.GetComponent<UIGrid>().RemoveChild(arrItemRequest[i]);
        }
    }
    public void AddItemReceive(Dictionary<string, object> info)
    {
        Transform item = Instantiate(ItemReceive) as Transform;
        gridViewReceive.gameObject.GetComponent<UIGrid>().AddChild(item);
        item.localScale = new Vector3(1, 1, 1);
        ItemReceive itemReceive = item.gameObject.GetComponent<ItemReceive>();
        itemReceive.SetData(info, this);
    }

    public void AddItemRequest(Dictionary<string, object> info)
    {
        Transform item = Instantiate(ItemReceive) as Transform;
        gridViewRequest.gameObject.GetComponent<UIGrid>().AddChild(item);
        item.localScale = new Vector3(1, 1, 1);
        ItemReceive itemReceive = item.gameObject.GetComponent<ItemReceive>();
        itemReceive.SetData(info, this);
        Debug.Log("them vao ---------- " + gridViewReceive.gameObject.GetComponent<UIGrid>().GetChildList());

    }

    public void ReceiveButton()
    {
        isInbox = true;
        gridViewRequest.gameObject.SetActive(false);
        gridViewReceive.gameObject.SetActive(true);
        transform.Find("TabReceiveOff").gameObject.SetActive(false);
        transform.Find("TabReceiveOn").gameObject.SetActive(true);
        transform.Find("TabRequestOff").gameObject.SetActive(true);
        transform.Find("TabRequestOn").gameObject.SetActive(false);
        lbInbox.text = MissionControl.Language["No_message"];
        if (gridViewReceive.childCount < 1)
        {
            lbInbox.gameObject.SetActive(true);
        }
        else
        {
            lbInbox.gameObject.SetActive(false);
        }
        gridViewReceive.parent.GetComponent<UIPanel>().clipOffset = new Vector2();
        gridViewReceive.parent.localPosition = new Vector2();
    }

    public void RequestButton()
    {
        isInbox = false;
        gridViewRequest.gameObject.SetActive(true);
        gridViewReceive.gameObject.SetActive(false);
        transform.Find("TabReceiveOff").gameObject.SetActive(true);
        transform.Find("TabReceiveOn").gameObject.SetActive(false);
        transform.Find("TabRequestOff").gameObject.SetActive(false);
        transform.Find("TabRequestOn").gameObject.SetActive(true);
        lbInbox.text = MissionControl.Language["No_request"];
        if (gridViewRequest.childCount < 1)
        {
            lbInbox.gameObject.SetActive(true);
        }
        else
        {
            lbInbox.gameObject.SetActive(false);
        }
        gridViewRequest.parent.GetComponent<UIPanel>().clipOffset = new Vector2();
        gridViewRequest.parent.localPosition = new Vector2();
    }

    public void AcceptAllButton()
    {
        List<Transform> arrItemReceive = gridViewReceive.gameObject.GetComponent<UIGrid>().GetChildList();
        List<Transform> arrItemRequest = gridViewRequest.gameObject.GetComponent<UIGrid>().GetChildList();
        if (transform.Find("TabReceiveOn").gameObject.activeInHierarchy)
        {
            for (int i = 0; i < arrItemReceive.Count; i++)
            {
                arrItemReceive[i].GetComponent<ItemReceive>().DeleteItemButton();
            }
            lbInbox.gameObject.SetActive(true);
        }
        else if (arrItemRequest.Count > 0)
        {
            string mUserId = "";
            for (int i = 0; i < arrItemRequest.Count; i++)
            {
                mUserId += arrItemRequest[i].gameObject.GetComponent<ItemReceive>().UserId + ",";
            }
            DFB.FBSendLife(mUserId.Split(','), result =>
            {
                if (!String.IsNullOrEmpty(result.Error))
                {
                    Debug.Log(result.Error);
                }
                else
                {
                    Debug.Log(result.Text);
                    fb_result = result.Text;
                    var dict = Json.Deserialize(result.Text) as IDictionary;
                    if (dict["cancelled"] != null)
                    {
                        fb_result += " Bam cancel => ko xoa request";
                        Debug.Log(" Bam cancel => ko xoa request");
                    }
                    else
                    {
                        fb_result += " Xoa list arrItemRequest.Count " + arrItemRequest.Count;
                        for (int i = 0; i < arrItemRequest.Count; i++)
                        {
                            arrItemRequest[i].GetComponent<ItemReceive>().DeleteItemButton();
                            //arrItemRequest.RemoveAt(i);
                        }
                    }
                    ShowNoMessage();
                }
            });
        }

    }

    void ShowNoMessage()
    {
        if (isInbox)
        {
            if (gridViewReceive.childCount > 0)
            {
                lbInbox.gameObject.SetActive(false);
            }
            else
            {
                lbInbox.gameObject.SetActive(true);
            }
        }
        else
        {
            if (gridViewRequest.childCount > 0)
            {
                lbInbox.gameObject.SetActive(false);
            }
            else
            {
                lbInbox.gameObject.SetActive(true);
            }
        }
    }

    //void OnGUI()
    //{
    //    //GUI.backgroundColor = Color.black;
    //    GUI.Label(new Rect(10, 100, 200, 200), fb_result);
    //}

    public override void ShowDialog(DialogAbs.CallBackShowDialog callback = null)
    {
        if (isInbox)
            lbInbox.text = MissionControl.Language["No_message"];
        else
            lbInbox.text = MissionControl.Language["No_request"];
        Show = true;
        transform.Find("Request").GetComponent<UILabel>().text = MissionControl.Language["Request"];
        transform.Find("Receive").GetComponent<UILabel>().text = MissionControl.Language["Receive"];
        transform.Find("Logo").Find("name").GetComponent<UILabel>().text = MissionControl.Language["Your_Message"];
        transform.Find("AcceptAll").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["Accept_all"];

        FB.API("v2.2/me/apprequests?fields=id,from,message,data&limit=30", Facebook.HttpMethod.GET, result =>
        {
            try
            {
                if (!String.IsNullOrEmpty(result.Error))
                {
                    Debug.LogError(result.Error);
                    dialogLoading.gameObject.SetActive(false);
                    transform.parent.parent.GetComponent<MissionControl>().ShowConfirm();
                }
                else
                {
                    if (dialogLoading.gameObject.activeInHierarchy)
                    {
                        RemoveAllItem();
                        Debug.Log(JsonHelper.FormatJson(result.Text));
                        var dict = Json.Deserialize(result.Text) as IDictionary;
                        var data = dict["data"] as List<object>;
                        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAA " + data.Count);
                        for (int i = 0; i < data.Count; i++)
                        {
                            Dictionary<string, object> info = data[i] as Dictionary<string, object>;
                            string datasend = info["data"] as string;
                            //Loc tab receive va tab request
                            if (datasend == DString.DATA_ASK_LIFE)
                            {
                                //Cap nhat danh sach ban be len list view
                                AddItemRequest(info);
                            }
                            else if (datasend == DString.DATA_SEND_LIFE)
                            {
                                //Cap nhat danh sach ban be len list view
                                AddItemReceive(info);
                            }
                            else
                            {
                                Debug.Log("ko cho vao listview " + info);
                            }
                            Debug.Log("ITEM " + i);
                        }
                        LeanTween.scale(gameObject, new Vector3(1, 1, 1f), 0.3f).setUseEstimatedTime(true).setEase(LeanTweenType.easeOutBack);
                        gridViewReceive.parent.GetComponent<UIPanel>().clipOffset = new Vector2();
                        gridViewReceive.parent.localPosition = new Vector2();
                        gridViewRequest.parent.GetComponent<UIPanel>().clipOffset = new Vector2();
                        gridViewRequest.parent.localPosition = new Vector2();
                        ShowNoMessage();
                    }
                }
            }
            catch (Exception e)
            {
                transform.parent.parent.GetComponent<MissionControl>().ShowConfirm();
                Debug.Log("--------------------catch error StartCoroutine-------------------" + e.Message);
            }
        });
    }

    public override void HideDialog(DialogAbs.CallBackHideDialog callback = null)
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0f), 0.3f).setUseEstimatedTime(true).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
        {
            Show = false;
            RemoveAllItem();
            gameObject.SetActive(true);
            dialogLoading.gameObject.SetActive(false);
        });
        transform.parent.parent.GetComponent<MissionControl>().CountMessage();
    }
}
