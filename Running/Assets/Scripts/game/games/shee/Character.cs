using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Character : MonoBehaviour {
	public  Sprite[] failAction;
	public  Sprite Normal;
	public 	Sprite NoReaction;
	public	Image  item;
	public	Image  icream;

	private Image m_MyImage;
	private int m_IndexFail = 0;
	private float m_DelayTimeFailAction = 0.3f;

	void Start () {
		m_MyImage = GetComponent<Image>();
	}
	
	public void ActionFail(){
		item.enabled = false;
		icream.enabled = false;
		m_MyImage.sprite = failAction[m_IndexFail++];
		if(m_IndexFail >= failAction.Length){
			m_IndexFail = 0;
		}
		Invoke("ActionFail", m_DelayTimeFailAction);
	}

	public void ResetAction(){
		item.enabled = false;
		icream.enabled = true;
		m_MyImage.sprite = Normal;
		CancelInvoke("ActionFail");
	}

	public void ChangeAction(Sprite action){
		m_MyImage.sprite = action;
	}

	public void Open(Sprite icon){
		m_MyImage.sprite = NoReaction;
		item.sprite = icon;
		item.enabled = true;
		item.SetNativeSize();
	}
}
