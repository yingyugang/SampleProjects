using UnityEngine;

public class CameracontrollerScript : MonoBehaviour
{
    Vector2 start, stop, step;
    Vector2 pstart, pstop, pstep;
    int curStep, maxStep;
    Transform common;
    CommonObjectScript common1;
    GameObject audiocontroll;
    public GameObject panelPlant;
    UIPanel panelpla;
    PlantControlScript plant;

    Vector2 startPosition, stopPosition, neoPosition;
    // Use this for initialization
    void Start()
    {
        startPosition = Input.mousePosition;
        stopPosition = Input.mousePosition;
        neoPosition = new Vector2(-1, -1);
        curStep = 0;
        maxStep = 0;
        common = GameObject.FindGameObjectWithTag("CommonObject").GetComponent<Transform>();
        audiocontroll = GameObject.Find("AudioControl");
        common1 = common.GetComponent<CommonObjectScript>();
        //panelPlant = transform.parent.FindChild("PanelPlant").gameObject;
        panelpla = panelPlant.GetComponent<UIPanel>();
        plant = panelPlant.GetComponent<PlantControlScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isLoadingLevel)
            transform.localPosition = Vector3.zero;
        else if (!(CommonObjectScript.isGuide || CommonObjectScript.isEndGame))
            if (curStep < maxStep)//function auto move when you indentifine one target
            {
                curStep++;
                transform.localPosition = new Vector2(transform.localPosition.x + step.x, transform.localPosition.y + step.y);
                panelPlant.transform.localPosition = new Vector2(panelPlant.transform.localPosition.x + pstep.x, panelPlant.transform.localPosition.y + pstep.y);
                panelpla.clipOffset -= new Vector2(pstep.x, pstep.y);
                startPosition = neoPosition;
            }
            else //move follow player hand.
            {
                if (!(PlantControlScript.breedSelected >= 0 || common1.isOpenStorage || common1.isOpennew || CommonObjectScript.isViewPoppup || Time.timeScale == 0 || (common1.dialogTask != null && common1.dialogTask.isEnable)))
                {
                    if (Input.GetMouseButton(0))
                    {
                        stopPosition = Input.mousePosition;
                        if (startPosition.x < 0) { startPosition = stopPosition; return; }
                        step = startPosition - stopPosition;
                        startPosition = stopPosition;
                        if (step.x < 2 && step.y < 2 && step.x > -2 && step.y > -2) return;//avoid move when click                   
                        step *= 1.3f;
                        transform.localPosition = new Vector2(transform.localPosition.x + step.x, transform.localPosition.y + step.y);
                        if (transform.localPosition.x < -150)
                        {
                            transform.localPosition = new Vector2(-150, transform.localPosition.y);
                        }
                        if (transform.localPosition.x > 150)
                        {
                            transform.localPosition = new Vector2(150, transform.localPosition.y);
                        }
                        if (transform.localPosition.y < -150)
                        {
                            transform.localPosition = new Vector2(transform.localPosition.x, -150);
                        }
                        if (transform.localPosition.y > 150)
                        {
                            transform.localPosition = new Vector2(transform.localPosition.x, 150);
                        }
                    }
                    else if (startPosition.x >= 0) startPosition = neoPosition;
                }
            }
    }
    void LateUpdate()
    {
        if (common != null && gameObject != null)
        {
            common.transform.localPosition = transform.localPosition * 0.415f / 150;
            audiocontroll.transform.localPosition = common.transform.localPosition;
        }
    }
    public void Move(Vector2 stop, int maxStep)
    {
        curStep = 0;
        this.maxStep = maxStep;

        start = transform.localPosition;
        this.stop = stop;
        step = (this.stop - this.start) / maxStep;

        pstart = panelPlant.transform.localPosition;
        pstop = Vector3.zero;
        pstep = (pstop - pstart) / maxStep;
    }
}
