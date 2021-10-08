using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GachaCard : MonoBehaviour {

	Image mImgBg;
	Image mImgBall;
	Image mImgCard;
	Image mImgBorder;
	Image mImgIcon;
	Image mImgIconNew;
	Image mImgBgFlowLight;

	Transform effect1;
	Transform effect2;
	Transform effect3;

	public AudioClip effectClip;
	public AudioClip effectClip1;
	public GachaItem gachaCardItem;
	void Awake(){
		
	}
	/*
	public bool isStart;
	void Update(){
		if(isStart){
			Init (null,null,null,true,true);
			PlayFlash ();
			isStart = false;
		}
	}
*/
	bool mIsUp;
	bool isShowFlowLight;
	public void Init (GachaItem item,GachaController gc){
		gachaCardItem = item;
		mIsUp = item.ball_color == item.ball_color_show ? false : true;
		 isShowFlowLight = item.ball_color == 5 ? true : false;
		Sprite cardType =  gc.cardTypes [item.ball_color - 1];
		Sprite cardTypeIcon = gc.cardTypeIcons [item.ball_color - 1];

		mImgBg = transform.FindChild ("img_bg").GetComponent<Image>();
		mImgBall = transform.FindChild ("img_ball/img_ball").GetComponent<Image>();
		mImgBall.sprite = gc.ballTypes [item.ball_color_show - 1];


		mImgCard = transform.FindChild ("img_card").GetComponent<Image>();
		mImgBgFlowLight = transform.FindChild ("img_bgflowlight").GetComponent<Image>();
		//mImgBgFlowLight.sprite = cardType;
		mImgBorder = transform.FindChild("img_card/img_border").GetComponent<Image>();
		mImgIcon = transform.FindChild ("img_card/img_icon").GetComponent<Image> ();
		mImgIconNew = transform.FindChild ("img_card/img_iconnew").GetComponent<Image> ();

		effect1 = transform.FindChild ("img_ball/Effect1");
		effect2 = transform.FindChild ("img_ball/Effect2");
		Image imgBall1 = effect2.FindChild ("img_ball").GetComponent<Image>();
		imgBall1.sprite = gc.ballTypes [item.ball_color - 1];
		effect3 = transform.FindChild ("Effect3");

		mImgBg.sprite = cardType;
		mImgIcon.sprite = cardTypeIcon;
		mImgCard.sprite = item.cardItem.cardSprite;
		mImgBorder.sprite = item.cardItem.borderSprite;
		if(!mIsUp && isShowFlowLight){
			mImgBgFlowLight.gameObject.SetActive (true);
		}
		if (mIsUp) {
			mImgBall.gameObject.SetActive (true);
			mImgBg.gameObject.SetActive (false);
		} else {
			mImgBall.gameObject.SetActive (false);
			mImgBg.gameObject.SetActive (true);
		}
	}
	bool mPlayAudio;
	public void PlayFlash(bool playAudio = false){
		if (mIsUp) {
			mPlayAudio = playAudio;
			GachaColor[] cols = mImgBall.GetComponentsInChildren<GachaColor> (true);
			for(int i=0;i<cols.Length;i++){
				cols [i].gameObject.SetActive (true);
				cols [i].Play ();
			}
			StartCoroutine (_ToggleFlasher());

		}
	}

	public static GameObject playingGachaItemSound = null;
	IEnumerator _ToggleFlasher(){
		//if (effectClip != null && mPlayAudio)
		//	AudioSource.PlayClipAtPoint (effectClip, Vector3.zero);
		if (playingGachaItemSound == null)
			playingGachaItemSound = gameObject;
		if (ComponentConstant.SOUND_MANAGER != null && playingGachaItemSound == gameObject) {
			Debug.Log ("ComponentConstant.SOUND_MANAGER.Play(SoundEnum.gacha08_discoloration);");
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.gacha08_discoloration);
		}


		transform.FindChild ("img_ball/img_ring").gameObject.SetActive(true);
		transform.FindChild ("img_ball/img_mask").gameObject.SetActive(true);
		yield return new WaitForSeconds (0.5f);
		transform.FindChild ("img_ball/img_ring").gameObject.SetActive(false);
		transform.FindChild ("img_ball/img_mask").gameObject.SetActive(false);
		effect1.gameObject.SetActive (true);
		StartCoroutine(_HideBall());

		yield return new WaitForSeconds (1f);
		//this.mImgBall.enabled = false;
		effect1.gameObject.SetActive (false);
		//yield return new WaitForSeconds (1f);
		effect2.gameObject.SetActive (true);
		yield return new WaitForSeconds (0.8f);
		effect1.gameObject.SetActive (true);

		yield return new WaitForSeconds (0.2f);
		//effect2.gameObject.SetActive (false);
		//if (effectClip1 != null && mPlayAudio)
		//	AudioSource.PlayClipAtPoint (effectClip1, Vector3.zero);
		if (ComponentConstant.SOUND_MANAGER != null && playingGachaItemSound == gameObject )
		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.gacha09_change);
		yield return new WaitForSeconds (0.4f);
		effect3.gameObject.SetActive (true);
		if (isShowFlowLight) {
			this.mImgBgFlowLight.gameObject.SetActive (true);
		} else {
			this.mImgBg.gameObject.SetActive (true);
		}

		yield return new WaitForSeconds (0.5f);
		if (isShowFlowLight) {
			mImgBgFlowLight.gameObject.SetActive (true);
			StartCoroutine (_ShowFlowLight(mImgBgFlowLight.material));
		}
		yield return new WaitForSeconds (1f);
		effect3.gameObject.SetActive (false);
		effect2.gameObject.SetActive (false);
		this.mImgBall.gameObject.SetActive (false);
		this.mImgBg.gameObject.SetActive (true);
		effect1.gameObject.SetActive (false);

	}

	IEnumerator _ShowFlowLight(Material flowLightMat)
	{
		float t = 0f;
		float speed = 2f;
		while(t > -1f){
			t -= Time.deltaTime * speed;
			flowLightMat.SetFloat ("_SpeedX",t);
			yield return null;
		}
		mImgBgFlowLight.gameObject.SetActive (false);
	}


	IEnumerator _HideBall(){
		float t = 0;
		float dur = 0.8f;
		Vector3 targetSize = new Vector3 (1.5f,1.5f,2);
		while(t < 1){
			t += Time.deltaTime / dur;
			mImgBall.color = new Color (1,1,1,1-t-0.3f);
			mImgBall.transform.localScale = Vector3.Lerp (Vector3.one,targetSize,t);
			yield return null;
		}
	}

	public void ShowIcon(bool isNew = false){
		if (isNew) {
			mImgIcon.gameObject.SetActive (false);
			mImgIconNew.gameObject.SetActive (true);
		} else {
			mImgIcon.gameObject.SetActive (true);
			mImgIconNew.gameObject.SetActive (false);
		}
	}

	public void ToggleCard(){
		StartCoroutine (_Toggle());
	}

	float t = 0;
	float mDuration = 0.17f;
	IEnumerator _Toggle(){
		Transform mImgBgTrans = mImgBg.transform;
		Transform mImgCardTrans = mImgCard.transform;
		mImgCardTrans.gameObject.SetActive (true);
		mImgCardTrans.transform.SetParent(mImgBgTrans.parent);
		//mImgCardTrans.transform.SetSiblingIndex (0);
		mImgBgTrans.localScale = new Vector3 (1, 1, 1);
		mImgCardTrans.localScale = new Vector3 (1,0,1);
		while(t < 1){
			t += Time.deltaTime / mDuration;
			t = Mathf.Clamp (t, 0, 1f);
			mImgBgTrans.localScale = new Vector3 (1, 1-t, 1);
			mImgBgFlowLight.transform.localScale = new Vector3 (1, 1-t, 1);
			yield return null;
		}
		mImgBgTrans.localScale = new Vector3 (1, 0, 1);
		t = 0;
		while(t < 1){
			t += Time.deltaTime / mDuration;
			t = Mathf.Clamp (t, 0, 1f);
			mImgCardTrans.localScale = new Vector3 (1, t, 1);
			yield return null;
		}
		mImgCardTrans.localScale = new Vector3 (1, 1, 1);
	}


}
