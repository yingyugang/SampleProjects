using UnityEngine;
using System.Collections;

public class PanelDecoController : MonoBehaviour
{

    // Use this for initialization
    public GameObject[] arrayGameObjectLevel1;
    public GameObject[] arrayGameObjectlevel2;
    void Start()
    {
        SetVisiblePanel(MissionControl.max_mission);
    }

    // Update is called once per frame
    void Update()
    {
    }
    void SetPanel(bool centerBottomView, bool leftTopView, bool centerView, bool rightTopView, bool leftBottomView, bool rightBottomView)
    {
        arrayGameObjectLevel1[0].SetActive(centerBottomView);
        arrayGameObjectLevel1[1].SetActive(leftTopView);
        arrayGameObjectLevel1[2].SetActive(centerView);
        arrayGameObjectLevel1[3].SetActive(rightTopView);
        arrayGameObjectLevel1[4].SetActive(leftBottomView);
        arrayGameObjectLevel1[5].SetActive(rightBottomView);

        arrayGameObjectlevel2[0].SetActive(!centerBottomView);
        arrayGameObjectlevel2[1].SetActive(!leftTopView);
        arrayGameObjectlevel2[2].SetActive(!centerView);
        arrayGameObjectlevel2[3].SetActive(!rightTopView);
        arrayGameObjectlevel2[4].SetActive(!leftBottomView);
        arrayGameObjectlevel2[5].SetActive(!rightBottomView);
    }
    void SetVisiblePanel(int curentLevel)
    {
        if (curentLevel < 4)
        {
            SetPanel(true, true, true, true, true, true);
        }
        else if (curentLevel >= 4 && curentLevel < 9)
        {
            SetPanel(false, true, true, true, true, true);
        }
        else if (curentLevel >= 9 && curentLevel < 13)
        {
            SetPanel(false, false, true, true, true, true);
        }
        else if (curentLevel >= 13 && curentLevel < 17)
        {
            SetPanel(false, false, false, true, true, true);
        }
        else if (curentLevel >= 17 && curentLevel < 20)
        {
            SetPanel(false, false, false, false, true, true);
        }
        else if (curentLevel >= 20 && curentLevel < 23)
        {
            SetPanel(false, false, false, false, false, true);
        }
        else if (curentLevel >= 23)
        {
            SetPanel(false, false, false, false, false, false);
        }
    }
}
