using UnityEngine;
using System.Collections;
using Assets.Scripts.Common;
using Assets.Scripts.Farm;
using System.Collections.Generic;

public class TimerController : MonoBehaviour
{

    // Use this for initialization
    public float time;
    private float timeLeft;
    private float timeCount;

    public UILabel labelTimeLeft;
    public UITexture valTime;
    public GameObject Diamond_EfectPrefabs;
    private GameObject Diamond_EfectClone;

    private bool isEfect;
    private float timeDelay;
    private float countTimeDelay;

    private string textTimeProductHour;
    private string textTimeProductDay;
    private float timesLeft;
    private int dayLeft;
    private int hourLeft;

    public int minRation, maxRation;
    public GameObject MultimediaResultPrefabs, maketResultPrefabs, technogyResultPrefabs;
    private GameObject MultimediaResultClone, maketResultClone, technogyResultClone;
    private AudioControl audioControl;
    private int levelNext;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        timeCount = 0;
        timeDelay = 1.1f;
        countTimeDelay = 0.0f;
        isEfect = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Application.loadedLevelName.Equals("Mission"))
        {
            Destroy(this.gameObject);
        }
        SetVisible();
        if (timeCount < time)
        {
            if (!CommonObjectScript.isGuide)
            {
                timeLeft = time - timeCount;
                labelTimeLeft.text = ChangeTimeToText(timeLeft);
                timeCount += Time.deltaTime;
                valTime.GetComponent<UITexture>().fillAmount = timeCount / time;
            }
            else
            {
                labelTimeLeft.text = ChangeTimeToText(time);
            }
        }
        else
        {

            Destroy(gameObject);
            CreatResult();

        }
        if (isEfect)
        {
            if (countTimeDelay < timeDelay)
            {
                countTimeDelay += Time.deltaTime;
            }
            else
            {
                timeCount = time;
            }
        }
    }

    string ChangeTimeToText(float timeLeftClone)
    {
        dayLeft = (int)(timeLeftClone) / 24;
        hourLeft = (int)(timeLeftClone) % 24;
        if (hourLeft <= 0)
        {
            textTimeProductHour = "";
        }
        else if (hourLeft == 1)
        {
            textTimeProductHour = "1 " + FactoryScenesController.languageHungBV["HOUR"];
        }
        else if (hourLeft > 1)
        {
            textTimeProductHour = hourLeft.ToString() + " " + FactoryScenesController.languageHungBV["HOURS"];
        }
        if (dayLeft <= 0)
        {
            textTimeProductDay = "";
        }
        else if (dayLeft == 1)
        {
            textTimeProductDay = "1 " + FactoryScenesController.languageHungBV["DAY"] + " ";
        }
        else if (dayLeft > 1)
        {
            textTimeProductDay = dayLeft.ToString() + " " + FactoryScenesController.languageHungBV["DAYS"] + " ";
        }
        return (textTimeProductDay + textTimeProductHour);
    }

    public void UseDiamond()
    {
        //audioControl.PlaySoundInstance("Click 1", false, false, 0.5f);

        if (Application.loadedLevelName.Equals("Town"))
        {
            audioControl.PlaySound("Click 1");
            if (!isEfect)
            {
                if (VariableSystem.diamond >= 1)
                {
                    Diamond_EfectClone = (GameObject)Instantiate(Diamond_EfectPrefabs, transform.position, transform.rotation);
                    Diamond_EfectClone.transform.parent = transform;
                    Diamond_EfectClone.transform.localPosition = new Vector3(50, -50, 0);
                    Diamond_EfectClone.transform.localScale = new Vector3(-1, 1, 1);
                    isEfect = true;
                    AddCommonObject(0, -1);
                }
                else
                {
                    DialogInapp.ShowInapp();
                }
            }
        }
    }

    void CreatResult()
    {
        if (gameObject.name.Equals("TimerMutilMedia"))
        {
            MultimediaResultClone = (GameObject)Instantiate(MultimediaResultPrefabs, new Vector3(0, 0, 0), transform.rotation);
            MultimediaResultClone.GetComponent<MultimediaResultController>().minRation = minRation;
            MultimediaResultClone.GetComponent<MultimediaResultController>().maxRation = maxRation;
            TownScenesController.queuePopup.Enqueue(MultimediaResultClone);

        }
        else if (gameObject.name.Equals("TimerMaketResearch"))
        {
            maketResultClone = (GameObject)Instantiate(maketResultPrefabs, new Vector3(0, 0, 0), transform.rotation);
            maketResultClone.GetComponent<MaketResearchResultController>().precisionMin = minRation;
            maketResultClone.GetComponent<MaketResearchResultController>().precisionMax = maxRation;
            maketResultClone.name = maketResultPrefabs.name;
            TownScenesController.queuePopup.Enqueue(maketResultClone);
        }
        else if (gameObject.name.Equals("TimerTechnogy"))
        {

            if (maxRation <= 3)
            {
                foreach (FieldFarm fieldFarm in MissionData.farmDataMission.fieldFarms)
                {
                    if (fieldFarm.idField == maxRation)
                    {
                        MissionData.farmDataMission.fieldFarms[fieldFarm.idField - 1].currentLevel += 1;
                        levelNext = MissionData.farmDataMission.fieldFarms[fieldFarm.idField - 1].currentLevel;
                        break;
                    }
                }
            }
            else
            {
                for (int count = 0; count < MissionData.factoryDataMission.machinedatas.Count; count++)
                {
                    if ((maxRation - 3) == (MissionData.factoryDataMission.machinedatas[count].iDMachine))
                    {
                        if (FactoryScenesController.ListQueue == null)
                        {
                            MissionData.factoryDataMission.machinedatas[count].currentLevel += 1;
                            levelNext = MissionData.factoryDataMission.machinedatas[count].currentLevel;
                        }
                        // print(MissionData.factoryDataMission.machinedatas[count].iDMachine + "-" + MissionData.factoryDataMission.machinedatas[count].currentLevel);                      
                        else
                        {
                            UpdateLevelForMachine(MissionData.factoryDataMission.machinedatas[count].index);
                        }
                        break;
                    }
                }
            }

            technogyResultClone = (GameObject)Instantiate(technogyResultPrefabs, new Vector3(0, 0, 0), transform.rotation);
            technogyResultClone.GetComponent<TechnogyResultController>().IDObject = maxRation;
            technogyResultClone.GetComponent<TechnogyResultController>().levelObject = levelNext;
            technogyResultClone.name = technogyResultPrefabs.name;
            TownScenesController.queuePopup.Enqueue(technogyResultClone);
        }
        if (!Application.loadedLevelName.Equals("Town"))
            GameObject.Find("CommonObject").GetComponent<CommonObjectScript>().WarningVisible(CommonObjectScript.Button.City);
    }
    void AddCommonObject(int dollar, int diamond)
    {
        GameObject commonObject = GameObject.Find("CommonObject");
        if (commonObject != null)
        {
            commonObject.GetComponent<CommonObjectScript>().AddDollar(dollar);
            commonObject.GetComponent<CommonObjectScript>().AddDiamond(diamond);
        }
    }
    void SetVisible()
    {
        if (!Application.isLoadingLevel)
        {
            if (!Application.loadedLevelName.Equals("Town"))
            {
                this.gameObject.GetComponent<UIPanel>().enabled = false;

            }
            else
            {
                this.gameObject.GetComponent<UIPanel>().enabled = true;
            }
        }
    }
    void UpdateLevelForMachine(int index)
    {
        if (FactoryScenesController.ListQueue != null)
        {
            print("vao day -------------");
            MissionData.factoryDataMission.machinedatas[index].currentLevel += 1;
            levelNext = MissionData.factoryDataMission.machinedatas[index].currentLevel;
            FactoryScenesController.ListQueue[index] = MissionData.factoryDataMission.machinedatas[index].currentLevel;
            FactoryScenesController.isChangeLevel = true;
        }
    }
}
