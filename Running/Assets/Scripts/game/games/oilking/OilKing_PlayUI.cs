using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class OilKing_PlayUI : MonoBehaviour
{

	public static OilKing_PlayUI s_Instance;
	public Text deltaTime;
	private float reachDistance = 1f;
	public Transform last_Pos;
	Vector3 startPos;
	public Button hitBtn, hitBtnChild;
	public Button throwBtn, throwBtnChild;
	public Button feverBtn, feverBtnChild;
	public Button utilSkillBtn;
//	public Button nopeBtn;
	//red bg when player hit bomb
	public GameObject redBG;

	 public Transform[] feverTransform;

	//	public Sprite[] imageOfDigging;
	//	public Sprite[] imageOfSpring;
	//	public Sprite[] imgHitBomb;
	//	public Sprite[] imgCurrent;
	private float[] pointTime = null;
	private float timeCurrent;
	private int indexStatusPlayer = 0;
	private float pointTimeCurrent = 0;

	private bool m_CheckSetSprite = false;

	//time freeze character
	private float m_CountTimeFreeze = 0;

	[HideInInspector]
	public bool checkFreezeChar;

	public Color colorScoreAdd, colorScoreMinus;

	//public Image imgHitButton, imgThrowButton;
	//	public Sprite spriteBomb;

	private Vector3 posTempTextEffect;
	IEnumerator effectColorText;
	public Outline outlineTextEffect;

	void Awake ()
	{
		s_Instance = this;
//		imageOfDigging = new Sprite[4];
//		imageOfSpring = new Sprite[4];
		//imgHitBomb = new Sprite[2];
//		imgCurrent = new Sprite[2];
	}

	void Start ()
	{
//		SetSpriteCharacter ();
		ActiveFeverButton (false);
		GetResourcesFromAssetBundle ();
		checkFreezeChar = false;
//		startPos = OilKing_BlockManager.Instance.GetPosBlock ();
//		deltaTime.transform.position = startPos;
		InvokeRepeating ("InvokeStatusCharacter", 0.1f, 0.5f);
	}

	public void ButtonDownHit ()
	{
		OilKingGamePlay.Instance.PlayerHit ();
	}

	public void ButtonUpHit ()
	{
		OilKingGamePlay.Instance.PlayerTouchUpHit ();
	}

	public void ButtonDownThrow ()
	{
		OilKingGamePlay.Instance.PlayerThrow ();
	}

	public void ButtonUpThrow ()
	{
		OilKingGamePlay.Instance.PlayerTouchUpThrow ();
	}

	void GetResourcesFromAssetBundle ()
	{
		//GetAssetBundle hitBomb
		//imgHitBomb [0] = OilKingAssetLoader.s_Instance.getSprite ("diggingbomb");
		//imgHitBomb [1] = OilKingAssetLoader.s_Instance.getSprite ("springbomb");

		//get sprite for Util Skill
		utilSkillBtn.GetComponent <Image> ().sprite = OilKingAssetLoader.s_Instance.getSprite (TypeSprite.ButtonDrill);

		//for (int i = 0; i < feverTransform.Length - 1; i++) {
		//	feverTransform [i].GetComponent <Image> ().sprite = OilKingAssetLoader.s_Instance.getSprite (TypeSprite.O_Item_5);
		//}

		//get sprite for Cutin King Show
		feverTransform [feverTransform.Length - 1].GetComponent <Image> ().sprite = OilKingAssetLoader.s_Instance.getSprite (TypeSprite.CutinKing);

		//get sprite for Fever Button
		feverBtn.GetComponent<Image> ().sprite = OilKingAssetLoader.s_Instance.getSprite (TypeSprite.ButtonFever);


	}

	public static OilKing_PlayUI Instance {
		get {
			if (s_Instance == null) {
				s_Instance = GameObject.FindObjectOfType<OilKing_PlayUI> ();
			}
			return s_Instance;
		}
	}

	void Update ()
	{
		if (checkFreezeChar && !Header.Instance.isPause) {
			FreezeCharacterWhenHitBomb ();
		}
	}

	public void SetSpriteCharacter ()
	{
		if (indexStatusPlayer >= 3) {
			return;
		}
		//if (indexStatusPlayer == 0) {
			//			Debug.Log ("LoadingUIOilKing.s_Instance.idDigging=" + LoadingUIOilKing.s_Instance.idDigging
			//			+ " LoadingUIOilKing.s_Instance.idSpring=" + LoadingUIOilKing.s_Instance.idSpring);
			//imgHitButton.sprite = OilKingAssetLoader.s_Instance.getSpriteCharacter (LoadingUIOilKing.s_Instance.idDigging + 1,
			//	TypeSprite.SuffixHitUI, 0);
			//imgThrowButton.sprite = OilKingAssetLoader.s_Instance.getSpriteCharacter (LoadingUIOilKing.s_Instance.idSpring + 1,
			//	TypeSprite.SuffixThrowUI, 0);
			//imgHitButton.sprite = LoadingUIOilKing.s_Instance.imageOfCharacter[LoadingUIOilKing.s_Instance.idDigging + 1];
			//imgThrowButton.sprite = LoadingUIOilKing.s_Instance.imageOfCharacter[LoadingUIOilKing.s_Instance.idSpring + 1];
			
		//} else {
		//	imgHitButton.sprite = OilKingAssetLoader.s_Instance.getSpriteCharacter (LoadingUIOilKing.s_Instance.idDigging + 1,
		//		TypeSprite.SuffixHitUI, indexStatusPlayer);
		//	imgThrowButton.sprite = OilKingAssetLoader.s_Instance.getSpriteCharacter (LoadingUIOilKing.s_Instance.idSpring + 1,
		//		TypeSprite.SuffixThrowUI, indexStatusPlayer);
		//}

	}

	void EmojiWhenHitBomb ()
	{
//		imgHitButton.sprite = spriteBomb;
//		imgThrowButton.sprite = spriteBomb;
		//yield return new WaitForSeconds (ParameterServer.FreezeTime);
		//SetSpriteCharacter ();
	}

	public void HitBomb ()
	{
		//StartCoroutine (EmojiWhenHitBomb ());
		EmojiWhenHitBomb ();
	}

	public void RaycastTargetButtons (bool isEnable)
	{
		hitBtn.GetComponent <Image> ().raycastTarget = isEnable;
		hitBtnChild.GetComponent<Image>().raycastTarget = isEnable;
		throwBtn.GetComponent <Image> ().raycastTarget = isEnable;
		throwBtnChild.GetComponent<Image>().raycastTarget = isEnable;
		feverBtn.interactable = isEnable;
		feverBtnChild.interactable = isEnable;
//		nopeBtn.gameObject.SetActive(!isEnable);
	}

	void FreezeCharacterWhenHitBomb ()
	{
		m_CountTimeFreeze += Time.deltaTime;
		if (m_CountTimeFreeze >= ParameterServer.FreezeTime) {
			RaycastTargetButtons (true);
			checkFreezeChar = false;
			m_CountTimeFreeze = 0;
			OilKingGamePlay.Instance.ResetToNormalCharaterSprite ();
			SetSpriteCharacter ();
		}
	}

	public void ActiveFeverButton (bool isEnable)
	{
		feverBtn.gameObject.SetActive (isEnable);
		throwBtn.gameObject.SetActive (!isEnable);
		hitBtn.gameObject.SetActive (!isEnable);
	}

	IEnumerator ActiveRedBG ()
	{
		redBG.SetActive (true);
		yield return new WaitForSeconds (0.3f);
		redBG.SetActive (false);
	}

	public void ExecuteActiveRedBG ()
	{
		StartCoroutine (ActiveRedBG ());
	}

	public void InteractiveUtilSkillBtn (bool isEnale)
	{
		utilSkillBtn.interactable = isEnale;
	}

	Tweener tweenTextTime = null;

	public void EffectTextTime (float _timeRemaingTime)
	{
		deltaTime.transform.position = OilKing_BlockManager.Instance.GetPosBlock ();
		//color outline reset
		Color colorOutLine = outlineTextEffect.effectColor;
		colorOutLine.a = 1;
		outlineTextEffect.effectColor = colorOutLine;
		Color colorText = deltaTime.color;
		colorText.a = 1;
		deltaTime.color = colorText;

		deltaTime.gameObject.SetActive (true);
		if (_timeRemaingTime < 0) {
			//			deltaTime.color = colorScoreMinus;
			deltaTime.text = _timeRemaingTime.ToString ("F1") + " 秒";

			checkFreezeChar = true;
			RaycastTargetButtons (false);
		} else {
			//			deltaTime.color = colorScoreAdd;
			deltaTime.text = "+ " + _timeRemaingTime.ToString ("F1") + " 秒";
		}
		tweenTextTime.Restart (false);
		tweenTextTime = deltaTime.transform.DOMoveY (deltaTime.transform.position.y + 1f, 0.8f);

		tweenTextTime.OnComplete (() => {
			
			if (effectColorText != null) {
				StopCoroutine (effectColorText);
			}
			effectColorText = watingAlphaColor (deltaTime);
			StartCoroutine (effectColorText);
		});
	}

	IEnumerator watingAlphaColor (Text txtTime)
	{
		yield return new WaitForEndOfFrame ();
		Color colorOutLine = outlineTextEffect.effectColor;
		colorOutLine.a = 1;
		outlineTextEffect.effectColor = colorOutLine;

		Color colorText = deltaTime.color;
		colorText.a = 1;
		deltaTime.color = colorText;

		float speed = 4;

		while (colorOutLine.a >= 0) {
			colorOutLine.a -= Time.deltaTime * speed;
			outlineTextEffect.effectColor = colorOutLine;

			colorText.a -= Time.deltaTime * speed;
			deltaTime.color = colorText;
			yield return new WaitForEndOfFrame ();
		}
		deltaTime.gameObject.SetActive (false);
	}

	void InvokeStatusCharacter ()
	{
		if (LoadingUIOilKing.s_Instance.m_IsDone && OilKingUtils.isRunGame) {
			timeCurrent = ParameterServer.GameTime - Header.Instance.GetLifeTime ();
//			Debug.Log ("ParameterServer.GameTime=" + ParameterServer.GameTime + " Header.Instance.GetLifeTime ()=" + Header.Instance.GetLifeTime ());
			if (checkFreezeChar && Header.Instance.isPause) {
				return;
			}
			//init point
			if (pointTime == null && ParameterServer.GameTime != 0) {
				pointTime = new float[4];
				indexStatusPlayer = 0;
				for (int i = 0; i < pointTime.Length; i++) {
					pointTime [i] = ParameterServer.GameTime / 4 * i;
				}
			}
			if (pointTime.Length != 0) {
				if (indexStatusPlayer < pointTime.Length) {
					if (timeCurrent >= pointTime [indexStatusPlayer]) {
						SetSpriteCharacter ();
						indexStatusPlayer++;
//						Debug.Log ("change status " + indexStatusPlayer + " timeCurrent=" + timeCurrent);
					}
				}
			}
		}
	}

	public void RunAnimCharactersFever()
	{
		for (int i = 0; i < feverTransform.Length - 1; i++)
		{
			//load image from asset bundle
			feverTransform[i].GetComponent<OilKingAnimImage> ().RunAnim();
		}
	}

	public void NopeButton()
	{
		if (ComponentConstant.SOUND_MANAGER != null)
		{
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se43_oil_mattock);
		}
	}
	public void GetResourceFromAssetbundleCharacterFever(){

		for (int i = 0; i < feverTransform.Length - 1; i++)
		{
			//load image from asset bundle
			OilKingAnimImage animImage = feverTransform[i].GetComponent<OilKingAnimImage> ();
			animImage.lstImage=new Sprite[4];

			for (int j = 0; j < animImage.lstImage.Length; j++) {
				animImage.lstImage [j] = OilKingAssetLoader.s_Instance.getSpriteCharacter (i+1, TypeSprite.SuffixHit, j);
			}
			animImage.lstImage[3] = OilKingAssetLoader.s_Instance.getSpriteCharacter(i + 1, TypeSprite.SuffixHit, 2);

			animImage.RestartAnim ();
		}
	}
}
