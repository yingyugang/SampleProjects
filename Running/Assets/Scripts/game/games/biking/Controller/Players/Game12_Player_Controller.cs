using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public enum Game12_Character
{
	dekapan = 1,
	dayon = 2
}

public class Game12_Player_Controller : MonoBehaviour
{

//	private static Game12_Player_Controller _instance;
//
//	public static Game12_Player_Controller instance {
//		get {
//			
//			if (_instance == null)
//				_instance = GameObject.FindObjectOfType<Game12_Player_Controller> ();
//			
//			if (_instance == null)
//				Debug.Log ("Game12_Player_Controller is null");
//			
//			return _instance;
//		}
//	}
	public SpriteRenderer PlayerImage;
	public Game12_Character choosePlayer;
	public Game12_Player_Properties playerInfo;
	public Sprite[] Character_Sprite;
	public GameObject TitanWheel;
	public Rigidbody2D Player;
	public BoxCollider2D PlayerCollider;
	public COM_Moto[] Moto_CoM;
	public Transform rayFront;
	public Transform rayBack;
	public RaycastHit2D hitFrontWheel;
	public RaycastHit2D hitBackWheel;
	public GameObject PlayerFrefab;
	public GameObject DeadthPosition;
	public float TimeDetectDead = 0.05f;
	public bool IsTitan;
	//[HideInInspector]public bool IsJumpAfter = false;
	[HideInInspector]public bool IsInvulnerable = false;
	public bool IsJump = false;
	public bool IsAir = false;
	public bool IsFalling = false;
	public bool IsDoubleJump = false;
	public bool IsSpring = false;

	private float m_maxSpeed;
	private float m_jumpPower;
	private float m_jumpRamp;
	private float m_jumpDamp;
	private float m_falling_distance = .01f;
	private float m_moto_rotation = 12.0f;
	private Vector3 m_Position_Reborn;
	private Vector2 m_direction;
	private float m_spring_motobuff;
	public LayerMask LayerMask_Temp;
	//life player
	public static int life_player = 2;

	public Game12_Item_Life_Controller itemLife;
	private Vector3 lastPos;
	public Game12_Player_Controller Instance;
	public static Game12_Player_Controller instance;
	void Awake ()
	{
		Application.runInBackground = true;
		if(Instance) instance = Instance;
		else instance = GetComponent<Game12_Player_Controller>();

		LayerMask_Temp =  1 << LayerMask.NameToLayer("TransparentFX");
//		_instance = this;
		m_maxSpeed = BikingKey.GameConfig.MaxSpeed;	
		m_jumpPower = BikingKey.GameConfig.JumPower;
		m_jumpDamp = BikingKey.GameConfig.JumDamp;
		m_jumpRamp =  BikingKey.GameConfig.JumRamp;
		Physics2D.gravity = new Vector2 (.0f, -60.0f);
		Time.fixedDeltaTime = 0.01f;
		//life_player = 3;
		lastPos = transform.position;
	}

	public void Listener ()
	{
		UnListener();
		PauseManager.s_Instance.SetOnExitCallback(Game12_GUI_Manager.instance.Restart_Game);
		Game12_Player_RunningEvent.instance.AddOnstacleCollisionListener (PlayerController_Obstacle_Listener);
	}

	public void UnListener ()
	{
		Game12_Player_RunningEvent.instance.RemoveOnOnstacleCollisionListener (PlayerController_Obstacle_Listener);
	}

	void Start (){
		Listener ();
		PlayerLoadInfo (playerInfo, (int)choosePlayer);
		m_Cound_Death_Time = 0f;
	}

	public void PlayerStopOnObstacle (int itemID, string itemName)
	{
		PlayerHided ();
		// Init bang effect
		if (itemID == 5 || itemID == 6 || itemID == 7 || itemID == 0) {
			Instantiate (Game12_Item_Manager.instance.ObstaclePrefabs [3], transform.position, transform.rotation);
			PlayerDeadthAnim (playerInfo.GetID (), false);
		} 
	}

	public void PlayerHided ()
	{
		Player.isKinematic = true;
		Player.gameObject.SetActive (false);

//		Player.GetComponent<SpriteRenderer> ().enabled = false;
	}

	public void PlayerController_Obstacle_Listener (GameObject obj, int itemID, string name, string imageResources, int itemType, float effect_Value1,
	                                                float effect_Value2, int percentage, int percentageReduction, string desciption)
	{
//		PlayerStopOnObstacle (itemID, name);
		Debug.Log(obj.name);
		if(itemID == 0 && obj.name != BikingKey.Terrain.Terrain_Collision){
			Vector2 worldPos = obj.GetComponent<EdgeCollider2D>().transform.TransformPoint(obj.GetComponent<EdgeCollider2D>().points[obj.GetComponent<EdgeCollider2D>().points.Length-1]);
			DeadthPosition.transform.position = new Vector3(worldPos.x,
				worldPos.y,
				Player.transform.position.z);
		}
		else{
			DeadthPosition.transform.position = Player.position;
		}
		Debug.Log ("PlayerController_Obstacle_Listener");
		Player_Death ();
	}

	float m_Cound_Death_Time = 0f;

	void Player_Death ()
	{
		Debug.Log ("You are dead.");
		DOTween.KillAll ();
		Player.transform.DOScale (Vector3.one, .01f);
		PlayerHided ();
		Instantiate (Game12_Item_Manager.instance.ObstaclePrefabs [3], transform.position, transform.rotation);
		PlayerDeadthAnim (playerInfo.GetID (), false);

		//life for player
		life_player--;
		//life_player = Mathf.Clamp (life_player, 0, 1);

		Game12_GUI_Manager.instance.GUI_Obstacle_Listener ();
		Game12_GameManager.instance.GameManager_Obstacle_Listener ();
		m_Cound_Death_Time = 0f;
	}

	public void PlayerInsertLife ()
	{
		life_player++;
		Game12_Player_Controller.instance.PlayerAvatar_Deadth (Game12_GUI_Manager.instance.Lifes [0], true);
		Game12_Player_Controller.instance.PlayerAvatar_Deadth (Game12_GUI_Manager.instance.Lifes [1], true);
	}

	void Update(){
		if (Game12_GameManager.instance.myState != Game12_GameState.playing)
			return;
		if (Input.GetMouseButtonDown(0))
		{
			// does not work on Android
			if (Input.mousePosition.y < Screen.height * ( 1920f - 235f) / 1920f)
				JumpCal ();
			//if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
			
			return;
		}
//		if (Input.GetMouseButtonDown (0))
//			JumpCal ();
	//	if (IsAir) {
	//	InitWheelRaycast();
	//	}
	}
	bool InitWheelRaycast(){
		float TitanWheel = 4f;
		hitFrontWheel = Physics2D.Raycast (rayFront.position, -Vector2.up, 10f, LayerMask_Temp);
		hitBackWheel = Physics2D.Raycast (rayBack.position, -Vector2.up, 10f, LayerMask_Temp);
		//Physics2D.Linecast ();
		if (IsTitan)
			TitanWheel = 10f;
		return (hitFrontWheel.collider != null && hitBackWheel.collider != null && hitFrontWheel.distance < TitanWheel && hitBackWheel.distance < TitanWheel);
	}
	void FixedUpdate ()
	{

		if (Game12_GameManager.instance.myState != Game12_GameState.playing)
			return;

		if (IsInvulnerable)
			PlayerCollider.enabled = false;
		else
			PlayerCollider.enabled = true;
		
	
		if (IsJump && !Moto_CoM [0].IsGrounded) {// && !Moto_CoM [0].IsGrounded)
			IsAir = true;
		}
		if (IsAir) {
			//IsJump = true;
			Player.constraints = RigidbodyConstraints2D.FreezeRotation;
			if (Moto_CoM [0].IsGrounded){ // || Moto_CoM [0].IsGrounded) {
				Player.velocity = new Vector2(m_maxSpeed, .0f);
				IsJump = false;
				IsAir = false;
			}
		}
		float eucle = 0f;

		if (Moto_CoM [0].IsGrounded){
			Player.constraints = RigidbodyConstraints2D.None;
			m_moto_rotation = 12.0f;
		}
		else {
			Player.constraints = RigidbodyConstraints2D.FreezeRotation;
			if (InitWheelRaycast()) {
				//Debug.Log ("hit");
				m_direction = hitFrontWheel.point - hitBackWheel.point;
				//if (hitBackWheel.point.y > hitFrontWheel.point.y)
				//	m_direction = Vector2.right;
				//Quaternion angel = Quaternion.FromToRotation (Vector2.right, m_direction);
				eucle = Vector2.Angle (Vector2.right, m_direction);
				if (eucle < 2f)
					eucle = 0f;
				/*
				Debug.Log ("angle = " + eucle);
				Debug.DrawLine(hitFrontWheel.point, hitBackWheel.point, new Color(1,0,0), 1f);
				Debug.DrawLine(hitFrontWheel.point, rayFront.position, new Color(1,1,0), 1f);
				Debug.DrawLine(hitBackWheel.point, rayBack.position, new Color(0,1,1), 1f);
				*/
				//if (hitFrontWheel.distance > 0.5f && hitBackWheel.distance > 0.5f) {

					if (eucle < 70f) {// && (m_direction.magnitude < 1.5f)){
						if (m_direction.y < 0)
							eucle = - eucle;
						
					//Debug.Log (hitFrontWheel.distance);

					eucle = Mathf.LerpAngle (transform.eulerAngles.z, eucle,Time.fixedDeltaTime * 30f / hitFrontWheel.distance);
						transform.eulerAngles = new Vector3 (0, 0, eucle);
					} else {
						Debug.Log ("Crash ?? " + eucle);
						//	transform.eulerAngles = new Vector3(0, 0, 0);
					}
			} else {
				//Debug.Log ("Backto normal");
				//Player.transform.rotation = angel;
				//transform.DORotate (new Vector3 (.0f, .0f, 0f), 0.1f, RotateMode.Fast);
				eucle = Mathf.LerpAngle (transform.eulerAngles.z, 0,Time.fixedDeltaTime * 20f);
				transform.eulerAngles = new Vector3(0, 0, eucle);
			}
		}

		//-->| by anhgh
		//let him move
//		Debug.Log(Player.velocity.magnitude);

		if (Player.velocity.x < m_maxSpeed)
			Player.AddRelativeForce (Vector2.right * m_maxSpeed * Time.fixedDeltaTime *1000f);//, ForceMode.VelocityChange);
		else
			Player.velocity = new Vector2 (m_maxSpeed, Player.velocity.y); //Mathf.Sign (Player.velocity.x) * 
		//Player.velocity += new Vector2 (Game12_GameManager.instance.startSpeed * (Time.fixedDeltaTime / 0.001f), 0.0f);
		//ok now make my character dead
		//Debug.Log ("Game12_GameManager.instance.Distance = " + Game12_GameManager.instance.Distance);
		float deltaX = transform.position.x - lastPos.x;
//		Debug.Log (deltaX);
		if (deltaX <= 0.01) {
			m_Cound_Death_Time += Time.fixedDeltaTime;
			if (m_Cound_Death_Time >= TimeDetectDead) {
				if (Game12_GameManager.instance.Distance > -20){ //player wont die when countdown
					DeadthPosition.transform.position = Player.position;
					Debug.Log ("Player cant move");
					Player_Death ();
				}
				else
					m_Cound_Death_Time = 0f;
			}
		} else
			m_Cound_Death_Time = 0f;
		//--<| by anhgh
		lastPos = transform.position;
	}

	/// <summary>
	/// We calculating the jumper
	/// </summary>
	public void JumpCal ()
	{
		Debug.Log ("JumpCal");
		m_spring_motobuff = .0f;

		if (Player.transform.localEulerAngles.z > 0f && Player.transform.localEulerAngles.z < 90.0f) {
			m_spring_motobuff = Mathf.Tan (Player.transform.rotation.z) * (Player.velocity.x + 1.0f);//localEulerAngles.z/5;
			Debug.Log ("Uphill");
		}
		else{
			m_spring_motobuff = .0f;
			if (Player.transform.localEulerAngles.z > 270f && Player.transform.localEulerAngles.z < 360.0f) {
				m_spring_motobuff = Mathf.Tan (Player.transform.rotation.z) * (Player.velocity.x + 1.0f);
				Debug.Log ("Downhill");
			}

		}

		if (!IsJump) {
			// Rotation moto
			/*
			InitWheelRaycast();
			float angel = 0;
			m_direction = hitFrontWheel.point - hitBackWheel.point;
			angel = Vector3.Angle (m_direction, Vector3.right);
			if (m_direction.y <= 0){
				m_moto_rotation = 12.0f;
			}
			else{
				m_moto_rotation = angel + 6.0f;
			}
			Player.transform.DORotate (new Vector3 (.0f, .0f, m_moto_rotation), 0.3f, RotateMode.Fast);
			*/
			JumpCmnd ();
		} else if (IsDoubleJump) {
			JumpCmnd ();
			IsDoubleJump = false;
		}
	}

	public void JumpCmnd ()
	{
		//Player.constraints = RigidbodyConstraints2D.FreezeRotation;
		if (IsSpring) {
			// Play sound fx
			Game12_GUI_Manager.instance.PlaySound (SoundEnum.SE34_bike_ramp);
			Player.velocity = new Vector2 (Player.velocity.x , (m_jumpRamp - m_jumpDamp + m_spring_motobuff));
			IsJump = true;
			IsSpring = false;
			IsDoubleJump = true;
		} else {
			// Play sound fx
			Game12_GUI_Manager.instance.PlaySound (SoundEnum.SE31_bike_jump);
			Player.velocity = new Vector2 (Player.velocity.x , (m_jumpPower - m_jumpDamp + m_spring_motobuff));
			IsJump = true;
			IsDoubleJump = true;
		}
	}
		
	public IEnumerator Move_Player_to_Reborn_point(){
		Vector3 find_reborn_pos = FindRebornPos (DeadthPosition.transform.position);
		while(Vector3.Distance(Player.transform.position, find_reborn_pos) > 0.05f)
		{
			Player.transform.position = Vector3.Lerp(Player.transform.position, find_reborn_pos, 7f * Time.deltaTime);
			yield return null;
		}
		Player.transform.position = find_reborn_pos;
	}

	/// <summary>
	/// We make player become invulnerable after rebirth.
	public IEnumerator PlayerOnInvulnerable (SpriteRenderer sprite, float duration)
	{
		Color clr = sprite.color;
		int i = 0;
		duration = duration / 12;
		while (duration > 0) {
			IsInvulnerable = true;
			duration -= Time.deltaTime;
			yield return new WaitForSeconds (.2f);
			i++;
			if (i % 2 == 0)
				clr = new Color (255, 255, 255, 0.5f);
			else
				clr = new Color (255, 255, 255, 1.0f);
			sprite.color = clr;
			yield return null;
		}
		clr = new Color (255, 255, 255, 1.0f);
		sprite.color = clr;
		// Restore abys collider
		Terrain_Controller.instance.AbyssOnInvulnerable (false);
		// Player' invulnerable shut down
		IsInvulnerable = false;
	}

	public void PlayerAvatar_Deadth (Image player, bool isAlive)
	{
		if (isAlive)
			player.color = new Color (255.0f, 255.0f, 255.0f, 255);
		else
			player.color = new Color32 (150, 150, 150, 255);
	}

	void ConfigMoto (Game12_Player_Properties playerInfo)
	{
		m_maxSpeed = BikingKey.GameConfig.MaxSpeed;
		m_jumpPower = BikingKey.GameConfig.JumPower;
		m_maxSpeed = playerInfo.GetVelocity () * m_maxSpeed;
		m_jumpPower = playerInfo.GetJumpPower () * m_jumpPower;
	}

	/// <summary>
	/// We rebuild the player after rebirt with new position, properties, etc.
	/// </summary>
	/// <param name="nextPlayerID">Next player I.</param>
	/// 
	/// 
	public Vector3 FindRebornPos(Vector3 diedPos){
		//Terrain_AbyssTrigger[] Aby = Routes [currentPattern.ID].gameObject.GetComponentsInChildren<Terrain_AbyssTrigger> ();
		//if (item) {
		//	Debug.Log ("FindRebornPos " + item.gameObject.name);
		//	return item.transform.position;
		//}
		Vector3 startPos = diedPos;// + Vector3.up * 100f;
		startPos.y = 100f;
		for (int i = 1; i < 100; i++) {
			Vector3 detectPos = startPos + Vector3.right * i;
			RaycastHit2D hit = Physics2D.Raycast (detectPos, -Vector2.up, 150f, Game12_Player_Controller.instance.LayerMask_Temp);
			if (hit.collider != null && hit.point.y > -19f) {
				//Debug.Log ("FindRebornPos " + hit.point);
//				Debug.Log ("FindRebornPos on " + hit.collider.gameObject.name);
				startPos.x = hit.point.x - 4f;
				startPos.y = hit.point.y + 8f;
				startPos.z = diedPos.z;
				//Debug.DrawRay (detectPos,  startPos - detectPos, Color.green, 2);

				return startPos;
			}
		}
		return startPos; 
	}

	public void PlayerRebirth ()
	{
		// Reset normal state
		int nextPlayerID;
		if (playerInfo.GetID () == (int)Game12_Character.dekapan)
			nextPlayerID = (int)Game12_Character.dayon;
		else
			nextPlayerID = (int)Game12_Character.dekapan;
		
		Game12_Item_Manager.instance.CancelReturnPeople ();
		Player.transform.localScale = Vector3.one;
		TitanWheel.SetActive (false);
		// Update player info
		PlayerLoadInfo (playerInfo, nextPlayerID);
		Player.gameObject.SetActive (true);
//		Player.GetComponent<SpriteRenderer> ().enabled = true;
		// Make the moto no flip on air
		Player.transform.eulerAngles = new Vector3 (.0f, .0f, .0f);
		Moto_CoM [0].IsGrounded = false;
	//	Moto_CoM [1].IsGrounded = false;
		int Currentpattern = Terrain_Controller.instance.currentPattern.ID;
		Debug.Log ("Die in " + Currentpattern);
		Terrain_Controller.instance.Routes [Currentpattern].GetComponent<Terrain_Elmp>().ShowHitchhike();
		// Need right position and become invulnerable
		//Player.transform.position = find_reborn_pos;//new Vector3 (DeadthPosition.transform.position.x, DeadthPosition.transform.position.y + 7.0f, Player.transform.position.z);
		// Change player' sprite to next player
		ImagePlayer ().sprite = Character_Sprite [nextPlayerID - 1];
		// Set player on invulnerable
		StartCoroutine (PlayerOnInvulnerable (ImagePlayer (), 3.0f));
		Player.isKinematic = false;
		Player.velocity = Vector3.zero;
		IsTitan = false;
		IsAir = true;
		IsDoubleJump = false;
		IsJump = false;
		// Start game now
		Game12_GameManager.instance.myState = Game12_GameState.start;
		Terrain_Controller.instance.AbyssOnInvulnerable (true);
	}

	/// <summary>
	/// We load player info here from CSV data
	/// </summary>
	/// <param name="playerInfo">Player info.</param>
	/// <param name="id">Identifier.</param>
	public void PlayerLoadInfo (Game12_Player_Properties playerInfo, int id)
	{
		id -= 1;
		//--> Load character' param
		playerInfo.SetID (((int)Game12_GameParams.instance.characterData [id] ["id"]));
		playerInfo.SetName (((string)Game12_GameParams.instance.characterData [id] ["name"]));
		playerInfo.SetImageSource (((string)Game12_GameParams.instance.characterData [id] ["image_resource"]));
		playerInfo.SetVelocity (((float)Game12_GameParams.instance.characterData [id] ["speed"]));
		playerInfo.SetJumpPower (((float)Game12_GameParams.instance.characterData [id] ["jump_ability"]));
		playerInfo.SetItemRate (((float)Game12_GameParams.instance.characterData [id] ["item_appear_rate"]));
		Debug.Log ("item_appear_rate : " + Game12_GameParams.instance.characterData [id] ["item_appear_rate"]);
		//playerInfo.SetDescription (((string)Game12_GameParams.instance.characterData [id] ["description"]));
		playerInfo.SetMultiJump (BikingKey.GameConfig.multiJump);
		//--< Load character' param
		// Config the moto
		ConfigMoto (playerInfo);
	}

	public Sprite GetSpriteFromID (int playerID)
	{
		return Character_Sprite [playerID];
	}

	public SpriteRenderer ImagePlayer ()
	{
		return PlayerImage;
	//	return Player.GetComponent<SpriteRenderer> ();
	}

	public void PlayerDeadthAnim (int id, bool isDieByAbyss)
	{
		GameObject clone = Instantiate (PlayerFrefab, Player.transform.position, Player.transform.rotation) as GameObject;
		clone.GetComponent<Game12_Player_Animated> ().isDieByAbyss = isDieByAbyss;
		clone.GetComponent<SpriteRenderer> ().sprite = Character_Sprite [id - 1];
		clone.GetComponent<SpriteRenderer> ().sortingOrder = 10;
	}

}
