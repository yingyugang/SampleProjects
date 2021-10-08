using UnityEngine;
using System.Collections;
using System;

public class PanelTextMachineController : MonoBehaviour
{

    public static Animator animator;

    public UILabel nameMachine;
    public UILabel timeProduct;

    private string textTimeProductHour;
    private string textTimeProductDay;

    private float timesLeft;
    private int dayLeft;
    private int hourLeft;
    void Start()
    {
        animator = GetComponent<Animator>();
        nameMachine.text = MachineController.machineSelect.GetComponent<MachineController>().nameMachine;
        Vector3 pos = new Vector3(MachineController.machineSelect.transform.position.x / 3.6f, (MachineController.machineSelect.transform.position.y + 1 ) / 3.6f, 0);
        gameObject.transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        if (MachineController.machineSelect != null)
        {
            timesLeft = MachineController.machineSelect.GetComponent<MachineController>().timeLeft;
            timeProduct.text = ChangeTimeToText(timesLeft);
        }
    }

    string ChangeTimeToText(float timeLeftClone)
    {
        dayLeft =(int)(timeLeftClone *24/30) / 24;
        hourLeft = (int)(timeLeftClone * 24 / 30) % 24;
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
}
