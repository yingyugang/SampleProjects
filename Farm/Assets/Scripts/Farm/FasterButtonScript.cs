using UnityEngine;

public class FasterButtonScript : MonoBehaviour
{
    public GameObject plant;
    public StaffTimerScript staffHouse;
    // Use this for initialization
    public void Clickme()
    {
        if (plant != null)
            plant.GetComponent<PlantControlScript>().Faster_Click();
        else staffHouse.BtnFaster_Click();
    }
}
