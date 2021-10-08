using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GuiHandler : MonoBehaviour {
	public Text scoreCounter;
	public Text scoreCounterShadow;
	public GameObject scoreComplex;
	public GameObject gameOverComplex;

	public int Score;
	public int HighScore;
	public Text ScoreText;
	public Text HighscoreText;

	bool ShowGameOverMenu;
	float menuLerp;
	float yOriginalPos;
	float lerpSpeed = 2f;

	// Use this for initialization
	void Start () {
		yOriginalPos = gameOverComplex.transform.position.y;
		gameOverComplex.SetActive (false);
		HighScore = PlayerPrefs.GetInt ("HighScore");
	}
	
	// Update is called once per frame
	void Update () {
		if (Score > HighScore) {
			HighScore=Score;
			PlayerPrefs.SetInt("HighScore",HighScore); // save highscore everytime it's > than the stored
		}

		scoreCounter.text = Score.ToString ();
		scoreCounterShadow.text = scoreCounter.text;

		if (ShowGameOverMenu) {
		Vector3 menuPos = gameOverComplex.transform.position;

			menuLerp+= Time.deltaTime*lerpSpeed;
			menuLerp = Mathf.Clamp(menuLerp,0,1);

			menuPos.y = Mathf.Lerp(500,yOriginalPos,menuLerp);
			gameOverComplex.transform.position = menuPos;


		}




	}

	public void ShowScore(){                  // This will be called at the restart of the game (after gameOver)
		scoreComplex.SetActive (true);
		gameOverComplex.SetActive (false);
		ShowGameOverMenu = false;
		menuLerp = 0;

	}
	public void ShowGameOverComplex(){         // The message that appears when the Game is Over!

		scoreComplex.SetActive (false);
		gameOverComplex.SetActive (true);
		ScoreText.text = Score.ToString ();
		HighscoreText.text = HighScore.ToString ();
		ShowGameOverMenu = true;
	
	}





}
