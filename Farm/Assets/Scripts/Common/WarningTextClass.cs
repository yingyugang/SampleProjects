using UnityEngine;
using System.Collections;

public class WarningTextClass  {

    public string text {set; get; }
    public int type { set; get; }

    public WarningTextClass( string text, int type)
    {
        this.text = text;
        this.type = type;
    }
}
