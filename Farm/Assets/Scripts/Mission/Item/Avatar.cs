using UnityEngine;
using System.Collections;
using System;

public class Avatar : MonoBehaviour {

    public string UserId = "";

    //Dung Monobehaviour cua cha no. vi nhieu luc no ko active tren Hierarchi thi fai dung den cha cua no
    public void SetData(MonoBehaviour monobehavior, string mUserId = "")
    {
        this.UserId = mUserId;
        if (mUserId.Length > 0)
        {
            //Load avatar va save no laj neu chua co trong bo nho cache
            string nameImage = mUserId + ".jpg";
            string dirPath = Application.persistentDataPath + "/cacheavatar/";
            string filePath = dirPath + nameImage;
            if (System.IO.File.Exists(filePath))
            {
                WWW imageToLoadPath = new WWW("file://" + filePath);
                Texture2D texture2d = new Texture2D(128, 128, TextureFormat.RGB24, false);
                imageToLoadPath.LoadImageIntoTexture(texture2d);
                this.GetComponent<UITexture>().mainTexture = texture2d;
               
            }
            else
            {
                if(FB.IsLoggedIn )
                {
                    DFB.FBLoadAndSaveAvatar(monobehavior, "" + UserId, texture =>
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
                            this.GetComponent<UITexture>().mainTexture = texture;
                        }
                        catch (Exception e)
                        {
                            Debug.Log("---------------ERROR------------------" + e.Message);
                        }
                    }, false, 128, 128);
                }
            }
        }
    }
}
