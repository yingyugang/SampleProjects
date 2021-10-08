using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game12_GUI_Manager : MonoBehaviour
{
	public delegate void OnGameOverDelegate ();

	private event OnGameOverDelegate OnGameOverEvent;

	public delegate void OnStartGameDelegate ();

	private event OnStartGameDelegate OnStartGameEvent;

	public static Game12_GUI_Manager _instance;

	public static Game12_GUI_Manager instance {
		get {
//			if (_instance == null)
//				_instance = GameObject.FindObjectOfType<Game12_GUI_Manager> ();
			if (_instance == null)
				Debug.Log ("Game12_GUI_Manager is null !!");
			return _instance;
		}
	}

	bool isAnimeSkip;
	public Game12_GameIntro GameIntro;
	public GameObject restart_Button;
	public GameObject startGame_Panel;
	public GameObject TouchArea;
	public Header G_Header;
	public Game12_Footer G_Footer;
	public Image[] Lifes;
	public Image ghost;
	public Image shadow;
	public Text deathCoutdown;
	public Image StartWhistle;
	private bool m_IsWhistle;
	public Image pauseBtn;
	IEnumerator countdown;

	void Update ()
	{
		if (!GameCountDownMediator.didEndCountDown)
			return;
		if (Game12_GameManager.instance.myState == Game12_GameState.start) {
			Game12_GUI_Manager.instance.TouchArea.SetActive (true);
			G_Header.gameObject.SetActive (true);
			G_Footer.gameObject.SetActive (true);
			GameIntro.Opening.SetActive (false);
			GameIntro.gameObject.SetActive (false);
			Game12_GameManager.instance.myState = Game12_GameState.playing;
		}

	}

	public void Listener ()
	{
		UnListener();
		Game12_Player_RunningEvent.instance.AddOnstacleCollisionListener (GUI_Obstacle_Listener);
	}

	public void UnListener ()
	{
		Game12_Player_RunningEvent.instance.RemoveOnOnstacleCollisionListener (GUI_Obstacle_Listener);
	}

	void Awake ()
	{
		_instance = this;

		PlayerPrefs.SetInt (BikingKey.ImageResources.item1, 0);
		PlayerPrefs.SetInt (BikingKey.ImageResources.item2, 0);
		PlayerPrefs.SetInt (BikingKey.ImageResources.item3, 0);
	}

	void Start ()
	{
		Listener ();
		Header.Instance.SetOnPauseCallback (PauseGame);
		PauseManager.s_Instance.Init (6, Game12_GameManager.instance.audio);
		PauseManager.s_Instance.SetOnBackToGameCallback (ResumeGame);

	}

	public void PlaySound (SoundEnum soundEnum)
	{
		if (ComponentConstant.SOUND_MANAGER != null) {
			ComponentConstant.SOUND_MANAGER.Play (soundEnum);
		}
	}

	public void OnStartGameListener (OnStartGameDelegate callback)
	{
		OnStartGameEvent += callback;
	}

	public void RemoveStartGameListener (OnStartGameDelegate callback)
	{
		OnStartGameEvent -= callback;
	}

	public void OnGameOverListener (OnGameOverDelegate callback)
	{
		OnGameOverEvent += callback;
	}

	public void RemoveGameOverListener (OnGameOverDelegate callback)
	{
		OnGameOverEvent -= callback;
	}

	public void Screen_Touch ()
	{
		Game12_Player_Controller.instance.JumpCal ();
	}

	public void PauseGame ()
	{
		TouchArea.SetActive (false);
		Game12_GameManager.instance.myState = Game12_GameState.pause;
		Game12_Player_Controller.instance.Player.isKinematic = true;
		PauseManager.s_Instance.SetVisible (true);
	}

	public void ResumeGame ()
	{
		TouchArea.SetActive (true);
		Game12_Player_Controller.instance.Player.isKinematic = false;
		Game12_GameManager.instance.myState = Game12_GameState.start;
	}

	public void StartGame_OnClick ()
	{
//		isAnimeSkip = true;
//		startGame_Panel.SetActive(false);
//		OnStartGameEvent();
	}

	public void ShowGhost (bool isShow)
	{
		ghost.gameObject.SetActive (!isShow);
	}

	public void PlayerStopOnObstacle (bool isAlive, int itemID = -1, string itemName = "")
	{
		if (_instance == null)
			return;
		// Shut down area touch
		if (!TouchArea)
			return;
		TouchArea.SetActive (false);
		// Turn on shadow
		m_IsWhistle = true;
		StartCoroutine (ShadowAppear (itemID, shadow, true));
		// Sound countdown
		//countdown = SoundToneCountDown ();
	}
	//--> Listener on Obstacle trigger detected from GUI
	public void GUI_Obstacle_Listener (GameObject obj, int itemID, string name, string imageResources, int itemType, float effect_Value1,
	                                   float effect_Value2, int percentage, int percentageReduction, string desciption)
	{
//		GUI_Obstacle ();
	}

	public void GUI_Obstacle_Listener ()
	{
//		Debug.Log ("GUI_Obstacle_Listener");
		GUI_Obstacle ();
	}

	void GUI_Obstacle ()
	{
		if (this == null)
			return;
		Debug.Log ("GUI_Obstacle");
		if (Game12_Player_Controller.life_player == 0) {
			//game over
			TouchArea.SetActive (false);
			Invoke ("FireGameOverEvent", 2.5f);
			// Play sound fx
			//Invoke ("WhitlseOnGameOver", 0.7f);
			StartCoroutine (FadeOut (BikingKey.GameConfig.GameOverID, shadow, true));
		} else {
			PlayerStopOnObstacle (true);
		}
		if (Game12_Player_Controller.life_player < 2) { //for debug player
			Game12_Player_Controller.instance.PlayerAvatar_Deadth (Lifes [Game12_Player_Controller.instance.playerInfo.GetID () - 1], false);
			// Shaking life icon
			StartCoroutine (ObjShaking (Lifes [Game12_Player_Controller.instance.playerInfo.GetID () - 1].transform));
		}
		Game12_GameManager.instance.myState = Game12_GameState.pause;
	}

	void FireGameOverEvent ()
	{
		shadow.gameObject.SetActive (false);
		OnGameOverEvent ();
	}

	void WhitlseOnGameOver ()
	{
		PlaySound (SoundEnum.se07_timeup);
	}
	//--< Listener on Obstacle trigger detected from GUI
	public IEnumerator FadeOut (int itemID, Image sprite, bool isFadeOut)
	{
		Debug.Log ("FadeOut");
		Color clr = sprite.color;
		clr.a = 0f;
		sprite.color = clr;
		sprite.gameObject.SetActive (true);
		Game12_GUI_Manager.instance.pauseBtn.raycastTarget = false;
		yield return new WaitForSeconds (1.0f);
		PlaySound (SoundEnum.se07_timeup);
		while (clr.a < 0.7f) {
			clr.a += 0.01f;
			sprite.color = clr;
			yield return new WaitForSeconds (0.01f);
		}
	}

	public IEnumerator ShadowAppear (int itemID, Image sprite, bool isFadeOut)
	{
		Color clr = sprite.color;
		clr.a = 0f;
		sprite.color = clr;
		shadow.gameObject.SetActive (true);

		int duration = 4;
		Game12_GUI_Manager.instance.pauseBtn.raycastTarget = false;
		if (itemID == 0)
			yield return new WaitForSeconds (0.7f);
		//else
		//	yield return new WaitForSeconds (1.0f);
		while (clr.a < 0.7f) {
			clr.a += 0.02f;
			sprite.color = clr;
			yield return new WaitForSeconds (0.01f);
		}


		//Color clr = sprite.color;
		//StartCoroutine (countdown);
		while (duration > 0) {
			duration--;
			if (itemID != BikingKey.GameConfig.GameOverID) {
				if (duration > 0) {
					deathCoutdown.text = duration.ToString ();
					PlaySound (SoundEnum.se04_start_countdown);
					yield return new WaitForSeconds (1.0f);
				}
				else {
					deathCoutdown.text = "";
					m_IsWhistle = false;
					PlaySound (SoundEnum.se05_start);
					StartWhistle.gameObject.SetActive (true);
					//yield return new WaitForSeconds (0.5f);
				}
			}
		}

		while (clr.a > 0f) {
			clr.a -= 0.02f;
			if (clr.a < 0.4f)
				StartWhistle.gameObject.SetActive (false);
			sprite.color = clr;
			yield return new WaitForSeconds (0.01f);
		}
		DeathCoutdownFinish (itemID);
		//yield return new WaitForSeconds (0.1f);
		Game12_Player_Controller.instance.PlayerRebirth ();
	}


	public IEnumerator SoundToneCountDown ()
	{
		while (m_IsWhistle) {
			// Play sound fx
			PlaySound (SoundEnum.se04_start_countdown);
			yield return new WaitForSeconds (1.0f);
		}
		StartWhistle.gameObject.SetActive (true);
		// Play sound fx
		PlaySound (SoundEnum.se05_start);
	}

	void DeathCoutdownFinish (int playerID)
	{
		//shadow.color = new Color32 (0, 0, 0, 0);
		//shadow.gameObject.SetActive (true);
		//Enable PauseBtn
		shadow.gameObject.SetActive (false);
		Game12_GUI_Manager.instance.pauseBtn.raycastTarget = true;
		shadow.color = new Color32 (0, 0, 0, 180);
		deathCoutdown.text = "";
	}

	public void Restart_Game ()
	{
		UnListener();
		Game12_Player_Controller.instance.UnListener();
		Game12_GameManager.instance.UnListener();
		Game12_GameManager.instance.ItemUnListener();
		RemoveStartGameListener (GameIntro.OnStartGameListener);
	}

	public IEnumerator ObjShaking (Transform tf)
	{
		float shake = 2.0f;
		float amount = 18.30f;
		float df = 0.3f;
		while (shake > 0) {
			tf.localPosition = Random.insideUnitSphere * amount;
			shake -= .5f * df;
			yield return 0;
		}
	}
}
