using UnityEngine;

public class AddValueScript : MonoBehaviour
{
    float laterTime = 0;
    public UILabel[] labels;
    public UITexture texture;
    Animator animator;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        if (name.Equals("textShow") || name.Equals("vang") || name.Equals("star") || name.Equals("coin"))
        {
        }
        else if (name.Equals("gold") || name.Equals("bonus"))
        {
            animator.speed = 0;
        }
        else
        {
            texture.mainTexture = Resources.Load("Factory/Button/Images/Material/" + name) as Texture;
            texture.width = 60;
            texture.height = 60;
            animator.speed = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.speed == 0)
        {
            laterTime += Time.deltaTime;
            if (laterTime >= 1f)
            {
                animator.speed = 1;
            }
        }
    }

    public void setValue(string value, bool isShowImg = true)
    {
        if (!isShowImg)
        {
            labels[1].text = value;
            texture.gameObject.SetActive(false);
            labels[0].gameObject.SetActive(false);
        }
        else
        {
            labels[0].text = value;
            labels[1].gameObject.SetActive(false);
        }
    }
    public void setValue(string value, string link)
    {
        labels[0].text = value;
        labels[1].gameObject.SetActive(false);
        if (!link.Equals(""))
        {
            texture.mainTexture = Resources.Load(link) as Texture;
        }
    }
    public void setSub()
    {
        GetComponent<Animator>().Play("SubValue");
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
}
