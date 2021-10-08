using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Introduce : MonoBehaviour {
	public Sprite[] Openning;
	public Image BGImg;

	private int m_Index;
	private Animator m_Anim;
	// Use this for initialization
	void Awake () {
		m_Anim = GetComponent<Animator>();	
		Init();
	}

	void Init(){
		m_Index = 0;
		m_Anim.CrossFade("Idle", 0f);
	}

	public void PlayIntro(){
		m_Anim.SetTrigger("next");
	}

	public void SkipIntro(){
		NextScreen();
	}

	void SetBG(){
		if(m_Index == Openning.Length){
			NextScreen();
		}
		BGImg.sprite = Openning[m_Index++];
	}

	void NextScreen(){
		m_Index = 0;
		m_Anim.SetTrigger("next");
		Game10_Manager.instance.PlayScreen.IsStartPlay = true;
		Debug.Log("Click");
		Game10_Manager.instance.NextScreen();
	}
}
