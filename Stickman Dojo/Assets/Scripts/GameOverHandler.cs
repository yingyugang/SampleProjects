using UnityEngine;
using System.Collections;

public class GameOverHandler : MonoBehaviour {
	Commands Player;
	GuiHandler GuiH;
	BotAi[] Bots;
	bool GameIsOver;

	Vector3 bot1StartPos;
	Vector3 bot2StartPos;
	Vector3 playerStartPos;


	public float ShowBannerAfterXSeconds = 5f; // for better performance is good to show banner after some time
	float bannerCoold;

	AdNetworks adNetworks;



	// Use this for initialization
	void Start () {
		adNetworks = FindObjectOfType<AdNetworks>();      // THIS IS FOR AdNetworks!

		GuiH = FindObjectOfType<GuiHandler> ();
		Player = FindObjectOfType<Commands> ();
		Bots = FindObjectsOfType<BotAi> ();

		playerStartPos = Player.transform.position;
		bot1StartPos = Bots [0].transform.position;
		bot2StartPos = Bots [1].transform.position;


	
	}
	
	// Update is called once per frame
	public void GameOver(){
		GameIsOver = true;
		Player.gameObject.SetActive (false);
		foreach (BotAi bot in Bots) {
			bot.enabled = false;
			bot.GetComponent<Animator>().enabled= false;
		}
		GuiH.ShowGameOverComplex ();
		adNetworks.HideBanner ();   // THIS IS FOR BANNERS they get hidden when the game is over and re-appear when you restart


	}

	void Update(){
		if (GameIsOver == false){
			if (ShowBannerAfterXSeconds>bannerCoold){
				bannerCoold+=Time.deltaTime;
				if (ShowBannerAfterXSeconds<bannerCoold){            // THIS IS FOR BANNERS (It will occur once and stay loaded) AdNetworks 
					Debug.Log ("Showing banner");          
					adNetworks.ShowBanner();        //  showing the banner
				}
			}

		}


	}
	public void RestartGame(){
		adNetworks.ShowInterstitial ();            // THIS IS FOR INTERSTITIAL! AdNetworks
		GameIsOver = false; 
		GuiH.ShowScore();
		RestartEngine ();
		adNetworks.ShowBanner ();   //  re-showing the banners
	}


	 void RestartEngine(){
		GuiH.Score = 0;
		Player.gameObject.SetActive (true);
		foreach (BotAi bot in Bots) {
			bot.enabled = true;

			bot.GetComponent<Animator>().enabled= true;
		}
		// Clean The room
		GameObject[] Cleanable = GameObject.FindGameObjectsWithTag ("Cleanable");
		foreach (GameObject clean in Cleanable) {
			Destroy(clean);
		}
		
		Player.transform.position = playerStartPos;
		Bots [0].transform.position = bot1StartPos;
		Bots [1].transform.position = bot2StartPos;


	}



}
