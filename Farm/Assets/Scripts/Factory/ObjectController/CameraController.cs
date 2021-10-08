using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
   
    #region For DragCamera
    Vector2  step;
    private Vector2 startPosition, stopPosition, neoPosition;
    #endregion

    #region For Move Camera
    private Vector3 targetPosition;
    public static int IDPosition;
    public static bool isDrag;
    public GameObject btn_Result;
    private GameObject commonObject;
    private Transform btn_ResultCommon;
    //public static bool isViewPopup;
    #endregion
    void Start()
    {
        commonObject = GameObject.Find("CommonObject");
        btn_ResultCommon = commonObject.transform.Find("Btn_Result");
        GoogleAnalytics.instance.LogScreen("Factory Screen");
        if (VariableSystem.mission == 1)
        {
            this.gameObject.transform.position = new Vector3(0, -0.33f, -10);
        }
        startPosition = Input.mousePosition;
        stopPosition = Input.mousePosition;
        neoPosition = new Vector2(-1, -1);

        IDPosition = 0;
        isDrag = true;
       // isViewPopup = false;


    }
    void Update()
    {
        if (btn_ResultCommon.gameObject.activeInHierarchy)
        {
            btn_Result.SetActive(true);
        }
        if (!CommonObjectScript.isViewPoppup)
        {
            DragCamera();
            MoveCamera();
        }
    }

    void DragCamera()
    {
        if (!FactoryScenesController.isHelp)
        {
            if (isDrag)
            {
                if (Input.GetMouseButton(0))
                {
                    stopPosition = Input.mousePosition;
                    if (startPosition.x < 0) { startPosition = stopPosition; return; }
                    step = startPosition - stopPosition;
                    startPosition = stopPosition;
                    step *= 0.01f;
                    transform.position = new Vector3(transform.position.x + step.x, transform.position.y + step.y, transform.position.z);
                    if (transform.position.x < -1.7f)
                    {
                        transform.position = new Vector3(-1.7f, transform.position.y, transform.position.z);
                    }
                    if (transform.position.x > 1.7f)
                    {
                        transform.position = new Vector3(1.7f, transform.position.y, transform.position.z);
                    }
                    if (transform.position.y < -1.1)
                    {
                        transform.position = new Vector3(transform.position.x, -1.1f, transform.position.z);
                    }
                    if (transform.position.y > 1.1)
                    {
                        transform.position = new Vector3(transform.position.x, 1.1f, transform.position.z);
                    }
                }
                else if (startPosition.x >= 0) startPosition = neoPosition;
            }
        }
    }
    void MoveCamera()
    {
        if (!FactoryScenesController.isHelp)
        {
            if (!isDrag)
                transform.position = Vector3.MoveTowards(transform.position, SetPositionCameraMove(IDPosition), 4.0f * Time.deltaTime);
        }
    }
    Vector3 SetPositionCameraMove(int IDPositionTouch)
    {
        switch (IDPositionTouch)
        {

            case 1: targetPosition = new Vector3(-1.506616f, 1.224696f, -10f); break;
            case 2: targetPosition = new Vector3(0, 1.107448f, -10f); break;
            // case 2: targetPosition = new Vector3(0, 0, -10f); break;
            case 3: targetPosition = new Vector3(1.832747f, 0.3527888f, -10f); break;
            case 4: targetPosition = new Vector3(-1.724938f, 0.002208829f, -10f); break;
            case 5: targetPosition = new Vector3(-1.760874f, 0.2090434f, -10f); break;
            // case 5: targetPosition = new Vector3(0, 0, -10f); break;
            case 6: targetPosition = new Vector3(0, -0.9409152f, -10f); break;

            default: targetPosition = new Vector3(0, 0, -10); break;
        }
        return targetPosition;
    }
}