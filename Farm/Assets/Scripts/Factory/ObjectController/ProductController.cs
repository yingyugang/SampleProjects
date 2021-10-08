using UnityEngine;
using System.Collections;

public class ProductController : MonoBehaviour
{

    // Use this for initialization
    private Animator animator;
    private Vector2 changePointPosition;
    private Vector2 changeEndPosition;
    private bool isMove;
    private bool isTurnRight;
    public int countPosition;
    public int sortingLayerID;
    public int IDProduct;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(IDProduct.ToString());
        animator.speed = 0;

        SetIDLayer(this.sortingLayerID);
        isMove = false;

        changeEndPosition = new Vector2(6.712935f, -0.1344479f);
        if (((int)(countPosition / 4)).Equals(1))
            changePointPosition = new Vector2(-0.001495562f, -3.687596f);
        else if (((int)(countPosition / 4)).Equals(0))
            changePointPosition = new Vector2(3.763261f, -1.790118f);

        Vector3 conveyorPoint = SetconveyorPoint(countPosition);
        Vector3 MidPoint = new Vector3(conveyorPoint.x + 0.3f, conveyorPoint.y + 1.8f, transform.position.z);

        Vector3[] lstPosition = 
        {
           gameObject.transform.position,
           MidPoint,
           conveyorPoint
        };
        iTween.MoveTo(gameObject, iTween.Hash("path", lstPosition, "time", 1.5f, "islocal", true, "onComplete", "FinishMoveBezier", "easetype", iTween.EaseType.linear));
    }

    void FinishMoveBezier()
    {
        animator.speed = 0.4f;
        isMove = true;
    }


    void Update()
    {
        if (!Application.loadedLevelName.Equals("Factory")) Destroy(gameObject);
        if (isMove)
        {
            if (transform.position.x != changePointPosition.x && !isTurnRight)
            {
                transform.position = Vector2.MoveTowards(transform.position, changePointPosition, 1.5f * Time.deltaTime);
            }
            else
            {
                isTurnRight = true;
                if (transform.position.x != changeEndPosition.x)
                    transform.position = Vector2.MoveTowards(transform.position, changeEndPosition, 1.5f * Time.deltaTime);
                else
                {
                    if (CommonObjectScript.isGuide && VariableSystem.mission == 1)
                    {
                        FactoryScenesController.isHelp = false;
                        //Application.LoadLevel("Farm");
                        LoadingScene.ShowLoadingScene("Farm", true);
                    }
                    Destroy(gameObject);
                }

            }
        }

    }

    Vector3 SetconveyorPoint(int position)
    {
        Vector3 vectorTemp = new Vector3(0, 0, 0);
        switch (position)
        {
            case 1: vectorTemp = new Vector3(-1.830404f, 0.8599999f, 0); break;
            case 2: vectorTemp = new Vector3(0.1755382f, -0.09463531f, 0); break;
            case 3: vectorTemp = new Vector3(2.145229f, -1.025103f, 0); break;
            case 4: vectorTemp = new Vector3(-5.612692f, -0.3242313f, 0); break;
            case 5: vectorTemp = new Vector3(-3.570498f, -1.580966f, 0); break;
            case 6: vectorTemp = new Vector3(-0.9361882f, -3.200221f, 0); break;
        }
        return vectorTemp;
    }
    void SetIDLayer(int sortingLayerID)
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < transforms.Length; i++)
        {
            GameObject gObject = transforms[i].gameObject;
            if (gObject.GetComponent<SpriteRenderer>() != null)
            {
                gObject.GetComponent<SpriteRenderer>().sortingLayerID = sortingLayerID;
            }
        }
    }

}
