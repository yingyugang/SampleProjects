using UnityEngine;
using System.Collections;

public class ProductEfectController : MonoBehaviour
{

    private Vector2 targetPosition;
    private int maxStep;
    int curStep;
    Vector2 speed;
    float timeWait;

    public int IDProduct;
    void Start()
    {

        timeWait = 0.2f;
        maxStep = 35;
        this.GetComponent<UITexture>().mainTexture = MaketController.listSpriteProduct[IDProduct - 7];
        //this.GetComponentInChildren<ParticleSystem>().renderer.sortingLayerID = 3;
        // p0 = this.transform.localPosition;
        targetPosition = new Vector2(-570, -300);
        curStep = 0;
        speed = new Vector2((targetPosition.x - this.GetComponent<Transform>().localPosition.x) / maxStep, (targetPosition.y - this.GetComponent<Transform>().localPosition.y) / maxStep);
    }

    // Update is called once per frame
    void Update()
    {
        #region
        //if (timeCount < timeDelay)
        //{
        //    timeCount += Time.deltaTime;
        //    transform.localScale = new Vector3(transform.localScale.x + 4*Time.deltaTime, transform.localScale.y + 4 * Time.deltaTime, transform.localScale.z);
        //}
        //else
        //{
        //    if (transform.localPosition.x != targetPosition.x && transform.localPosition.y != targetPosition.y)
        //        MoveAndScale();
        //    else
        //    {
        //        print("vao day");
        //        Destroy(gameObject);
        //    }
        //}
        #endregion
        
        if (timeWait > 0)
        {
            timeWait -= Time.deltaTime;
            if (this.GetComponent<UITexture>().width < 200)
            {
                this.GetComponent<UITexture>().width += 3;
                this.GetComponent<UITexture>().height += 3;
            }
        }
        else
        {
            if (curStep >= maxStep)
            {
                GameObject.Find("CommonObject").GetComponent<CommonObjectScript>().Storage_Active();
                GameObject.Destroy(this.gameObject);
            }
            else
            {
                curStep++;
                this.GetComponent<Transform>().localPosition += new Vector3(speed.x, speed.y, 0);
                {
                    this.GetComponent<UITexture>().width -= 2;
                    this.GetComponent<UITexture>().height -= 2;
                }
            }
        }
    }
}
