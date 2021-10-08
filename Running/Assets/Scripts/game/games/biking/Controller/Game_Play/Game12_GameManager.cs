using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public enum Game12_GameState
{
	intro,
	start,
	playing,
	pause,
	gameover,
	restart,
	replay,
	none
}

public class Game12_GameManager : MonoBehaviour
{
	public static Game12_GameManager _instance;

	public static Game12_GameManager instance {
		get {
			//if(_instance == null) _instance = GameObject.FindObjectOfType<Game12_GameManager>();
			if (_instance == null)
				Debug.Log ("Game12_GameManager is null");
			return _instance;
		}
	}

	[HideInInspector]public float startSpeed = .0f;
	[HideInInspector]public int multiJump = 2;
	[HideInInspector]public int PlayerPosID = 0;
	public float DistancePerPatern = 300f;
	[HideInInspector]public float Distance = .0f;
	public AudioSource audio;
	private AudioSource m_Audio;
	public bool Ending;
	bool m_playerBG = true;
	public Game12_GameState myState;
	public GameEndLogic GameEndLogic;
	public GameParameter GameParameter;
	float cardBonus;
	void Update ()
	{
		if (!GameCountDownMediator.didEndCountDown) {
			//Disable PauseBtn
			Game12_GUI_Manager.instance.pauseBtn.raycastTarget = false;

			return;
		}
		// Play biking background music
		if (m_playerBG) {
			//Enable PauseBtn
			Game12_GUI_Manager.instance.pauseBtn.raycastTarget = true;

			if (ComponentConstant.SOUND_MANAGER != null) {
				if(mCheatData!=null && mCheatData.key.Trim() != "")
					ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm01_title, GetAudioSource);
				else
					ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm09_bike, GetAudioSource);
				m_playerBG = false;
			}
		}

		if (myState == Game12_GameState.playing) {
			while (startSpeed < BikingKey.GameConfig.timeToMaxSpeed)
				startSpeed += Time.deltaTime;
			Distance = Game12_Player_Controller.instance.Player.position.x / 125f * DistancePerPatern;//Terrain_Controller.instance.pattern_width_group * DistancePerPatern;
			if (Distance >= 0) {
				Game12_GUI_Manager.instance.G_Header.SetScore (Distance.ToString ("F0"));
			}
		}
		cardBonus = GetCardBonus ();
	}

	float GetCardBonus(){
		//CardRate.GetTotal(2,BreackoutConfig.GAME_ID);
		return CardRate.GetTotal(2,6);
	}

	void Awake ()
	{

		BikingPhysicalMatrix(true);
		LoadGameParameter();
		Distance = -DistancePerPatern;
		_instance = this;
		if (ComponentConstant.SOUND_MANAGER != null) {
			if (CheatController.GetLastMatchCheat () != null) {
				ComponentConstant.SOUND_MANAGER.StoreSounds (new List<SoundEnum> () {
					SoundEnum.bgm01_title,
					SoundEnum.bgm10_invicible,
					SoundEnum.bgm15_title_back
				});
			} else {
				ComponentConstant.SOUND_MANAGER.StoreSounds (new List<SoundEnum> () {
					SoundEnum.bgm09_bike,
					SoundEnum.bgm10_invicible,
					SoundEnum.bgm15_title_back
				});
			}

		}

	}


	CheatData mCheatData;
	Dictionary<string,Sprite> mCheatDictionary;
	void SetCheat(){
		mCheatData = CheatController.GetLastMatchCheat ();
		if(mCheatData != null){
			//StartCoroutine(ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAssetBundle ("bike_cheat_sprite" , GetCheatResource<AssetBundle>));
			StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAllResourcesList<Texture2D> ("bike_cheat_sprite", (List<Texture2D> list) => {
				mCheatDictionary = TextureToSpriteConverter.ConvertToSpriteDictionary (list);
				GetCheatResource();
			}));
		}
	}

	void GetCheatResource (){
		string key = mCheatData.key.Trim ();
		if(key==null || key == ""){
			List<Sprite> cheatSprite = new List<Sprite> ();
			if(mCheatDictionary.ContainsKey("life_oso")){
				cheatSprite.Add (mCheatDictionary["life_oso"]);
			}
			if(mCheatDictionary.ContainsKey("life_choro")){
				cheatSprite.Add (mCheatDictionary["life_choro"]);
			}
			if(mCheatDictionary.ContainsKey("life_ichi")){
				cheatSprite.Add (mCheatDictionary["life_ichi"]);
			}
			if(mCheatDictionary.ContainsKey("life_jyushi")){
				cheatSprite.Add (mCheatDictionary["life_jyushi"]);
			}
			if(mCheatDictionary.ContainsKey("life_kara")){
				cheatSprite.Add (mCheatDictionary["life_kara"]);
			}
			if(mCheatDictionary.ContainsKey("life_todo")){
				cheatSprite.Add (mCheatDictionary["life_todo"]);
			}
			if(cheatSprite.Count>0){
				Sprite sprite = cheatSprite [Random.Range (0, cheatSprite.Count)];
				Texture2D txt = sprite.texture;
				Sprite s = Sprite.Create (txt, new Rect (0, 0, txt.width, txt.height), new Vector2 (0.5f, 0), 100, 0, SpriteMeshType.FullRect);
				Game12_Item_Life_Controller.instance.itemLife.lstLifeSprite [0] = s;
				cheatSprite.Remove (Game12_Item_Life_Controller.instance.itemLife.lstLifeSprite [0]);
			}
			if(cheatSprite.Count>0){
				Sprite sprite = cheatSprite [Random.Range (0, cheatSprite.Count)];
				Texture2D txt = sprite.texture;
				Sprite s = Sprite.Create (txt, new Rect (0, 0, txt.width, txt.height), new Vector2 (0.5f, 0), 100, 0, SpriteMeshType.FullRect);
				Game12_Item_Life_Controller.instance.itemLife.lstLifeSprite [1] = s;
			}
			return;
		}
		Dictionary<string,BikeCheatData> cheatDic = Game12_GameParams.instance.bikingCheatDic;
		if(cheatDic!=null && cheatDic.ContainsKey(key)){
			BikeCheatData bikeCheatData = cheatDic [key];
			if (mCheatDictionary.ContainsKey (bikeCheatData.headSprite)) {
				Game12_GUI_Manager.instance.Lifes [0].sprite = mCheatDictionary [bikeCheatData.headSprite];
				Game12_GUI_Manager.instance.Lifes [0].SetNativeSize ();
				Game12_GUI_Manager.instance.Lifes [0].transform.parent.localPosition -= new Vector3 (-11,9,0);
			}
			if(mCheatDictionary.ContainsKey(bikeCheatData.headSprite1)){
				Game12_GUI_Manager.instance.Lifes [1].sprite = mCheatDictionary [bikeCheatData.headSprite1];
				Game12_GUI_Manager.instance.Lifes [1].SetNativeSize ();
				Game12_GUI_Manager.instance.Lifes [1].transform.parent.localPosition -= new Vector3 (11,0,0);
			}
			if(mCheatDictionary.ContainsKey(bikeCheatData.lifeSprite)){
				Texture2D txt = mCheatDictionary [bikeCheatData.lifeSprite].texture;
				Sprite s = Sprite.Create (txt, new Rect (0, 0, txt.width, txt.height), new Vector2 (0.5f, 0), 100, 0, SpriteMeshType.FullRect);
				Game12_Item_Life_Controller.instance.itemLife.lstLifeSprite[0] = s;
			}
			if(mCheatDictionary.ContainsKey(bikeCheatData.lifeSprite1)){
				Texture2D txt = mCheatDictionary [bikeCheatData.lifeSprite1].texture;
				Sprite s = Sprite.Create (txt, new Rect (0, 0, txt.width, txt.height), new Vector2 (0.5f, 0), 100, 0, SpriteMeshType.FullRect);
				Game12_Item_Life_Controller.instance.itemLife.lstLifeSprite[1] = s;
			}
			if(mCheatDictionary.ContainsKey(bikeCheatData.bikeSprite)){
				Game12_Player_Controller.instance.Character_Sprite[0] = mCheatDictionary [bikeCheatData.bikeSprite];
				Game12_Player_Controller.instance.PlayerImage.sprite = mCheatDictionary [bikeCheatData.bikeSprite];
				if(Game12_Item_Life_Controller.instance.itemLife.GetComponent<BoxCollider2D> ()!=null)
					Game12_Item_Life_Controller.instance.itemLife.GetComponent<BoxCollider2D> ().offset = Vector2.zero;
			}
			if(mCheatDictionary.ContainsKey(bikeCheatData.bikeSprite1)){
				Game12_Player_Controller.instance.Character_Sprite[1] = mCheatDictionary [bikeCheatData.bikeSprite1];
			}
		}
	}

	void BikingPhysicalMatrix(bool ignore){
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("TransparentFX"), LayerMask.NameToLayer("TransparentFX"), ignore);
	}
	void LoadGameParameter(){
		if (APIInformation.GetInstance == null)
			return;
		Debug.Log ("Load param from server");
		GameParameter.total_comb_var = APIInformation.GetInstance.gameparameter.total_comb_var;
		GameParameter.flick_sensitivity = APIInformation.GetInstance.gameparameter.flick_sensitivity;
		GameParameter.player_lifes = APIInformation.GetInstance.gameparameter.player_lifes;
		GameParameter.item_appear_percentage = APIInformation.GetInstance.gameparameter.item_appear_percentage;
		GameParameter.start_distance = APIInformation.GetInstance.gameparameter.start_distance;

		Game12_Player_Controller.life_player = GameParameter.player_lifes;
		Debug.Log ("start_distance " + GameParameter.start_distance);
		Vector3 player_pos = Game12_Player_Controller.instance.Player.transform.position;
		player_pos.x = -(GameParameter.start_distance / DistancePerPatern * 125f) + 2.5f;
		Game12_Player_Controller.instance.Player.transform.position = player_pos;
	}
	public void Listener ()
	{
		Game12_Player_RunningEvent.instance.AddOnstacleCollisionListener (GameManager_Obstacle_Listener);
	}

	public void UnListener ()
	{
		Game12_Player_RunningEvent.instance.RemoveOnOnstacleCollisionListener (GameManager_Obstacle_Listener);
	}

	public void ItemListener ()
	{
		Debug.Log ("ItemListener");
		Game12_Player_RunningEvent.instance.AddItemCollisionListener (GameManager_Item_Listener);
	}

	public void ItemUnListener ()
	{
		Debug.Log ("ItemUnListener");
		Game12_Player_RunningEvent.instance.RemoveItemCollisionListener (GameManager_Item_Listener);
	}

	void Start ()
	{
		SetCheat ();
		myState = Game12_GameState.start;
		Listener ();
		ItemListener ();
		Init ();

	}

	void Init ()
	{
		if (GameResaultManager.Instance == null)
			return;
		GameResaultManager.Instance.SetImageHeaderPanelResault (6);
		GameResaultManager.Instance.SetLastLevel (UpdateInformation.GetInstance.player.lv,UpdateInformation.GetInstance.player.exp);
//		Header.Instance.popupCountDown.showOrHideBg += InitBackGroundSound; // Playing sound when ending countdown.
	}

	float m_OriginBgVolume;
	void InitBackGroundSound (bool value = false)
	{
		if (ComponentConstant.SOUND_MANAGER != null) {
			ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm06_oden, (audio) => {

				m_Audio = audio;
//				m_OriginBgVolume = audio.volume;
//				m_Audio.volume = VOLUME_BACKGROUND_SOUND;
				if (m_Audio != null) {
					PauseManager.s_Instance.Init (BreackoutConfig.GAME_ID, m_Audio);
				}
			}
			);
		}
	}

	public void GetAudioSource (AudioSource audio)
	{
		if (m_Audio == null || m_Audio != audio) {
			m_Audio = audio;
			PauseManager.s_Instance.Init (BikingKey.GameConfig.GameID, m_Audio);
		}
	}

	//--> Listener on Obstacle trigger detected from Game Manager
	public void GameManager_Obstacle_Listener (GameObject obj, int itemID, string name, string imageResources, int itemType, float effect_Value1,
	                                           float effect_Value2, int percentage, int percentageReduction, string desciption)
	{
		Debug.Log ("GameManager_Obstacle_Listener");
		GameManager_Obstacle ();
	}
	//--< Listener on Obstacle trigger detected from Game Manager

	public void GameManager_Obstacle_Listener ()
	{
		Debug.LogWarning ("GameManager_Obstacle_Listener 2");
		// Play sound fx
		GameManager_Obstacle ();
	}

	void GameManager_Obstacle ()
	{
		Debug.Log ("life " + Game12_Player_Controller.life_player);
		Game12_GUI_Manager.instance.PlaySound (SoundEnum.SE35_bike_explosion);

		if (Game12_Player_Controller.life_player >= 1) {
			startSpeed = .0f;
			if (Game12_Player_Controller.instance.playerInfo.GetID () == (int)Game12_Character.dekapan) {

				StartCoroutine (CountdownOnDeadth (-1, BikingKey.GameConfig.timerebirth, (int)Game12_Character.dayon));

			} else if (Game12_Player_Controller.instance.playerInfo.GetID () == (int)Game12_Character.dayon) {
				StartCoroutine (CountdownOnDeadth (-1, BikingKey.GameConfig.timerebirth, (int)Game12_Character.dekapan));
			}

		} else if (Game12_Player_Controller.life_player == 0) {
			UnListener ();
			Game12_Player_Controller.instance.UnListener ();
		}
		myState = Game12_GameState.pause;
	}

	//--> Listener on Items trigger detected from Game Manager
	public void GameManager_Item_Listener (GameObject obj, int itemID, string name, string imageResources, int itemType, float effect_Value1,
	                                       float effect_Value2, int percentage, int percentageReduction, string desciption)
	{
		switch (itemID) {
		case 0:
				// Do nothing
		case 1:
				// We do nothing if player's Dekapan
			if (Game12_Player_Controller.instance.playerInfo.GetID () == (int)Game12_Character.dekapan) {
				return;
			}
				// Play sound fx
			Game12_GUI_Manager.instance.PlaySound (SoundEnum.SE32_bike_hitchhike);
			startSpeed = .0f;
			Game12_GameManager.instance.myState = Game12_GameState.pause;
			Game12_GUI_Manager.instance.PlayerStopOnObstacle (true, itemID, name);
			Game12_Item_Manager.instance.PlayerOnDekapanHit (itemID, (int)Game12_Character.dekapan);
			obj.SetActive (false);
			break;
		case 2:
		case 10:
				// We do nothing if player on playing on Titan
			if (Game12_Player_Controller.instance.IsTitan) {
				return;
			}
				// Play sound fx
			Game12_GUI_Manager.instance.PlaySound (SoundEnum.SE33_bike_giant);
			Game12_Item_Manager.instance.PlayerOnTitan (effect_Value1, effect_Value2);
				// Sure no titan item on terrain untill effect finish.
			obj.SetActive (false);
			break;
		case 3:
		case 8:
		case 9:
		case 11:
				// Score item
				// Play sound fx
			Debug.Log ("Get Item " + obj.name);
			if (obj.GetComponent<SpriteRenderer> ().sprite) {
				imageResources = obj.GetComponent<SpriteRenderer> ().sprite.name;
				Game12_GameManager.instance.item_num++;
				Game12_GUI_Manager.instance.PlaySound (SoundEnum.se11_scoreitem);
				Game12_GUI_Manager.instance.G_Footer.AddItemScore (Mathf.RoundToInt (obj.GetComponent<Game12_Item_Component> ().itemProperties.GetEffectValue1 ()), imageResources);
				obj.SetActive (false);
			}
			break;

		case 4:
			break;
		case 5:
			break;
		case 6:
			break;
		case 7:
			break;
		default:
			break;
		}

	}
	//--< Listener on Items trigger detected from Game Manager

	public IEnumerator CountdownOnDeadth (int itemID, float delay, int nextPlayerID)
	{
		if (itemID == 0)
			yield return new WaitForSeconds (1.7f);
		else
			yield return new WaitForSeconds (1.0f);
		// Need right position and become invulnerable

		StartCoroutine(Game12_Player_Controller.instance.Move_Player_to_Reborn_point()); // = find_reborn_pos;
		/*
		while (delay > 0) {
			delay -= 0.1f;
			yield return new WaitForSeconds (0.1f);
		}
		*/
	}

	void ResetPhysical2D ()
	{
		Physics2D.gravity = new Vector2 (.0f, -9.81f);
		Time.fixedDeltaTime = 0.02f;
	}

	public void SetGameOver ()
	{
		Debug.Log(jungle_reach + " " + desert_reach + " " + akatsuka_reach);
		if(ComponentConstant.SOUND_MANAGER != null){
			ComponentConstant.SOUND_MANAGER.StopBGM ();
		}
		BikingPhysicalMatrix(false);
		Game12_GameManager.instance.Ending = false;
		Game12_GameManager.instance.myState = Game12_GameState.none;
		ResetPhysical2D ();
		SendGameEndingAPI ();
	}

	[HideInInspector]public int hitchhike1 = 0;
	[HideInInspector]public int hitchhike2 = 0;
	public int desert_reach = 1;
	public int jungle_reach = 0;
	public int akatsuka_reach = 0;
	[HideInInspector]public int item_num = 0;
	[HideInInspector]public int total_score_item = 0;

	public void SendGameEndingAPI ()
	{
		GameEndLogic.m_game_id = 6;
		GameEndLogic.move_metre = Mathf.RoundToInt (Distance);
		GameEndLogic.item_get_num = item_num;
		GameEndLogic.hitchhike1 = hitchhike1;
		GameEndLogic.hitchhike2 = hitchhike2;
		GameEndLogic.jungle_reach = jungle_reach;
		GameEndLogic.akatsuka_reach = akatsuka_reach;
		// total score
		GameEndLogic.score = Mathf.RoundToInt ((Distance * GameParameter.total_comb_var  + total_score_item) * (1 + this.cardBonus / 100));
		GameEndLogic.card_bonus = this.cardBonus;
		GameEndLogic.card_ids = CardRate.GetAdditionalCardID (2,6);
		GameEndLogic.complete = SendAPICompleteHandler;
		GameEndLogic.error = SendAPIErrorHandler;
		GameEndLogic.SendAPI ();
	}

	void SendAPICompleteHandler ()
	{
		Debug.Log ("SendAPICompleteHandler!" + UpdateInformation.GetInstance.game_list [1].score);
		Debug.Log ("SendAPICompleteHandler!" + APIInformation.GetInstance.rank);
		Debug.Log ("SendAPICompleteHandler!" + Player.GetInstance.lv);
		Debug.Log ("SendAPICompleteHandler!" + Player.GetInstance.exp);
		Debug.Log ("SentPara");
		Debug.Log("total Item Score " + total_score_item);
		/// 1,2,3: score, combo, maxcombo
		GameResaultManager.Instance.SetGameResultInformation (Mathf.RoundToInt (Distance * GameParameter.total_comb_var), total_score_item, 0, APIInformation.GetInstance.rank,cardBonus, false, false, true);
	}

	void SendAPIErrorHandler (string str)
	{
		GameResaultManager.Instance.SetGameResultInformation(Mathf.RoundToInt(Distance * GameParameter.total_comb_var), total_score_item, 0, APIInformation.GetInstance.rank,cardBonus);
        Debug.Log ("SendAPIErrorHandler! " + str);
	}

	public void OnDisable ()
	{
		ResetPhysical2D ();
	}
}
