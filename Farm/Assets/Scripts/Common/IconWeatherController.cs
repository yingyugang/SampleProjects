using UnityEngine;
using System.Collections;

public class IconWeatherController : MonoBehaviour
{
    int timeOfday = 8; // 1/3 day
    int[,] positionsMove = new int[,] { { -75, -50 }, { 0, -50 }, { 50, 0 }, { 50, 75 } };// those position
    Vector3 stepMove = Vector2.zero;

    float MinScale = 1.0f;
    float MaxScale = 1.5f;

    CommonObjectScript common;
    static bool isScale;
    float scale;
    bool isEncrease;
    // Use this for initialization
    void Start()
    {
        common = transform.GetComponentInParent<CommonObjectScript>();
        transform.localPosition = new Vector2(positionsMove[0, 0], positionsMove[0, 1]);
        isScale = false;
        scale = 1.0f;
        isEncrease = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (common.countTimeOneDay == 0)
        {
            transform.localPosition = new Vector2(positionsMove[0, 0], positionsMove[0, 1]);
            if (!isScale)
            {
                scale = MinScale;
                isEncrease = true;
                transform.localScale = Vector3.one * scale;
            }
            else transform.localScale = Vector3.one * 1.5f;
        }
        else if (common.countTimeOneDay < timeOfday)
        {
            stepMove.x = 75f / timeOfday * Time.deltaTime;
            stepMove.y = 0f / timeOfday * Time.deltaTime;
            stepMove += transform.localPosition;
            transform.localPosition = stepMove;
        }
        else if (common.countTimeOneDay < timeOfday * 2)
        {
            stepMove = Vector2.one * 50f / timeOfday * Time.deltaTime;
            stepMove += transform.localPosition;
            transform.localPosition = stepMove;
        }
        else
        {
            stepMove.x = 0f / timeOfday * Time.deltaTime;
            stepMove.y = 75f / timeOfday * Time.deltaTime;
            stepMove += transform.localPosition;
            transform.localPosition = stepMove;
        }

        //if (isScale)
        //{
        //    if (isEncrease && scale < MaxScale)
        //    {
        //        scale += 0.01f;
        //        if (scale >= MaxScale) isEncrease = false;
        //    }
        //    else if (!isEncrease && scale > MinScale)
        //    {
        //        scale -= 0.01f;
        //        if (scale <= MinScale) isEncrease = true;
        //    }
        //    transform.localScale = Vector3.one * scale;
        //}
    }
    public static void setScale(bool scale)
    {
        isScale = scale;
    }
}
