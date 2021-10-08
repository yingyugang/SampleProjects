using UnityEngine;
using System.Collections;
using BaPK;

public class TimeLeftMachineController : MonoBehaviour
{

    public Label timeLeftLabel;

    private string textTimeProductHour;
    private string textTimeProductDay;
    private float timesLeft;
    private int dayLeft;
    private int hourLeft;

    void Start()
    {
        timeLeftLabel.GetComponent<New1FontRead>().New1Read("12", 1, TextAlignment.Center,"", 0f, 10f);
    }

    // Update is called once per frame
    void Update()
    {

        if (MachineController.machineSelect != null)
        {
            timesLeft = MachineController.machineSelect.GetComponent<MachineController>().timeLeft;
            timeLeftLabel.setText(ChangeTimeToText(timesLeft));
            timeLeftLabel.refresh();
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
}
