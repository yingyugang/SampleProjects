using UnityEngine;
using System.Collections;
using Assets.Scripts.Common;

public class ItemRequireMission : MonoBehaviour
{

    Transform dialogTask;
    public int id;

    void Awake()
    {
        dialogTask = GameObject.Find("DialogTask").transform;
        //transform.FindChild("Bgrequire").GetComponent<UITexture>().depth = GetComponent<UITexture>().depth + 1;
        //transform.FindChild("Quantity").GetComponent<UILabel>().depth = GetComponent<UITexture>().depth + 2;
    }

    void Start()
    {
        gameObject.GetComponent<UILabel>().depth = 8;
        transform.localScale = new Vector3(1, 1, 1);
    }

    //Target money + target Star
    public void SetDataTargetMission(ItemAbstract item)
    {
        string str = MissionControl.Language["Earned"] + " " + DString.ConvertString(item.getTarget()) + " $";
        //if Fill rate
        if (item.typeShow == 0)
        {
            str = MissionControl.Language["Customers_rate_is"] + " " + DString.ConvertString(item.getTarget()) + " " + MissionControl.Language["star"];
        }
        dialogTask.GetComponent<DialogTask>().AddItemTask(str, item);
        this.GetComponent<UILabel>().text = "- " + str;
    }

    //Mua + nang cap may
    public void SetDataMarchine(ItemAbstract item)
    {
        if (item.typeShow == 1)
        {
            //Mua so luong
            this.id = item.getType();
            string str = "";
            switch (id)
            {
                default:
                case 1://banh my
                    str = MissionControl.Language["Bakery_machine"] + " ";
                    break;
                case 2://banh my
                    str = MissionControl.Language["BBQ_machine"] + " ";
                    break;
                case 3://banh my
                    str = MissionControl.Language["Fruit_machine"] + " ";
                    break;
                case 4://banh my
                    str = MissionControl.Language["Dairy_machine"] + " ";
                    break;
            }
            this.GetComponent<UILabel>().text = "- " + MissionControl.Language["Buy"] + " " + item.getTarget() + " " + str;
            dialogTask.GetComponent<DialogTask>().AddItemTask(MissionControl.Language["Buy"] + " " + item.getTarget() + " " + str, item);
        }
        else
        {
            this.id = item.getType();
            string str = "";
            switch (id)
            {
                default:
                case 1://banh my
                    str = MissionControl.Language["Upgrade_bakery"] + " ";
                    break;
                case 2://banh my
                    str = MissionControl.Language["Upgrade_BBQ"] + " ";
                    break;
                case 3://banh my
                    str = MissionControl.Language["Upgrade_Fruit"] + " ";
                    break;
                case 4://banh my
                    str = MissionControl.Language["Upgrade_Dairy"] + " ";
                    break;
            }
            this.GetComponent<UILabel>().text = "- " + str + item.getTarget();
            dialogTask.GetComponent<DialogTask>().AddItemTask(str + item.getTarget(), item);
        }
    }
    
    //Mo khoa vi tri dat may
    public void SetDataMarchineUnlockPosition(ItemAbstract item)
    {

        this.GetComponent<UILabel>().text = "- " + MissionControl.Language["Unlock"] + " " + item.getTarget() + " " + MissionControl.Language["position_put_machine"];
        dialogTask.GetComponent<DialogTask>().AddItemTask(MissionControl.Language["Unlock"] + " " + item.getTarget() + " " + MissionControl.Language["position_put_machine"], item);
    }
    
    //Ban + San xuat san pham rieng
    public void SetDataProduct(ItemAbstract item)
    {
        this.id = item.getType();
        string str = "";
        if (item.typeShow == 0)//Muc tieu san xuat
        {
            str = MissionControl.Language["Produce"] + " ";
        }
        else //Muc tieu ban
        {
            str = MissionControl.Language["Sell"] + " ";
        }
        switch (id)
        {
            default:
            case 7://banh my
                str += item.getTarget() + " " + MissionControl.Language["breads"];
                break;
            case 8://banh ngot
                str += item.getTarget() + " " + MissionControl.Language["cookie"];
                break;
            case 9://banh nho
                str += item.getTarget() + " " + MissionControl.Language["grapes_pie"];
                break;
            case 10://banh dau
                str += item.getTarget() + " " + MissionControl.Language["strawberry_pie"];
                break;
            case 11://banh tom
                str += item.getTarget() + " " + MissionControl.Language["shirmp_cake"];
                break;
            case 12://banh nuong
                str += item.getTarget() + " " + MissionControl.Language["fish_roast"];
                break;
            case 13://ga quay
                str += item.getTarget() + " " + MissionControl.Language["chicken_roast"];
                break;
            case 14://banh humbugr
                str += item.getTarget() + " " + MissionControl.Language["Humburger"];
                break;
            case 15://ca chua
                str += item.getTarget() + " " + MissionControl.Language["tomato"];
                break;
            case 16://nho
                str += item.getTarget() + " " + MissionControl.Language["grapes"];
                break;
            case 17://dau
                str += item.getTarget() + " " + MissionControl.Language["strawberry"];
                break;
            case 18://chai sua dau
                str += item.getTarget() + " " + MissionControl.Language["strawberry_milk"];
                break;
            case 19://bich sua tuoi
                str += item.getTarget() + " " + MissionControl.Language["fresh_milk"];
                break;
            case 20://mieng bo
                str += item.getTarget() + " " + MissionControl.Language["butter"];
                break;
            case 21://chai sua nho
                str += item.getTarget() + " " + MissionControl.Language["grapes_milk"];
                break;
            case 22://mieng fomai
                str += item.getTarget() + " " + MissionControl.Language["cheess"];
                break;
        }
        this.GetComponent<UILabel>().text = "- " + str;
        dialogTask.GetComponent<DialogTask>().AddItemTask(str, item);
    }

    //Ban  + san xuat san pham chung + san pham theo thoi tiet
    public void SetDataProductComon(ItemAbstract item, int type)//type = 0 : san xuat, type  = 1: ban san pham chung, 2: san pham theo thoi tiet
    {
        string str = "";
        if(type == 0)
        {
            str = MissionControl.Language["Produce"] + " " + item.getTarget() + " " + MissionControl.Language["products"];
        }
        else if (type == 1)
        {
            str = MissionControl.Language["Sell"] + " " + item.getTarget() + " " + MissionControl.Language["products"];
        }
        else if (type == 2)
        {
            str = MissionControl.Language["Sell"] + " " + item.getTarget() + " " + MissionControl.Language["products_with_weather_gift"];
        }
        this.GetComponent<UILabel>().text = "- " + str;
        dialogTask.GetComponent<DialogTask>().AddItemTask(str, item);
    }

    //Mo + Nang cap ruong, ao, chuong
    public void SetDataField(ItemAbstract item)
    {
        if (item.typeShow == 1)//number
        {
            string str = "";
            switch (item.getType())
            {
                default:
                case 1://ruong
                    str = MissionControl.Language["Open"] + " " + item.getTarget() + " " + MissionControl.Language["fields"];
                    break;
                case 2://chuong
                    str = MissionControl.Language["Open"] + " " + item.getTarget() + " " + MissionControl.Language["barns"];
                    break;
                case 3://ao
                    str = MissionControl.Language["Open"] + " " + item.getTarget() + " " + MissionControl.Language["ponds"];
                    break;
            }
            this.GetComponent<UILabel>().text = "- " + str;
            dialogTask.GetComponent<DialogTask>().AddItemTask( str, item);
        }
        else //Level
        {
            string str = "";
            switch (item.getType())
            {
                default:
                case 1://ruong
                    str = MissionControl.Language["Upgrade_fields"] + " " + item.getTarget();
                    break;
                case 2://chuong
                    str = MissionControl.Language["Upgrade_barns"] + " " + item.getTarget();
                    break;
                case 3://ao
                    str = MissionControl.Language["Upgrade_ponds"] + " " + item.getTarget();
                    break;
            }
            this.GetComponent<UILabel>().text = "- " + str;
            dialogTask.GetComponent<DialogTask>().AddItemTask(str, item);
        }
    }

    //Thu hoach rieng -  target chinh la target trong cay
    public void SetDataBreed(ItemAbstract item)
    {
        string str = "";
        switch (item.getType())
        {
            default:
            case 1://lua my
                str = MissionControl.Language["wheat"];
                break;
            case 2://ca ca chua
                str = MissionControl.Language["tomato_field"];
                break;
            case 3://nho
                str = MissionControl.Language["grapes_field"];
                break;
            case 4://dautay
                str = MissionControl.Language["strawberry_field"];
                break;
            case 5://dui ga
                str = MissionControl.Language["chicken_thighs"];
                break;
            case 6://thit lon
                str = MissionControl.Language["pock"];
                break;
            case 7://binh sua
                str = MissionControl.Language["milk_crates"];
                break;
            case 8://ca
                str = MissionControl.Language["fishs"];
                break;
            case 9://Tom
                str = MissionControl.Language["shrimph"];
                break;
        }
        this.GetComponent<UILabel>().text = "- " + MissionControl.Language["Harvest"] + " " + item.getTarget() + " " + str;
        dialogTask.GetComponent<DialogTask>().AddItemTask(MissionControl.Language["Harvest"] + " " + item.getTarget() + " " + str, item);
    }

    //Thu hoach chung
    public void SetDataBreedComon(ItemAbstract item, int type)//type = 0: ruong, 1: chuong
    {
        string str = "";
        if (type == 0)
        {
            str = MissionControl.Language["fields"];
        }
        else
        {
            str = MissionControl.Language["barns"];
        }
        this.GetComponent<UILabel>().text = "- " + MissionControl.Language["Harvest"] + " " + item.getTarget() + " " + str;
        dialogTask.GetComponent<DialogTask>().AddItemTask(MissionControl.Language["Harvest"] + " " + item.getTarget() + " " + str, item);
    }

    //Dao tao nhan vien len level x + Quay so so 
    public void SetTownData(ItemAbstract item, int type)//type = 0: dao tao nhan vien. type = 1: Quay so so
    {
        string str = "";
        if (type == 0)
        {
            item.typeShow = 0;
            int target = item.getTarget();
            item.typeShow = 1;
            int from = item.getTarget();
            str = MissionControl.Language["Training"] + " " + from + " " + MissionControl.Language["staffs_to_level"] + " " + target;
        }
        else
        {
            int target = item.getTarget();
            str = MissionControl.Language["Spin_lucky_wheel_in"] + " " + target + " " + MissionControl.Language["times"];
        }
        this.GetComponent<UILabel>().text = "- " + str;
        dialogTask.GetComponent<DialogTask>().AddItemTask(str, item);
    }
}
