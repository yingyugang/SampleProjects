using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Facebook.MiniJSON;

public class DFB
{
    public delegate void LoadPictureCallback(Texture2D texture);

    public static string UserId = "";
    public static string UserName = "Me";
    #region Init, login, logout FB
    public static void FBInit()
    {
        try
        {
            FB.Init(
           delegate()
           {
               Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
           },
           delegate(bool isGameShown)
           {
               Debug.Log("Is game showing? " + isGameShown);
           });
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public static void FBLogin(Facebook.FacebookDelegate callback = null, String appPermision = "publish_actions, user_friends")
    {
        try
        {
            if (callback == null)
            {
                callback = result =>
                {
                    //Debug.Log(result.Text);
                    IDictionary dict = Json.Deserialize(result.Text) as IDictionary;
                    if (dict != null && dict["is_logged_in"] != null)
                    {
                        if ((bool)dict["is_logged_in"])
                        {
                            FB.API("v2.2/me", Facebook.HttpMethod.GET, rsl =>
                            {
                                IDictionary dict1 = Json.Deserialize(rsl.Text) as IDictionary;
                                if (dict1 != null && dict1["id"] != null)
                                {
                                    UserId = "" + dict1["id"];
                                    UserName = "" + dict1["name"];
                                }
                            });
                        }
                    }
                };
            }
            FB.Login(appPermision, callback);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public static void FBLogout()
    {
        try
        {
            FB.Logout();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    #endregion

    public static void FBRequestSelector(
        string TieuDeHopThoai = "",
        string NoiDungTinNhan = "Help",
        //Hiển thị các selectbox để chọn bạn bè(Tất cả, Những người đang chơi, Những người chưa chơi)
        string DanhSachLoc = "[\"all\",\"app_users\",\"app_non_users\"]",
        Facebook.FacebookDelegate callback = null)
    {
        string DuLieuLuuTru = "Test Du Lieu Luu Tru";
        //Danh sach loại trừ
        string DanhSachIdLoaiTru = "";
        string FriendSelectorMax = "";
        int? SoLuongToiDa = null;
        if (FriendSelectorMax != "")
        {
            try
            {
                SoLuongToiDa = Int32.Parse(FriendSelectorMax);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        if (callback == null)
        {
            callback = result =>
            {
                Debug.Log(result.Text);
            };
        }
        // include the exclude ids
        string[] MangIdLoaiTru = (DanhSachIdLoaiTru == "") ? null : DanhSachIdLoaiTru.Split(',');
        List<object> DanhSachLocJson = null;
        if (!String.IsNullOrEmpty(DanhSachLoc))
        {
            try
            {
                DanhSachLocJson = Facebook.MiniJSON.Json.Deserialize(DanhSachLoc) as List<object>;
            }
            catch
            {
                throw new Exception("JSON Parse error");
            }
        }
        try
        {
            // include the exclude ids
            FB.AppRequest(
                NoiDungTinNhan,
                null,//List id bạn bè chọn sẵn => bên Direct Request
                DanhSachLocJson,
                MangIdLoaiTru,//Danh sách bạn bè bị loại trừ ra khỏi lựa chọn
                SoLuongToiDa,
                DuLieuLuuTru,
                TieuDeHopThoai,
                callback
            );
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public static void FBRequestDirect(string[] DanhSachIdNguoiNhan = null, string TieuDe = "Tieu De", string NoiDungTinNhan = "Noi Dung Tin Nhan", Facebook.FacebookDelegate callback = null)
    {
        string DuLieuLuuTru = "Test Du Lieu Luu Tru";
        List<object> DanhSachLocJson = null;
        if (callback == null)
        {
            callback = result =>
            {
                Debug.Log(result.Text);
            };
        }
        try
        {
            if (DanhSachIdNguoiNhan == null)
            {
                //throw new ArgumentException("Danh Sach Nguoi Nhan Khong Ton Tai", DanhSachIdNguoiNhan);
                Debug.LogError("Danh sach nguoi nhan ko hop le " + DanhSachIdNguoiNhan);
            }
            FB.AppRequest(
                NoiDungTinNhan,
                DanhSachIdNguoiNhan,
                DanhSachLocJson,
                null,
                null,
                DuLieuLuuTru,
                TieuDe,
                callback
            );

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    #region FB.Feed()
    #region Feed Param
    ////Id ba?n be` ma` mi`nh muô´n viê´t lên tuo`ng 
    //public string FeedToId = "";
    //public string FeedLink = "http://unity3d.com/";
    //public string FeedLinkName = "Link Name";
    //public string FeedLinkCaption = "Link Caption";
    //public string FeedLinkDescription = "Link Description";
    //public string FeedPicture = "https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcQFWLtAnaifE-6xU_lRwpErfufrznX8TOkdLql9Bcsn-sZiwwps";
    ////Tên link bên ca?nh nu´t chia se? 
    //public string FeedActionName = "Feed Action Name";
    ////link bên ca?nh chia se? 
    //public string FeedActionLink = "https://translate.google.com.vn/";
    //string FeedReference = "";
    //string FeedMediaSource = "";
    #endregion
    public static void FBFeed(Facebook.FacebookDelegate callbackSuccess = null,
                          Facebook.FacebookDelegate callbackError = null,
                          Facebook.FacebookDelegate callbackFBError = null,
                         string IdNguoiNhan = "",
                         string TenGame = "Link Name",
                         string LinkGame = "http://unity3d.com/",
                         string LinkHinhAnh = "https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcQFWLtAnaifE-6xU_lRwpErfufrznX8TOkdLql9Bcsn-sZiwwps",
                         string MoTa1 = "",
                         string MoTa2 = "")
    {
        string TenLinkLienKet = "";
        string LinkLienKet = "";
        string FeedReference = "";
        string FeedMediaSource = "";
        bool IncludeFeedProperties = false;
        Dictionary<string, string[]> FeedProperties = new Dictionary<string, string[]>();
        Dictionary<string, string[]> feedProperties = null;
        if (IncludeFeedProperties)
        {
            feedProperties = FeedProperties;
        }
        try
        {
            FB.Feed(
            toId: IdNguoiNhan,
            link: LinkGame,
            linkName: TenGame,
            linkCaption: MoTa1,
            linkDescription: MoTa2,
            picture: LinkHinhAnh,
            mediaSource: FeedMediaSource,
            actionName: TenLinkLienKet,
            actionLink: LinkLienKet,
            reference: FeedReference,
            properties: feedProperties,
            callback: result =>
            {
                if (!String.IsNullOrEmpty(result.Error))
                {
                    Debug.Log(result.Error);
                    if (callbackFBError != null)
                    {
                        callbackFBError(result);
                    }
                }
                else
                {
                    IDictionary dict = Json.Deserialize(result.Text) as IDictionary;
                    if (dict != null && dict["id"] != null)
                    {
                        if (callbackSuccess != null)
                        {
                            callbackSuccess(result);
                        }
                        //Debug.Log("FEED SUCCESS");
                    }
                    else
                    {
                        if (callbackError != null)
                        {
                            callbackError(result);
                        }
                        //Debug.Log("FEED CANCLE");
                    }
                }
            }
            );
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    #endregion

    public static void FBGetScore(Facebook.FacebookDelegate callback = null)
    {
        if (callback == null)
        {
            callback = result =>
            {
                Debug.Log(result.Text);
            };
        }
        if (FB.IsLoggedIn)
        {

            try
            {
                FB.API("v2.2/" + FB.AppId + "/scores", Facebook.HttpMethod.GET, callback);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }

    public static void FBSubmitScore(int Diem, Facebook.FacebookDelegate callback = null)
    {
        if (callback == null)
        {
            callback = result =>
            {
                Debug.Log(result.Text);
            };
        }
        if (FB.IsLoggedIn)
        {
            var query = new Dictionary<String, String>();
            query["score"] = Diem.ToString();

            try
            {
                FB.API("v2.2/me/scores", Facebook.HttpMethod.POST, callback, query);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }

    public static void FBGetInviteFriend(int limit = 100, Facebook.FacebookDelegate callback = null)
    {
        if (callback == null)
        {
            callback = result =>
            {
                Debug.Log(result.Text);
            };
        }
        try
        {
            FB.API("/v2.2/me/invitable_friends?fields=id,name,picture&limit=" + limit, Facebook.HttpMethod.GET, callback);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public static void FBGetUserFriend(int limit = 100, Facebook.FacebookDelegate callback = null)
    {
        if (callback == null)
        {
            callback = result =>
            {
                Debug.Log(result.Text);
            };
        }
        try
        {
            FB.API("/v2.2/me/friends?fields=id,name,picture&limit=" + limit, Facebook.HttpMethod.GET, callback);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    #region FB.GetDeepLink()

    private static void FBGetDeepLink(Facebook.FacebookDelegate callback = null)
    {
        if (callback == null)
        {
            callback = result =>
            {
                Debug.Log(result.Text);
            };
        }
        try
        {
            FB.GetDeepLink(callback);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    #endregion

    #region Take Screenshot
    private static IEnumerator TakeScreenshot(Facebook.FacebookDelegate callback)
    {
        yield return new WaitForEndOfFrame();
        var width = Screen.width;
        var height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        byte[] screenshot = tex.EncodeToPNG();

        var wwwForm = new WWWForm();
        wwwForm.AddBinaryData("image", screenshot, "InteractiveConsole.png");
        wwwForm.AddField("message", "herp derp.  I did a thing!  Did I do this right?");

        try
        {
            FB.API("me/photos", Facebook.HttpMethod.POST, callback, wwwForm);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public static void FBScreenShot(MonoBehaviour behaviour, Facebook.FacebookDelegate callbackSuccess = null, Facebook.FacebookDelegate callbackError = null)
    {
        try
        {
            behaviour.StartCoroutine(TakeScreenshot(result =>
            {
                if (!String.IsNullOrEmpty(result.Error))
                {
                    Debug.Log(result.Error);
                }
                else
                {
                    Debug.Log(result.Text);
                    IDictionary dict = Json.Deserialize(result.Text) as IDictionary;
                    if (dict != null && dict["id"] != null && callbackSuccess != null)
                    {
                        callbackSuccess(result);
                    }
                    else if (callbackError != null)
                    {
                        callbackError(result);
                    }
                }
            }));
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    #endregion

    public static void FBDeleteRequest(string RequestId = "", Facebook.FacebookDelegate callback = null)
    {
        if (callback == null)
        {
            callback = result =>
            {
                Debug.Log(result.Text);
            };
        }
        if (RequestId.Length > 0)
        {
            try
            {
                FB.API("v2.2/" + RequestId + "?method=delete", Facebook.HttpMethod.GET, callback);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        else
        {
            Debug.LogError("RequestId not valid");
        }
    }

    public static void FBLoadAvatar(MonoBehaviour monoBehaviour, string userId, LoadPictureCallback callback = null, int width = 128, int height = 128)
    {
        try
        {
            string url = Util.GetPictureURL(userId, width, height);
            //Debug.Log("LINK " + url);
            FB.API("v2.2/" + url, Facebook.HttpMethod.GET, result =>
            {
                if (result.Error != null)
                {
                    Util.LogError(result.Error);
                    return;
                }
                var imageUrl = Util.DeserializePictureURLString(result.Text);
                if (callback == null)
                {
                    callback = texture =>
                    {
                        Util.Log("MyPictureCallback");
                    };
                }
                try
                {
                    monoBehaviour.StartCoroutine(LoadPictureEnumerator(imageUrl, callback));
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            });
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public static void FBLoadAndSaveAvatar(MonoBehaviour monoBehaviour, string userId, LoadPictureCallback callback = null, bool replaceIfExist = true, int width = 128, int height = 128, string name = "", string path = "")
    {
        try
        {
            string nameImage = name.Length > 0 ? name : userId + ".jpg";
            string dirPath = path.Length > 0 ? path : Application.persistentDataPath + "/cacheavatar/";
            String filePath = dirPath + nameImage;
            string url = Util.GetPictureURL(userId, width, height);
            Debug.Log("LINK " + url);
            FB.API("v2.2/" + url, Facebook.HttpMethod.GET, result =>
            {
                if (result.Error != null)
                {
                    Util.LogError(result.Error);
                    return;
                }
                var dict = Json.Deserialize(result.Text) as IDictionary;

                var imageUrl = Util.DeserializePictureURLString(result.Text);
                if (callback == null)
                {
                    callback = texture =>
                    {
                        byte[] bytes = texture.EncodeToJPG();

                        if (!System.IO.Directory.Exists(dirPath))
                        {
                            System.IO.Directory.CreateDirectory(dirPath);
                            Debug.Log("Create directory");
                        }
                        System.IO.File.WriteAllBytes(filePath, bytes);
                        WWW imageToLoadPath = new WWW("file://" + filePath);
                        Texture2D texture2d = new Texture2D(width, height, TextureFormat.RGB24, false);
                        imageToLoadPath.LoadImageIntoTexture(texture2d);
                    };
                }
                try
                {
                    AudioControl.getMonoBehaviour().StartCoroutine(LoadPictureEnumerator(imageUrl, callback));
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            });
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public static IEnumerator LoadPictureEnumerator(string url, LoadPictureCallback callback)
    {
        WWW www = new WWW(url);
        yield return www;
        callback(www.texture);
    }

    public static void FBSendLife(string[] userIds, Facebook.FacebookDelegate callback = null)
    {
        if (callback == null)
        {
            callback = result =>
            {
                if (!String.IsNullOrEmpty(result.Error))
                {
                    Debug.Log(result.Error);
                }
                else
                {
                    Debug.Log(result.Text);
                }
            };
        }
        try
        {
            FB.AppRequest(DString.SEND_LIFE, Facebook.OGActionType.Send, DString.OBJID, userIds, DString.DATA_SEND_LIFE, "Send a life to friend", callback);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public static void FBAskForLife(string[] userIds, Facebook.FacebookDelegate callback = null)
    {
        if (callback == null)
        {
            callback = result =>
            {
                if (!String.IsNullOrEmpty(result.Error))
                {
                    Debug.Log(result.Error);
                }
                else
                {
                    Debug.Log(result.Text);
                }
            };
        }
        try
        {
            FB.AppRequest(DString.ASK_LIFE, Facebook.OGActionType.AskFor, DString.OBJID, userIds, DString.DATA_ASK_LIFE, "Ask for a life", callback);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
