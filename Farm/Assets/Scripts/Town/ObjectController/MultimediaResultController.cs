using UnityEngine;
using System.Collections;

public class MultimediaResultController : MonoBehaviour
{

    // Use this for initialization
    public UILabel[] label;
    public int minRation;
    public int maxRation;
    private int result;
    string resultString;
    private Animator animator;
    AudioControl audioControl;
    void OnEnable()
    {
        CommonObjectScript.isViewPoppup = true;

       
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        animator = GetComponent<Animator>();
        label[0].text = TownScenesController.languageTowns["CONGRATULATIONS"];
        label[1].text = SetResult();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || CommonObjectScript.isEndGame || CommonObjectScript.isViewComplete)
        {
            if (!TownScenesController.isHelp)
                Close_Click();
        }
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            if (!TownScenesController.isHelp)
                Close_Click();
        }
    }
    public void Close_Click()
    {
        audioControl.PlaySound("Click 1");
        animator.Play("InVisible");
    }
    public void Destroy()
    {
        //this.gameObject.SetActive(false);
        TownScenesController.townsBusy[2] = false;
        CommonObjectScript.isViewPoppup = false;
        Destroy(gameObject);
        CreatTownScenesController.isDenyContinue = false;
    }

    int Result()
    {
        int temp = Random.Range(0, 1000) % 99;
        if (temp <= 30) result = maxRation;
        else
            result = Random.Range(minRation * 1000, (maxRation - 1) * 1000) / 1000;
        ShopCenterScript.getBonueResearch(result);
        return result;
    }

    string SetResult()
    {

        resultString = TownScenesController.languageTowns["MutilmediaResultOne"] + Result() + TownScenesController.languageTowns["MutilmediaResultTow"];
        return resultString;
    }
}
