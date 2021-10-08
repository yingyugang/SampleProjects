using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.ComponentModel;
using System.Diagnostics;


public class OilKingGamePlay : MonoBehaviour
{
	public static bool checkColorCoincident = false;

	public Transform throwHuman;
	public Transform hitHuman;
	public Transform utilSkillTransform;

	public Transform blackGroundTransform;
	public OilKingAnimCharacter animCharHit, animCharThrow;
	public OilKingAnimDrill animDrill;

	[HideInInspector]
	public bool isHit;
	[HideInInspector]
	public bool isThrow;
	[HideInInspector]
	public bool isFever;

	//util skill
	[HideInInspector]
	public bool isWaitUtilSkill;

	private float m_LimitTimeUtilSkill;
	private float m_CountTimeUtilSkill = 0f;

	private int tmpHitNumber;

	//fever
	public float feverTime;
	private float m_CountTime;
	private float m_OffsetTime = 2.5f;

	private int m_CoinMin;
	private int m_CoinMax;
	private int m_Coin = 0;
	private int m_CurrentTime = 99999;
	private int m_CounDownTime = 5;

	//check if you throwed a block (not bomb), use for check complete mission
	[HideInInspector]
	public bool checkThowItem = true;

	private bool m_IsButtonHit = false;
	private bool m_IsButtonThrow = false;
	private SoundEnum[] m_BlockSoundWhenHit;
	private static OilKingGamePlay m_Instance;

	public static OilKingGamePlay Instance {
		get {
			if (m_Instance == null) {
				m_Instance = GameObject.FindObjectOfType<OilKingGamePlay> ();
			}
			return m_Instance;
		}
	}


	void Awake ()
	{
		m_Instance = this;
	}

	void Start ()
	{
		checkThowItem = true;
		isHit = false;
		isThrow = false;
		isFever = false;
		m_CurrentTime = 99999;
		OilKingUtils.MY_TIME = ParameterServer.GameTime;

		feverTime = ParameterServer.FeverTime;
		m_LimitTimeUtilSkill = ParameterServer.DrillRateTime < 3 ? 3 : ParameterServer.DrillRateTime;
		m_BlockSoundWhenHit = new SoundEnum[(int)Block.Limit];
	}

	void InitSoundEffect() {
		//init default se for blocks
		for (int i = 0; i < (int)Block.Limit; i++)
			m_BlockSoundWhenHit[i] = SoundEnum.limit; 

		m_BlockSoundWhenHit[(int)Block.Stone] = SoundEnum.se43_oil_mattock;
		m_BlockSoundWhenHit[(int)Block.Fossil] = SoundEnum.se43_oil_mattock;
		m_BlockSoundWhenHit[(int)Block.Plaster] = SoundEnum.se43_oil_mattock;
		m_BlockSoundWhenHit[(int)Block.Treasure1] = SoundEnum.se43_oil_mattock;
		m_BlockSoundWhenHit[(int)Block.Treasure2] = SoundEnum.se43_oil_mattock;
		m_BlockSoundWhenHit[(int)Block.Treasure3] = SoundEnum.se43_oil_mattock;
		m_BlockSoundWhenHit[(int)Block.Treasure4] = SoundEnum.se43_oil_mattock;
		m_BlockSoundWhenHit[(int)Block.Treasure5] = SoundEnum.se43_oil_mattock;
		m_BlockSoundWhenHit[(int)Block.Treasure6] = SoundEnum.se43_oil_mattock;
	}	

	public void GetResourcesFromAssetBundle ()
	{
		blackGroundTransform.GetComponent<SpriteRenderer> ().sprite = OilKingAssetLoader.s_Instance.getSprite (TypeSprite.OilKing_Background);
		animCharHit.GetResourceFromAssetBundle();
		animDrill.GetResourceFromAssetBundle();
		animCharThrow.GetResourceFromAssetBundle();

		OilKing_PlayUI.s_Instance.GetResourceFromAssetbundleCharacterFever ();
	}

	void Update ()
	{
		WaitForNextActiveUtilSkill ();
		CountdownFeverTime ();

		if (LoadingUIOilKing.s_Instance.m_IsDone) {
			int gettime = Mathf.RoundToInt(Header.Instance.GetLifeTime());
			if (gettime < m_CurrentTime && OilKingUtils.isRunGame && !isFever)
			{
				m_CurrentTime = gettime;
				if (m_CurrentTime <= m_CounDownTime && m_CurrentTime > 0)
				{
					PlaySound(SoundEnum.se06_timeup_countdown);
				}
				else if (m_CurrentTime <= 0)
				{
					PlaySound(SoundEnum.se07_timeup);

					PlaySound(SoundEnum.bgm15_title_back);
					OilKingManager.s_Instance.OnGameOver();

				}
			}
		}

		#if UNITY_EDITOR
		TestBugBlock ();
		#endif
	}

	public void PlayerHit ()
	{
		if (!m_IsButtonThrow && !m_IsButtonHit) {
			Block tmpBlock = OilKing_BlockManager.Instance.GetBlockAhead().nameBlock;

			m_IsButtonHit = true;

			animCharHit.RunAnim();

			OilKing_BlockManager.Instance.ResetTimeShake ();

			if (CountdowntHitBlock () <= 0) {
				ExecuteAfterBlockBroken (tmpBlock);
			}
			else 
				PlaySoundWhenHit(tmpBlock);
		}
	}

	public void PlayerThrow ()
	{
		if (!m_IsButtonThrow && !m_IsButtonHit) {
			animCharThrow.RunAnim();
			if (!OilKing_BlockManager.Instance.GetBlockAhead ().canThrow)
			{ 
				PlaySound(SoundEnum.SE16_mutsugo_fall);
				return; 
			}
			m_IsButtonThrow = true;
			DetermineBoomThrown ();
			PlaySound (SoundEnum.se02_ng);

			isThrow = true;
			StartCoroutine (ShowBlockThrowed ());
			if (OilKing_BlockManager.Instance.GetBlockAhead ().nameBlock != Block.Boom) {
				checkThowItem = false;
			}
		}
	}

	public void TurnOnButton ()
	{
		m_IsButtonThrow = false;
		m_IsButtonHit = false;
	}

	public void PlayerTouchUpThrow ()
	{
		m_IsButtonThrow = false;
	}

	public void PlayerTouchUpHit ()
	{
		m_IsButtonHit = false;
	}

	public void FeverButton ()
	{
		m_IsButtonThrow = false;
		m_IsButtonHit = false;

		OilKing_BlockManager.Instance.ResetTimeShake ();
		OilKing_PlayUI.Instance.RunAnimCharactersFever();
		
		if (CountdowntHitBlock () <= 0) {
			Block tmpBlock = OilKing_BlockManager.Instance.GetBlockAhead().nameBlock;
			ExecuteAfterBlockBroken (tmpBlock);
		}
	}

	IEnumerator ShowBlockThrowed ()
	{
		GameObject objectThrow = PoolManagerOilKing.s_Instance.GetFreeObject (OilKingConfig.POOL_NAME_OBJECT_THROW, OilKing_BlockManager.Instance.GetPosBlock ());
		objectThrow.GetComponent<SpriteRenderer> ().sprite = OilKing_BlockManager.Instance.GetBlockAhead ().imgBlock;
		yield return new WaitForSeconds (1f);
		objectThrow.SetActive (false);
	}


	int CountdowntHitBlock ()
	{
		return OilKing_BlockManager.Instance.GetBlockAhead ().hitNumber--;
	}

	/// <summary>
	/// Count time for next active button drill skill.
	/// </summary>
	void WaitForNextActiveUtilSkill ()
	{
		if (isWaitUtilSkill && !Header.Instance.isPause) {
			m_CountTimeUtilSkill += Time.deltaTime;
			if (m_CountTimeUtilSkill >= m_LimitTimeUtilSkill) {
				m_CountTimeUtilSkill = 0f;
				isWaitUtilSkill = false;
				OilKing_PlayUI.Instance.InteractiveUtilSkillBtn (true);
			}
		}
	}

	/// <summary>
	/// Set function for button drill skill
	/// </summary>
	public void UtilSkill ()
	{
		PlaySound (SoundEnum.se42_oil_drill);
		isWaitUtilSkill = true;
		OilKing_PlayUI.Instance.InteractiveUtilSkillBtn (false);
		OilKing_Animation.Instance.ExecuteUtilSkillAnim ();
	}

	void PlaySoundWhenHit(Block tmpBlock) {
		if (m_BlockSoundWhenHit[(int)tmpBlock] != SoundEnum.limit)
		{
			PlaySound(SoundEnum.se39_oil_rock);
		}
	}

	void ExecuteAfterBlockBroken (Block tmpBlock)
	{

		DetermineCoin ();
		DetermineScore ();

		int coinGet = Random.Range (m_CoinMin, m_CoinMax + 1);
		SpawnCoin (OilKing_BlockManager.Instance.GetPosBlock (), coinGet);
		CheckColorCoincident ();

		switch (tmpBlock) {
			case Block.Boom:
				PlaySound(SoundEnum.SE35_bike_explosion);
				HitBomb ();
				break;
			case Block.Fossil:
				PlaySound(SoundEnum.se40_oil_break);
				break;
			case Block.Plaster:
				PlaySound(SoundEnum.se40_oil_break);
				break;
			case Block.Stone:
				PlaySound(SoundEnum.se40_oil_break);
				break;
			case Block.Treasure1:
			case Block.Treasure2:
			case Block.Treasure3:
			case Block.Treasure4:
			case Block.Treasure5:
			case Block.Treasure6:
				PlaySound(SoundEnum.SE24_oden_block);
				OilKing_BlockManager.Instance.ExecuteGenerateItem ();
				break;
			case Block.Item1:
				PlaySound(SoundEnum.se12_bonusitem);
				OilKing_PlayUI.s_Instance.EffectTextTime (ParameterServer.TimeSpanByBomb);
				Header.Instance.SetLifeTime (RemainingTime (false));
				break;
			case Block.Item2:
			case Block.Item3:
			case Block.Item4:
				PlaySound(SoundEnum.se11_scoreitem);
				isHit = true;
				OilKingFooter.Instance.SetLeftValue (tmpBlock);
				break;
			case Block.Item5:
				PlaySound(SoundEnum.se12_bonusitem);
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm10_invicible);
				isFever = true;
				OilKing_PlayUI.Instance.ActiveFeverButton (true);
				ExecutePerformanceFever ();
				OilKing_BlockManager.Instance.ReplaceBlockByFever (.1f);
				break;
			case Block.Item6: //bomb same with Block.Boom
				PlaySound(SoundEnum.SE35_bike_explosion);
				HitBomb ();
				break;
			case Block.Fever:
				PlaySound(SoundEnum.se41_oil_bottle);
				break;
		}

		if (!tmpBlock.ToString ().Contains ("Treasure"))
			isHit = true;
	}

	/// <summary>
	/// when player hit a bomb
	/// </summary>
	void HitBomb ()
	{
		OilKing_PlayUI.Instance.ExecuteActiveRedBG ();	
		DetermineBoomHited ();
		StartCoroutine (BoomEffect ());
		OilKing_PlayUI.s_Instance.EffectTextTime (-ParameterServer.TimeSpanByBomb);
		Header.Instance.SetLifeTime (RemainingTime (true));
		OilKing_PlayUI.s_Instance.HitBomb ();

		//animation
		animCharHit.StopAnim ();
		animCharThrow.StopAnim ();

		animCharHit.SetSpriteWhenHitBoom();
		animCharThrow.SetSpriteWhenHitBoom();
	}

	/// <summary>
	/// Check color of hit character and color of block treasure,
	/// if color of hit character equal color of block treasure --> appear item fever
	/// </summary>
	void CheckColorCoincident ()
	{
		//order of type block Treasure at 5 - 10,
		//id color of character hit at 0 - 5,
		//so we subtract for 4 blocks precede
		int idBlock = (int)OilKing_BlockManager.Instance.GetBlockAhead ().nameBlock - 4;

		if (idBlock == LoadingUIOilKing.s_Instance.idHit)
			checkColorCoincident = true;
		else
			checkColorCoincident = false;
	}

	void DetermineCoin ()
	{
		m_CoinMin = OilKing_BlockManager.Instance.GetBlockAhead ().coinMin;
		m_CoinMax = OilKing_BlockManager.Instance.GetBlockAhead ().coinmax;
	}

	void DetermineScore ()
	{
		OilKing_Item tmpItem = OilKing_BlockManager.Instance.GetBlockAhead () as OilKing_Item;
		if (tmpItem != null) {
			OilKingUtils.MY_SCORE += tmpItem.score;
		}
	}

	void DetermineTimePlay (float deltaTime)
	{
		OilKingUtils.MY_TIME += deltaTime;
	}

	void DetermineBoomThrown ()
	{
		if (OilKing_BlockManager.Instance.GetBlockAhead ().nameBlock == Block.Boom) {
			OilKingUtils.THROW_BOMB++;
		}
	}

	void DetermineBoomHited ()
	{
		OilKingUtils.HIT_BOMB++;
	}

	public void CountdownFeverTime ()
	{
		if (isFever && !Header.Instance.isPause) {
			m_CountTime += Time.deltaTime;
			UpdateFever (feverTime - m_CountTime + m_OffsetTime);
			if (m_CountTime >= feverTime + m_OffsetTime) {
				if(!OilKing_Animation.Instance.checkStartUtilSkill)
				{
					isFever = false;
					m_CountTime = 0;
					StartCoroutine(PerformanceFever(false));
					OilKingFooter.Instance.DisactiveFeverBoard();
					OilKing_PlayUI.Instance.RaycastTargetButtons(false);
					OilKing_PlayUI.Instance.ActiveFeverButton(false);
					ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm13_oil);
					InvisibleCharacters(true);
					OilKing_BlockManager.Instance.ResetRotationBlocks();

					TurnOnButton();
				}
			}


		}
	}

	//play animation appear if isAppear == true, else play animation exit
	IEnumerator PerformanceFever (bool isAppear)
	{
		OilKing_PlayUI.Instance.RaycastTargetButtons (false);
		OilKing_PlayUI.Instance.InteractiveUtilSkillBtn (false);

		OilKing_Animation.Instance.ExecuteFeverAnim (isAppear);

		yield return new WaitForSeconds (isAppear ? 2.6f : 0.5f);

		if (!OilKing_Animation.Instance.checkStartUtilSkill) { 
			OilKing_PlayUI.Instance.RaycastTargetButtons (true); 
		}
		OilKing_PlayUI.Instance.InteractiveUtilSkillBtn (true);
	}

	public void ExecutePerformanceFever ()
	{
		StartCoroutine (PerformanceFever (true));
		OilKingFooter.Instance.ActiveFeverBoard ();
	}

	void UpdateFever (float countTime)
	{
		float value = countTime / feverTime;
		OilKingFooter.Instance.SetSliderValue (value);
	}

	public void SpawnCoin (Vector3 pos, int numObjs)
	{
		if (numObjs > 0) {
			for (int i = 0; i < numObjs; i++) {
				GameObject coin = PoolManagerOilKing.s_Instance.GetFreeObject ("PickupCoin", pos);
				coin.transform.position = pos;
			}	
			SetCoin (numObjs, true);
		}

	}

	public void UpdateCoin ()
	{
		Header.Instance.SetScore (OilKingUtils.MY_COIN.ToString ());
	}

	//Set Score, Coin
	public void SetCoin (int deltaScore, bool check)
	{
		if (check) {
			m_Coin += deltaScore;
		} else
			m_Coin -= deltaScore;

		OilKingUtils.MY_COIN = m_Coin;
		UpdateCoin ();
	}

	//Remaing Time
	float RemainingTime (bool isBoom)
	{
		float tmpTime = isBoom ? -ParameterServer.TimeSpanByBomb : ParameterServer.TimeSpanByBomb;
		float time = Header.Instance.GetLifeTime () + tmpTime;
		DetermineTimePlay (tmpTime);
		if (time <= 0f)
			return 0f;
		else
			return time;
	}

	public void PlaySound (SoundEnum sound)
	{
		if (ComponentConstant.SOUND_MANAGER != null) {
			ComponentConstant.SOUND_MANAGER.Play (sound);
		}
	}

	IEnumerator BoomEffect ()
	{
		GameObject effectBoom = PoolManagerOilKing.s_Instance.GetFreeObject (OilKingConfig.POOL_NAME_EFFECT_BOOM, OilKing_BlockManager.Instance.GetPosBlock ());
		yield return new WaitForSeconds (0.5f);
		effectBoom.SetActive (false);
	}

	public void ResetToNormalCharaterSprite()
	{
		animCharHit.RestartAnim();
		animCharThrow.RestartAnim();
	}

	public void InvisibleCharacters(bool isEnable)
	{
		hitHuman.gameObject.SetActive(isEnable);
		throwHuman.gameObject.SetActive(isEnable);
	}
	void TestBugBlock(){
		if (Input.GetKeyDown (KeyCode.X)) {
			PlayerHit ();
		}
		if (Input.GetKeyUp (KeyCode.X)) {
			PlayerTouchUpHit ();
		}

		if (Input.GetKeyDown (KeyCode.Z)) {
			PlayerThrow ();
		}
		if (Input.GetKeyUp (KeyCode.Z)) {
			PlayerTouchUpThrow ();
		}
	}
}
