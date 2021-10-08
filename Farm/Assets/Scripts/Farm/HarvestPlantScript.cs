using UnityEngine;

public class HarvestPlantScript : MonoBehaviour
{
    Vector3 p1 = new Vector3(-570, -300, 0);
    string link = "";
    int maxSize = 100;

    int maxStep = 50;
    int curStep = 0;

    float speedWait;
    Vector3 tocdo;

    UITexture thisTexture;
    CommonObjectScript common;
    // Use this for initialization
    void Start()
    {
        common = GameObject.FindGameObjectWithTag("CommonObject").GetComponent<CommonObjectScript>();
        thisTexture = GetComponent<UITexture>();
        speedWait = Random.Range(0.5f, 1.5f);
        if (!link.Equals(""))
        {
            if (Application.loadedLevelName.Equals("Farm"))
                p1 += GameObject.Find("UI Root").transform.Find("Camera").localPosition;
            thisTexture.mainTexture = Resources.Load(link) as Texture;
        }
        tocdo = new Vector3((p1.x - transform.localPosition.x) / maxStep, (p1.y - transform.localPosition.y) / maxStep);
    }

    // Update is called once per frame
    void Update()
    {
        if (speedWait > 0) //grow up
        {
            speedWait -= Time.deltaTime;
            if (thisTexture.width < maxSize)
            {
                thisTexture.width += 3;
                thisTexture.height += 3;
            }
        }
        else//move
        {
            if (curStep >= maxStep)
            {
                if (link.EndsWith("vang"))
                {
                    common.Coin_Active();
                }
                else if (link.EndsWith("sao"))
                {
                    common.Star_Active();
                }
                else
                {
                    common.Storage_Active();
                }
                GameObject.Destroy(gameObject);
            }
            else
            {
                curStep++;
                transform.localPosition += tocdo;
                if (thisTexture.width > 35 && curStep % 2 == 0)
                {
                    thisTexture.width -= 1;
                    thisTexture.height -= 1;
                }
            }
        }
    }
    public void setValue(string link, Vector3 target, int maxSize = -1)
    {
        if (maxSize != -1)
        {
            this.maxSize = maxSize;
            p1 = target;
        }
        this.link = link;
    }
}
