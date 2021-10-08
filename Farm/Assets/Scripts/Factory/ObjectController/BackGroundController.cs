using UnityEngine;
using System.Collections;

public class BackGroundController : MonoBehaviour
{
    private Vector3 posMouseClick;
    private Vector3 posMouseUpdate;

    AudioControl audioControl;
    #region For Test
    //public GameObject commonObject;
   // private WarningTextView wabt;
    #endregion

    void Start()
    {
        audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        audioControl.PlaySoundInstance("Bang chuyen", true, false,0.2f);
       
    }

    
    void Test()
    {
       // wabt.FinishWarning(wabt.IDWarning);
    }
    void OnMouseUp()
    {
        if (FactoryScenesController.isHelp)
        {
          //  MissionControl.Language["MISSION"];
        }
        else
        {
            if (ButtonViewController.animator != null)
                ButtonViewController.animator.SetTrigger("Collape");
            if (ProductQueueController.animator != null)
                ProductQueueController.animator.SetTrigger("Collape");
        }

        if (Mathf.Abs(posMouseClick.x - posMouseUpdate.x) > 0.5f && Mathf.Abs(posMouseClick.y - posMouseUpdate.y) > 0.5f)
        {
            CameraController.isDrag = true;
        }
        else
        {         
            CameraController.isDrag = false;
            CameraController.IDPosition = 0;
        }
       

    }

    void OnMouseDown()
    {
        posMouseClick = Input.mousePosition;
    }

    void Update()
    {
        posMouseUpdate = Input.mousePosition;
        //print(GameObject.FindGameObjectsWithTag("Machine").Length);
       // BackButton();
        if (FactoryScenesController.isChangeLevel)
        {
            UpdateLevel();
            FactoryScenesController.isChangeLevel = false;
        }
    }

    void UpdateLevel()
    {
        foreach (GameObject objectTemp in GameObject.FindGameObjectsWithTag("Machine"))
        {
           // print("indext: " + objectTemp.GetComponent<MachineController>().indextMachine +  "level " + FactoryScenesController.ListQueue[objectTemp.GetComponent<MachineController>().indextMachine]);
            if (objectTemp.GetComponent<MachineController>().levelMachine != FactoryScenesController.ListQueue[objectTemp.GetComponent<MachineController>().indextMachine])
            {
                objectTemp.GetComponent<MachineController>().DestroyPopup();
                objectTemp.GetComponent<MachineController>().levelMachine = FactoryScenesController.ListQueue[objectTemp.GetComponent<MachineController>().indextMachine];
            }
        }
    }

  
}
