using UnityEngine;
using System.Collections;

public class PanelHelpController : MonoBehaviour
{
    private Animator animator;
    private string orientationPanel = "RightToLeft";

    public UILabel label;
    private bool updateText;
    private string textView;

    public int countClick;
    public UITexture backGroundLabel;
    private SpriteRenderer[] TexttureObjects;
    private UITexture[] uiTextureBG;
    private UILabel uiLabel;

    void Start()
    {
        countClick = 0;
        animator = GetComponent<Animator>();
        if (orientationPanel.Equals("RightToLeft"))
        {
            animator.Play("RightToLeft");
        }
        else
        {
            animator.Play("LeftToRight");
            backGroundLabel.transform.localPosition = new Vector3(-205f, -235f, 0);
            backGroundLabel.transform.localScale = new Vector3(-1, 1, 1);
        }
        TexttureObjects = gameObject.GetComponentsInChildren<SpriteRenderer>();
        uiTextureBG = gameObject.GetComponentsInChildren<UITexture>();
        uiLabel = gameObject.GetComponentInChildren<UILabel>();
    }


    void Update()
    {
        if (updateText)
        {
            label.text = textView;
            updateText = false;
        }
    }
    void CompleteAnimation()
    {
        HelpGirlController.animator.Play("talk");
    }

    public void setOrientationPanel(string RightToLeftOrLeftToRight)
    {
        orientationPanel = RightToLeftOrLeftToRight;
    }

    public void setTextView(string text)
    {
        textView = text;
        updateText = true;
    }

    public void SetStatusHelp(bool isView)
    {
        gameObject.GetComponent<BoxCollider>().enabled = isView;
        if (TexttureObjects == null || TexttureObjects.Length == 0)
        {
            TexttureObjects = gameObject.GetComponentsInChildren<SpriteRenderer>();
            uiTextureBG = gameObject.GetComponentsInChildren<UITexture>();
            uiLabel = gameObject.GetComponentInChildren<UILabel>();
        }
        foreach (SpriteRenderer TexttureObject in TexttureObjects)
        {
            TexttureObject.enabled = isView;
        }
        foreach (UITexture uiTexttureObject in uiTextureBG)
        {
            uiTexttureObject.enabled = isView;
        }
        uiLabel.enabled = isView;
    }

    public void BtnNext_Click()
    {
        countClick++;
        HelpGirlController.animator.Play("talk");
    }

    void OnClick()
    {
        countClick++;
        HelpGirlController.animator.Play("talk");
    }
}
