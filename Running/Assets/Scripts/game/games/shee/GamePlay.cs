using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GamePlay : MonoBehaviour {

	public QuestionManager 	MyQuestion;
	public Slider 			FeverSlider;
	public EndGame 			Ending;
	public Character 		IyamiCharacter;
	public GameObject		Arrow;
	public GameObject		RightCircle;
	public GameObject		pauseBtn;

	//Particle FeverTime
	public GameObject FeverParticle;
	public GameObject FooterParticle;

	[HideInInspector]public int 	MAX_Combo = 0;
	[HideInInspector]public int 	totalCombo = 0;
	[HideInInspector]public int 	numCombo = 0;
	[HideInInspector]public bool 	IsSwiped;
	[HideInInspector]public bool 	IsStartPlay = false;

	[HideInInspector]public bool 	IsFever = false;


	//Time in Game
	public  float 	SectionTime;
	public  float 	FailTime;
	public  float 	delayHide;
	public  float 	delayShee;
	public  float 	timeCountDown;

	private float 	m_GameTime;
	private float 	m_CountTime;

	//Point in Game
	private int 	m_Score = 0;
	private int 	m_Combo = 0;

	void Start(){
		m_GameTime = Game10_Manager.instance.GameTime;
		FeverSlider.maxValue = Game10_Manager.instance.FeverTime;
		FeverSlider.value = FeverSlider.maxValue;
//		SheeSoundManager.instance.PlayBackground(SheeType.PRACTICE);
		//Game10_Manager.instance.PlaySound(SoundEnum.bgm08_shee);
	}

	public void Run(){
		Waiting_Question();
	}

	void Update(){
		if(Game10_Manager.instance.GameState != SHEESTATE.PLAYING || Game10_Manager.instance.isEnded) return;

		if(IsFever){
			Time_Fever();
		}
		//else{
			Time_CountDown();
		//}
		TimeFlick();
	}

	//-->Countdown Game time
	void Time_CountDown(){
		m_GameTime -= Time.deltaTime;
		if(m_GameTime <= timeCountDown && m_GameTime > 0f){
			timeCountDown--;
			Debug.Log(Mathf.RoundToInt(m_GameTime));
			Game10_Manager.instance.PlaySound(SoundEnum.se06_timeup_countdown);
		}
		if(m_GameTime <= 0f) {
			Game10_Manager.instance.PlaySound(SoundEnum.se07_timeup);
			EndGame();
		}
	}
	//--<

	//-->Countdown flick time
	void TimeFlick(){
		if(!IsSwiped) return;
		m_CountTime += Time.deltaTime;
		if(m_CountTime > SectionTime){
			Answer(Swipe.None);
		}
	}
	//--<

	//-->Countdown fever time
	void Time_Fever(){
		FeverParticle.gameObject.SetActive(true);
		FeverSlider.value -= Time.deltaTime;
		if(FeverSlider.value <= 0){
//			Header.Instance.ResumeTimer();
			ResetFever();
//			SheeSoundManager.instance.PlayBackground(SheeType.PRACTICE);
			Game10_Manager.instance.PlaySound(SoundEnum.bgm08_shee);
		}
	}
	//--<

	void ResetFever(){
		IsFever = false;
		GUIManager.instance.UIFooter.SetActive(false);
		FeverSlider.value = Game10_Manager.instance.FeverTime;
		FeverParticle.gameObject.SetActive(false);
	}

	void Play(){
		Debug.Log("Playing");
		IsSwiped = true;
		Arrow.SetActive(true);
		MyQuestion.Get_Question(IsFever);
		IyamiCharacter.Open(MyQuestion.GetCurrentQuestion().Open);
//		IyamiCharacter.ChangeAction(MyQuestion.GetCurrentQuestion().Open);
//		SheeSoundManager.instance.PlayEffect(SheeType.OPEN);
		Game10_Manager.instance.PlaySound(SoundEnum.SE30_shee_open);
	}

	public void Waiting_Question(){
		Debug.Log("Waiting");
		IsSwiped = false;
		m_CountTime = .0f;
		MyQuestion.Hide_Question();
		IyamiCharacter.ResetAction();
		RightCircle.SetActive(false);
		//Play: allow swipe
		StartCoroutine(DelayAction(delayHide, () => Play()));
	}

	public void Answer(Swipe sw){
		IsSwiped = false;
		if(MyQuestion.Get_Answer(sw)){
		//-->Correct
			Header.Instance.SetScore(++m_Score+"回");
			CheckCombo();
			IyamiCharacter.ChangeAction(MyQuestion.GetCurrentQuestion().Action);
			RightCircle.SetActive(true);
//			SheeSoundManager.instance.PlayEffect(SheeType.RIGHT);
			Game10_Manager.instance.PlaySound(SoundEnum.SE29_shee_right);
			//-->Check Special
			if(MyQuestion.Check_Akatsuka()){
				SpecialEffect();
			}//--<

			//-->Check Handsome
			else if(MyQuestion.Check_Handsome()){
				HandsomeEffect();
			}//--<

			else{
				StartCoroutine(DelayAction(delayShee, () => Waiting_Question()));
			}
		//--<
		}else{
		//-->Wrong
//			SheeSoundManager.instance.PlayEffect(SheeType.MISS);
			Game10_Manager.instance.PlaySound(SoundEnum.se13_miss);
			Arrow.SetActive(false);
			IyamiCharacter.ActionFail();
			StartCoroutine(DelayAction(FailTime, () => Waiting_Question()));
			CountCombo(0);
			GUIManager.instance.SetCombo(m_Combo);
			Game10_Manager.instance.noMiss = 0;
		//--<
		}
	}

	void CountCombo(int reset){
		if(m_Combo >= 2) {
			totalCombo += m_Combo - 1;
			numCombo++;
		}
		m_Combo = reset;
	}

	void SpecialEffect(){
//		SheeSoundManager.instance.PlayEffect(SheeType.BONUS);
		Game10_Manager.instance.PlaySound(SoundEnum.se12_bonusitem);
		GUIManager.instance.OpenCutinEffect(GUIManager.instance.feverSye);
		IsFever = true;
		MyQuestion.RandomFeverQuestion ();
		GUIManager.instance.UIFooter.SetActive(true);
		Game10_Manager.instance.isFever = 1;
//		Header.Instance.PauseTimer();
//		SheeSoundManager.instance.PlayBackground(SheeType.INVICIBLE);
		Game10_Manager.instance.PlaySound(SoundEnum.bgm10_invicible);
	}

	void HandsomeEffect(){
		GUIManager.instance.OpenCutinEffect(GUIManager.instance.other);
		Game10_Manager.instance.isSyonosukeShow = 1;
	}

	void CheckCombo(){
		if(m_CountTime <= Game10_Manager.instance.CombSpan){			
			m_Combo ++;
			if(MAX_Combo < m_Combo){
				MAX_Combo = m_Combo;
				GUIManager.instance.MaxCombo.text = MAX_Combo + "";
			} 
		}else{
			CountCombo(1);
		}
		GUIManager.instance.SetCombo(m_Combo);
	}


	//-->time delay action
	public IEnumerator DelayAction(float dtime, System.Action callback)
	{
		float timeDelay = 0f;
		while (timeDelay < dtime) {
			if(Game10_Manager.instance.GameState == SHEESTATE.PLAYING){
				timeDelay += Time.deltaTime;
			}
			yield return new WaitForEndOfFrame();


		}
		yield return new WaitForEndOfFrame();
//		yield return new WaitForSeconds(dtime);
		callback();
	}
	//--<

	void EndGame(){
		ResetFever();
		IsSwiped = false;
		pauseBtn.GetComponent<Image>().raycastTarget = false;
		Game10_Manager.instance.isEnded = true;
		StopAllCoroutines();
		Game10_Manager.instance.PlaySound(SoundEnum.bgm15_title_back);
		Ending.gameObject.SetActive(true);
		Ending.ShowEnding();
		CountCombo(0);
	}

	public int GetScore(){
		return m_Score;
	}
}
