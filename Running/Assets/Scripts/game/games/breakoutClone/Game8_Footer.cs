using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game8_Footer : MonoBehaviour
{
	// Use this for initialization


	public Text textScore1;
	public Text textScore2;
	public Text textScore3;
	public Text textStage;
	private static Game8_Footer _instance;

	public static Game8_Footer Instance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<Game8_Footer> ();
			return _instance;
		}
	}


	public void SetNumberScore2Text (int _itemscore)
	{
		if (_itemscore > BreackoutConfig.MAXITEM) {
			textScore2.text = BreackoutConfig.MAXITEM.ToString();
		}
		else 
			textScore2.text = _itemscore.ToString ();
	}

	public void SetNumberScore1Text (int _itemscore)
	{
		if (_itemscore > BreackoutConfig.MAXITEM) {
			textScore1.text = BreackoutConfig.MAXITEM.ToString();
		}
		else 
			textScore1.text = _itemscore.ToString ();
	}

	public void SetNumberScore3Text (int _itemscore)
	{
		if (_itemscore > BreackoutConfig.MAXITEM) {
			textScore3.text = BreackoutConfig.MAXITEM.ToString();
		}
		else 
			textScore3.text = _itemscore.ToString ();
	}

	public void SetStageFooter (int _stage)
	{
		textStage.text = "ステージ " + _stage.ToString ();
	}


}
