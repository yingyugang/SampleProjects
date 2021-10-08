using UnityEngine;
using System.Collections;

public abstract class DialogAbs : MonoBehaviour {
    public delegate void CallBackShowDialog();
    public delegate void CallBackHideDialog();

    public bool Show;
    public abstract void ShowDialog(CallBackShowDialog callback = null);
    public abstract void HideDialog(CallBackHideDialog callback = null);
}
