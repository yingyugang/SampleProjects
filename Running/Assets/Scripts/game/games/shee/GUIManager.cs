using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : MonoBehaviour {

	private static GUIManager _instance;
	public static GUIManager instance{
		get{
			if(_instance == null) _instance = GameObject.FindObjectOfType<GUIManager>();
			return _instance;
		}
	}

//	public Text Score;
//	public Text CountDown;
	public Text Combos;
	public Text MaxCombo;
	public GameObject InfoCombo;

	public GameObject UIFooter;
	public GameObject EffectImg;
	public Sprite feverSye;
	public Sprite other;

	void Start(){
		Header.Instance.SetScore("0回");
		MaxCombo.text = "0";
		Combos.text = "0";
	}

	public void Start_Btn_Click(){
		Game10_Manager.instance.Start_Game();
	}

	public void Skip_Btn_Click(){
		Game10_Manager.instance.Skip_Introduce();
	}


	public void Home(){
		Global_GameOver.instance.GoHome("Home");
	}

	public void OpenCutinEffect(Sprite sp){
		EffectImg.transform.GetChild(0).GetComponent<Image>().sprite = sp;
		EffectImg.SetActive(true);
		EffectImg.GetComponent<Animator>().Play(SheeResources.GameInfo.Anim_CUTIN, -1, 0f);
		Invoke("CloseCutinEffect", EffectImg.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
	}

	public void CloseCutinEffect(){
		EffectImg.SetActive(false);
		Game10_Manager.instance.PlayScreen.Waiting_Question();
	}

	public void SetCombo(int count){
		if(count <= 1) return;
		Debug.Log("Show COmbo");
		Combos.text = count + "";
		InfoCombo.SetActive(false);
		InfoCombo.SetActive(true);
	}
}
