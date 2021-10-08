using UnityEngine;
using System.Collections;

public class ItemTechnogyController : MonoBehaviour {

    public int iDButton;
    public int levelObject;
    public int maxLevel;
    void OnClick()
    {
        Transform parent = this.transform.parent.parent.parent.parent;
        parent.GetComponent<TechnogyController>().iDSelected = this.iDButton;
        parent.GetComponent<TechnogyController>().levelSelected = this.levelObject;
        parent.GetComponent<TechnogyController>().maxSelected = this.maxLevel;
    }
}
