using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class GachaController : MonoBehaviour
{

	public OnTweenDone onGachaFinish;

	public Image gachaImg;
	//Animation capsuleAnim;
	public Animation doorAnim;
	public Transform container_room;
	public Transform container_cardshow;
	public Sprite[] cardTypes;
	public Sprite[] cardTypeIcons;
	public Sprite[] ballTypes;
	public Material flowLightMat;
	public GameObject ballPrefab;
	public GameObject[] capsules;

	GachaRotate mGachaBtnRotate;
	Image[] doorBgImgs;
//0,1,2,3,4
	Image[] doorBgTextImgs;
	Image[] frameBackImgs;

	public GameObject[] centerPeoples;
	Image[] sidePeoples;
	Image img_mask;
	GachaColor mMaskColor;
	Transform container_stars;
	Transform container_doorbacks;

	Transform mDoorBgTextContainer;
	GachaSize mDoorBgTextSize;
	GachaColor mDoorBgTextColor;

	Image img_frame1;
	Image img_frame2;

	Image starBigImg;
	GachaSize mStarBigSize;
	GachaColor mStarBigColor;

	Transform container_capsules;
	Animation mCapsuleAnim;

	GameObject container_selectballs;
	GachaPosition[] mSelectBallPositions;
	List<GameObject> mSelectBalls;
	Transform container_selectballstartpos;
	Image[] mSelectBallPrefabs;

	Transform headTrans;
	GachaPosition mHeadPos;
	Transform container_headangry;
	Image img_light;

	Image img_mask1;
	GachaColor mMaskColor1;

	Image img_mask2;
	GachaColor mMaskColor2;

	Transform container_cardstartposition;
	Transform container_cardprefabs;
	GachaCard[] mCardPrefabs;
	Transform container_cardendpositions;
	Transform container_cardendposition1;
	Transform container_flypeoples;
	Image[] mCardEndPositions;
	List<GachaCard> mGachaCards;

	Image img_flasher;
	GachaSize mFlasherSize;
	Image img_totoko;
	GachaSize mTotokoSize;
	GachaColor mTotokoColor;
	Button nextBtn;

	Vector3 stepFrom = Vector3.zero;
	Vector3 stepTo = new Vector3 (0, 0, 180);
	float stepDur = 0.4f;
	Vector3 stepFrom1 = new Vector3 (0, 0, 180);
	Vector3 stepTo1 = new Vector3 (0, 0, 360);
	float stepDur1 = 0.4f;

	bool mIsShowUp;
	bool mIsReOpenDoors;
	bool mIsShowFlowLight;
	bool mIsShowLED;

	int mTargetGachaType = 0;
	int mPeopleCount = 2;
	List<GachaItem> mGachaItems;
	bool mIsInited;
	//List<int> mSelectBallIdxs;//0-4

	Transform mTrans;

	void Awake ()
	{
		Init ();
		#if UNITY_STANDALONE
		Screen.SetResolution(9 * 50	,16 * 50,false);
		#endif
	}

	void Init ()
	{
		if (mIsInited)
			return;
		mTrans = transform;
		container_room = mTrans.FindChild ("container_room");
		container_cardshow = mTrans.FindChild ("container_cardshow");
		doorAnim = mTrans.FindChild ("container_room/container_platform/container_doors").GetComponent<Animation> ();
		container_doorbacks = mTrans.FindChild ("container_room/container_platform/container_doorbacks");
		container_stars = mTrans.FindChild ("container_room/container_stars");
		//container_capsules = mTrans.FindChild ("container_room/container_capsules");
		//mCapsuleAnim = container_capsules.GetComponent<Animation> ();
		mGachaBtnRotate = gachaImg.GetComponent<GachaRotate> ();
		img_frame1 = mTrans.FindChild ("container_room/img_frame1").GetComponent<Image> ();
		img_frame2 = mTrans.FindChild ("container_room/img_frame2").GetComponent<Image> ();
		img_mask = mTrans.FindChild ("container_room/img_mask").GetComponent<Image> ();
		mMaskColor = img_mask.GetComponent<GachaColor> ();
		starBigImg = mTrans.FindChild ("container_room/img_starbig").GetComponent<Image> ();
		mStarBigSize = starBigImg.GetComponent<GachaSize> ();
		mStarBigColor = starBigImg.GetComponent<GachaColor> ();
		container_selectballs = mTrans.FindChild ("container_room/container_selectballs").gameObject;
		mSelectBallPositions = container_selectballs.GetComponentsInChildren<GachaPosition> ();
		container_selectballstartpos = mTrans.FindChild ("container_room/container_selectballstartpos");
		mSelectBallPrefabs = container_selectballstartpos.GetComponentsInChildren<Image> (true);
		headTrans = mTrans.FindChild ("container_room/img_head");
		mHeadPos = headTrans.GetComponent<GachaPosition> ();
		img_light = mTrans.FindChild ("container_room/container_light").GetComponent<Image>();
		container_headangry = mTrans.FindChild ("container_room/container_headangry");
		img_mask1 = mTrans.FindChild ("img_mask1").GetComponent<Image> ();
		mMaskColor1 = img_mask1.GetComponent<GachaColor> ();
		img_mask2 = mTrans.FindChild ("img_mask2").GetComponent<Image> ();
		mMaskColor2 = img_mask2.GetComponent<GachaColor> ();
		sidePeoples = mTrans.FindChild ("container_room/container_sidepeoples").GetComponentsInChildren<Image> (true);
		//centerPeoples = mTrans.FindChild ("container_room/container_platform/container_centerpeoples").GetComponentsInChildren<Image> (true);
		doorBgImgs = mTrans.FindChild ("container_room/container_platform/container_doorbacks").GetComponentsInChildren<Image> (true);
		mDoorBgTextContainer = mTrans.FindChild ("container_room/container_gachatext");
		mDoorBgTextColor = mDoorBgTextContainer.GetComponent<GachaColor> ();
		mDoorBgTextSize = mDoorBgTextContainer.GetComponent<GachaSize> ();
		doorBgTextImgs = mDoorBgTextContainer.GetComponentsInChildren<Image> (true);
		frameBackImgs = mTrans.FindChild ("container_room/container_framebacks").GetComponentsInChildren<Image> (true);
		container_cardstartposition = mTrans.FindChild ("container_cardshow/container_cardstartposition");
		container_cardprefabs = mTrans.FindChild ("container_cardshow/container_cardprefabs");
		mCardPrefabs = container_cardprefabs.GetComponentsInChildren<GachaCard> (true);
		container_cardendpositions = mTrans.FindChild ("container_cardshow/container_cardendpositions");
		container_cardendposition1 = mTrans.FindChild ("container_cardshow/container_cardendposition1");
		mCardEndPositions = container_cardendpositions.GetComponentsInChildren<Image> (true);
		img_flasher = mTrans.FindChild ("container_cardshow/img_flasher").GetComponent<Image> ();
		img_totoko = mTrans.FindChild ("container_cardshow/img_totoko").GetComponent<Image> ();
		container_flypeoples = mTrans.FindChild ("container_cardshow/container_flypeoples");
		mFlasherSize = img_flasher.GetComponent<GachaSize> ();
		mTotokoSize = img_totoko.GetComponent<GachaSize> ();
		mTotokoColor = img_totoko.GetComponent<GachaColor> ();
		nextBtn = mTrans.FindChild ("btn_next").GetComponent<Button> ();
		nextBtn.onClick.AddListener (OnNextClick);
		mIsInited = true;
	}

	void ResetToBegin ()
	{
		mIsReOpenDoors = false;
		mIsShowUp = false;
		mIsShowFlowLight = false;
		container_room.gameObject.SetActive (true);
		container_cardshow.gameObject.SetActive (false);
		container_doorbacks.gameObject.SetActive (false);
		nextBtn.gameObject.SetActive (false);
		mTrans.FindChild ("container_room/img_frame1").gameObject.SetActive (true);
		mTrans.FindChild ("container_room/img_frame2").gameObject.SetActive (true);
//		if(mTrans.FindChild ("img_back")!=null)mTrans.FindChild ("img_back").gameObject.SetActive(true);
		img_frame1.gameObject.SetActive (false);
		img_frame2.gameObject.SetActive (false);
		img_mask.gameObject.SetActive (true);
		img_mask1.gameObject.SetActive (false);
		img_mask2.gameObject.SetActive (false);
		container_stars.gameObject.SetActive (false);
		container_flypeoples.gameObject.SetActive (false);
		img_light.gameObject.SetActive (false);
		container_headangry.gameObject.SetActive (false);
		mDoorBgTextContainer.gameObject.SetActive (false);
		starBigImg.gameObject.SetActive (false);
		img_flasher.gameObject.SetActive (false);
		img_totoko.gameObject.SetActive (false);
		StopAllCoroutines ();
		doorAnim.Play ("GachaResetDoors");
		foreach(Image img in doorBgImgs){
			img.gameObject.SetActive (false);
		}
		if (mSelectBalls != null) {
			for (int i = 0; i < mSelectBalls.Count; i++) {
				Destroy (mSelectBalls [i]);
			}
		}
		if (mGachaCards != null) {
			for (int i = 0; i < mGachaCards.Count; i++) {
				Destroy (mGachaCards [i].gameObject);
			}
		}
		//doorAnim ["GachaOpenDoors"].time = 0.033f;
		//doorAnim ["GachaOpenDoors"].enabled = true;
		//doorAnim.Play ("GachaOpenDoors");
		//doorAnim.Play ("GachaOpenDoors");
		//doorAnim.Stop ();
		//doorAnim.Sample ();
	}

	void OnDisable(){
		//if (EventSystem.current != null)
		//	EventSystem.current.enabled = true;
	}

	//gachaType 0:normal,1:gold,2:recycle
	//peopleCount 1-6
	//items cards
	public void Play (int gachaType, int peopleCount, List<GachaItem> items,bool isShowLED = true)
	{
		//if (EventSystem.current != null)
		//	EventSystem.current.enabled = false;
		//GameObject go = GameObject.Find("Home");
		//go.transform.FindChild ("Canvas").gameObject.SetActive(false);
		GachaCard.playingGachaItemSound = null;
		gameObject.SetActive (true);
		Init ();
		ResetToBegin ();
		this.mTargetGachaType = gachaType;
		this.mGachaItems = items;
		this.mPeopleCount = peopleCount;
		this.mIsShowLED = isShowLED;
		if (peopleCount == 1) {
			mIsReOpenDoors = true;
		} else {
			mIsReOpenDoors = false;
		}
		for (int i = 0; i < mGachaItems.Count; i++) {
			if (mGachaItems [i].ball_color != mGachaItems [i].ball_color_show) {
				mIsShowUp = true;
				//mIsShowFlowLight = true;
				//break;
			}
			if(mGachaItems [i].ball_color ==5){
				mIsShowFlowLight = true;
			}
		}
		int capsuleAnimType = gachaType;
		if(capsuleAnimType == 8){
			capsuleAnimType = 1;
		}else if(capsuleAnimType == 9){
			capsuleAnimType = 8;
		}
		Debug.Log("capsuleAnimType:" + capsuleAnimType);
		for(int i=0;i< capsules.Length;i++)
		{
			if (capsuleAnimType == i + 1) {
				capsules [i].SetActive (true);
				mCapsuleAnim = capsules [i].GetComponent<Animation> ();
			} else {
				capsules [i].SetActive (false);
			}
		}
		//List<int> peopleImgIdxs = new List<int> ();

		for(int i = 1;i < centerPeoples.Length;i++){
			centerPeoples [i].GetComponentInChildren<Image>(true).enabled = false;
		}

		List<GameObject> tmpPeoples = new List<GameObject> (centerPeoples);
		List<GameObject> selectedPeoples = new List<GameObject> ();
		selectedPeoples.Add (tmpPeoples[0]);
		tmpPeoples[0].GetComponentInChildren<Image>(true).enabled = true;
		tmpPeoples.RemoveAt (0);
		for (int i = 1; i < mPeopleCount; i++) {
			if(tmpPeoples.Count == 0){
				Debug.LogError ("tmpPeoples count is 0");
				break;
			}
			GameObject p = tmpPeoples [Random.Range (0, tmpPeoples.Count)];
			selectedPeoples.Add (p);
			p.GetComponentInChildren<Image>(true).enabled = true;
			tmpPeoples.Remove (p);
		}

		/*
		Transform peopleParent = centerPeoples [0].transform.parent;
		for(int i = 1;i < mPeopleCount;i ++){
			peopleParent.GetChild (i).GetComponentInChildren<Image>(true).enabled = true;
		}
		*/
		//ShowImageByIndexs (centerPeoples, peopleImgIdxs);

		switch (mTargetGachaType) {
		case 1:
			ShowImageByIndexs (sidePeoples, new List<int> (new int[2]{ 0, 4 }));
			ShowImageByIndexs (frameBackImgs, new List<int> (new int[2]{0,9}));
			break;
		case 2:
			ShowImageByIndexs (sidePeoples, new List<int> (new int[2]{ 1, 5 }));
			ShowImageByIndex (frameBackImgs, 1);
			break;
		case 3:
			ShowImageByIndexs (sidePeoples, new List<int> (new int[2]{ 2, 6 }));
			ShowImageByIndexs (frameBackImgs, new List<int> (new int[2]{ 2, 3 }));
			break;
		case 4:
			ShowImageByIndexs (sidePeoples, new List<int> (new int[2]{ 2, 6 }));
			ShowImageByIndexs (frameBackImgs,new List<int> (new int[2]{ 2, 4 }));
			break;
		case 5:
			ShowImageByIndexs (sidePeoples, new List<int> (new int[2]{ 2, 6 }));
			ShowImageByIndexs (frameBackImgs, new List<int> (new int[2]{ 2, 5 }));
			break;
		case 6:
			ShowImageByIndexs (sidePeoples, new List<int> (new int[2]{ 2, 6 }));
			ShowImageByIndexs (frameBackImgs, new List<int> (new int[2]{ 2, 6}));
			break;
		case 7:
			ShowImageByIndexs (sidePeoples, new List<int> (new int[2]{ 2, 6 }));
			ShowImageByIndexs (frameBackImgs,new List<int> (new int[2]{ 2, 7 }));
			break;
		case 8:
			ShowImageByIndexs (sidePeoples, new List<int> (new int[2]{ 0, 4 }));
			ShowImageByIndexs (frameBackImgs, new List<int> (new int[2]{0,8}));
			break;
		case 9:
			ShowImageByIndexs (sidePeoples, new List<int> (new int[2]{ 3, 7 }));
			ShowImageByIndexs (frameBackImgs, new List<int> (new int[1]{10}));
			break;
		default:
			ShowImageByIndexs (sidePeoples, new List<int> (new int[2]{ 0, 3 }));
			Debug.LogError ("target gachaType: " + mTargetGachaType + " not exsiting");
			break;
		}
		mGachaBtnRotate.ResetToBegin ();
		//mGachaBtnRotate.onTweenDone = PlayCapsuleAnim;
		//panelAnim.Play ();//main entre;
		StartCoroutine (_Play ());

	}

	float mDelta = 0.03333f;
	bool isPause;
	IEnumerator _Play ()
	{
		int frame = 1;
		yield return new WaitForSeconds (mDelta * frame);
		//HideMask ();
		frame = 17;
		yield return new WaitForSeconds (mDelta * frame);

		GachaButtonPlayForward ();
		frame = 45;
		yield return new WaitForSeconds (mDelta * frame);

		GachaButtonPlayBack ();
		frame = 33;
		yield return new WaitForSeconds (mDelta * frame);

		if (mIsShowLED){
			ShowFrameFlash ();
			frame = 38;
			yield return new WaitForSeconds (mDelta * frame);
			OpenTheDoors ();
			frame = 107;
			yield return new WaitForSeconds (mDelta * frame);

			if (mIsReOpenDoors) {
				ReOpenTheDoors ();
				frame = 15;
				yield return new WaitForSeconds (mDelta * frame);
				for(int i = 1;i < centerPeoples.Length;i++){
				//	if (Random.Range (0, 2) == 1) {
				//		centerPeoples [i].transform.SetSiblingIndex (Random.Range(1,centerPeoples.Length));
				//	}
					centerPeoples [i].GetComponentInChildren<Image>(true).enabled = true;
				}

				//List<int> peopleImgIdxs = new List<int> ();
				//for (int i = 0; i < 6; i++) {
				//	peopleImgIdxs.Add (i);
				//}
				//ShowImageByIndexs (centerPeoples, peopleImgIdxs);

				mPeopleCount = 6;
				frame = 15;
				yield return new WaitForSeconds (mDelta * frame);
			}
			ShowDoorBack ();
			frame = 5;
			yield return new WaitForSeconds (mDelta * frame);

			ShowDoorBackText ();
			frame = 25;
			yield return new WaitForSeconds (mDelta * frame);

			//HideDoorBackText ();
			frame = 2;
			yield return new WaitForSeconds (mDelta * frame);
		}

		SelectTenBalls ();
		isPause = true;
		while (isPause) {
			yield return null;
		}

		ShowHead ();
		frame = 32;
		yield return new WaitForSeconds (mDelta * frame);

		ShowHeadAngry ();
		frame = 10;
		yield return new WaitForSeconds (mDelta * frame);

		img_light.gameObject.SetActive (true);
		frame = 8;
		yield return new WaitForSeconds (mDelta * frame);

		ShowMask1 ();
		frame = 19;
		yield return new WaitForSeconds (mDelta * frame);

		ChangeToCardShow ();
		frame = 13;
		yield return new WaitForSeconds (mDelta * frame);

		container_flypeoples.gameObject.SetActive (true);
		frame = 5;
		yield return new WaitForSeconds (mDelta * frame);

		ShowCards ();
		isPause = true;//wait untill card finish;
		while (isPause) {
			yield return null;
		}

		if (mIsShowUp) {
			ShowUp ();
			frame = 115;
			yield return new WaitForSeconds (mDelta * frame);
			if (mIsShowFlowLight) {
				frame = 3;
				yield return new WaitForSeconds (mDelta * frame);
			}
		}
		ShowFlasher ();
		ShowTotoko ();
		frame = 30;
		yield return new WaitForSeconds (mDelta * frame);
		
		//frame = 5;
		//yield return new WaitForSeconds (mDelta * frame);

		if (mIsShowFlowLight) {
			ShowCardFlowLight ();
			frame = 15;
			yield return new WaitForSeconds (mDelta * frame);
		}

		ToggleCard ();
		frame = 54;
		yield return new WaitForSeconds (mDelta * frame);

		ShowCardIcons ();
		frame = 11;
		yield return new WaitForSeconds (mDelta * frame);

		ShowBtnNext ();


		yield return null;
	}

	void HideMask ()
	{
		mMaskColor.Play ();
	}

	void GachaButtonPlayForward ()
	{
		mGachaBtnRotate.from = this.stepFrom;
		mGachaBtnRotate.to = this.stepTo;
		mGachaBtnRotate.duration = this.stepDur;
		mGachaBtnRotate.Play ();
		StartCoroutine (_PlayeAnim(0.3f));
		if (ComponentConstant.SOUND_MANAGER != null)
		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.gacha01_handle);
	}

	IEnumerator _PlayeAnim(float delay){
		yield return new WaitForSeconds (delay);
		mCapsuleAnim["GachaCapsulesAnim"].speed = 1.5f;
		mCapsuleAnim.Play ("GachaCapsulesAnim");
	}

	void GachaButtonPlayBack ()
	{
		mGachaBtnRotate.from = this.stepFrom1;
		mGachaBtnRotate.to = this.stepTo1;
		mGachaBtnRotate.duration = this.stepDur1;
		mGachaBtnRotate.ResetToBegin ();
		mGachaBtnRotate.Play ();
		//mCapsuleAnim.Stop ();
		//mCapsuleAnim.Blend("GachaCapsulesAnim1");
		StartCoroutine (_PlayeAnim(0.3f));
		if (ComponentConstant.SOUND_MANAGER != null)
		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.gacha01_handle);
	}

	void ShowFrameFlash ()
	{
		img_frame1.gameObject.SetActive (true);
		img_frame2.gameObject.SetActive (true);
		if (ComponentConstant.SOUND_MANAGER != null)
		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.gacha02_1_fusumaopen);
	}

	void OpenTheDoors ()
	{
		doorAnim.Play ("GachaOpenDoors");
		container_stars.gameObject.SetActive (true);
	}

	void ReOpenTheDoors ()
	{
		doorAnim.Play ("GachaCloseOpenDoors");
		if (ComponentConstant.SOUND_MANAGER != null)
		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.gacha02_2_fusumarepeat);
	}

	void ShowDoorBack ()
	{
		container_doorbacks.gameObject.SetActive (true);
		ShowImageByIndex (doorBgImgs, mPeopleCount - 2);
	}

	void ShowDoorBackText ()
	{
		if (ComponentConstant.SOUND_MANAGER != null)
		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.gacha03_1_nomal);
		if (mPeopleCount == 6)
			if (ComponentConstant.SOUND_MANAGER != null)
				ComponentConstant.SOUND_MANAGER.Play(SoundEnum.gacha03_2_congrats);
		ShowImageByIndex (doorBgTextImgs, mPeopleCount - 2);
		mDoorBgTextColor.targetImage = doorBgTextImgs [mPeopleCount - 2];
		mDoorBgTextContainer.gameObject.SetActive (true);
		starBigImg.gameObject.SetActive (true);
		//mDoorBgTextSize.gameObject.SetActive (true);
		//mStarBigSize.gameObject.SetActive (true);
		mTrans.FindChild ("container_room/container_sidepeoples/container_leftside").GetComponent<Animation> ().Play ();
		mTrans.FindChild ("container_room/container_sidepeoples/container_rightside").GetComponent<Animation> ().Play ();
	}

	/*
	void HideDoorBackText ()
	{
		mDoorBgTextColor.targetImage = doorBgTextImgs [mPeopleCount - 2];
		mDoorBgTextColor.Play ();
	}
*/
	void ShowImageByIndexs (GameObject[] imgs, List<int> idxs)
	{
		for (int i = 0; i < imgs.Length; i++) {
			if (imgs [i].activeInHierarchy) {
				imgs [i].SetActive (false);
			}
		}
		for (int i = 0; i < idxs.Count; i++) {
			if (idxs.Contains (idxs [i]) && !imgs [idxs [i]].activeInHierarchy) {
				imgs [idxs [i]].SetActive (true);
			}
		}
	}

	void ShowImageByIndexs (Image[] imgs, List<int> idxs)
	{
		for (int i = 0; i < imgs.Length; i++) {
			if (imgs [i].gameObject.activeInHierarchy) {
				imgs [i].gameObject.SetActive (false);
			}
		}
		for (int i = 0; i < idxs.Count; i++) {
			if (idxs.Contains (idxs [i]) && !imgs [idxs [i]].gameObject.activeInHierarchy) {
				imgs [idxs [i]].gameObject.SetActive (true);
			}
		}
	}

	void SelectTenBalls ()
	{
		StartCoroutine (_SelectTenBalls ());
	}

	void ShowHead ()
	{
		if (ComponentConstant.SOUND_MANAGER != null)
		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.gacha05_osomatsu);
		headTrans.gameObject.SetActive (true);
		mHeadPos.Play ();
	}

	void ShowHeadAngry ()
	{
		if (ComponentConstant.SOUND_MANAGER != null)
		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.gacha06_osomatsuopen);
		headTrans.gameObject.SetActive (false);
		container_headangry.gameObject.SetActive (true);
		//StartCoroutine (_HideDelay (container_headangry.gameObject, 0.83f));
	}

	void ShowMask1 ()
	{
		img_mask1.gameObject.SetActive (true);
		mMaskColor1.Play ();
	}

	void ChangeToCardShow ()
	{
		this.container_room.gameObject.SetActive (false);
		this.container_cardshow.gameObject.SetActive (true);
	}

	void ShowCards ()
	{
		StartCoroutine (_ShowCards ());
	}

	void ShowCardFlowLight(){
		StartCoroutine (_ShowFlowLight());
	}

	void ShowUp ()
	{
		bool isUpAudio = false;
		for (int i = 0; i < mGachaCards.Count; i++) {
			if (mGachaCards [i].gachaCardItem.ball_color != mGachaCards [i].gachaCardItem.ball_color_show && !isUpAudio) {
				isUpAudio = true;
				mGachaCards [i].PlayFlash (true);
			} else {
				mGachaCards [i].PlayFlash ();
			}
		}
	}

	void ShowFlasher ()
	{
		img_flasher.gameObject.SetActive (true);
		if (ComponentConstant.SOUND_MANAGER != null)
		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.gacha10_explosion);
		//mFlasherSize.Play ();
		//mTotokoColor.Play ();
	}

	void ShowTotoko ()
	{
		img_totoko.gameObject.SetActive (true);
		//mTotokoSize.Play ();
	}

	void ToggleCard ()
	{
		StartCoroutine (_ToggleCard ());
	}

	void ShowCardIcons ()
	{
		for (int i = 0; i < mGachaCards.Count; i++) {
			mGachaCards [i].ShowIcon (mGachaItems [i].isNew);
		}
//		if (gachaNext != null)
//			AudioSource.PlayClipAtPoint (gachaNext,Vector3.zero);
	}

	void ShowBtnNext ()
	{
		nextBtn.gameObject.SetActive (true);
		if (ComponentConstant.SOUND_MANAGER != null)
		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.gacha12_next);
	}

	void OnNextClick ()
	{
		mMaskColor2.gameObject.SetActive (true);
		mMaskColor2.onTweenDone = Close;
		mMaskColor2.Play ();
		StartCoroutine (_CloseCardShow ());
		nextBtn.gameObject.SetActive (false);
		if (ComponentConstant.SOUND_MANAGER != null)
		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se01_ok);
	}

	IEnumerator _CloseCardShow ()
	{
		yield return new WaitForSeconds (0.25f);
		this.container_cardshow.gameObject.SetActive (false);
		if (onGachaFinish != null) {
			onGachaFinish ();
			//ComponentConstant.SOUND_MANAGER.Play(SoundEnum.gacha07_2_cardrare);
		}
	}

	void Close ()
	{
		gameObject.SetActive (false);
	}

	float mToggleCardInterval = 0.15f;

	IEnumerator _ToggleCard ()
	{
		for (int i = 0; i < mGachaCards.Count; i++) {
			mGachaCards [i].ToggleCard ();
			if (ComponentConstant.SOUND_MANAGER != null)
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.gacha11_cardflip);
			yield return new WaitForSeconds (mToggleCardInterval);
		}
		yield return null;
	}

	IEnumerator _ShowCards ()
	{
		float interval;
		mGachaCards = new List<GachaCard> ();
		for (int i = 0; i < mGachaItems.Count; i++) {
			if (i == 0 || i == 1) {
				interval = 6 / 30f;
			} else if (i >= 2 && i <= 6) {
				interval = 5 / 30f;
			} else {
				interval = 4 / 30f;
			}
			//GameObject prefab = mCardPrefabs [this.mGachaItems [i].itemType].gameObject;
			GameObject go = Instantiate (mCardPrefabs [0].gameObject, container_cardstartposition.position, Quaternion.identity) as GameObject;
			GachaCard gachaCard = go.GetComponent<GachaCard> ();
			if(mGachaItems [i].ball_color_show != mGachaItems [i].ball_color){
				if (ComponentConstant.SOUND_MANAGER != null)
				ComponentConstant.SOUND_MANAGER.Play(SoundEnum.gacha07_3_ball);
			}
			else if (mGachaItems [i].ball_color_show > 3) {
				if (ComponentConstant.SOUND_MANAGER != null)
				ComponentConstant.SOUND_MANAGER.Play(SoundEnum.gacha07_2_cardrare);
			} else {
				if (ComponentConstant.SOUND_MANAGER != null)
				ComponentConstant.SOUND_MANAGER.Play(SoundEnum.gacha07_1_cardnomal);
			}
			gachaCard.Init ( mGachaItems [i],this);

			mGachaCards.Add (gachaCard);
			go.SetActive (true);
			Transform targetTrans = mCardEndPositions [i].transform;
			if (mGachaItems.Count == 1) {
				targetTrans = container_cardendposition1;
			}
			go.transform.SetParent (targetTrans);
			go.transform.localScale = Vector3.one;
			if (mGachaItems.Count == 1) {
				go.transform.localScale = Vector3.one * 1.5f;
			}
			StartCoroutine (_Move (go.transform, targetTrans,interval));
			yield return new WaitForSeconds (interval);
		}
		yield return new WaitForSeconds(0.5f);
		isPause = false;
	}

	IEnumerator _ShowFlowLight(){
		float t = 0f;
		float speed = 2f;
		while(t > -1f){
			t -= Time.deltaTime * speed;
			flowLightMat.SetFloat ("_SpeedX",t);
			yield return null;
		}	

		//Transform[] trans = UnityEditor.Selection.GetTransforms (UnityEditor.SelectionMode.Deep);
	}


	IEnumerator _HideDelay (GameObject go, float delay)
	{
		yield return new WaitForSeconds (delay);
		go.SetActive (false);
	}

	float mSelectBallInterval = 0.1f;
	float mMoveDuration = 0.15f;

	IEnumerator _SelectTenBalls ()
	{
		ballPrefab.SetActive (true);
		Image ballImg = ballPrefab.transform.FindChild ("img_ball").GetComponent<Image>();
		ballImg.sprite = this.ballTypes [mGachaItems [0].ball_color_show - 1];

		yield return new WaitForSeconds (0.8f);
		mSelectBalls = new List<GameObject> ();
		for (int i = 0; i < mGachaItems.Count; i++) {
			Transform targetTrans = mSelectBallPositions [i].transform;
			if(mGachaItems.Count == 1){
				targetTrans = mSelectBallPositions [7].transform;
			}
			ballImg.sprite = this.ballTypes [mGachaItems [i].ball_color_show - 1];
			GameObject go = Instantiate (ballPrefab, container_selectballstartpos.position, Quaternion.identity) as GameObject;
			if (ComponentConstant.SOUND_MANAGER != null)
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.gacha04_capsule);
			go.SetActive (true);
			go.transform.SetParent (targetTrans);
			go.transform.localScale = Vector3.one;
			mSelectBalls.Add (go);
			StartCoroutine (_Move (go.transform, targetTrans,mMoveDuration));
			if(i == mGachaItems.Count - 1){
				ballPrefab.SetActive (false);
			}
			yield return new WaitForSeconds (mSelectBallInterval + mMoveDuration);
		}
		isPause = false;
	}

	IEnumerator _Move (Transform trans, Transform targetTrans,float dur)
	{
		float t = 0;
		Vector3 startPos = trans.position;
		while (t < 1) {
			t += Time.deltaTime / dur;
			trans.position = Vector3.Lerp (startPos, targetTrans.position, t);
			yield return null;
		}
		trans.localPosition = Vector3.zero;
	}

	Image ShowImageByIndex (Image[] imgs, int index)
	{
		if (imgs.Length <= index) {
			Debug.LogError ("the index :" + index + " not exsiting");
			return null;
		}
		Image img = null;
		for (int i = 0; i < imgs.Length; i++) {
			if (i != index) {
				if (imgs [i].gameObject.activeInHierarchy)
					imgs [i].gameObject.SetActive (false);
			} else {
				if (!imgs [i].gameObject.activeInHierarchy)
					imgs [i].gameObject.SetActive (true);
				img = imgs [i];
			}
		}
		return img;
	}
	/*
	void PlayCapsuleAnim ()
	{
		capsuleAnim.Play ();

	}
	*/
}
/*
[System.Serializable]
public class GachaItem
{
	public bool isNew;
	//public bool isUp;
	[Range (1, 5)]
	public int ball_color_show = 1;
	[Range (1, 5)]
	public int ball_color = 1;
	public Sprite cardSprite;
	public Sprite cardFrameSprite;
}
*/