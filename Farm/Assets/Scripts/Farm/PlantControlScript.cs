using Assets.Scripts.Common;
using Assets.Scripts.Farm;
using System;
using UnityEngine;

public class PlantControlScript : MonoBehaviour
{
    public static int breedSelected;
    public GameObject[] listObjects;//list object in game
    public GameObject KhungItem;//cover list item useable
    public GameObject PlantingOb;//cover diamond,timeBar
    public GameObject frame_timer;
    public GameObject guide_farm;//cover guide UI
    public GameObject panelPopup;
    public CameracontrollerScript cameraScript;

    public FarmCenterScript farmCenter;
    public GuideFarmScript guidefarmScript;

    int indexPopup, nindex, timeShow;

    Animator animatorKhungitem;
    Transform[] TransformsOfTimer, transformOfListItem;

    ReadXML frameItemDataXML;
    GameObject HarvestPlantPrefabs, HarvestPlant, HealingPrefabs, HealingObject, valuePrefabs;

    //position of animal
    int[,] positionAnimal = new int[,] { { -13, -35 }, { -65, -8 }, { 31, -10 }, { -13, 11 } };
    int[,] positionAnimal2 = new int[,] { { -16, 0 }, { 0, 16 }, { 0, -16 }, { 16, 0 } };

    float countTime = 0f;
    int countTempTime = 0, tempValue;


    GameObject plantPrefabs, plant, animalPrefabs, animal;
    Color32 tempColor;//color of breed
    Transform tempObject;//temp object to get Object
    string[] tempNames = new string[] { "dat", "chuong", "chuongca" };
    bool isVN;
    // Use this for initialization
    void Start()
    {
        breedSelected = -1;
        GoogleAnalytics.instance.LogScreen("Farm Screen");
        if (VariableSystem.language.Equals("Vietnamese")) isVN = true;
        else isVN = false;
        valuePrefabs = (GameObject)Resources.Load("Farm/AddValue");
        HarvestPlantPrefabs = (GameObject)Resources.Load("Farm/HarvestPlant");
        HealingPrefabs = (GameObject)Resources.Load("Farm/Thuoc");
        plantPrefabs = (GameObject)Resources.Load("Farm/Breed/Plant");


        guidefarmScript = guide_farm.GetComponent<GuideFarmScript>();
        TransformsOfTimer = frame_timer.GetComponentsInChildren<Transform>();
        frame_timer.SetActive(false);
        transformOfListItem = KhungItem.GetComponentsInChildren<Transform>();//get childrens in Popup

        animatorKhungitem = KhungItem.GetComponentInChildren<Animator>();
        indexPopup = -1;
        farmCenter = GameObject.FindGameObjectWithTag("FarmObject").GetComponent<FarmCenterScript>();
        farmCenter.common.WarningInvisible(CommonObjectScript.Button.Farm);
        UpdateImgAllCell();
    }

    // Update is called once per frame
    void Update()
    {
        countTime += Time.deltaTime;
        if (countTime > 4f)
        {
            countTime = 0;
            countTempTime++;
            if (countTempTime >= 8) countTempTime = 0;
            tempValue = UnityEngine.Random.Range(0, 1250) % 100;
            if (!(tempValue > 50))
            {
                if (countTempTime == 0) CommonObjectScript.audioControl.PlaySound("Chim keu");
                else if (countTempTime == 1 && checkPlant("chicken")) CommonObjectScript.audioControl.PlaySound("Ga keu");
                else if (countTempTime == 2 && checkPlant("cow")) CommonObjectScript.audioControl.PlaySound("Bo keu");
                else if (countTempTime == 3) CommonObjectScript.audioControl.PlaySound("Tieng gio");
                else if (countTempTime == 4 && checkPlant("pig")) CommonObjectScript.audioControl.PlaySound("Lon keu");
                else if (countTempTime == 6) CommonObjectScript.audioControl.PlaySound("Con vat keu bg");
                else if (countTempTime == 7) CommonObjectScript.audioControl.PlaySound("Cho sua");
                else if (checkPlant("fish")) CommonObjectScript.audioControl.PlaySound("Ca boi");
            }
        }
        for (int i = 0; i < MissionData.farmDataMission.fieldFarms.Count; i++)
        {
            if (MissionData.farmDataMission.fieldFarms[i].idField == 1)
            {
                //if(farmCenter.fieldFarms[0])
            }
        }

        if (farmCenter.idNeedUpdate != -1)
        {
            UpdateImgAllCell();
            farmCenter.idNeedUpdate = -1;
        }
        if (!(indexPopup == -1 || farmCenter.listDatas[indexPopup].idBreed == 0))//showing timer bar
        {
            if (farmCenter.listDatas[indexPopup].stage == 3)
            {
                animatorKhungitem.Play("CloseKhungItem");
                CollapseFrame_Timer();
                indexPopup = -1;
            }
            else
            {
                TransformsOfTimer[7].GetComponent<UITexture>().fillAmount = farmCenter.listDatas[indexPopup].timeGrowUp / farmCenter.listDatas[indexPopup].maxtimeGrowUp;
                timeShow = (int)(farmCenter.listDatas[indexPopup].maxtimeGrowUp - farmCenter.listDatas[indexPopup].timeGrowUp);
                TransformsOfTimer[8].GetComponent<UILabel>().text = ConvertIntToDays(timeShow);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) || CommonObjectScript.isEndGame || CommonObjectScript.isViewComplete)
        {
            #region back action handle
            if (CommonObjectScript.isGuide) return;
            if (CommonObjectScript.isViewPoppup)
            {
                tempObject = panelPopup.transform.Find("PopupConfirmFarm");
                tempObject.GetComponent<PopupImproveField>().Cancel_Click();
            }
            else if (indexPopup != -1)
            {
                animatorKhungitem.Play("CloseKhungItem");
                if (farmCenter.listDatas[indexPopup].idBreed != 0)
                {
                    CollapseFrame_Timer();
                }
                indexPopup = -1;
            }
            #endregion
        }
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            #region back action handle
            if (CommonObjectScript.isGuide) return;
            tempObject = panelPopup.transform.Find("PopupConfirmFarm");
            if (tempObject.gameObject.activeSelf)
                tempObject.GetComponent<PopupImproveField>().Cancel_Click();
            if (indexPopup != -1)
            {
                animatorKhungitem.Play("CloseKhungItem");
                if (farmCenter.listDatas[indexPopup].idBreed != 0)
                {
                    CollapseFrame_Timer();
                }
                indexPopup = -1;
            }
            #endregion
        }
    }

    private string ConvertIntToDays(int timeShow)
    {
        if (timeShow <= 0)
        {
            return "";
        }
        else if (timeShow == 1)
        {
            return "1 " + (isVN ? "Giờ" : "Hour");
        }
        else if (timeShow < 24)
        {
            return timeShow + (isVN ? " Giờ" : " Hours");
        }
        else
        {
            int temp = timeShow / 24;
            if (temp < 2)
            {
                return "1" + (isVN ? " Ngày " : " Day ") + ConvertIntToDays(timeShow % 24);
            }
            else
            {
                return temp + (isVN ? " Ngày " : " Days ") + ConvertIntToDays(timeShow % 24);
            }
        }
    }

    bool checkPlant(string fedd)//check breed to play sound
    {
        if (fedd.Equals("fish"))
        {
            for (int i = 18; i < 22; i++) if (farmCenter.listDatas[i].idBreed != 0) return true;
        }
        for (int i = 12; i < 18; i++)
        {
            if (farmCenter.listDatas[i].nameBreed.Equals(fedd)) return true;
        }
        return false;
    }

    private void UpdateImgAllCell()// update image for all the cell wwhen player come back Farm.
    {
        for (int i = 0; i < 22; i++)
        {
            if (!farmCenter.isOpenCells[i])
            {
                #region not open cell
                if (i < 12)
                    listObjects[i].GetComponentsInChildren<UIButton>()[0].normalSprite = tempNames[0] + "0";
                else if (i < 18)
                {
                    tempObject = listObjects[i].transform.Find("wattle");
                    listObjects[i].GetComponentsInChildren<UIButton>()[0].normalSprite = tempNames[1] + "0";
                    tempObject.GetComponent<UISprite>().enabled = false;
                }
                else listObjects[i].GetComponentsInChildren<UIButton>()[0].normalSprite = tempNames[2] + "0";
                #endregion
            }
            else
            {
                if (i < 12)
                {
                    #region field
                    listObjects[i].GetComponentsInChildren<UIButton>()[0].normalSprite = tempNames[0] + farmCenter.fieldFarms[0].currentLevel;
                    #endregion
                }
                else
                {
                    #region img cell
                    if (i < 18)
                    {
                        #region cage
                        tempObject = listObjects[i].transform.Find("wattle");
                        listObjects[i].GetComponentsInChildren<UIButton>()[0].normalSprite = tempNames[1] + farmCenter.fieldFarms[1].currentLevel + "-0";
                        tempObject.GetComponent<UISprite>().enabled = true;
                        tempObject.GetComponent<UISprite>().spriteName = tempNames[1] + farmCenter.fieldFarms[1].currentLevel + "-1";
                        #endregion
                    }
                    else
                    {
                        #region pond
                        listObjects[i].GetComponentsInChildren<UIButton>()[0].normalSprite = tempNames[2] + farmCenter.fieldFarms[2].currentLevel;
                        #endregion

                    }
                    #endregion
                }
                UpdateImgCell(i);
            }
        }
    }

    private void UpdateImgCell(int i)// update image for one cell.
    {
        /* Update one field
             * if exist breed is planted: If harvested => destroy object. If explaned => change Img cell
             * If not exist breed is planted: if change state => change animation. If new plant => create object
             */
        if (i < 0 || !farmCenter.isOpenCells[i]) { print("Error parameter : " + i); return; }
        if (i < 12)
        {
            #region field
            tempObject = listObjects[i].transform.Find("plant");
            if (farmCenter.listDatas[i].idBreed == 0)
            {
                if (tempObject != null) GameObject.Destroy(tempObject.gameObject);
                else if (farmCenter.isOpenCells[i]) listObjects[i].GetComponentsInChildren<UIButton>()[0].normalSprite = tempNames[0] + farmCenter.fieldFarms[0].currentLevel;
            }
            else
            {
                if (tempObject != null)
                {
                    #region change animation
                    UISprite spriteOfPlant = tempObject.GetComponentInChildren<UISprite>();
                    spriteOfPlant.spriteName = farmCenter.listDatas[i].nameBreed + farmCenter.listDatas[i].stage.ToString();
                    if (farmCenter.listDatas[i].stage == 1)
                    {
                        spriteOfPlant.width = 175;
                        spriteOfPlant.height = 95;
                    }
                    else if (farmCenter.listDatas[i].stage == 2)
                    {
                        spriteOfPlant.width = 190;
                        spriteOfPlant.height = 120;
                    }
                    else
                    {
                        spriteOfPlant.width = 200;
                        spriteOfPlant.height = 130;
                    }
                    tempObject.GetComponent<Animator>().Play("stage" + farmCenter.listDatas[i].stage);
                    setStatusSick(i, farmCenter.listDatas[i].status, spriteOfPlant);
                    #endregion
                }
                else
                {
                    create_Plant(i, farmCenter.listDatas[i].nameBreed, farmCenter.listDatas[i].stage);
                }
            }
            #endregion
        }
        else if (i < 18)
        {
            #region cage
            tempObject = listObjects[i].transform.Find("animal0");
            if (farmCenter.listDatas[i].idBreed == 0)
            {
                if (tempObject != null)
                {
                    Animator[] temps = listObjects[i].transform.GetComponentsInChildren<Animator>();
                    foreach (Animator temp in temps)
                    {
                        GameObject.Destroy(temp.gameObject);
                    }
                }
                else
                {
                    tempObject = listObjects[i].transform.Find("wattle");
                    listObjects[i].GetComponentsInChildren<UIButton>()[0].normalSprite = tempNames[1] + farmCenter.fieldFarms[1].currentLevel + "-0";
                    tempObject.GetComponent<UISprite>().enabled = true;
                    tempObject.GetComponent<UISprite>().spriteName = tempNames[1] + farmCenter.fieldFarms[1].currentLevel + "-1";
                }
            }
            else
            {
                if (tempObject != null)
                {
                    for (int j = 0; j < farmCenter.listDatas[i].Yield / 2; j++)
                    {
                        tempObject = listObjects[i].transform.Find("animal" + j);
                        if (tempObject == null)
                        {
                            create_Animal(i, farmCenter.listDatas[i].nameBreed, farmCenter.listDatas[i].stage, farmCenter.listDatas[i].Yield / 2, true);
                        }
                        else
                        {
                            tempObject.GetComponent<Animator>().Play("stage" + (farmCenter.listDatas[i].stage == 3 ? "2" : "1"));
                            setStatusSick(i, farmCenter.listDatas[i].status, tempObject.GetComponentsInChildren<UISprite>());
                        }
                    }
                }
                else
                {
                    create_Animal(i, farmCenter.listDatas[i].nameBreed, farmCenter.listDatas[i].stage, farmCenter.listDatas[i].Yield / 2);
                }
            }
            #endregion
        }
        else
        {
            #region pond
            tempObject = listObjects[i].transform.Find("animal0");
            if (farmCenter.listDatas[i].idBreed == 0)//after harvest
            {
                if (tempObject != null)
                {
                    Animator[] temps = listObjects[i].transform.GetComponentsInChildren<Animator>();
                    foreach (Animator temp in temps)
                    {
                        GameObject.Destroy(temp.gameObject);
                    }
                }
                else listObjects[i].GetComponentsInChildren<UIButton>()[0].normalSprite = tempNames[2] + farmCenter.fieldFarms[2].currentLevel;
            }
            else
            {
                if (tempObject != null)
                {
                    for (int j = 0; j < farmCenter.listDatas[i].Yield / 2; j++)
                    {
                        tempObject = listObjects[i].transform.Find("animal" + j);
                        if (tempObject == null)
                        {
                            create_Animal(i, farmCenter.listDatas[i].nameBreed, farmCenter.listDatas[i].stage, farmCenter.listDatas[i].Yield / 2, true);
                        }
                        else
                        {
                            tempObject.GetComponent<Animator>().Play("stage" + (farmCenter.listDatas[i].stage == 3 ? "2" : "1"));
                            setStatusSick(i, farmCenter.listDatas[i].status, tempObject.GetComponentsInChildren<UISprite>());
                        }
                    }
                }
                else
                {
                    create_Animal(i, farmCenter.listDatas[i].nameBreed, farmCenter.listDatas[i].stage, farmCenter.listDatas[i].Yield / 2);
                }
            }
            #endregion
        }
        if (farmCenter.listDatas[i].stage == 3 && DialogShop.BoughtItem[7])
        {
            #region auto harvest
            CreateAnimationHarvest(farmCenter.listDatas[i].idBreed + "." + farmCenter.listDatas[i].nameBreed, farmCenter.listDatas[i].Yield, i);
            CreateAnimationAddValue(farmCenter.listDatas[i].idBreed + "." + farmCenter.listDatas[i].nameBreed, "+" + farmCenter.listDatas[i].Yield, true, i);
            CommonObjectScript.arrayMaterials[farmCenter.listDatas[i].idBreed - 1] += farmCenter.listDatas[i].Yield;
            MissionData.farmDataMission.breedsFarm[farmCenter.breedFarms[farmCenter.listDatas[i].idBreed - 1].index].currentNumber++;
            if (farmCenter.listDatas[i].idBreed < 5) MissionData.farmDataMission.harvestField.currentNumber++;
            else MissionData.farmDataMission.harvestCage.currentNumber++;
            farmCenter.listDatas[i] = new Breed();
            CommonObjectScript.audioControl.PlaySound("Chat hang");
            if (indexPopup == i)
            {
                animatorKhungitem.Play("CloseKhungItem");
                CollapseFrame_Timer();
                indexPopup = -1;
            }
            UpdateImgCell(i);
            #endregion
        }
    }

    private void create_Plant(int index, string name, int stage)//create one plant
    {
        plant = (GameObject)Instantiate(plantPrefabs);
        plant.name = "plant";
        plant.transform.parent = listObjects[index].transform;
        plant.GetComponent<Transform>().localPosition = Vector3.zero;
        plant.GetComponent<Transform>().localScale = Vector3.one;
        UISprite spriteOfPlant = plant.GetComponentInChildren<UISprite>();
        spriteOfPlant.spriteName = name + stage.ToString();
        if (stage == 1)
        {
            spriteOfPlant.width = 175;
            spriteOfPlant.height = 95;
        }
        else if (stage == 2)
        {
            spriteOfPlant.width = 190;
            spriteOfPlant.height = 120;
        }
        else
        {
            spriteOfPlant.width = 200;
            spriteOfPlant.height = 130;
        }
        setRenderer(spriteOfPlant, index);
        setStatusSick(index, farmCenter.listDatas[index].status, spriteOfPlant);
        plant.GetComponent<Animator>().Play("stage" + stage);
    }
    private void create_Animal(int index, string name, int stage, int number, bool isAdd = false)
    {
        animalPrefabs = (GameObject)Resources.Load("Farm/Breed/" + name);
        if (!isAdd)
            for (int i = number - 1; i >= 0; i--)
            {
                animal = (GameObject)Instantiate(animalPrefabs);
                animal.name = "animal" + i;
                animal.transform.parent = listObjects[index].transform;
                if (index < 18)
                    animal.GetComponent<Transform>().localPosition = new Vector3(positionAnimal[i, 0], positionAnimal[i, 1], 0);
                else
                    animal.GetComponent<Transform>().localPosition = new Vector3(positionAnimal2[i, 0], positionAnimal2[i, 1], 0);
                animal.GetComponent<Animator>().Play("stage" + (farmCenter.listDatas[index].stage == 3 ? "2" : "1"));
                animal.GetComponent<Transform>().localScale = Vector3.one;
                UISprite[] spriteOfAnimals = animal.GetComponentsInChildren<UISprite>();
                setRenderer(spriteOfAnimals[0], index, spriteOfAnimals, name);
                setStatusSick(index, farmCenter.listDatas[index].status, spriteOfAnimals, name);
            }
        else
        {
            if (number >= 4) return;
            animal = (GameObject)Instantiate(animalPrefabs);
            animal.name = "animal" + number;
            animal.transform.parent = listObjects[index].transform;
            if (index < 18)
                animal.GetComponent<Transform>().localPosition = new Vector3(positionAnimal[number, 0], positionAnimal[number, 1], 0);
            else
                animal.GetComponent<Transform>().localPosition = new Vector3(positionAnimal2[number, 0], positionAnimal2[number, 1], 0);
            animal.GetComponent<Animator>().Play("stage" + (farmCenter.listDatas[index].stage == 3 ? "2" : "1"));
            animal.GetComponent<Transform>().localScale = Vector3.one;
            UISprite[] spriteOfAnimals = animal.GetComponentsInChildren<UISprite>();
            setRenderer(spriteOfAnimals[0], index, spriteOfAnimals, name);
            setStatusSick(index, farmCenter.listDatas[index].status, spriteOfAnimals, name);
        }
    }
    private void setRenderer(UISprite sprite, int index, UISprite[] sprites = null, string breed = "")
    {
        switch (index)
        {
            case 0: sprite.depth = 24; break;
            case 1:
            case 3: sprite.depth = 25; break;
            case 2:
            case 4:
            case 6: sprite.depth = 26; break;
            case 5:
            case 7:
            case 9: sprite.depth = 27; break;
            case 8:
            case 10: sprite.depth = 28; break;
            case 11: sprite.depth = 29; break;
            case 12:
                {
                    #region
                    if (breed.Equals("cow"))
                    {
                        sprites[0].depth = 22;
                        sprites[1].depth = 20;
                        sprites[2].depth = 20;
                        sprites[3].depth = 21;
                        sprites[4].depth = 21;
                        sprites[5].depth = 19;
                        sprites[6].depth = 19;
                    }
                    else if (breed.Equals("chicken"))
                    {
                        sprites[0].depth = 21;
                        sprites[1].depth = 21;
                        sprites[2].depth = 20;
                        sprites[3].depth = 22;
                        sprites[4].depth = 20;
                        sprites[5].depth = 22;
                        sprites[6].depth = 19;
                        sprites[7].depth = 20;
                        sprites[8].depth = 19;
                    }
                    else if (breed.Equals("pig"))
                    {
                        sprites[0].depth = 22;
                        sprites[1].depth = 20;
                        sprites[2].depth = 20;
                        sprites[3].depth = 21;
                        sprites[4].depth = 21;
                        sprites[5].depth = 21;
                        sprites[6].depth = 21;
                        sprites[7].depth = 19;
                        sprites[8].depth = 19;
                    }
                    #endregion
                    break;
                }
            case 13:
            case 15:
                {
                    #region
                    if (breed.Equals("cow"))
                    {
                        sprites[0].depth = 16;
                        sprites[1].depth = 14;
                        sprites[2].depth = 14;
                        sprites[3].depth = 15;
                        sprites[4].depth = 15;
                        sprites[5].depth = 13;
                        sprites[6].depth = 13;
                    }
                    else if (breed.Equals("chicken"))
                    {
                        sprites[0].depth = 15;
                        sprites[1].depth = 15;
                        sprites[2].depth = 14;
                        sprites[3].depth = 16;
                        sprites[4].depth = 14;
                        sprites[5].depth = 16;
                        sprites[6].depth = 13;
                        sprites[7].depth = 14;
                        sprites[8].depth = 13;
                    }
                    else if (breed.Equals("pig"))
                    {
                        sprites[0].depth = 16;
                        sprites[1].depth = 14;
                        sprites[2].depth = 14;
                        sprites[3].depth = 15;
                        sprites[4].depth = 15;
                        sprites[5].depth = 15;
                        sprites[6].depth = 15;
                        sprites[7].depth = 13;
                        sprites[8].depth = 13;
                    }
                    #endregion
                    break;
                }
            case 14:
            case 16:
                {
                    #region
                    if (breed.Equals("cow"))
                    {
                        sprites[0].depth = 10;
                        sprites[1].depth = 8;
                        sprites[2].depth = 8;
                        sprites[3].depth = 9;
                        sprites[4].depth = 9;
                        sprites[5].depth = 7;
                        sprites[6].depth = 7;
                    }
                    else if (breed.Equals("chicken"))
                    {
                        sprites[0].depth = 9;
                        sprites[1].depth = 9;
                        sprites[2].depth = 8;
                        sprites[3].depth = 10;
                        sprites[4].depth = 8;
                        sprites[5].depth = 10;
                        sprites[6].depth = 7;
                        sprites[7].depth = 8;
                        sprites[8].depth = 7;
                    }
                    else if (breed.Equals("pig"))
                    {
                        sprites[0].depth = 10;
                        sprites[1].depth = 8;
                        sprites[2].depth = 8;
                        sprites[3].depth = 9;
                        sprites[4].depth = 9;
                        sprites[5].depth = 9;
                        sprites[6].depth = 9;
                        sprites[7].depth = 7;
                        sprites[8].depth = 7;
                    }
                    #endregion
                    break;
                }
            case 17:
                {
                    #region
                    if (breed.Equals("cow"))
                    {
                        sprites[0].depth = 5;
                        sprites[1].depth = 3;
                        sprites[2].depth = 3;
                        sprites[3].depth = 4;
                        sprites[4].depth = 4;
                        sprites[5].depth = 2;
                        sprites[6].depth = 2;
                    }
                    else if (breed.Equals("chicken"))
                    {
                        sprites[0].depth = 4;
                        sprites[1].depth = 4;
                        sprites[2].depth = 3;
                        sprites[3].depth = 5;
                        sprites[4].depth = 3;
                        sprites[5].depth = 5;
                        sprites[6].depth = 2;
                        sprites[7].depth = 3;
                        sprites[8].depth = 2;
                    }
                    else if (breed.Equals("pig"))
                    {
                        sprites[0].depth = 8;
                        sprites[1].depth = 3;
                        sprites[2].depth = 3;
                        sprites[3].depth = 4;
                        sprites[4].depth = 4;
                        sprites[5].depth = 4;
                        sprites[6].depth = 4;
                        sprites[7].depth = 2;
                        sprites[8].depth = 2;
                    }
                    #endregion
                    break;
                }
        }
    }
    private void setStatusSick(int index, string status, UISprite[] sprites, string isShrimp = "")
    {
        if (status.StartsWith("sick"))
        {
            if (isShrimp.Equals("shrimp")) tempColor = new Color32(100, 100, 100, 255);
            else tempColor = new Color32(164, 134, 200, 255);
            createHeal(index, false);
        }
        else
        {
            tempColor = new Color32(255, 255, 255, 255);
            if (sprites[0].color.g != 255)//healing complete
            {
                tempObject = listObjects[index].transform.Find("Thuoc");
                if (tempObject != null) tempObject.gameObject.SetActive(false);
            }
        }
        foreach (UISprite sprite in sprites)
        {
            sprite.color = tempColor;
        }
    }
    private void setStatusSick(int index, string status, UISprite sprite)
    {
        if (status.StartsWith("sick"))
        {
            tempColor = new Color32(111, 111, 111, 255);
            createHeal(index, false);
        }
        else
        {
            tempColor = new Color32(255, 255, 255, 255);
            if (sprite.color.g != 255)//healing complete
            {
                tempObject = listObjects[index].transform.Find("Thuoc");
                if (tempObject != null) tempObject.gameObject.SetActive(false);
            }
        }
        sprite.color = tempColor;
    }

    public void Cell_Click(GameObject index)//active when player tap on one cell
    {
        nindex = Convert.ToInt16(index.gameObject.name.Substring(4));//get index of current cell select

        if (CommonObjectScript.isGuide)
        {
            #region guide
            if (VariableSystem.mission == 1)
            {
                if (nindex != 0) return;
                if (guidefarmScript.indexGuide == 6 || guidefarmScript.indexGuide == 10 || guidefarmScript.indexGuide == 14)
                {
                    //action
                    guidefarmScript.NextGuideText();
                }
                else return;
            }
            else if (VariableSystem.mission == 3)
            {
                if (nindex != 12) return;
                if (guidefarmScript.indexGuide == 2 || guidefarmScript.indexGuide == 6) guidefarmScript.NextGuideText();
                else return;
            }
            else if (VariableSystem.mission == 4)
            {
                if (nindex != 0) return;
                if (guidefarmScript.indexGuide == 10) guidefarmScript.NextGuideText();
                else return;
            }
            else if (VariableSystem.mission == 8)
            {
                if (nindex != 8) return;
                if (guidefarmScript.indexGuide == 2) guidefarmScript.NextGuideText();
                else return;
            }
            else if (VariableSystem.mission == 38)
            {
                if (nindex != 0) return;
                if (guidefarmScript.indexGuide == 2) guidefarmScript.NextGuideText();
                else return;
            }
            else
                //if (VariableSystem.mission == 7 || VariableSystem.mission == 11 ||
                //VariableSystem.mission == 15 || VariableSystem.mission == 18 || VariableSystem.mission == 26) 
                return;
            #endregion
        }

        if (farmCenter.isOpenCells[nindex])//cell is available to farm
        {
            if (farmCenter.listDatas[nindex].stage == 3)
            {
                #region available harvest
                CreateAnimationHarvest(farmCenter.listDatas[nindex].idBreed + "." + farmCenter.listDatas[nindex].nameBreed, farmCenter.listDatas[nindex].Yield);
                CreateAnimationAddValue(farmCenter.listDatas[nindex].idBreed + "." + farmCenter.listDatas[nindex].nameBreed, "+" + farmCenter.listDatas[nindex].Yield);
                CommonObjectScript.arrayMaterials[farmCenter.listDatas[nindex].idBreed - 1] += farmCenter.listDatas[nindex].Yield;
                StorageController.checknewMaterial(farmCenter.listDatas[nindex].idBreed - 1);

                if (farmCenter.breedFarms[farmCenter.listDatas[nindex].idBreed - 1].index >= MissionData.farmDataMission.breedsFarm.Count)
                {
                    print("Vượt quá mảng, giờ tính sao ??? cứ tạm lấy thằng max");
                    MissionData.farmDataMission.breedsFarm[MissionData.farmDataMission.breedsFarm.Count - 1].currentNumber++;
                }
                else
                    MissionData.farmDataMission.breedsFarm[farmCenter.breedFarms[farmCenter.listDatas[nindex].idBreed - 1].index].currentNumber++;
                if (farmCenter.listDatas[nindex].idBreed < 5) MissionData.farmDataMission.harvestField.currentNumber++;
                else MissionData.farmDataMission.harvestCage.currentNumber++;

                farmCenter.listDatas[nindex] = new Breed();
                CommonObjectScript.audioControl.PlaySound("Chat hang");
                UpdateImgCell(nindex);
                if (farmCenter.textHarvest == null) farmCenter.textHarvest = new WarningTextView();
                farmCenter.textHarvest.RemoveWarning(8);
                if (indexPopup != -1)
                {
                    animatorKhungitem.Play("CloseKhungItem");
                    CollapseFrame_Timer();
                    indexPopup = -1;
                }
                #endregion
            }
            else
            {
                #region show popup
                if (indexPopup == nindex)//current selected
                {
                    animatorKhungitem.Play("CloseKhungItem");
                    CollapseFrame_Timer();
                    indexPopup = -1;
                    if (farmCenter.listDatas[nindex].status.StartsWith("sick"))
                    {
                        farmCenter.listDatas[nindex].status = "sick_fix";
                        createHeal(nindex);
                        if (farmCenter.textHarvest == null) farmCenter.textHarvest = new WarningTextView();
                        farmCenter.textHarvest.RemoveWarning(5);
                    }
                }
                else//another selected
                {
                    if (!(indexPopup == -1 || farmCenter.listDatas[indexPopup].idBreed == 0))
                    {
                        CollapseFrame_Timer();
                    }
                    if (farmCenter.listDatas[nindex].status.StartsWith("sick"))
                    {
                        farmCenter.listDatas[nindex].status = "sick_fix";
                        createHeal(nindex);
                        if (farmCenter.textHarvest == null) farmCenter.textHarvest = new WarningTextView();
                        farmCenter.textHarvest.RemoveWarning(5);
                        if (indexPopup != -1)
                            animatorKhungitem.Play("CloseKhungItem");
                        indexPopup = -1;
                    }
                    else
                    {
                        indexPopup = nindex;
                        getDataBreed();
                        KhungItem.GetComponent<Transform>().position = new Vector3(index.transform.position.x, index.transform.position.y, 0);
                        animatorKhungitem.Play("OpenKhungItem");
                        if (farmCenter.listDatas[nindex].idBreed != 0)
                        {
                            ShowFrame_Timer();
                            if (indexPopup < 12) CommonObjectScript.audioControl.PlaySound("Cuoc dat");
                            else if (indexPopup > 17) CommonObjectScript.audioControl.PlaySound("Ca boi");
                            else
                            {
                                if (farmCenter.listDatas[indexPopup].nameBreed.Equals("chicken")) CommonObjectScript.audioControl.PlaySound("Ga keu");
                                else if (farmCenter.listDatas[indexPopup].nameBreed.Equals("pig")) CommonObjectScript.audioControl.PlaySound("Lon keu");
                                else CommonObjectScript.audioControl.PlaySound("Bo keu");
                            }
                        }
                    }

                    if (nindex < 12)
                    {
                        cameraScript.Move(new Vector2(0, -75), 20);
                    }
                    else if (nindex < 18)
                    {
                        cameraScript.Move(new Vector2(130, 150), 20);
                    }
                    else
                    {
                        cameraScript.Move(new Vector2(-130, 20), 20);
                    }
                }
                #endregion
            }
        }
        else
        {
            #region
            CommonObjectScript.audioControl.PlaySound("Click 1");
            if (VariableSystem.mission >= 8)
            {
                if ((nindex < 12 && farmCenter.fieldFarms[0].startNumber != 0) ||
                    (nindex >= 12 && nindex < 18 && farmCenter.fieldFarms[1].startNumber != 0) ||
                    (nindex >= 18 && farmCenter.fieldFarms[2].startNumber != 0))
                {
                    tempObject = panelPopup.transform.Find("PopupConfirmFarm");
                    tempObject.gameObject.SetActive(true);
                    tempObject.GetComponent<PopupImproveField>().setPrice(nindex, 1, this);
                    panelPopup.transform.localPosition = cameraScript.transform.localPosition;
                    CommonObjectScript.isViewPoppup = true;
                }
                else if (PlantingOb.transform.Find("textShow") == null)
                {
                    if (VariableSystem.language == null || VariableSystem.language.Equals("Vietnamese"))
                        CreateAnimationAddValue("textShow", "Không được mở khóa !", false);
                    else
                        CreateAnimationAddValue("textShow", "Unavailabel to unlock !", false);
                }
            }
            else if (PlantingOb.transform.Find("textShow") == null)
            {
                if (VariableSystem.language == null || VariableSystem.language.Equals("Vietnamese"))
                    CreateAnimationAddValue("textShow", "Đang khóa", false);
                else
                    CreateAnimationAddValue("textShow", "Locked !", false);
            }
            //BG_Click();
            if (indexPopup != -1)//current selected
            {
                animatorKhungitem.Play("CloseKhungItem");
                CollapseFrame_Timer();
                indexPopup = -1;
            }
            #endregion
        }
    }
    public void BG_Click()//active when player tap on background to turn off popup
    {
        if (!CommonObjectScript.isGuide)
        {
            cameraScript.Move(Vector2.zero, 10);
            if (indexPopup != -1)
            {
                animatorKhungitem.Play("CloseKhungItem");
                if (farmCenter.listDatas[indexPopup].idBreed != 0)
                {
                    CollapseFrame_Timer();
                }
                indexPopup = -1;
            }
        }
    }
    public void Item_click(GameObject item)//active when player tap one of items in popup
    {
        if (CommonObjectScript.isGuide)
        {
            #region guide
            if ((VariableSystem.mission == 1 && guidefarmScript.indexGuide == 8) ||
                (VariableSystem.mission == 3 && guidefarmScript.indexGuide == 3) ||
                (VariableSystem.mission == 4 && guidefarmScript.indexGuide == 11))
            {
                guidefarmScript.NextGuideText();
            }
            else return;
            #endregion
        }

        if (!(indexPopup == -1 || farmCenter.listDatas[indexPopup].idBreed == 0)) //destroy plant
        {
            int hoantra = (int)(farmCenter.listDatas[indexPopup].price * 0.3f);
            farmCenter.common.AddDollar(hoantra);
            CreateAnimationAddValue("coin", "+" + hoantra);
            CommonObjectScript.audioControl.PlaySound("Click 1");
            farmCenter.listDatas[indexPopup] = new Breed();
            CollapseFrame_Timer();
        }
        else //planting
        {
            if (indexPopup != -1)
            {
                tempValue = Convert.ToInt16(item.GetComponentInChildren<UILabel>().text);//price
                if (CommonObjectScript.dollar >= tempValue)
                {
                    int idBreed = Convert.ToInt16(item.GetComponent<UITexture>().mainTexture.ToString().Substring(0, 1));
                    farmCenter.listDatas[indexPopup] = new Breed(idBreed);
                    farmCenter.listDatas[indexPopup].setData(farmCenter.fieldFarms[(indexPopup < 12 ? 0 : (indexPopup < 18 ? 1 : 2))].currentLevel,
                        farmCenter.elementPlantDataXML.getDataByValue("id", item.GetComponent<UITexture>().mainTexture.ToString().Substring(0, 1)));
                    PlantingOb.GetComponentsInChildren<Transform>()[1].localPosition = listObjects[indexPopup].GetComponent<Transform>().localPosition;
                    PlantingOb.GetComponentsInChildren<UILabel>()[0].text = "-" + tempValue.ToString();
                    PlantingOb.GetComponent<Animator>().Play("StartPlanting");
                    farmCenter.common.AddDollar(-tempValue);
                    if (indexPopup < 12) CommonObjectScript.audioControl.PlaySound("Cuoc dat");
                    else if (indexPopup > 17) CommonObjectScript.audioControl.PlaySound("Ca boi");
                    else
                    {
                        if (farmCenter.listDatas[indexPopup].nameBreed.Equals("chicken")) CommonObjectScript.audioControl.PlaySound("Ga keu");
                        else if (farmCenter.listDatas[indexPopup].nameBreed.Equals("pig")) CommonObjectScript.audioControl.PlaySound("Lon keu");
                        else CommonObjectScript.audioControl.PlaySound("Bo keu");
                    }
                }
                else
                {
                    farmCenter.common.ChangeDolar(CommonObjectScript.dollar - tempValue);
                }
            }
        }
        animatorKhungitem.Play("CloseKhungItem");
        UpdateImgCell(indexPopup);
        indexPopup = -1;

        //cameraScript.Move(Vector2.zero, 20);
    }
    public void Faster_Click()//active when player tap on button grow up faster
    {
        if (CommonObjectScript.isGuide)
        {
            #region guide
            if ((VariableSystem.mission == 1 && guidefarmScript.indexGuide == 12) ||
                (VariableSystem.mission == 3 && guidefarmScript.indexGuide == 7))
            {
                guidefarmScript.NextGuideText();
            }
            else return;
            #endregion
        }
        animatorKhungitem.Play("CloseKhungItem");
        CollapseFrame_Timer();
        if (VariableSystem.diamond >= 1)
        {
            farmCenter.common.AddDiamond(-1);
            CreateAnimationDiamond();
            StartCoroutine(farmCenter.GrowUp(indexPopup));
        }
        else
        {
            DialogInapp.ShowInapp();
        }
        indexPopup = -1;
    }

    private void getDataBreed()//read xml file to get position each items in popup
    {
        int count = 1;
        if (farmCenter.listDatas[indexPopup].idBreed != 0)
        {
            count = 1;
        }
        else
        {
            if (nindex < 12)
            {
                count = farmCenter.countElementField;
            }
            else if (nindex < 18) count = farmCenter.countElementCage1;
            else count = farmCenter.countElementCage2;
        }
        frameItemDataXML = new ReadXML("Farm/XMLFile/FrameItem", (4 - count));

        #region change position's items in popup
        //popup
        transformOfListItem[1].gameObject.GetComponent<UITexture>().width =
            Convert.ToInt16(frameItemDataXML.getDataByIndex(5).Attributes.Item(0).Value);
        transformOfListItem[1].gameObject.GetComponent<UITexture>().height =
            Convert.ToInt16(frameItemDataXML.getDataByIndex(5).Attributes.Item(1).Value);
        transformOfListItem[1].gameObject.GetComponent<BoxCollider>().size = new Vector3(
            Convert.ToInt16(frameItemDataXML.getDataByIndex(5).Attributes.Item(2).Value), Convert.ToInt16(frameItemDataXML.getDataByIndex(5).Attributes.Item(2).Value), 1);

        for (int i = 0; i < 4; i++)
        {
            transformOfListItem[2 + i * 4].gameObject.GetComponent<Transform>().localPosition = new Vector3(
            Convert.ToInt16(frameItemDataXML.getDataByIndex(i).Attributes.Item(0).Value),
            Convert.ToInt16(frameItemDataXML.getDataByIndex(i).Attributes.Item(1).Value), 0);
            transformOfListItem[2 + i * 4].gameObject.GetComponent<Transform>().localScale = new Vector3(
                Convert.ToInt16(frameItemDataXML.getDataByIndex(i).Attributes.Item(2).Value), 1, 1);
            if (i == 0)
            {
                transformOfListItem[3].gameObject.GetComponent<UITexture>().enabled = true;
                transformOfListItem[4].gameObject.GetComponent<UILabel>().enabled = true;
                transformOfListItem[5].gameObject.GetComponent<UITexture>().enabled = true;
                if (!(indexPopup == -1 || farmCenter.listDatas[indexPopup].idBreed == 0))//xeng
                {
                    transformOfListItem[2].gameObject.GetComponent<Transform>().localScale = new Vector3(0.75f, 0.75f, 1);
                    transformOfListItem[2].gameObject.GetComponent<UITexture>().mainTexture = Resources.Load("Factory/Button/Images/Material/xeng") as Texture;
                    transformOfListItem[3].gameObject.GetComponent<UITexture>().enabled = false;
                    transformOfListItem[4].gameObject.GetComponent<UILabel>().enabled = false;
                    transformOfListItem[5].gameObject.GetComponent<UITexture>().enabled = false;
                }
            }
        }
        #endregion
        #region change information
        int idTemp = 0;
        for (int i = 0; i < count; i++)
        {
            if (i == 0 && indexPopup != -1 && farmCenter.listDatas[indexPopup].idBreed != 0)//xeng
                continue;

            if (nindex < 12) idTemp = farmCenter.idBreedsSorted[i];
            else if (nindex < 18) idTemp = farmCenter.idBreedsSorted[i + farmCenter.countElementField];
            else idTemp = farmCenter.idBreedsSorted[i + farmCenter.countElementField + farmCenter.countElementCage1];

            //img
            transformOfListItem[2 + i * 4].gameObject.GetComponent<UITexture>().mainTexture = Resources.Load("Farm/Icon/" + idTemp + "." + Breed.getName(idTemp)) as Texture;

            //price
            int discount = 0;
            if (MissionPowerUp.PriceDrop) discount += 25;
            if (DialogShop.BoughtItem[5]) discount += 10;
            discount = (int)(Convert.ToInt16(farmCenter.elementPlantDataXML.getDataByValue("id", idTemp.ToString()).Attributes["price"].Value) * (1 - discount / 100f));
            transformOfListItem[4 + i * 4].gameObject.GetComponent<UILabel>().text = discount.ToString();
        }
        #endregion
    }
    private void createHeal(int index, bool animator = true)
    {
        tempObject = listObjects[index].transform.Find("Thuoc");
        if (tempObject == null)
        {
            HealingObject = (GameObject)Instantiate(HealingPrefabs);
            HealingObject.name = "Thuoc";
            HealingObject.transform.parent = listObjects[index].transform;
            HealingObject.transform.localScale = Vector3.one * 0.5f;
            HealingObject.GetComponent<Animator>().enabled = animator;
            if (!animator) HealingObject.transform.localPosition = new Vector3(0, -40);
            else HealingObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            tempObject.gameObject.SetActive(true);
            tempObject.GetComponent<Animator>().enabled = animator;
            if (!animator) tempObject.transform.localPosition = new Vector3(0, -40);
            else tempObject.transform.localPosition = Vector3.zero;
        }
    }
    private void CreateAnimationHarvest(string name, int number, int index = -1)// create animation when user harvest one product
    {
        for (int i = 0; i < number; i++)
        {
            HarvestPlant = (GameObject)Instantiate(HarvestPlantPrefabs,
                new Vector3(0, 0, 0),
                Quaternion.identity);
            HarvestPlant.name = name;
            if (name.Equals("coin"))
            {
                HarvestPlant.GetComponent<HarvestPlantScript>().setValue("Common/vang", new Vector3(240, 340, 0), 75);
            }
            else
                HarvestPlant.GetComponent<HarvestPlantScript>().setValue("Factory/Button/Images/Material/" + name, new Vector3(-570, -300, 0), 100);
            HarvestPlant.transform.parent = PlantingOb.transform;
            if (index != -1)
                HarvestPlant.GetComponent<Transform>().localPosition = new Vector3(listObjects[index].GetComponent<Transform>().localPosition.x + UnityEngine.Random.Range(-40, 40),
                    listObjects[index].GetComponent<Transform>().localPosition.y + UnityEngine.Random.Range(-20, 20), -200);
            else
                HarvestPlant.GetComponent<Transform>().localPosition = new Vector3(listObjects[nindex].GetComponent<Transform>().localPosition.x + UnityEngine.Random.Range(-40, 40),
                    listObjects[nindex].GetComponent<Transform>().localPosition.y + UnityEngine.Random.Range(-20, 20), -200);
            HarvestPlant.GetComponent<Transform>().localScale = Vector3.one;
        }
    }
    public void CreateAnimationAddValue(string name, string number, bool isShowImg = true, int index = -1)// create animation when add money or item
    {
        GameObject valueAdd = (GameObject)Instantiate(valuePrefabs);
        valueAdd.name = name;
        valueAdd.transform.parent = PlantingOb.transform;
        if (index != -1)
            valueAdd.GetComponent<Transform>().localPosition = listObjects[index].GetComponent<Transform>().localPosition;
        else
            valueAdd.GetComponent<Transform>().localPosition = listObjects[nindex].GetComponent<Transform>().localPosition;
        valueAdd.GetComponent<Transform>().localScale = Vector3.one;
        valueAdd.GetComponent<AddValueScript>().setValue(number, isShowImg);
        if (name.Equals("coin"))
            CreateAnimationHarvest(name, 5);
    }
    public void CreateAnimationDiamond(int Value = -1)// create animation when user harvest one product
    {
        GameObject diamondPrefabs = (GameObject)Resources.Load("Farm/Diamond_Effect");
        GameObject diamond = (GameObject)Instantiate(diamondPrefabs);
        diamond.transform.parent = PlantingOb.transform;
        diamond.transform.localPosition = listObjects[nindex].transform.localPosition;
        diamond.transform.localScale = Vector3.one;
        diamond.GetComponent<DiamondEffectScript>().setValueDiamond(Value);
    }

    private void ShowFrame_Timer()//show timer bar of plant/animal when user are selecting one cell
    {
        frame_timer.SetActive(true);
        frame_timer.GetComponent<Transform>().localPosition = new Vector3(listObjects[nindex].GetComponent<Transform>().localPosition.x,
            listObjects[nindex].GetComponent<Transform>().localPosition.y, -200);
    }
    private void CollapseFrame_Timer()
    {
        frame_timer.SetActive(false);
    }
}