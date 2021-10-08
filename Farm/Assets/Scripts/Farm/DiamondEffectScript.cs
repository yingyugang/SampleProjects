using UnityEngine;

public class DiamondEffectScript : MonoBehaviour
{
    public UILabel Label;
    AudioControl audioControl;
    void Start()
    {
        audioControl = GameObject.Find("AudioControl").GetComponent<AudioControl>();
        audioControl.PlaySound("Kim cuong roi xuong");
    }
    public void setValueDiamond(int value, int sortingLayerID = 3)
    {
        Label.text = value.ToString();
        this.GetComponentInChildren<ParticleSystem>().GetComponent<Renderer>().sortingLayerID = sortingLayerID;
    }
    public void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }
}
