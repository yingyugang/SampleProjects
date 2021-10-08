using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FarmUpgradeImageScript : MonoBehaviour
{
    public List<UITexture> mainTextures;//ao, 2 chuong cho, gieng, kho, nha, xaygio, canh
    public GameObject dogObject;
    int levelPassed;
    int levelUpgrade;
    GameObject groundFarm, groundPrefabs;
    // Use this for initialization
    void Start()
    {
        levelPassed = 0;
        levelPassed = MissionControl.max_mission;
        if (levelPassed > 28) levelUpgrade = 7;
        else if (levelPassed > 21) levelUpgrade = 6;
        else if (levelPassed > 15) levelUpgrade = 5;
        else if (levelPassed > 21) levelUpgrade = 4;
        else if (levelPassed > 8) levelUpgrade = 3;
        else if (levelPassed > 5) levelUpgrade = 2;
        else levelUpgrade = 1;
        getGround();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            levelUpgrade++;
            if (levelUpgrade > 16) levelUpgrade = 16;
            print("Level upgrade :" + levelUpgrade);
            getGround();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            levelUpgrade--;
            if (levelUpgrade == 0) levelUpgrade = 1;
            print("Level upgrade :" + levelUpgrade);
            getGround();
        }
    }

    void getGround()
    {
        #region chó
        if (levelUpgrade > 6) dogObject.transform.localPosition = new Vector3(-94, -455);
        else if (levelUpgrade > 3) dogObject.transform.localPosition = new Vector3(-200, -420);
        else dogObject.transform.localPosition = new Vector3(-240, -450);
        #endregion

        #region giếng và chuồng chó
        if (levelUpgrade < 2)//level 1
        {
            mainTextures[3].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/gieng/1");
            mainTextures[1].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/chuong cho/1");
            mainTextures[2].enabled = false;
        }
        else if (levelUpgrade < 12)
        {
            mainTextures[3].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/gieng/2+3");
            mainTextures[1].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/chuong cho/2+3");
            mainTextures[2].enabled = (levelUpgrade < 7 ? false : true);
            mainTextures[2].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/chuong cho/2+3");
        }
        else
        {
            mainTextures[3].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/gieng/4");
            mainTextures[1].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/chuong cho/4");
            mainTextures[2].enabled = true;
            mainTextures[2].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/chuong cho/4");
        }
        #endregion

        #region nhà và ao
        if (levelUpgrade < 3)//level 1
        {
            mainTextures[5].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/nha/1");
            mainTextures[0].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/ao ca/1");
        }
        else if (levelUpgrade < 13)
        {
            mainTextures[5].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/nha/" + (levelUpgrade < 8 ? "2" : "3"));
            mainTextures[0].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/ao ca/" + (levelUpgrade < 8 ? "2" : "3"));
        }
        else
        {
            mainTextures[5].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/nha/4");
            mainTextures[0].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/ao ca/4");
        }
        #endregion

        #region cối xay gió và kho
        if (levelUpgrade < 5)//level 1
        {
            mainTextures[4].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/kho/1");
            mainTextures[4].transform.localScale = Vector3.one * 0.8f;
            mainTextures[6].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/xay gio/1");
            mainTextures[7].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/xay gio/x");
            mainTextures[7].transform.localPosition = new Vector3(20, -28);
        }
        else if (levelUpgrade < 15)
        {
            mainTextures[4].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/kho/" + (levelUpgrade < 10 ? "2" : "3"));
            mainTextures[4].transform.localScale = Vector3.one;
            mainTextures[6].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/xay gio/" + (levelUpgrade < 10 ? "2" : "3"));
            mainTextures[7].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/xay gio/" + (levelUpgrade < 10 ? "x" : "x2"));
            mainTextures[7].transform.localPosition = new Vector3(15, 42);
        }
        else
        {
            mainTextures[4].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/kho/4");
            mainTextures[4].transform.localScale = Vector3.one;
            mainTextures[6].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/xay gio/4");
            mainTextures[7].mainTexture = Resources.Load<Texture>("Farm/Upgrade/nag cap/xay gio/x2");
            mainTextures[7].transform.localPosition = new Vector3(-5, 90);
        }
        #endregion

        #region ground Farm

        Transform tempGround = transform.Find("ForeGround");
        if (tempGround != null)
        {
            GameObject.Destroy(tempGround.gameObject);
        }
        if (levelUpgrade == 1)
        {
            groundPrefabs = (GameObject)Resources.Load("Farm/GroundUpgrade/GroundObjectLv10");
            groundFarm = (GameObject)Instantiate(groundPrefabs);
        }
        else
        {
            string tempStr;
            if (levelUpgrade >= 16) tempStr = "40";
            else if (levelUpgrade >= 14) tempStr = "32";
            else if (levelUpgrade >= 12) tempStr = "31";
            else if (levelUpgrade >= 11) tempStr = "30";
            else if (levelUpgrade >= 9) tempStr = "22";
            else if (levelUpgrade >= 7) tempStr = "21";
            else if (levelUpgrade >= 6) tempStr = "20";
            else if (levelUpgrade >= 4) tempStr = "12";
            else if (levelUpgrade >= 2) tempStr = "11";
            else tempStr = "10";
            groundPrefabs = (GameObject)Resources.Load("Farm/GroundUpgrade/GroundObjectLv" + tempStr);
            groundFarm = (GameObject)Instantiate(groundPrefabs);
        }
        // print(groundPrefabs.name);
        groundFarm.name = "ForeGround";
        groundFarm.transform.parent = this.transform;
        groundFarm.transform.localPosition = Vector3.zero;
        groundFarm.transform.localScale = Vector3.one;
        #endregion
    }
}
