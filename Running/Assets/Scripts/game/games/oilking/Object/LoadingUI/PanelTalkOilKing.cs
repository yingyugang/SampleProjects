using UnityEngine;
using UnityEngine.UI;
using System.Collections;


using DG.Tweening;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

[System.Serializable]
public class ShowCharacterColor
{
	public Image[] arrCharacters;
}

public class PanelTalkOilKing : MonoBehaviour
{

	public static PanelTalkOilKing s_Instance;
	public Text textDigging;
	public Text textSpring;
	private float m_PosTextDigging;
	private float m_PosTextSpring;
	public Text nameDigging;
	public Text nameSpring;
	private string m_NameDiggingMssg;
	private string m_NameSpringMssg;

	public OilKingAnimImage imgDigging, imgSpring;
	//	public Sprite[] arrShowCharsTalking;
	//public Image imgDiggingTalkBG, imgSpringTalkBG;
	Image hitBtn, throwBtn;
	private float letterPause = 0.05f;
	private string messageHit, messageThrow;
	private Vector2 diggingPos, springPos;
	private float speed = 2.6f;

	private static bool[,] checkCombination;

	//show color
	//	public ShowCharacterColor[] characterShowColor;

	private string strFromServer;

	public Color[] arrColor;
	private int m_NumDigging = 6;
	private int m_NumSpring = 6;

	private float m_TimeBlink = 0.5f;
	private float m_TmpTimeBlink;
	private bool m_CheckBlink = false;
	private int m_TmpDigging;
	private int m_TmpSpring;

	public Image imgGotogame;

	void Awake ()
	{

		s_Instance = this;
		diggingPos = new Vector2 (160f, 250f);
		springPos = new Vector2 (-250f, -250f);

	}

	IEnumerator Start ()
	{
		yield return new WaitForEndOfFrame ();
		m_PosTextSpring = textSpring.rectTransform.position.y;
		m_PosTextDigging = textDigging.rectTransform.position.y;
		m_TmpTimeBlink = m_TimeBlink;
//		strFromServer = ParameterServer.Groups;
		Header.Instance.PauseBtnInteractive (false);
		checkCombination = new bool[m_NumDigging, m_NumSpring];
		//if (PlayerPrefs.HasKey(OilKingConfig.MISSION_SIX_BROTHERS_MEET))
		//	strFromServer = PlayerPrefs.GetString(OilKingConfig.MISSION_SIX_BROTHERS_MEET);
		InitPanelTalk ();

	}

	public void InitPanelTalk ()
	{
		CSVSerifTalk serifTalk;

		if (LoadingUIOilKing.s_Instance.m_CheckSerif) {
			//trick cheat

			//Set Sprite Digging

			//Set Sprite Spring
			LoadingUIOilKing.s_Instance.idThrow = LoadingUIOilKing.s_Instance.idThrow - 6;

			serifTalk = OilKingCSV.s_Instance.getSerifTalk (LoadingUIOilKing.s_Instance.idHit + 1,
				LoadingUIOilKing.s_Instance.idThrow + 1);
		} else {
			//normal

			//Set Sprite Digging
			LoadingUIOilKing.s_Instance.idHit = ParameterServer.GetCharacterSelected (false) - 1;

//			do {
			LoadingUIOilKing.s_Instance.idThrow = ParameterServer.GetCharacterSelected (true) - 1;
			//Set Sprite Spring
//			} while (LoadingUIOilKing.s_Instance.idSpring == LoadingUIOilKing.s_Instance.idDigging);

			serifTalk = OilKingCSV.s_Instance.GetSerifByID (ParameterServer.SerifID);
		}

		imgSpring.GetComponent<Image> ().sprite = OilKingAssetLoader.s_Instance.getSpriteCharacter (LoadingUIOilKing.s_Instance.idThrow + 1,
			TypeSprite.SuffixThrowUI, 0);
		imgDigging.GetComponent<Image> ().sprite = OilKingAssetLoader.s_Instance.getSpriteCharacter (LoadingUIOilKing.s_Instance.idHit + 1,
			TypeSprite.SuffixHitUI, 0);

		SetAnimationSerif ();

		DetermineIndexOfCharacter ();
		OilKingGamePlay.Instance.GetResourcesFromAssetBundle ();

		checkCombination [LoadingUIOilKing.s_Instance.idHit, LoadingUIOilKing.s_Instance.idThrow] = true;

		OilKingUtils.ID_SERIF = serifTalk.ID;
		messageHit = serifTalk.diggingSerif;
		messageThrow = serifTalk.throwingSerif;
		//swap UI

		m_NameDiggingMssg = serifTalk.diggingname;
		nameDigging.color = arrColor [LoadingUIOilKing.s_Instance.idHit];
		m_NameSpringMssg = serifTalk.throwingname;
		nameSpring.color = arrColor [LoadingUIOilKing.s_Instance.idThrow];

		//
		//if (LoadingUIOilKing.s_Instance.m_CheckSerif)
		//{
//			ParameterServer.DoneSerif += "|" + serifTalk.ID;
		//}


		OilKingUtils.current_group = (LoadingUIOilKing.s_Instance.idHit + 1) + "|" + (LoadingUIOilKing.s_Instance.idThrow + 1);
		ParameterServer.UpdateListSerifDone (serifTalk.ID);
		Debug.Log ("serifid=" + serifTalk.ID + "ParameterServer.DoneSerif=" + ParameterServer.DoneSerif);

//		Debug.Log ("GROUP FROM SERVER " + ParameterServer.CurrentGroup);
		Debug.Log ("ID Spring = " + (LoadingUIOilKing.s_Instance.idThrow) + " ID Digging = " + (LoadingUIOilKing.s_Instance.idHit));
		//Debug.Log ("GROUP SEND TO SERVER " + OilKingUtils.current_group+" ID_SERIF="+OilKingUtils.ID_SERIF+" NumSerifDone="+ParameterServer.lstSerifDone.Length);

//		ShowColorForCharacter ();

		//SetSpriteForBGCharacterTalking();

		//strFromServer = ReturnStringToServer ();
		//Debug.Log (strFromServer);
		//PlayerPrefs.SetString(OilKingConfig.MISSION_SIX_BROTHERS_MEET, strFromServer);

//		OilKing_PlayUI.s_Instance.imgCurrent [0] = imgDigging.sprite;
//		OilKing_PlayUI.s_Instance.imgCurrent [1] = imgSpring.sprite;
		//Set Sprite Button hit and throw
//		OilKing_PlayUI.Instance.hitBtn.image.sprite = GetColorFromID (LoadingUIOilKing.s_Instance.idDigging);
//		OilKing_PlayUI.Instance.throwBtn.image.sprite = GetColorFromID (LoadingUIOilKing.s_Instance.idSpring + 6);


		OilKing_PlayUI.Instance.hitBtn.image.sprite = OilKingAssetLoader.s_Instance.getSpriteButtonDigging (LoadingUIOilKing.s_Instance.idHit + 1);
		OilKing_PlayUI.Instance.throwBtn.image.sprite = OilKingAssetLoader.s_Instance.getSpriteButtonSpring (LoadingUIOilKing.s_Instance.idThrow + 1);

		//Check null sprite at button
		if (hitBtn == null) {
			hitBtn = OilKing_PlayUI.Instance.hitBtn.GetComponent<Image> ();
		}
		if (throwBtn == null) {
			throwBtn = OilKing_PlayUI.Instance.throwBtn.GetComponent<Image> ();
		}
		textDigging.text = "";
		textSpring.text = "";
		nameSpring.text = "";
		nameDigging.text = "";

		//text animation
		StartCoroutine (RunAndSwapSerif (serifTalk.anime));

		OilKing_PlayUI.s_Instance.SetSpriteCharacter ();
	}

	IEnumerator TypeText (string message, Text textComp)
	{
		foreach (char letter in message.ToCharArray()) {
			textComp.text += letter;
			yield return new WaitForSeconds (letterPause);
		}
	}

	//IEnumerator TextAnimation (int anime)
	//{

	//	//run text
	//	yield return new WaitForSeconds (1f);

	//	if (anime == 1) {
	//		textSpring.rectTransform.position = new Vector3(textSpring.rectTransform.position.x, m_PosTextDigging, textSpring.rectTransform.position.z);
	//		textDigging.rectTransform.position = new Vector3(textDigging.rectTransform.position.x, m_PosTextSpring, textDigging.rectTransform.position.z);
	//		yield return TypeText (messageThrow, textSpring);
	//		yield return TypeText (messageHit, textDigging);
	//	} else {
	//		yield return TypeText (messageHit, textDigging);
	//		yield return TypeText (messageThrow, textSpring);
	//	}

	//	//done complete
	//	yield return new WaitForSeconds (1f);
	//	LoadingUIOilKing.s_Instance.m_IsDone = true;
	//	LoadingUIOilKing.s_Instance.imgGotoGame.gameObject.SetActive (true);
	//}

	public void StartSerif ()
	{
		LoadingUIOilKing.s_Instance.serifNormalPanel.SetActive (false);
		LoadingUIOilKing.s_Instance.panelShowSerif.gameObject.SetActive (true);
	}

	public void StartGame ()
	{
		
		OilKing_PlayUI.Instance.RaycastTargetButtons (false);
		OilKingGamePlay.Instance.gameObject.SetActive (true);
		Header.Instance.GetComponent <CanvasGroup> ().alpha = 1;
		OilKingFooter.Instance.GetComponent <CanvasGroup> ().alpha = 1;

		Tweener tweener = LoadingUIOilKing.s_Instance.panelShowSerif.gameObject.transform.DOScaleY (0, 0.3f);
		tweener.OnComplete (() => {
			Header.Instance.SetLife (LifeType.Time, (int)ParameterServer.GameTime);
			LoadingUIOilKing.s_Instance.gameObject.SetActive (false);

			//blink link character
			//			characterShowColor [LoadingUIOilKing.s_Instance.idSpring].arrCharacters [m_TmpSpring].gameObject.SetActive (true);
			//			characterShowColor [LoadingUIOilKing.s_Instance.idDigging].arrCharacters [m_TmpDigging].gameObject.SetActive (true);

			Header.Instance.PauseBtnInteractive (true);
			OilKingManager.s_Instance.StartGame ();
		});
	}

	void Update ()
	{
		BlinkMomentCharacter ();
	}

	void ProcessStringFromServer ()
	{
		Debug.Log ("STRING " + strFromServer);
		string[] arrRowCharacter = strFromServer.Split (',');

		for (int i = 0; i < arrRowCharacter.Length; i++) {
			int tmpIndexI = int.Parse (arrRowCharacter [i].Substring (0, 1)) - 1;
			arrRowCharacter [i] = arrRowCharacter [i].Substring (2);
			if (arrRowCharacter [i].Length > 0) {
				string[] tmpArrHasSpring = arrRowCharacter [i].Split ('|');

				for (int j = 0; j < tmpArrHasSpring.Length; j++) {
					int tmpIndexJ = int.Parse (tmpArrHasSpring [j].Trim ()) - 1;
					checkCombination [tmpIndexI, tmpIndexJ] = true;
					//checkCombination [tmpIndexJ, tmpIndexI] = true;
				}
			}
		}
	}

	//	void ShowColorForCharacter ()
	//	{
	//		characterShowColor [0].arrCharacters [0].color = arrColor [0];
	//		//set color for character 1 (at index 0)
	//		for (int j = 1; j < m_NumSpring; j++) {
	//			if (checkCombination [0, j]) {
	//				characterShowColor [0].arrCharacters [j].color = arrColor [j];
	//			}
	//		}
	//
	//		//set color for character 2-6 (at index 1-5)
	//		for (int i = 1; i < m_NumDigging; i++) {
	//
	//			characterShowColor [i].arrCharacters [0].color = arrColor [i];
	//
	//			for (int j = 1; j < m_NumSpring; j++) {
	//				if (checkCombination [i, j] && i != j) {
	//					characterShowColor [i].arrCharacters [j].color = arrColor [j];
	//				}
	//			}
	//
	//			if (checkCombination [i, 0]) {
	//				characterShowColor [i].arrCharacters [i].color = arrColor [0];
	//			}
	//		}
	//
	//		characterShowColor [LoadingUIOilKing.s_Instance.idSpring].arrCharacters [m_TmpSpring].color = arrColor [LoadingUIOilKing.s_Instance.idDigging];
	//	}

	string ReturnStringToServer ()
	{
		string tmpString = "";
		for (int i = 0; i < m_NumDigging; i++) {
			tmpString += (i + 1) + "-";

			for (int j = 0; j < m_NumSpring; j++) {
				if (checkCombination [i, j] || checkCombination [j, i]) {
					tmpString += (j + 1) + "|";
				}
			}

			if (tmpString [tmpString.Length - 1].Equals ('|')) {
				tmpString = tmpString.Substring (0, tmpString.Length - 1);
			}

			tmpString += ",";
		}

		tmpString = tmpString.Substring (0, tmpString.Length - 1);

		return tmpString;
	}

	public bool CheckSixBrothersMeet ()
	{
		for (int i = 0; i < m_NumDigging; i++) {
			for (int j = 0; j < m_NumSpring; j++) {
				if (!checkCombination [i, j] && i != j) {
					return false;
				}
			}
		}

		return true;
	}

	void BlinkMomentCharacter ()
	{
		m_TmpTimeBlink -= Time.deltaTime;
		if (m_TmpTimeBlink <= 0) {
			m_TmpTimeBlink = m_TimeBlink;
			m_CheckBlink = !m_CheckBlink;

//			characterShowColor [LoadingUIOilKing.s_Instance.idSpring].arrCharacters [m_TmpSpring].gameObject.SetActive (m_CheckBlink);
//			characterShowColor [LoadingUIOilKing.s_Instance.idDigging].arrCharacters [m_TmpDigging].gameObject.SetActive (m_CheckBlink);
		}
	}


	void DetermineIndexOfCharacter ()
	{
		m_TmpSpring = LoadingUIOilKing.s_Instance.idHit != 0 ? LoadingUIOilKing.s_Instance.idHit : LoadingUIOilKing.s_Instance.idThrow;
		m_TmpDigging = LoadingUIOilKing.s_Instance.idThrow != 0 ? LoadingUIOilKing.s_Instance.idThrow : LoadingUIOilKing.s_Instance.idHit;
	}

	IEnumerator ShowCharacterAndName (RectTransform recSpring, Text charactername, float newpos)
	{ 
		float time = 0.5f;
		recSpring.DOLocalMoveX (newpos, time);
		yield return new WaitForSeconds (time);
		charactername.gameObject.SetActive (true);
		yield return new WaitForSeconds (0.1f);
	}

	IEnumerator RunAndSwapSerif (int anime)
	{
		RectTransform recDigging = imgDigging.transform.GetComponent<RectTransform> ();
		RectTransform recSpring = imgSpring.transform.GetComponent<RectTransform> ();
		nameDigging.gameObject.SetActive (false);
		nameSpring.gameObject.SetActive (false);
		//image animation

		yield return ShowCharacterAndName (recDigging, nameDigging, diggingPos.x);
		yield return TypeText (m_NameDiggingMssg, nameDigging);
		yield return ShowCharacterAndName (recSpring, nameSpring, springPos.x);
		yield return TypeText (m_NameSpringMssg, nameSpring);
		if (anime == 1) {
			//Vector3 posTmpDigging = recDigging.position;
			//Vector3 posTmpSpring = recSpring.position;

			//recSpring.position = new Vector3(posTmpSpring.x, posTmpDigging.y, posTmpSpring.z);
			//recDigging.position = new Vector3(posTmpDigging.x, posTmpSpring.y, posTmpDigging.z);

			//textSpring.rectTransform.position = new Vector3(textSpring.rectTransform.position.x, m_PosTextDigging, textSpring.rectTransform.position.z);
			//textDigging.rectTransform.position = new Vector3(textDigging.rectTransform.position.x, m_PosTextSpring, textDigging.rectTransform.position.z);

			//yield return ShowCharacterAndName(recSpring, nameSpring, springPos.x);
			//yield return TypeText(m_NameSpringMssg, nameSpring);
			//yield return ShowCharacterAndName(recDigging, nameDigging, diggingPos.x);
			//yield return TypeText(m_NameDiggingMssg, nameDigging);
			imgSpring.RunAnim ();
			yield return TypeText (messageThrow, textSpring);
			imgSpring.StopAnim ();
			imgDigging.RunAnim ();
			yield return TypeText (messageHit, textDigging);
			imgDigging.StopAnim ();

		} else {
			imgDigging.RunAnim ();
			yield return TypeText (messageHit, textDigging);
			imgDigging.StopAnim ();
			imgSpring.RunAnim ();
			yield return TypeText (messageThrow, textSpring);
			imgSpring.StopAnim ();
		}
		imgDigging.StopAnim ();
		imgSpring.StopAnim ();

		//done complete
		yield return new WaitForSeconds (0.5f);
		LoadingUIOilKing.s_Instance.m_IsDone = true;
		LoadingUIOilKing.s_Instance.imgGotoGame.gameObject.SetActive (true);
	}

	Sprite GetColorFromID (int id)
	{
		Sprite mySprite = null;
		switch (id) {
		case 0:
			mySprite = LoadingUIOilKing.s_Instance.imageOfCharacter [0];
			break;
		case 1:
			mySprite = LoadingUIOilKing.s_Instance.imageOfCharacter [1];
			break;
		case 2:
			mySprite = LoadingUIOilKing.s_Instance.imageOfCharacter [2];
			break;
		case 3:
			mySprite = LoadingUIOilKing.s_Instance.imageOfCharacter [3];
			break;
		case 4:
			mySprite = LoadingUIOilKing.s_Instance.imageOfCharacter [4];
			break;
		case 5:
			mySprite = LoadingUIOilKing.s_Instance.imageOfCharacter [5];
			break;
		case 6:
			mySprite = LoadingUIOilKing.s_Instance.imageOfCharacter [6];
			break;
		case 7:
			mySprite = LoadingUIOilKing.s_Instance.imageOfCharacter [7];
			break;
		case 8:
			mySprite = LoadingUIOilKing.s_Instance.imageOfCharacter [8];
			break;
		case 9:
			mySprite = LoadingUIOilKing.s_Instance.imageOfCharacter [9];
			break;
		case 10:
			mySprite = LoadingUIOilKing.s_Instance.imageOfCharacter [10];
			break;
		case 11:
			mySprite = LoadingUIOilKing.s_Instance.imageOfCharacter [11];
			break;
		default:
			break;
		}
		return mySprite;
	}

	//void SetSpriteForBGCharacterTalking()
	//{
	//	imgDiggingTalkBG.sprite = LoadingUIOilKing.s_Instance.arrBGColor[LoadingUIOilKing.s_Instance.idDigging].sprite;
	//	imgSpringTalkBG.sprite = LoadingUIOilKing.s_Instance.arrBGColor[LoadingUIOilKing.s_Instance.idSpring + 6].sprite;
	//}
	void SetAnimationSerif ()
	{
		imgDigging.lstImage = new Sprite[3];
		imgSpring.lstImage = new Sprite[3];

		for (int i = 0; i < imgDigging.lstImage.Length; i++) {
			imgDigging.lstImage [i] = OilKingAssetLoader.s_Instance.getSpriteCharacter (LoadingUIOilKing.s_Instance.idHit + 1,
				TypeSprite.SuffixHitUI, i);
			
			imgSpring.lstImage [i] = OilKingAssetLoader.s_Instance.getSpriteCharacter (LoadingUIOilKing.s_Instance.idThrow + 1,
				TypeSprite.SuffixThrowUI, i);
		}
	}

}