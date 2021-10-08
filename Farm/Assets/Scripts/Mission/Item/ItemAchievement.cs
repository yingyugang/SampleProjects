using UnityEngine;
using System.Collections;

public class ItemAchievement : MonoBehaviour
{
    public Transform DiamondEffect;
    UILabel lbTitle, lbDetail, lbDiamond, lbProgress;
    Transform incomplete;
    UIProgressBar progressBar;
    public bool finish;

    //Dung 3 bien nay de khi nguoi dung kich vao get item, thi nhiem vu dang sau(neu co) se duoc hien thi
    Achievement[] achievementGroup;//Mang achievement trong 1 group
    int currentLevelAchievement;
    int currentValue;
    int groupAchievement;
    int targetValue;

    void Awake()
    {
        finish = false;
        lbTitle = transform.Find("ItemAchievement").Find("Title").GetComponent<UILabel>();
        lbDetail = transform.Find("ItemAchievement").Find("Detail").GetComponent<UILabel>();
        lbDiamond = transform.Find("ItemAchievement").Find("lbDiamond").GetComponent<UILabel>();
        lbProgress = transform.Find("ItemAchievement").Find("ProgressBar").Find("lbProgress").GetComponent<UILabel>();
        progressBar = transform.Find("ItemAchievement").Find("ProgressBar").GetComponent<UIProgressBar>();
        incomplete = transform.Find("ItemAchievement").Find("Incomplete");
    }

    public void SetData(int group, Achievement[] achievementGroup, int currentLevelAchievement, int currentValue)
    {
        this.groupAchievement = group;
        this.achievementGroup = achievementGroup;
        this.currentLevelAchievement = currentLevelAchievement;
        this.currentValue = currentValue;
        //Neu nhiem vu cua group da hoan thanh => currentLevelAchievement > length cua group, lay noi dung cua achievement cuoi cung
        int idGroupValid = currentLevelAchievement - 1;
        if (currentLevelAchievement > achievementGroup.Length)
        {
            idGroupValid = achievementGroup.Length - 1;
        }
        Achievement achievement = achievementGroup[idGroupValid];
        //Debug.Log("achievement.Title " + achievement.Title);
        lbTitle.text = achievement.Title;
        lbDetail.text = achievement.Detail;
        if (VariableSystem.language != null && VariableSystem.language.Equals("Vietnamese"))
        {
            lbDetail.text = achievement.Detail_Vi;
        }
        lbDiamond.text = "" + achievement.Reward;
        lbProgress.text = "" + currentValue + "/" + achievement.Target;
        float progress = (float)currentValue / (float)achievement.Target;
        if (progress > 1)
        {
            progress = 1;
        }
        progressBar.value = progress;
        if (progress >= 1)
        {
            incomplete.gameObject.SetActive(false);
            finish = true;
        }
        else
        {
            finish = false;
            incomplete.gameObject.SetActive(true);
        }
        if (currentLevelAchievement > achievementGroup.Length)
        {
            finish = false;
            transform.Find("ItemAchievement").Find("ButtonGet").gameObject.SetActive(false);
        }
        this.targetValue = achievement.Target;
    }

    public void UpdateData(int currentLevelAchievement, int currentValue)
    {
        int idGroupValid = currentLevelAchievement - 1;
        if (currentLevelAchievement > achievementGroup.Length)
        {
            idGroupValid = achievementGroup.Length - 1;
        }
        Achievement achievement = achievementGroup[idGroupValid];
        this.currentValue = currentValue;
        this.targetValue = achievement.Target;
        lbProgress.text = "" + currentValue + "/" + targetValue;
        float progress = (float)currentValue / (float)targetValue;
        if (progress > 1)
        {
            progress = 1;
        }
        progressBar.value = progress;
        if (progress >= 1)
        {
            incomplete.gameObject.SetActive(false);
            finish = true;
        }
        else
        {
            incomplete.gameObject.SetActive(true);
            finish = false;
        }

        //Update ngon ngu
        if (currentLevelAchievement > achievementGroup.Length)
        {
            finish = false;
            transform.Find("ItemAchievement").Find("ButtonGet").gameObject.SetActive(false);
        }
        lbDetail.text = achievementGroup[idGroupValid].Detail;
        if (VariableSystem.language != null && VariableSystem.language.Equals("Vietnamese"))
        {
            lbDetail.text = achievementGroup[idGroupValid].Detail_Vi;
        }
        transform.Find("ItemAchievement").Find("Incomplete").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["Incomplete"];
        transform.Find("ItemAchievement").Find("ButtonGet").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["Get"];
    }
    public void ButtonGet()
    {
        //Xet du lieu hoan thanh mission vao day
        //Debug.Log("Button get click " + currentLevelAchievement + " achievementGroup.Length " + achievementGroup.Length);
        if (achievementGroup.Length >= currentLevelAchievement)
        {
            finish = false;
            Transform diamondEffect = Instantiate(DiamondEffect) as Transform;
            diamondEffect.transform.position = transform.Find("ItemAchievement").Find("ButtonGet").position;
            diamondEffect.transform.parent = transform;
            diamondEffect.transform.localScale = new Vector3(1, 1, 1);
            //currentLevelAchievement - 1 Lay phan thuong cua nhiem vu truoc
            diamondEffect.GetComponent<DiamondEffect>().SetData(achievementGroup[currentLevelAchievement - 1].Reward, true);
            //Debug.Log("------------- " + diamondEffect.name);
            //neu nhiem vu cuoi cung cua group thi se an nut get di
            if (currentLevelAchievement == achievementGroup.Length)
            {
                transform.Find("ItemAchievement").Find("ButtonGet").gameObject.SetActive(false);
            }
            //currentLevelAchievement se duoc +1 trong ham sau
            SetData(groupAchievement, achievementGroup, currentLevelAchievement + 1, currentValue);
            DataCache.AddAchievementCache(groupAchievement, 0, 1);
            //Xet lai de group co the hien thi thong bao
            DataCache.dataAchievementCache[groupAchievement - 1].Notify = 0;

            transform.parent.parent.parent.parent.GetComponent<DialogAchievement>().CountAchievementFinish();
        }
    }

    bool isVisible;
    bool objVisible;
    public void Update()
    {
        if (transform.position.y < 1 && transform.position.y > -1)
        {
            isVisible = true;
        }
        else
        {
            isVisible = false;
        }
        if (isVisible)
        {
            if (!objVisible)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                    objVisible = true;
                }
                //Debug.Log("Hien len");
            }
        }
        else
        {
            if (objVisible)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    objVisible = false;
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                //Debug.Log("An di");
            }
        }
    }
}

public class Achievement
{
    public int GroupId;
    public int LevelId;
    public string Title;
    public string Detail;
    public string Detail_Vi;
    public int Reward;
    public int Target;

    public Achievement(int groupId, int levelId, string title, string detail, string detail_vi, int reward, int target)
    {
        this.GroupId = groupId;
        this.LevelId = levelId;
        this.Title = title;
        this.Detail = detail;
        this.Detail_Vi = detail_vi;
        this.Target = target;
        this.Reward = reward;

    }
}