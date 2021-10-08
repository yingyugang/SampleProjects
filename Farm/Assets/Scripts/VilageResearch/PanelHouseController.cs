using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelHouseController : MonoBehaviour
{

    public UIButton[] arrayHouse;
    public static List<int> listIDHouseSelected, listDenyClick;
    public static List<Texture> listSpriteSelect, listSpriteNomal;
    public static int iDFirst;
    public static bool isUpdate;

   
    private int count;
    public GameObject controllerPrefabs;
    public static string nameScreenPre;
    void Start()
    {
        if (nameScreenPre == null)
        {
            nameScreenPre = "Town";
        }
        GameObject.Find("CommonObject").GetComponent<CommonObjectScript>().WarningInvisible(CommonObjectScript.Button.Result);
        count = 0;
        isUpdate = false;
        if (!VilageResearchController.isCreate)
        {
            Instantiate(controllerPrefabs).name = controllerPrefabs.name;
        }

        if (listIDHouseSelected == null)
        {
            listIDHouseSelected = new List<int>();
            listDenyClick = new List<int>();
            listSpriteSelect = new List<Texture>();
            listSpriteNomal= new List<Texture>();
        }

        foreach (int iDHouseSelected in listIDHouseSelected)
        {
            arrayHouse[iDHouseSelected].GetComponent<UITexture>().mainTexture = listSpriteSelect[count];
            count++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        BackButton();
        if (isUpdate)
        {
            arrayHouse[iDFirst].GetComponent<UITexture>().mainTexture = listSpriteNomal[0];
            isUpdate = false;
        }
    }
    public void Close_Click()
    {
        if (!TownScenesController.isHelp)
        {
            CreatTownScenesController.isDenyContinue = false;
            //Application.LoadLevel(nameScreenPre);
            LoadingScene.ShowLoadingScene(nameScreenPre, true);
        }
      
    }
    private void BackButton()
    {
        if (!TownScenesController.isHelp)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || CommonObjectScript.isEndGame || CommonObjectScript.isViewComplete)
            {
                CreatTownScenesController.isDenyContinue = false;
                //Application.LoadLevel(nameScreenPre);
                LoadingScene.ShowLoadingScene(nameScreenPre, true);
            }
        }
    }
    
}
