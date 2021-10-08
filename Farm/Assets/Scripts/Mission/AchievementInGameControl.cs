using UnityEngine;
using System.Collections;

public class AchievementInGameControl : MonoBehaviour {

    public static AchiFinish achiFinish;
    public Transform PopupAchievement;

	void Start () {
	}
	
	void Update () {
        if (achiFinish.finish)
        {
            achiFinish.finish = false;
            Transform popup = Instantiate(PopupAchievement) as Transform;
            popup.parent = this.transform;
            popup.localPosition = new Vector3(0, 450, 0);
            popup.localScale = new Vector3(1,1,1);
            popup.GetComponent<PopupAchievement>().ShowPopup(achiFinish.title, achiFinish.detail);
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            Transform popup = Instantiate(PopupAchievement) as Transform;
            popup.parent = this.transform;
            popup.localPosition = new Vector3(0, 450, 0);
            popup.localScale = new Vector3(1, 1, 1);
            popup.GetComponent<PopupAchievement>().ShowPopup("Test", "Achivement test");
        }
	}
}

public struct AchiFinish
{
    public bool finish;
    public string title;
    public string detail ; 
}