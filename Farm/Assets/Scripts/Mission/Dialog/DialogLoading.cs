using UnityEngine;
using System.Collections;

public class DialogLoading : MonoBehaviour
{

    bool isShow = false;

    public void ShowLoading(bool showLoad = true)
    {
        //Dialog loading bo truong hop nay dj
        //if (isShow)
        //{
        //    return;
        //}
        isShow = true;
        transform.Find("Loading").gameObject.SetActive(showLoad);
        this.gameObject.SetActive(true);
    }
    public void HideLoading()
    {
        isShow = false;
        this.gameObject.SetActive(false);
    }
}
