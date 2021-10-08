using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using System;

public class DialogSelectFriend : DialogAbs
{

    public Transform ItemSelectFriend;
    Transform gridView;
    UIButton buttonSendLife;
    UIButton buttonSendInvite;
    Transform dialogLoading;
    Transform checkBoxSelectAll;
    //Check has unselect item on scrollview.
    bool unselectFriend = false;
    int max_friend_list = 25;

    void Awake()
    {
        unselectFriend = false;
        gridView = transform.Find("ScrollView").Find("Grid");
        buttonSendLife = transform.Find("SendLife").GetComponent<UIButton>();
        buttonSendInvite = transform.Find("SendInvite").GetComponent<UIButton>();
        dialogLoading = transform.parent.Find("DialogLoading");
        checkBoxSelectAll = transform.Find("SelectAll").Find("CheckboxSelectAll");
        buttonSendLife.isEnabled = false;
        buttonSendInvite.isEnabled = false;
        gameObject.SetActive(false);
    }

    void Update()
    {
        List<Transform> arrItem = gridView.gameObject.GetComponent<UIGrid>().GetChildList();
        int count = 0;
        bool selectAll = true;
        for (int i = 0; i < arrItem.Count; i++)
        {
            if (arrItem[i].Find("Checkbox").gameObject.GetComponent<UIToggle>().value)
            {
                count++;
            }
            else
            {
                selectAll = false;
                unselectFriend = true;
            }
        }
        checkBoxSelectAll.gameObject.GetComponent<UIToggle>().value = selectAll;
        if (count > 0)
        {
            buttonSendLife.isEnabled = true;
            buttonSendInvite.isEnabled = true;
        }
        else
        {
            buttonSendLife.isEnabled = false;
            buttonSendInvite.isEnabled = false;
        }
    }

    private void RemoveAllItem()
    {
        List<Transform> arrItem = gridView.gameObject.GetComponent<UIGrid>().GetChildList();
        for (int i = 0; i < arrItem.Count; i++)
        {
            Destroy(arrItem[i].gameObject);
            gridView.gameObject.GetComponent<UIGrid>().RemoveChild(arrItem[i]);
        }
    }

    public void AddItemSelectFriendAPI(Dictionary<string, object> info, bool isAppUer)
    {
        Transform item = Instantiate(ItemSelectFriend) as Transform;
        gridView.gameObject.GetComponent<UIGrid>().AddChild(item);
        item.localScale = new Vector3(1, 1, 1);
        ItemSelectFriend itemSelectFriend = item.gameObject.GetComponent<ItemSelectFriend>();
        itemSelectFriend.SetDataAPI(info, this, isAppUer);
        gridView.parent.GetComponent<UIScrollView>().ResetPosition();
    }

    public void CloseButton()
    {
        LeanTween.scale(gameObject, new Vector3(0f, 0f, 0f), 0.3f).setUseEstimatedTime(true).setEase(LeanTweenType.easeInBack).setOnComplete(() =>
        {
            Show = false;
            gameObject.SetActive(true);
            dialogLoading.gameObject.SetActive(false);
            RemoveAllItem();
        });

    }

    public void SendInviteFriend()
    {
        string mUserIds = "";
        List<Transform> arrItem = gridView.gameObject.GetComponent<UIGrid>().GetChildList();
        for (int i = 0; i < arrItem.Count; i++)
        {
            if (arrItem[i].Find("Checkbox").gameObject.GetComponent<UIToggle>().value)
            {
                if (mUserIds.Length < 1)
                {
                    mUserIds += arrItem[i].gameObject.GetComponent<ItemSelectFriend>().UserId;
                }
                else
                {
                    mUserIds += "," + arrItem[i].gameObject.GetComponent<ItemSelectFriend>().UserId;
                }
            }
        }
        Debug.Log(" Count FRIEND --- " + mUserIds.Length);
        DFB.FBRequestDirect(mUserIds.Split(','), "Invite Your Friends", "Play this game with me!", (result) =>
        {
            int leng = mUserIds.Split(',').Length;
            Debug.Log(result.Text);
            var dict = Json.Deserialize(result.Text) as IDictionary;
            if (dict != null && dict["request"] != null)
            {
                //--------------Achievement 22------------------------------------
                DialogAchievement.AddDataAchievement(22, leng);
                CloseButton();
            }
        });
    }

    public void ShowDialogInviteFriend()
    {
        Show = true;
        gridView.parent.GetComponent<UIScrollView>().ResetPosition();
        gridView.transform.parent.GetComponent<UIPanel>().clipOffset = new Vector2();
        gridView.transform.parent.localPosition = new Vector2();
        this.transform.Find("SendLife").gameObject.SetActive(false);
        this.transform.Find("SendInvite").gameObject.SetActive(true);
        this.transform.Find("SendAskLife").gameObject.SetActive(false);
        this.transform.Find("ForeGround").Find("Title").gameObject.GetComponent<UILabel>().text = MissionControl.Language["Invite_friend_detail"];
        this.transform.Find("ForeGround").Find("Logo").Find("name").GetComponent<UILabel>().text = MissionControl.Language["Friend_list"];
        this.transform.Find("SelectAll").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["Select_all"];
        this.transform.Find("SendInvite").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["Send"];
        DFB.FBGetInviteFriend(150, result =>
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
                    var dict = Json.Deserialize(result.Text) as IDictionary;
                    if (dict != null && dict["data"] != null)
                    {
                        RemoveAllItem();
                        Debug.Log(JsonHelper.FormatJson(result.Text));
                        List<object> arrDanhSachBanBe = dict["data"] as List<object>;
                        int size = arrDanhSachBanBe.Count;
                        int startNumber = 0;
                        if (size > max_friend_list)
                        {
                            size = max_friend_list;
                            startNumber = UnityEngine.Random.Range(0, arrDanhSachBanBe.Count - max_friend_list);
                        }
                        for (int i = startNumber; i < size + startNumber; i++)
                        {
                            Dictionary<string, object> data = arrDanhSachBanBe[i] as Dictionary<string, object>;
                            AddItemSelectFriendAPI(data, false);
                        }
                    }
                    LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 0.3f).setUseEstimatedTime(true).setEase(LeanTweenType.easeInOutBack);
                }
            }
        });

    }

    public void SendHelpButton()
    {
        string mUserIds = "";
        List<Transform> arrItem = gridView.gameObject.GetComponent<UIGrid>().GetChildList();
        for (int i = 0; i < arrItem.Count; i++)
        {
            if (arrItem[i].Find("Checkbox").gameObject.GetComponent<UIToggle>().value)
            {
                mUserIds += arrItem[i].gameObject.GetComponent<ItemSelectFriend>().UserId + ",";
            }
        }
        DFB.FBSendLife(mUserIds.Split(','), result =>
        {
            int leng = mUserIds.Split(',').Length;
            if (!String.IsNullOrEmpty(result.Error))
            {
                Debug.Log(result.Error);
            }
            else
            {
                Debug.Log(result.Text);
                var dict = Json.Deserialize(result.Text) as IDictionary;
                if (dict != null && dict["request"] != null)
                {
                    //--------------Achievement 1------------------------------------
                    DialogAchievement.AddDataAchievement(1, leng - 1);
                    CloseButton();
                }
            }
        });
    }

    public void ShowDialogHelpFriend()
    {
        Show = true;
        gridView.parent.GetComponent<UIScrollView>().ResetPosition();
        gridView.transform.parent.GetComponent<UIPanel>().clipOffset = new Vector2();
        gridView.transform.parent.localPosition = new Vector2();
        this.transform.Find("SendLife").gameObject.SetActive(true);
        this.transform.Find("SendInvite").gameObject.SetActive(false);
        this.transform.Find("SendAskLife").gameObject.SetActive(false);
        this.transform.Find("ForeGround").Find("Title").gameObject.GetComponent<UILabel>().text = MissionControl.Language["Help_friend_detail"];
        this.transform.Find("ForeGround").Find("Logo").Find("name").GetComponent<UILabel>().text = MissionControl.Language["Friend_list"];
        this.transform.Find("SelectAll").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["Select_all"];
        this.transform.Find("SendLife").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["Send"];
        DFB.FBGetUserFriend(100, result =>
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
                        var dict = Json.Deserialize(result.Text) as IDictionary;
                        Debug.Log(JsonHelper.FormatJson(result.Text));
                        List<object> arrDanhSachBanBe = dict["data"] as List<object>;

                        int size = arrDanhSachBanBe.Count;
                        int startNumber = 0;
                        if (size > max_friend_list)
                        {
                            size = max_friend_list;
                            startNumber = UnityEngine.Random.Range(0, arrDanhSachBanBe.Count - max_friend_list);
                        }
                        for (int i = startNumber; i < size + startNumber; i++)
                        {
                            Dictionary<string, object> data = arrDanhSachBanBe[i] as Dictionary<string, object>;
                            AddItemSelectFriendAPI(data, true);
                        }
                        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 0.3f).setUseEstimatedTime(true).setEase(LeanTweenType.easeInOutBack);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("--------------------catch error StartCoroutine-------------------" + e.Message);
                transform.parent.parent.GetComponent<MissionControl>().ShowConfirm();
            }
        });
    }

    public void SelectAllButton()
    {
        bool isCheck = checkBoxSelectAll.gameObject.GetComponent<UIToggle>().value;
        List<Transform> arrItem = gridView.gameObject.GetComponent<UIGrid>().GetChildList();
        if (isCheck)
        {
            for (int i = 0; i < arrItem.Count; i++)
            {
                arrItem[i].Find("Checkbox").gameObject.GetComponent<UIToggle>().value = true;
            }
        }
        else
        {
            if (!unselectFriend)
            {
                for (int i = 0; i < arrItem.Count; i++)
                {
                    arrItem[i].Find("Checkbox").gameObject.GetComponent<UIToggle>().value = false;
                }
            }
        }
        unselectFriend = false;
    }

    public void SendAskFriendButton()
    {
        string mUserIds = "";
        List<Transform> arrItem = gridView.gameObject.GetComponent<UIGrid>().GetChildList();
        for (int i = 0; i < arrItem.Count; i++)
        {
            if (arrItem[i].Find("Checkbox").gameObject.GetComponent<UIToggle>().value)
            {
                mUserIds += arrItem[i].gameObject.GetComponent<ItemSelectFriend>().UserId + ",";
            }
        }
        DFB.FBAskForLife(mUserIds.Split(','), result =>
        {
            if (!String.IsNullOrEmpty(result.Error))
            {
                Debug.Log(result.Error);
            }
            else
            {
                Debug.Log(result.Text);
                CloseButton();
            }
        });
    }

    public void ShowDialogAskFriend()
    {
        Show = true;
        checkBoxSelectAll.gameObject.GetComponent<UIToggle>().value = true;
        this.transform.Find("SendLife").gameObject.SetActive(false);
        this.transform.Find("SendInvite").gameObject.SetActive(false);
        this.transform.Find("SendAskLife").gameObject.SetActive(true);
        this.transform.Find("ForeGround").Find("Title").gameObject.GetComponent<UILabel>().text = MissionControl.Language["Ask_friend_detail"];
        this.transform.Find("ForeGround").Find("Logo").Find("name").GetComponent<UILabel>().text = MissionControl.Language["Ask_friend"];
        this.transform.Find("SelectAll").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["Select_all"];
        this.transform.Find("SendAskLife").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["Send"];
        gridView.transform.parent.GetComponent<UIPanel>().clipOffset = new Vector2();
        gridView.transform.parent.localPosition = new Vector2();
        gridView.parent.GetComponent<UIScrollView>().ResetPosition();
        DFB.FBGetUserFriend(100, result =>
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
                    var dict = Json.Deserialize(result.Text) as IDictionary;
                    Debug.Log(JsonHelper.FormatJson(result.Text));
                    List<object> arrDanhSachBanBe = dict["data"] as List<object>;
                    int size = arrDanhSachBanBe.Count;
                    int startNumber = 0;
                    if (size > max_friend_list)
                    {
                        size = max_friend_list;
                        startNumber = UnityEngine.Random.Range(0, arrDanhSachBanBe.Count - max_friend_list);
                    }
                    for (int i = startNumber; i < size + startNumber; i++)
                    {
                        Dictionary<string, object> data = arrDanhSachBanBe[i] as Dictionary<string, object>;
                        AddItemSelectFriendAPI(data, true);
                    }
                    gameObject.SetActive(true);
                    LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 0.3f).setUseEstimatedTime(true).setEase(LeanTweenType.easeInOutBack);
                }
            }
        });
    }

    public override void ShowDialog(DialogAbs.CallBackShowDialog callback = null)
    {
        throw new NotImplementedException();
    }

    public override void HideDialog(DialogAbs.CallBackHideDialog callback = null)
    {
        throw new NotImplementedException();
    }
}
