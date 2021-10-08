using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Question : MonoBehaviour {
	public int 	 Id;
	public int   AppearProbality;
	public int   ReducePercent;
	public Swipe Direction;
	public bool  IsSpecial;
	public bool  IsSheeCool;
	public Sprite Answer;
	public Sprite Action;
	public Sprite Open;

	private Image m_MyImg;

	void Awake(){
		m_MyImg = GetComponent<Image>();

	}

	public void SetImage(){
		if (CheatController.GetFirstMatchCheat()!=null && IsSheeCool) {
			if(QuestionManager.GetInstance().OtherSheeCoolAnswers!=null && QuestionManager.GetInstance().OtherSheeCoolAnswers.Length > 0){

				int percentage = Random.Range (0,QuestionManager.GetInstance().totalPercentage);
				int index = 0;
				for(int i=0;i<QuestionManager.GetInstance().cheatImages.Count;i++){
					if(QuestionManager.GetInstance().cheatImages[i].percentage > percentage){
						index = i;
						break;
					}
				}
				Answer = QuestionManager.GetInstance().OtherSheeCoolAnswers [index];
				if(QuestionManager.GetInstance().OtherSheeCoolOpens.Length > index)
					Open = QuestionManager.GetInstance().OtherSheeCoolOpens[index];
			}
		}
		m_MyImg.sprite = Answer;
		m_MyImg.enabled = true;
		m_MyImg.SetNativeSize ();
	}

	public void HideQuestion(){
		GetComponent<Image>().enabled = false;
	}

	public void ReduceAppearProbality(){
		AppearProbality -= AppearProbality * ReducePercent / 100;
	}
}
