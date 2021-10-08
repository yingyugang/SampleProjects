using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QuestionManager : MonoBehaviour {

	//List Question
	public Question[] 	List_Question ;
	public Image 		Mask;
	private Question m_Question;
	private int 	 m_IndexFever;
	private int[]    m_Percentage;
	private int 	 m_TotalProbality;
	private int[]	 m_SquenceFever = new int[4]{1,2,3,4};

	public Sprite[] OtherSheeCoolAnswers;//for cheat
	public Sprite[] OtherSheeCoolOpens;//for cheat
	public List<CheatImage> cheatImages;
	public int totalPercentage;

	public static QuestionManager GetInstance(){
		return instance;
	}

	static QuestionManager instance;

	Dictionary<string,Sprite> mCheatDictionary;

	void Awake(){
		instance = this;
		m_IndexFever = 0;
		m_Question = List_Question[0];
		SetPercentage();
	}

	void Start(){
		if(CheatController.GetFirstMatchCheat()!=null){
			LoadCheat ();
		}
	}

	void LoadCheat(){
		StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> ("shee_update_cheat", (List<Texture2D> list) => {
			mCheatDictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (list);
			CombineCheat();
		}));
	}

	void CombineCheat(){
		List<Sprite> uppers = new List<Sprite> ();
		List<Sprite> unders = new List<Sprite> ();
		for(int i=0;i<cheatImages.Count;i++){
			if (mCheatDictionary.ContainsKey (cheatImages [i].under) && mCheatDictionary.ContainsKey (cheatImages [i].upper)) {
				uppers.Add (mCheatDictionary [cheatImages [i].upper]);
				unders.Add (mCheatDictionary [cheatImages [i].under]);
			} else {
				cheatImages.RemoveAt (i);
				i--;
			}
		}
		totalPercentage = 0;
		for(int i =0;i<cheatImages.Count;i++){
			int percent = cheatImages [i].percentage;
			cheatImages [i].percentage += totalPercentage;
			totalPercentage += percent;
		}
		OtherSheeCoolAnswers = uppers.ToArray ();
		OtherSheeCoolOpens = unders.ToArray ();
	}

	void SetPercentage(){
		m_Percentage = new int[List_Question.Length];
		m_Percentage[0] = List_Question[0].AppearProbality;
		for(int i = 1; i < m_Percentage.Length; i++){
			m_Percentage[i] = m_Percentage[i-1] + List_Question[i].AppearProbality;
		}
		m_TotalProbality = m_Percentage[m_Percentage.Length - 1];

	}

	int Random_Quesiton(){		
		float rand = Random.Range(0, m_TotalProbality);
		for(int i = 0; i < m_Percentage.Length; i++){
			if(m_Percentage[i] > rand) return i;
		}
		return 0;
	}

	public void Get_Question(bool isFever){
		Mask.enabled = false;
		if(isFever){
			Fever_Question();
		}else{
			m_IndexFever = 0;
			Normal_Question();
		}
		m_Question.SetImage();
	}

	public Question GetCurrentQuestion(){
		return m_Question;
	}

	void Fever_Question(){
		int id = m_SquenceFever[m_IndexFever++];
		if(id == 4) id = Random.Range(4, 7);
		m_Question = GetQuestionById(id);
		if(m_Question == null) m_Question = List_Question[Random_Quesiton()];
		if(m_IndexFever == m_SquenceFever.Length){
			m_IndexFever = 0;
		}
	}

	public void RandomFeverQuestion(){
		if (Random.Range (0, 2) == 0) {
			m_SquenceFever = new int[4]{1,2,3,4};
		} else {
			m_SquenceFever = new int[4]{3,2,1,4};
		}
	}

	void Normal_Question(){
		m_Question = List_Question[Random_Quesiton()];
	}

	public bool Get_Answer(Swipe sw){
		if(sw == m_Question.Direction){
			return true;
		}
		return false;
	}

	public bool Check_Akatsuka(){
		if(m_Question.IsSpecial){
			m_Question.ReduceAppearProbality();
			SetPercentage();
			return true;
		}
		return false;
	}

	public bool Check_Handsome(){
		if(m_Question.IsSheeCool){
			return true;
		}
		return false;
	}

	public void Hide_Question(){
		Mask.enabled = true;
		if(m_Question != null) m_Question.HideQuestion();
	}

	Question GetQuestionById(int id){
		foreach(Question question in List_Question){
			if(question.Id == id){
				return question;
			}
		}
		return null;
	}
}
