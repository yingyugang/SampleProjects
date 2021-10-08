using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace scene{
	public class GameLoader : MonoBehaviour {

		public GameObject timePrefab;

		void Start () {
			//StartCoroutine (_GetGameManager());
			//StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.assetbundleLoader.Load("Scene_GameManager",GetResource<AssetBundle>));
			StartCoroutine(ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAssetBundle ("Scene_GameManager" , GetResource<AssetBundle>));
			StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAssetBundle("Scene_CanvasBack",GetCanvasBack<AssetBundle>));
			StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAssetBundle("Scene_CanvasFrant",GetCanvasFrant<AssetBundle>));
			StartCoroutine (ComponentConstant.ASSETBUNDLE_RESOURCES_GETTER.GetAssetBundle("Scene_Ending",GetEnding<AssetBundle>));
			//Resources.UnloadUnusedAssets();
		}

		private void GetResource<T> (T t)
		{
			AssetBundle assetBundle = t as AssetBundle;
			GameObject prefab = assetBundle.LoadAsset<GameObject> ("Scene_GameManager");
			GameObject go = Instantiate (prefab) as GameObject;
		}

		private void GetCanvasBack<T> (T t)
		{
			AssetBundle assetBundle = t as AssetBundle;
			GameObject prefab = assetBundle.LoadAsset<GameObject> ("Scene_CanvasBack");
			GameObject go = Instantiate (prefab) as GameObject;
			go.transform.SetParent(UIManager.GetInstance ().CanvasBack.transform); 
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = Vector3.zero;
			go.transform.SetSiblingIndex (0);
			go.SetActive (true);
		}

		private void GetEnding<T>(T t)
		{
			AssetBundle assetBundle = t as AssetBundle;
			GameObject prefab = assetBundle.LoadAsset<GameObject> ("Scene_Ending");
			GameObject go = Instantiate (prefab) as GameObject;
			go.transform.SetParent(UIManager.GetInstance ().CanvasFrant.transform); 
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = Vector3.zero;
	
			go.GetComponent<RectTransform> ().SetInsetAndSizeFromParentEdge (RectTransform.Edge.Top, 0, 1920f);
			go.GetComponent<RectTransform> ().SetInsetAndSizeFromParentEdge (RectTransform.Edge.Left, 0, 1080f);
			UIManager uiMgr = UIManager.GetInstance ();
			uiMgr.ending = go;
			//go.transform.SetSiblingIndex (0);
			//go.SetActive (true);
		}

		private void GetCanvasFrant<T> (T t)
		{
			AssetBundle assetBundle = t as AssetBundle;
			GameObject prefab = assetBundle.LoadAsset<GameObject> ("Scene_CanvasFrant");
			GameObject go = Instantiate (prefab) as GameObject;
			go.transform.SetParent(UIManager.GetInstance ().CanvasFrant.transform); 
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = Vector3.zero;
			go.transform.SetSiblingIndex (0);
			go.SetActive (true);
			UIManager uiMgr = UIManager.GetInstance ();
			uiMgr.controllers = new System.Collections.Generic.List<Button>(go.transform.FindChild ("Controllers").GetComponentsInChildren<Button> (true));
			uiMgr.questionText = go.transform.FindChild ("QuestionBack/Question").GetComponent<Text> ();
			uiMgr.questionText.fontSize = 52;
			uiMgr.questionText.lineSpacing = 0.55f;
			uiMgr.questionText.transform.parent.gameObject.SetActive (false);
			uiMgr.questionTimeSlider = go.transform.FindChild ("Slider").GetComponent<Slider>();
			uiMgr.correctAnswer = go.transform.FindChild("Answer0").gameObject;
			uiMgr.wrongAnswer = go.transform.FindChild("Answer1").gameObject;
			uiMgr.timeoutAnswer = go.transform.FindChild("Answer2").gameObject;
			uiMgr.characterAnim = go.GetComponentInChildren<CharacterAnim> (true);
			uiMgr.characterAnim.transform.parent.SetSiblingIndex (0);


			uiMgr.txtCurrentStage = go.transform.FindChild ("ImageStage/TextStage").GetComponent<Text>();
			uiMgr.txtContinueCorrectAnswerSum = go.transform.FindChild ("ImageCombo/TextCombo").GetComponent<Text>();
			go.transform.FindChild ("ImageStage").SetSiblingIndex (0);
			go.transform.FindChild ("ImageCombo").SetSiblingIndex (0);

			Transform trans = go.transform.FindChild ("QuestionBack/Button/Text");
			if (trans != null)
				trans.gameObject.SetActive (false);

			uiMgr.jumpDialogBtn = go.transform.FindChild ("QuestionBack").GetComponentInChildren<Button>(true);
			uiMgr.jumpDialogBtn.enabled = false;
			go.transform.FindChild ("Text (1)").gameObject.SetActive(false);
			uiMgr.txtTimeAsign = go.transform.FindChild ("TimeNum").GetComponent<Text> ();
			uiMgr.txtTimeAsign.gameObject.SetActive (false);
			uiMgr.txtTimeAsign.fontStyle = FontStyle.Normal;
			uiMgr.txtTimeAsign.raycastTarget = false;
			//uiMgr.jumpDialogBtn.GetComponent<Image> ().eventAlphaThreshold = 0;
			uiMgr.timePrefab = timePrefab;
			uiMgr.Init ();
		}
	}
}
