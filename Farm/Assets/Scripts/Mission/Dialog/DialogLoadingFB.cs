using UnityEngine;
using System.Collections;

public class DialogLoadingFB : DialogAbs
{

    public override void ShowDialog(DialogAbs.CallBackShowDialog callback = null)
    {
        Show = true;
        transform.Find("BackGround").gameObject.SetActive(true);
    }

    public override void HideDialog(DialogAbs.CallBackHideDialog callback = null)
    {
        Show = false;
        transform.Find("BackGround").gameObject.SetActive(false);
    }

    public static void ShowFBLoading()
    {
        GameObject dialog = GameObject.Find("DialogLoadingFB");
        if(dialog != null)
        {
            dialog.GetComponent<DialogLoadingFB>().ShowDialog();
        }
    }

    public static void HideFBLoading()
    {
        GameObject dialog = GameObject.Find("DialogLoadingFB");
        if (dialog != null)
        {
            dialog.GetComponent<DialogLoadingFB>().HideDialog();
        }
    }
}
