using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OdenChangeBackground : MonoBehaviour {

	public Sprite[] BackGround;
	private SpriteRenderer m_Img;
	private int m_IndexImg;
	// Use this for initialization
	void Start () {
		m_IndexImg = 0;
		m_Img = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
		m_Img.sprite = BackGround[m_IndexImg];
	
	}

	public void ChangeBackGround(){
		if (m_IndexImg > 0) {
			m_IndexImg = 0;
		} else {
			m_IndexImg++;
		}
		m_Img.sprite = BackGround [m_IndexImg];

	
	}
}
