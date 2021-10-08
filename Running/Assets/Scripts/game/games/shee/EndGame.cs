using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGame : MonoBehaviour {
	public Sprite[] Ending;
	public GameObject[] infoTxt;
	private Animator m_Anim;
	private Image m_Img;
	private int m_IndexImg;
	private float m_Delay = 1.0f;

	void Awake () {
		m_IndexImg = 0;
		m_Anim = GetComponent<Animator>();
		m_Img = transform.GetChild(0).gameObject.GetComponent<Image>();
		m_Img.sprite = Ending[m_IndexImg];
	}

	public void ShowEnding(){
		m_Anim.Play(SheeResources.GameInfo.Anim_END);
	}

	public void ChangeImg(){
		ShowInfo();
		m_IndexImg++;
		if(m_IndexImg >= Ending.Length){
//			ShowGameResult();
//			return;
			m_IndexImg = 0;
		}
		m_Img.sprite = Ending[m_IndexImg];
		Invoke("ChangeImg", m_Delay);
	}

	void ShowInfo(){
		if(Game10_Manager.instance.isAllowTouch) return;
		Game10_Manager.instance.isAllowTouch = true;
		for(int i = 0; i < infoTxt.Length; i++){
			infoTxt[i].SetActive(true);
		}
	}

//	void ShowGameResult(){
//		this.gameObject.SetActive(false);
//		Game10_Manager.instance.GameOver();
//	}
}
