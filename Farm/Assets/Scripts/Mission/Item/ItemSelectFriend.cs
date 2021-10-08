using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemSelectFriend : MonoBehaviour
{

    public string UserId = "";
    public string UserName = "Me";
    bool hide;
    Transform bg, khungavata,avata, name, checkBg, checkMark;

    //public void SetData(Dictionary<string, object> info)
    //{
    //    //Debug.Log("Id " + info["uid"] + " Ten " + info["name"]);
    //    UserId = info["uid"] + "";
    //    UserName = info["name"] as string;
    //    transform.FindChild("Name").gameObject.GetComponent<UILabel>().text = UserName;

    //    //Load and show Avatar
    //    string nameImage = UserId + ".jpg";
    //    string dirPath = Application.persistentDataPath + "/cacheavatar/";
    //    string filePath = dirPath + nameImage;
    //   // Debug.Log(UserId + "   " + nameImage);
    //    if (System.IO.File.Exists(filePath))
    //    {
    //        WWW imageToLoadPath = new WWW("file://" + filePath);
    //        Texture2D texture2d = new Texture2D(128, 128);
    //        imageToLoadPath.LoadImageIntoTexture(texture2d);
    //        this.transform.FindChild("Avatar").GetComponent<UITexture>().mainTexture = texture2d;
    //    }
    //    else
    //    {
    //        DFB.FBLoadAndSaveAvatar(this, "" + UserId, texture =>
    //        {
    //            byte[] bytes = texture.EncodeToJPG();
    //            if (!System.IO.Directory.Exists(dirPath))
    //            {
    //                System.IO.Directory.CreateDirectory(dirPath);
    //                Debug.Log("Create directory");
    //            }
    //            System.IO.File.WriteAllBytes(filePath, bytes);
    //            this.transform.FindChild("Avatar").GetComponent<UITexture>().mainTexture = texture;
    //        }, false, 128, 128);
    //    }
    //}

    void Awake()
    {
        bg = transform.Find("Background");
        khungavata = transform.Find("KhungAvatar");
        avata = transform.Find("Avatar");
        name = transform.Find("Name");
        checkBg = transform.Find("Checkbox").Find("Background");
        checkMark = transform.Find("Checkbox").Find("Checkmark");
    }

    public void SetDataAPI(Dictionary<string, object> info, MonoBehaviour monoBehavier, bool isAppUser = false)
    {
        UserId = info["id"] + "";
        UserName = info["name"] as string;
        transform.Find("Name").gameObject.GetComponent<UILabel>().text = UserName;
        if (isAppUser)
        {
            //Load avatar va save no laj neu chua co trong bo nho cache
            string nameImage = UserId + ".jpg";
            string dirPath = Application.persistentDataPath + "/cacheavatar/";
            string filePath = dirPath + nameImage;
            if (System.IO.File.Exists(filePath))
            {
                WWW imageToLoadPath = new WWW("file://" + filePath);
                Texture2D texture2d = new Texture2D(128, 128, TextureFormat.RGB24, false);
                imageToLoadPath.LoadImageIntoTexture(texture2d);
                try
                {
                    this.transform.Find("Avatar").GetComponent<UITexture>().mainTexture = texture2d;
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
            else
            {
                if (monoBehavier == null)
                    monoBehavier = this;
                DFB.FBLoadAndSaveAvatar(monoBehavier, "" + UserId, texture =>
                {
                    byte[] bytes = texture.EncodeToJPG();
                    if (!System.IO.Directory.Exists(dirPath))
                    {
                        System.IO.Directory.CreateDirectory(dirPath);
                        Debug.Log("Create directory");
                    }
                    System.IO.File.WriteAllBytes(filePath, bytes);
                    try
                    {
                        this.transform.Find("Avatar").GetComponent<UITexture>().mainTexture = texture;
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                }, false, 128, 128);
            }
        }
        else
        {
            Dictionary<string, object> pic = info["picture"] as Dictionary<string, object>;
            Dictionary<string, object> picData = pic["data"] as Dictionary<string, object>;
            string url = picData["url"] as string;
            try
            {
                StartCoroutine(DFB.LoadPictureEnumerator(url, texture =>
                {
                    Util.Log("MyPictureCallback");
                    try
                    {
                        this.transform.Find("Avatar").GetComponent<UITexture>().mainTexture = texture;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                }));
            }
            catch (Exception e)
            {
                Debug.Log("--------------------catch error StartCoroutine-------------------" + e.Message);
            }
        }
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

    void Hide()
    {
        if (!hide)
        {
            hide = true;
            bg.GetComponent<UITexture>().enabled = false;
            khungavata.GetComponent<UITexture>().enabled = false;
            avata.GetComponent<UITexture>().enabled = false;
            name.GetComponent<UILabel>().enabled = false;
            checkBg.GetComponent<UITexture>().enabled = false;  
            checkMark.GetComponent<UITexture>().enabled = false;
        }
    }

    void Show()
    {
        if (hide)
        {
            hide = false;
            bg.GetComponent<UITexture>().enabled = true;
            khungavata.GetComponent<UITexture>().enabled = true;
            avata.GetComponent<UITexture>().enabled = true;
            name.GetComponent<UILabel>().enabled = true;
            checkBg.GetComponent<UITexture>().enabled = true;
            checkMark.GetComponent<UITexture>().enabled = true;
        }
    }
}
