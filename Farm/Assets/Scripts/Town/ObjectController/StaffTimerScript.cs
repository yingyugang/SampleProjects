using UnityEngine;
using System.Collections;

public class StaffTimerScript : MonoBehaviour
{
    public ShopCenterScript shopcenter;
    public GameObject frameTimer;
    public UITexture valueImg;
    public UILabel valueText;
    public GameObject popupStaff;
    private bool showTimer;

    public GameObject Diamond_EfectPrefabs;
    private GameObject Diamond_EfectClone;
    // Use this for initialization
    void Start()
    {
        GoogleAnalytics.instance.LogScreen("Town Screen");
        shopcenter = GameObject.FindGameObjectWithTag("ShopObject").GetComponent<ShopCenterScript>();
        if (shopcenter.indexTraining != -1)
        {
            frameTimer.SetActive(true);
            showTimer = true;
        }
        else
        {
            frameTimer.SetActive(false);
            showTimer = false;
        }
        if (shopcenter == null) print("NULL SHOPCENTER222");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (showTimer)
        {
            valueImg.fillAmount = shopcenter.countTimeTraining / 48;
            valueText.text = ((int)shopcenter.countTimeTraining).ToString() + " Hour" + (((int)(shopcenter.countTimeTraining) == 1) ? "" : "s");
            if (shopcenter.countTimeTraining <= 0)
            {
                frameTimer.SetActive(false);
                showTimer = false;
                if (popupStaff.activeSelf) popupStaff.GetComponent<StaffHouseControllScript>().staff_Click(null, -1);
            }
        }
    }

    public void BtnFaster_Click()
    {
        if (VariableSystem.diamond >= 1)
        {
            Diamond_EfectClone = (GameObject)Instantiate(Diamond_EfectPrefabs, transform.position, transform.rotation);
            Diamond_EfectClone.transform.parent = transform;
            Diamond_EfectClone.transform.localPosition = new Vector3(50, -185, 0);
            Diamond_EfectClone.transform.localScale = Vector3.one;
            shopcenter.countTimeTraining = 0;
            Diamond_EfectClone.GetComponent<DiamondEffectScript>().setValueDiamond(-1);
            shopcenter.Upgrade();
            frameTimer.SetActive(false);
            showTimer = false;
        }
        else DialogInapp.ShowInapp();
    }
    public void ActiveTimer()
    {
        frameTimer.SetActive(true);
        showTimer = true;
    }
}
