using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace SixRun{
	public class GUIManager : SixRunSingleMono<GUIManager> {

		public List<GameObject> backs;
		public Dictionary<string,GameObject> backDic;
		CanvasGroup preBack;
		CanvasGroup currentBack;
		public float backgroundToggleDuration = 3;

		public Material grayMat;

		public bool isPaused;
		public List<BackgroundData> backgroundDatas;
		public GameObject ending;
		public GameObject cutIn;
		public Sprite cheatCutIn;

		void Start(){
			backgroundDatas = GameParams.GetInstance ().backgroundDatas;
			backDic = new Dictionary<string, GameObject> ();
			for(int i=0;i<backs.Count;i++){
				backDic.Add (backs[i].name,backs[i]);
			}
			ending.transform.SetParent (GUIFrant.GetInstance().frameCanvas.transform);
			cutIn.transform.SetParent (GUIFrant.GetInstance().frameCanvas.transform);

			#if UNITY_EDITOR
			Shader shader = Shader.Find ("UI/Default-Gray");
			GUIManager.GetInstance ().grayMat.shader = shader;
			#endif
		}

		public float distance;

		void Update(){
			if(GameManager.GetInstance().isPaused){
				return;
			}
			if (GameParams.GetInstance ().backgroundDatas.Count == 0)
				return;
			if(!mIsToggle && distance >= backgroundDatas[0].appear_distance){
				BackgroundData bgd =backgroundDatas[0];
				backgroundDatas.RemoveAt(0);
				GameObject go;
				if(backDic.TryGetValue(bgd.image_resource,out go)){
					go.SetActive (true);
					currentBack = go.GetComponent<CanvasGroup> ();
					StartCoroutine (_ToggleBackground());
				}
			}
		}

		public void CutIn(){
			StartCoroutine (HideCutIn());
		}

		IEnumerator HideCutIn()
		{
			//GameManager.GetInstance ().PauseGame ();
			if (ComponentConstant.SOUND_MANAGER != null)
				ComponentConstant.SOUND_MANAGER.Play (SoundEnum.bgm10_invicible);
			CheatData mCheatData = CheatController.GetLastMatchCheat ();
			if (mCheatData!=null && mCheatData.key != "1") {
				Image img = cutIn.transform.FindChild("CutIn").GetComponentInChildren<Image> (true);
				img.sprite = cheatCutIn;
			}
			GameManager.GetInstance ().Pause1 ();
			cutIn.SetActive(true);
			yield return StartCoroutine(WaitForRealTime(1.5f));
			cutIn.SetActive(false);
			GameManager.GetInstance ().Resume1 ();
			//GameManager.GetInstance ().ResumeGame ();
		}

		public IEnumerator WaitForRealTime(float delay)
		{
			while (true)
			{
				float pauseEndTime = Time.realtimeSinceStartup + delay;
				while (Time.realtimeSinceStartup < pauseEndTime)
				{
					yield return 0;
				}
				break;
			}
		}




		bool mIsToggle;
		IEnumerator _ToggleBackground(){
			mIsToggle = true;
			float t = 0;
			while(t<1){
				t += Time.deltaTime / backgroundToggleDuration;
				if(preBack!=null)
					preBack.alpha = 1 - t;
				currentBack.alpha = t;
				yield return null;
			}
			if (preBack != null) {
				preBack.gameObject.SetActive (false);
			}
			currentBack.alpha = t;
			preBack = currentBack;
			mIsToggle = false;
		}

		public void ShowEnding()
		{
			Invoke ("_CloseEnding",2);

			ending.SetActive(true);
		}

		void _CloseEnding(){
			ending.GetComponentInChildren<Button> (true).onClick.AddListener(CloseEnding);
		}

		public void CloseEnding()
		{
			ending.SetActive(false);
			GameManager.GetInstance().SendGameEndingAPI();
			ComponentConstant.SOUND_MANAGER.StopBGM();
		}
	}
}