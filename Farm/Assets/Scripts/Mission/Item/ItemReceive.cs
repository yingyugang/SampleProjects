using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Facebook.MiniJSON;

public class ItemReceive : MonoBehaviour
{
    public Texture iconLife;

    string requestId = "";
    string content = "gave you a life. Accept and send back!";
    string data = "";
    string userId;

    public string UserId { get { return userId; } }

    public void SetData(Dictionary<string, object> info, MonoBehaviour monoBehavier = null)
    {
        this.requestId = info["id"] as string;
        this.data = info["data"] as string;
        Dictionary<string, object> sender = info["from"] as Dictionary<string, object>;
        this.userId = sender["id"] as string;
        string from_name = sender["name"] as string;

        string messageShow = "";
        Texture icon = iconLife;
        if (data == DString.DATA_SEND_LIFE)//khi ban be gui mang den cho minh
        {
            messageShow = "gave you a life. Accept and send back!";
            if ("Vietnamese".Equals(VariableSystem.language))
            {
                messageShow = "tặng bạn 1 tim. Chấp nhận và gửi lại tim!";
            }
        }
        else if (data == DString.DATA_ASK_LIFE)//Khi ban be nho minh cho mang
        {
            messageShow = "need a life. Accept and send back!";
            if ("Vietnamese".Equals(VariableSystem.language))
            {
                messageShow = "cần giúp đỡ. Chấp nhận để gửi tim!";
            }
        }
        //Load avatar va save no laj neu chua co trong bo nho cache
        string nameImage = userId + ".jpg";
        string dirPath = Application.persistentDataPath + "/cacheavatar/";
        string filePath = dirPath + nameImage;
        if (System.IO.File.Exists(filePath))
        {
            WWW imageToLoadPath = new WWW("file://" + filePath);
            Texture2D texture2d = new Texture2D(128, 128, TextureFormat.RGB24, false);
            imageToLoadPath.LoadImageIntoTexture(texture2d);
            this.transform.Find("Avatar").GetComponent<UITexture>().mainTexture = texture2d;
        }
        else
        {
            if (monoBehavier == null)
                monoBehavier = this;
            DFB.FBLoadAndSaveAvatar(monoBehavier, "" + userId, texture =>
            {
                byte[] bytes = texture.EncodeToJPG();
                if (!System.IO.Directory.Exists(dirPath))
                {
                    System.IO.Directory.CreateDirectory(dirPath);
                    Debug.Log("Create directory");
                }
                System.IO.File.WriteAllBytes(filePath, bytes);
                this.transform.Find("Avatar").GetComponent<UITexture>().mainTexture = texture;
            }, false, 128, 128);
        }
        this.transform.Find("IconItem").GetComponent<UITexture>().mainTexture = icon;
        this.content = from_name + ": " + messageShow;
        transform.Find("Content").gameObject.GetComponent<UILabel>().text = content;
        transform.Find("ButtonSend").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["Send"];
    }

    public void DeleteItemButton(Facebook.FacebookDelegate callback = null)
    {
        if (callback == null)
        {
            callback = result =>
            {
                if (!String.IsNullOrEmpty(result.Error))
                {
                    Debug.LogError(result.Error);
                }
                else
                {
                    //Cong tim khi nguoi dung nhan 
                    AudioControl.AddHeart(1);
                    if (this != null)
                    {
                        Transform gridView = this.transform.parent;//This => Grid
                        gridView.GetComponent<UIGrid>().RemoveChild(this.transform);
                        Destroy(this.gameObject);
                        
                    }
                }
            };
        }
        //Delete Request && Item
        if (requestId.Length > 0)
        {
            DFB.FBDeleteRequest(requestId, callback);
        }
        else
        {
            Debug.LogError("Request id not valid: " + requestId);
        }
    }

    //Send backrequest and delete curent request
    public void SendButton()
    {
        DFB.FBSendLife(this.userId.Split(','), result =>
        {
            if (!String.IsNullOrEmpty(result.Error))
            {
                Debug.Log(result.Error);
            }
            else
            {
                IDictionary dict = Json.Deserialize(result.Text) as IDictionary;
                bool cancel = dict["cancelled"] != null && (bool) dict["cancelled"];
                if (!cancel)
                {                  
                    DeleteItemButton();
                }
            }
        });
    }

    bool isVisible;
    bool objVisible;
    public void Update()
    {
        if (transform.position.y < 1.5f && transform.position.y > -1.5f)
        {
            isVisible = true;
        }
        else
        {
            isVisible = false;
        }
        if (isVisible)
        {
            if (!objVisible)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                    objVisible = true;
                }
                //Debug.Log("Hien len");
            }
        }
        else
        {
            if (objVisible)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    objVisible = false;
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                //Debug.Log("An di");
            }
        }
    }
}
