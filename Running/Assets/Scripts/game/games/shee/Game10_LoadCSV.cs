using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game10_LoadCSV : MonoBehaviour {
	public QuestionManager Questions;
	private Ranking[] m_SheeRanking;
	public CheatImage[] cheatImages;
	// Use this for initialization
	void Awake () {
		LoadCSV();
		LoadRanking();
		LoadParams();
		LoadSheeCheatImages ();
	}

	void LoadSheeCheatImages(){
		List<Dictionary<string,object>> data = CSVReader.Read (SheeResources.GameInfo.CSV_CHEATIMAGES);
		cheatImages = new CheatImage[data.Count];
		int percent;
		int totalPercent = 0;
		Questions.cheatImages = new List<CheatImage> ();
		CheatData cd = CheatController.GetFirstMatchCheat ();
		if (cd != null) {
			for(int i=0; i < data.Count; i++) {
				Debug.Log (data[i].Count);
				CheatImage cheatImage = new CheatImage ();
				cheatImage.id = (int)data[i]["id"];
				cheatImage.upper = ((string)data[i]["upper"]).Trim();
				cheatImage.under = ((string)data[i]["under"]).Trim();
				cheatImage.percentage = (int)data[i]["percentage"];
				cheatImage.key = data[i]["key"].ToString().Trim();
				if (cd.key.Trim() == cheatImage.key.Trim()) {
					Questions.cheatImages.Add(cheatImage);
				}
			}
		}
	}

	void LoadCSV(){
		List<Dictionary<string,object>> dataCharacter = CSVReader.Read (SheeResources.GameInfo.CSV_CHARACTER);

		for(int i=0; i < dataCharacter.Count; i++) {
			Questions.List_Question[i].Id = (int)dataCharacter[i]["id"];
			Questions.List_Question[i].Direction = (Swipe)dataCharacter[i]["direction"];
			Questions.List_Question[i].IsSpecial = (int)dataCharacter[i]["is_fever_character"] > 0;
			Questions.List_Question[i].AppearProbality = (int)dataCharacter[i]["appear_probality"];
			Questions.List_Question[i].ReducePercent = (int)dataCharacter[i]["reduction_percentage"];
		}
	}

	void LoadParams(){
		if(APIInformation.GetInstance == null) return;
		Game10_Manager.instance.GameTime = APIInformation.GetInstance.gameparameter.game_time;
		Game10_Manager.instance.FlickSensitivity = APIInformation.GetInstance.gameparameter.flick_sensitivity;
		Game10_Manager.instance.CombSpan = APIInformation.GetInstance.gameparameter.comb_span_second;
		Game10_Manager.instance.FeverTime = APIInformation.GetInstance.gameparameter.fever_time;
		Game10_Manager.instance.PlayScreen.FailTime = APIInformation.GetInstance.gameparameter.freeze_time4miss;
		Game10_Manager.instance.combvar1 = APIInformation.GetInstance.gameparameter.comb_var1;
		Game10_Manager.instance.combvar2 = APIInformation.GetInstance.gameparameter.comb_var2;
		Game10_Manager.instance.combvar3 = APIInformation.GetInstance.gameparameter.comb_var3;
	}

	void LoadRanking(){
		m_SheeRanking = new Ranking[8];
		int index = 0;
		List<Dictionary<string,object>> dataCharacter = CSVReader.Read (SheeResources.GameInfo.CSV_RANK);

		for(int i=0; i < dataCharacter.Count; i++) {
			int id = (int)dataCharacter[i]["id"];
			string rank = (string)dataCharacter[i]["rank"];
			int min = (int)dataCharacter[i]["min"];
			int max = (int)dataCharacter[i]["max"];
			if(index < m_SheeRanking.Length)	m_SheeRanking[index++] = new Ranking(id, rank, min, max);
		}
	}

	public int GetRanking(int score){
		foreach(Ranking rank in m_SheeRanking){
			if(rank.CheckRanking(score) > 0){
				return rank.CheckRanking(score);
			}				
		}
		return 1;
	}

	class Ranking{
		private int m_Id;
		private string m_Rank;
		private int m_Min;
		private int m_Max;

		public Ranking(int id, string rank, int min, int max){
			m_Id = id;
			m_Rank = rank;
			m_Min = min;
			m_Max = max;
		}

		public int CheckRanking(int score){
			if( (score >= m_Min && score <= m_Max) || (m_Max == 0 && score >= m_Min) ) 	return m_Id;
			return -1;
		}
	}
}

public class CheatImage{
	public int id;
	public string upper;
	public string under;
	public int percentage;
	public string key;
}