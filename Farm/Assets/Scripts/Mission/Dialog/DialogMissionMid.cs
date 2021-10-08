using UnityEngine;
using System.Collections;
using Assets.Scripts.Common;
using System.Collections.Generic;

public class DialogMissionMid : DialogAbs
{
    Transform star1, star2, star3;
    public Transform ItemRequireMission;
    Transform tableRequire;
    // Use this for initialization
    void Awake()
    {
        star1 = transform.Find("Star1");
        star2 = transform.Find("Star2");
        star3 = transform.Find("Star3");
        tableRequire = transform.Find("TableRequire");
    }

    public void setData(int level)
    {
        //Target Star
        transform.Find("LbStar2").GetComponent<UILabel>().text = DString.ConvertString(MissionData.starMission.twoStar);
        transform.Find("LbStar3").GetComponent<UILabel>().text = DString.ConvertString(MissionData.starMission.threeStar);
        //---------------------------------------FARM----------------------------------------------
        #region Farm Require
        //FARM FIELD
        for (int i = 0; i < MissionData.farmDataMission.fieldFarms.Count; i++)
        {
            //yeu cau so luong
            if (MissionData.farmDataMission.fieldFarms[i].targetNumber > 0)
            {
                MissionData.farmDataMission.fieldFarms[i].typeShow = 1;
                Transform tf = Instantiate(ItemRequireMission) as Transform;
                tableRequire.GetComponent<UIGrid>().AddChild(tf);
                tf.GetComponent<ItemRequireMission>().SetDataField(MissionData.farmDataMission.fieldFarms[i]);
            }
            //yeu cau level
            if (MissionData.farmDataMission.fieldFarms[i].targetLevel > 1)
            {
                MissionData.farmDataMission.fieldFarms[i].typeShow = 0;
                Transform tf = Instantiate(ItemRequireMission) as Transform;
                tableRequire.GetComponent<UIGrid>().AddChild(tf);
                tf.GetComponent<ItemRequireMission>().SetDataField(MissionData.farmDataMission.fieldFarms[i]);
            }
        }
        //FARM - thu hoach  rieng(cay trong + vat nuoi)
        for (int i = 0; i < MissionData.farmDataMission.breedsFarm.Count; i++)
        {
            //yeu cau so luong
            if (MissionData.farmDataMission.breedsFarm[i].targetNumber > 0)
            {
                MissionData.farmDataMission.breedsFarm[i].typeShow = 1;
                Transform tf = Instantiate(ItemRequireMission) as Transform;
                tableRequire.GetComponent<UIGrid>().AddChild(tf);
                tf.GetComponent<ItemRequireMission>().SetDataBreed(MissionData.farmDataMission.breedsFarm[i]);
            }
        }
        //FARM - thu hoach Chung ruong
        if (MissionData.farmDataMission.harvestField.targetNumber > 0)
        {
            MissionData.farmDataMission.harvestField.typeShow = 1;
            Transform tf = Instantiate(ItemRequireMission) as Transform;
            tableRequire.GetComponent<UIGrid>().AddChild(tf);
            tf.GetComponent<ItemRequireMission>().SetDataBreedComon(MissionData.farmDataMission.harvestField, 0);
        }
        //FARM - thu hoach  Chung chuong - ao
        if (MissionData.farmDataMission.harvestCage.targetNumber > 0)
        {
            MissionData.farmDataMission.harvestCage.typeShow = 1;
            Transform tf = Instantiate(ItemRequireMission) as Transform;
            tableRequire.GetComponent<UIGrid>().AddChild(tf);
            tf.GetComponent<ItemRequireMission>().SetDataBreedComon(MissionData.farmDataMission.harvestCage, 1);
        }
        #endregion
        //---------------------------------------FACTORY-------------------------------------------
        #region Factory Require
        //FACTORY - MO RONG VI TRI DAT MAY
        if (MissionData.factoryDataMission.positionUnlock.targetPositionUnlock > 0)
        {
            MissionData.factoryDataMission.positionUnlock.typeShow = 1;
            Transform tf = Instantiate(ItemRequireMission) as Transform;
            tableRequire.GetComponent<UIGrid>().AddChild(tf);
            tf.GetComponent<ItemRequireMission>().SetDataMarchineUnlockPosition(MissionData.factoryDataMission.positionUnlock);
        }
        //FACTORY - Mua may + nang cap may
        for (int i = 0; i < MissionData.factoryDataMission.machinedatas.Count; i++)
        {
            //Nang cap may
            if (MissionData.factoryDataMission.machinedatas[i].targetLevel > 1)
            {
                MissionData.factoryDataMission.machinedatas[i].typeShow = 0;
                Transform tf = Instantiate(ItemRequireMission) as Transform;
                tableRequire.GetComponent<UIGrid>().AddChild(tf);
                tf.GetComponent<ItemRequireMission>().SetDataMarchine(MissionData.factoryDataMission.machinedatas[i]);
            }
            //So luong may yeu cau
            if (MissionData.factoryDataMission.machinedatas[i].targetNumber > 0)
            {
                MissionData.factoryDataMission.machinedatas[i].typeShow = 1;
                Transform tf = Instantiate(ItemRequireMission) as Transform;
                tableRequire.GetComponent<UIGrid>().AddChild(tf);
                tf.GetComponent<ItemRequireMission>().SetDataMarchine(MissionData.factoryDataMission.machinedatas[i]);
            }
        }
        //FACTORY - SAN XUAT RIENG
        for (int i = 0; i < MissionData.shopDataMission.listProducts.Count; i++)
        {
            if (MissionData.shopDataMission.listProducts[i].targetProduction > 0)
            {
                MissionData.shopDataMission.listProducts[i].typeShow = 0;
                Transform tf = Instantiate(ItemRequireMission) as Transform;
                tableRequire.GetComponent<UIGrid>().AddChild(tf);
                tf.GetComponent<ItemRequireMission>().SetDataProduct(MissionData.shopDataMission.listProducts[i]);
            }
        }
        // FACTORY -  SAN XUAT CHUNG
        if (MissionData.factoryDataMission.targetProducts > 0)
        {
            MissionData.factoryDataMission.typeShow = 1;
            Transform tf = Instantiate(ItemRequireMission) as Transform;
            tableRequire.GetComponent<UIGrid>().AddChild(tf);
            tf.GetComponent<ItemRequireMission>().SetDataProductComon(MissionData.factoryDataMission, 0);
        }
        #endregion
        //---------------------------------------SHOP----------------------------------------------
        #region Shop Require
        //SHOP - BAN RIENG
        for (int i = 0; i < MissionData.shopDataMission.listProducts.Count; i++)
        {
            if (MissionData.shopDataMission.listProducts[i].targetSell >= 1)
            {
                MissionData.shopDataMission.listProducts[i].typeShow = 1;
                Transform tf = Instantiate(ItemRequireMission) as Transform;
                tableRequire.GetComponent<UIGrid>().AddChild(tf);
                tf.GetComponent<ItemRequireMission>().SetDataProduct(MissionData.shopDataMission.listProducts[i]);
            }
        }
        //SHOP -  BAN CHUNG
        if (MissionData.shopDataMission.targetSells > 0)
        {
            MissionData.shopDataMission.typeShow = 1;
            Transform tf = Instantiate(ItemRequireMission) as Transform;
            tableRequire.GetComponent<UIGrid>().AddChild(tf);
            tf.GetComponent<ItemRequireMission>().SetDataProductComon(MissionData.shopDataMission, 1);
        }
        //SHOP - BAN SAN PHAM THOI TIET
        if (MissionData.shopDataMission.targetItem > 0)
        {
            MissionData.shopDataMission.typeShow = 0;
            Transform tf = Instantiate(ItemRequireMission) as Transform;
            tableRequire.GetComponent<UIGrid>().AddChild(tf);
            tf.GetComponent<ItemRequireMission>().SetDataProductComon(MissionData.shopDataMission, 2);
        }
        #endregion
        //---------------------------------------CITY----------------------------------------------
        #region City Require
        //CITY - DAO TAO NHAN VIEN LEN LEVEL
        if (MissionData.townDataMission.targetTraning.targetNumber > 0)
        {
            //Khong can set type show
            //MissionData.townDataMission.targetTraning.typeShow = 1;
            Transform tf = Instantiate(ItemRequireMission) as Transform;
            tableRequire.GetComponent<UIGrid>().AddChild(tf);
            tf.GetComponent<ItemRequireMission>().SetTownData(MissionData.townDataMission.targetTraning, 0);
        }
        //CITY - QUAY SO SO
        if (MissionData.townDataMission.targetNumberLoterry > 0)
        {
            MissionData.townDataMission.typeShow = 1;
            Transform tf = Instantiate(ItemRequireMission) as Transform;
            tableRequire.GetComponent<UIGrid>().AddChild(tf);
            tf.GetComponent<ItemRequireMission>().SetTownData(MissionData.townDataMission, 1);
        }
        #endregion
        //---------------------------------------COMON----------------------------------------------
        #region Common Require
        //TARGET - Common
        if (MissionData.targetCommon.maxTime < 0)
        {
            MissionData.targetCommon.maxTime = 0;
        }
        //Muc tieu fill rate khach hang
        if (MissionData.targetCommon.targetCustomerRate > 0)
        {
            MissionData.targetCommon.typeShow = 0;
            Transform targetMoney = Instantiate(ItemRequireMission) as Transform;
            tableRequire.GetComponent<UIGrid>().AddChild(targetMoney);
            targetMoney.GetComponent<ItemRequireMission>().SetDataTargetMission(MissionData.targetCommon);
        }
        //Muc tieu theo tien
        if (MissionData.targetCommon.targetMoney > 0)
        {
            MissionData.targetCommon.typeShow = 1;
            Transform targetMoney = Instantiate(ItemRequireMission) as Transform;
            tableRequire.GetComponent<UIGrid>().AddChild(targetMoney);
            targetMoney.GetComponent<ItemRequireMission>().SetDataTargetMission(MissionData.targetCommon);
        }
        #endregion
        ChangeLanguage(level);
        tableRequire.GetComponent<UIGrid>().Reposition();
        tableRequire.GetComponent<UIGrid>().repositionNow = true;
    }

    public void Hide()
    {
        for (int i = 0; i < transform.Find("TableRequire").childCount; i++)
        {
            Destroy(transform.Find("TableRequire").GetChild(i).gameObject);
        }
    }
    public void ChangeLanguage(int level)
    {
        transform.Find("BackGround").Find("Logo").Find("name").GetComponent<UILabel>().text = MissionControl.Language["MISSION"] + " " + level;
        transform.Find("Day").GetComponent<UILabel>().text = "" + MissionData.targetCommon.maxTime + " " + MissionControl.Language["Days"];
        transform.Find("Money").GetComponent<UILabel>().text = "" + DString.ConvertString(MissionData.targetCommon.startMoney) + " $";
        transform.Find("BackGround").Find("lbTimeLimit").GetComponent<UILabel>().text = MissionControl.Language["Time_limit"];
        transform.Find("BackGround").Find("lbMoney").GetComponent<UILabel>().text = MissionControl.Language["Fund"];
        transform.Find("BackGround").Find("lbRequired").GetComponent<UILabel>().text = MissionControl.Language["Require"];
        transform.Find("Score").GetComponent<UILabel>().text = MissionControl.Language["Your_Score"] + ": ";
        transform.Find("Start").Find("Label").GetComponent<UILabel>().text = MissionControl.Language["START"];
    }

    public override void ShowDialog(DialogAbs.CallBackShowDialog callback = null)
    {
        star1.gameObject.SetActive(false);
        star2.gameObject.SetActive(false);
        star3.gameObject.SetActive(false);
        //Debug.Log("VariableSystem.mission " + VariableSystem.mission +" Star "+ DataMissionControlNew.missionData[VariableSystem.mission].Star);
        if (DataCache.dataMissionCache[VariableSystem.mission - 1].Star > 0)
        {
            star1.gameObject.SetActive(true);
        }
        if (DataCache.dataMissionCache[VariableSystem.mission - 1].Star > 1)
        {
            star2.gameObject.SetActive(true);
        }
        if (DataCache.dataMissionCache[VariableSystem.mission - 1].Star > 2)
        {
            star3.gameObject.SetActive(true);
        }
        LeanTween.scale(gameObject, new Vector3(1, 1, 1f), 0.3f).setUseEstimatedTime(true).setEase(LeanTweenType.easeOutBack);
        transform.Find("Score").Find("Label").GetComponent<UILabel>().text = "" + DataCache.dataMissionCache[VariableSystem.mission - 1].Score;
        transform.Find("LbStar1").GetComponent<UILabel>().text = MissionControl.Language["All_target"];
    }

    public override void HideDialog(DialogAbs.CallBackHideDialog callback = null)
    {
        throw new System.NotImplementedException();
    }
}
