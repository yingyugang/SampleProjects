using Assets.Scripts.Common;
using Assets.Scripts.Farm;
using UnityEngine;

public class PopupImproveField : MonoBehaviour
{
    int price = 1;
    int index;
    UILabel[] labelsinPopup;
    PlantControlScript plantScript;
    CommonObjectScript common;
    // Use this for initialization
    void OnEnable()
    {
        #region show Text
        labelsinPopup = GetComponentsInChildren<UILabel>();
        labelsinPopup[2].text = price.ToString();
        if (VariableSystem.language == null || VariableSystem.language.Equals("Vietnamese"))
        {
            labelsinPopup[0].text = "MỞ RỘNG";
            labelsinPopup[1].text = "Bạn muốn cải tạo không?";
            labelsinPopup[3].text = "Đồng ý";
            labelsinPopup[4].text = "Hủy bỏ";
        }
        else
        {
            labelsinPopup[0].text = "IMPROVE";
            labelsinPopup[1].text = "You want to improve this?";
            labelsinPopup[3].text = "Ok";
            labelsinPopup[4].text = "Cancel";
        }
        #endregion
    }
    public void setPrice(int index, int price, PlantControlScript plant)
    {
        this.index = index;
        this.price = price;
        plantScript = plant;
        common = plantScript.farmCenter.common;
    }
    public void DeActive()
    {
        CommonObjectScript.isViewPoppup = false;
        this.gameObject.SetActive(false);
    }
    public void OK_Click()
    {
        Cancel_Click();
        if (VariableSystem.diamond >= price)
        {
            common.AddDiamond(-price);
            plantScript.farmCenter.isOpenCells[index] = true;

            plantScript.farmCenter.idNeedUpdate = index;
            plantScript.CreateAnimationDiamond(-price);

            int temp = index < 12 ? 1 : (index < 18 ? 2 : 3);
            for (int i = 0; i < MissionData.farmDataMission.fieldFarms.Count; i++)
            {
                if (MissionData.farmDataMission.fieldFarms[i].idField == temp)
                {
                    MissionData.farmDataMission.fieldFarms[i].currentNumber++;
                    break;
                }
            }
            if (VariableSystem.mission == 8 && CommonObjectScript.isGuide)
            {
                plantScript.guidefarmScript.NextGuideText();
                GetComponent<Animator>().Play("ClosePopup");
            }
        }
        else
        {
            DialogInapp.ShowInapp();
        }
    }
    public void Cancel_Click()
    {
        if (!CommonObjectScript.isEndGame)
            CommonObjectScript.audioControl.PlaySound("Click 1");
        if (VariableSystem.mission == 8 && CommonObjectScript.isGuide) return;
        GetComponent<Animator>().Play("ClosePopup");
    }
}
