using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;

public class VilageResearchController : MonoBehaviour
{

    // Use this for initialization
    private float timeTalk;
    private float countTimeTalk;
    private bool isRead;
    public static bool isCreate;
   // private List<int> listProductInSuperMarket;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {

        // MissionData.townDataMission.itemsInShop; cái này mới đúng, sau này dùng
        //listProductInSuperMarket = new List<int> { 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
       // listProductInSuperMarket = MissionData.townDataMission.itemsInShop;
        isCreate = true;
        isRead = false;
        timeTalk = 4.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (PanelHouseController.listIDHouseSelected.Count != 0)
        {
            if (!isRead)
            {
                PanelHouseController.iDFirst = PanelHouseController.listIDHouseSelected[0];
                PanelFrameController.iDtalKing = PanelHouseController.listIDHouseSelected[0];

                PanelFrameController.isUpdate = true;
                PanelHouseController.isUpdate = true;
                isRead = true;
            }

            if (countTimeTalk <= timeTalk)
            {
                countTimeTalk += Time.deltaTime;
            }
            else
            {

                print("Đưa ra kết quả");
                RandomProduct();
                PanelFrameController.iDFinishTalk.Add(PanelHouseController.listIDHouseSelected[0]);
                PanelFrameController.isUpdate = true;
                PanelHouseController.listSpriteNomal.RemoveAt(0);
                if (!Application.loadedLevelName.Equals("VilageResearch") && PanelHouseController.listIDHouseSelected.Count == 1)
                {
                    GameObject.Find("CommonObject").GetComponent<CommonObjectScript>().WarningVisible(CommonObjectScript.Button.Result);
                }
                PanelHouseController.listIDHouseSelected.RemoveAt(0);
                countTimeTalk = 0;
                isRead = false;

            }
        }
        if (Application.loadedLevelName.Equals("Mission"))
        {
            Destroy(gameObject);
        }
    }

    void RandomProduct()
    {
        int tempID = Random.Range(0, 10000) % MissionData.townDataMission.itemsInShop.Count;
        PanelFrameController.iDProductResearch.Add(MissionData.townDataMission.itemsInShop[tempID]);
    }
    public static void ResetVilage()
    {
        if (PanelHouseController.listDenyClick != null)
        {
            PanelHouseController.listDenyClick.Clear();
            PanelHouseController.listIDHouseSelected.Clear();
            PanelHouseController.listSpriteSelect.Clear();
            PanelHouseController.listSpriteNomal.Clear();
        }
        if (PanelFrameController.iDFinishTalk != null)
        {
        PanelFrameController.iDFinishTalk.Clear();
        PanelFrameController.iDProductResearch.Clear();
        }

    }
}
