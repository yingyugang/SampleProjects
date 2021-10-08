using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class GameOver_Controller : MonoBehaviour {
	//-->Delegate
	public delegate float HighScoreDelegate(float highscore, float totalscore, Text tg);
	public delegate void EXPBarDelegate(float currentEXP, float playerEXP, Image expBar, Text lvl);
	//--<Delegate
	private void EXP_SubmitServer(){
		//----->API feature
		//For test - PlayerPrefs "PlayerEXP_Server" is a server
		PlayerPrefs.SetFloat(BikingKey.SSTring.PlayerEXP_Server, Global_GameOver.instance.GameOver_Element.game_exp);
		//-----<API feature
	}
	private float EXP_GetFromServer(){
		//----->API feature
		//For test - PlayerPrefs "PlayerEXP_Server" is a server
		return PlayerPrefs.GetFloat(BikingKey.SSTring.PlayerEXP_Server);
		//-----<API feature
	}

	//-->EXP Bar
	public void EXPBarStateDel(EXPBarDelegate EXPBardel){
		EXPBardel(Global_GameOver.instance.GameOver_Element.game_exp, Global_GameOver.instance.GameOver_Element.player_exp, Global_GameOver.instance.ExpBar,
			Global_GameOver.instance.TxtExpLvl);
	}
	public void EXPBarStateCal(float gameEXP, float playerEXP, Image expBar, Text lvl){
		//Render exp bar
		float _expUp = 0;
		float _expCurrent = 0;
		//-->Set exp bar state
		for(int i = 0; i < Global_GameOver.instance.GameOver_Element.ExpLevelTarget.Length; i++){
			if(playerEXP <= Global_GameOver.instance.GameOver_Element.ExpLevelTarget[i]){
				lvl.text = "LV " + (i+1);
				PlayerPrefs.SetInt(BikingKey.SSTring.PlayerCurrentLvl, i+1);
				if(i == 0){
					_expUp = Global_GameOver.instance.GameOver_Element.ExpLevelTarget[i];
					_expCurrent = playerEXP;
				} else{
					_expUp = Global_GameOver.instance.GameOver_Element.ExpLevelTarget[i] - Global_GameOver.instance.GameOver_Element.ExpLevelTarget[i-1];
					_expCurrent = playerEXP - Global_GameOver.instance.GameOver_Element.ExpLevelTarget[i-1];
				}
				break;
			}
		}
		float _p = _expCurrent/_expUp;
		expBar.fillAmount = _p;
		//--<Set exp bar state

		//-->Start bar progress recursive
		float _q = gameEXP/_expUp;
		IEnumerator c = Global_GameOver.instance.GlobalGuiEnt.BarProgress(expBar, _q, _p, 0.40f, "");
		BarProgressAnim(c);
		//--<Start bar progress recursive
	}
	private void BarProgressAnim(IEnumerator imgDel){
		IEnumerator c = imgDel;
		Global_GameOver.instance.bCoroutine.Add(c);
		StartCoroutine(c);
	}
	//--<Bar

	//-->High score cal
	public float HighScoreDel(GameOver_Controller.HighScoreDelegate  HighScoreDel) {
		return HighScoreDel(Global_GameOver.instance.GameOver_Element.highScore, Global_GameOver.instance.GameOver_Element.totalScore,
			Global_GameOver.instance.TxtMoreHighScore);
	}
	public float HighScoreCal(float highscore, float totalscore, Text tg){
		if(totalscore <= highscore) return 0;
		else{
			Global_GameOver.instance.GameOver_Element.moreHighScore = totalscore;
			return totalscore;
		}
	}
	//--<High score cal

	//-->Total score cal
	public float TotalScore(float score, float itemScore, float comboScore, Text tg) {
		float total = score + itemScore + comboScore;
		Global_GameOver.instance.GameOver_Element.totalScore = total;
		return total;
	}
	//--<Total score cal

	//--<Ranking cal

	public string Ranking(float points) {
		for(int i = 0; i < Global_GameOver.instance.GameOver_Element.rankPoint.Count; i++){
			if(points <= Global_GameOver.instance.GameOver_Element.rankPoint[i]) return Global_GameOver.instance.GameOver_Element.rankSign[i];
			if(points > Global_GameOver.instance.GameOver_Element.rankPoint[i] && i == Global_GameOver.instance.GameOver_Element.rankPoint.Count-1)
				return Global_GameOver.instance.GameOver_Element.rankSign[i+1];
		}
		return "";
	}
	//--<Ranking cal
}
