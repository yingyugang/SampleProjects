using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class Game12_GameIntro : MonoBehaviour {

	//--> Manga
	public Image OpeningAnime;
	public Image EndingAnime;
	public Sprite[] OpeningSprites;
	public Sprite[] EndingSprites;
	public GameObject Opening;
	public GameObject Ending;
	//--< Manga
	private GameObject Terrain_Standard;
	private MeshRenderer Terrain_Standard_Mesh;
	void Start(){
		//--> Listener
		Game12_GUI_Manager.instance.OnStartGameListener(OnStartGameListener);
		Game12_GUI_Manager.instance.OnGameOverListener(OnGameOverListener);
		//--< Listener
	}
	void Update(){
		if(Game12_GameManager.instance.Ending && (Game12_GameManager.instance.myState == Game12_GameState.pause)){
			if(Input.GetMouseButtonDown(0)){
				StopAllCoroutines();
				Game12_GameManager.instance.SetGameOver();
				Ending.SetActive(false);
			}
		}
	}
	public void OnStartGameListener(){
		Game12_GUI_Manager.instance.RemoveStartGameListener(OnStartGameListener);
		Opening.SetActive(true);
		StartCoroutine(PlayOpening());
	}
	public void OnGameOverListener(){
		// Play sound fx
		if (this == null)
			return;
		Game12_GUI_Manager.instance.PlaySound(SoundEnum.bgm15_title_back);
		Debug.Log ("OnGameOverListener");
		if(gameObject && gameObject.activeSelf == false){
			gameObject.SetActive(true);
		}
		Ending.SetActive(true);
		Game12_GameManager.instance.Ending = true;
		StartCoroutine(PlayEnding());
		Game12_GUI_Manager.instance.RemoveGameOverListener(OnGameOverListener);
	}
	//--> Play opening like a manga
	IEnumerator PlayOpening(){
		float time = 4.5f;
		while(time > 0){
			OpeningAnime.rectTransform.localScale += new Vector3(.0003f, .0003f, .0003f);
			time -= 0.01f;
			if(time < 1.0f)OpeningAnime.sprite = OpeningSprites[3];
			else if(time < 2.0f)OpeningAnime.sprite = OpeningSprites[2];
			else if(time < 3.0f)OpeningAnime.sprite = OpeningSprites[1];
			yield return 0;
		}
		// Allow touch to play
		Game12_GUI_Manager.instance.TouchArea.SetActive(true);
		Game12_GameManager.instance.myState = Game12_GameState.start;
		Game12_GUI_Manager.instance.G_Header.gameObject.SetActive(true);
		Game12_GUI_Manager.instance.G_Footer.gameObject.SetActive(true);
		Opening.SetActive(false);
		this.gameObject.SetActive(false);
	}
	//--< Play opening like a manga

	//--> Ending
	IEnumerator PlayEnding(){
		yield return new WaitForSeconds(2.0f);
		while(Game12_GameManager.instance.Ending){
			for(int i = 0; i < EndingSprites.Length; i++){
				yield return new WaitForSeconds(0.4f);
				EndingAnime.sprite = EndingSprites[i];				
			}
			Game12_Player_RunningEvent.instance.RemoveOnOnstacleCollisionListener(Game12_GUI_Manager.instance.GUI_Obstacle_Listener);
			Game12_Player_RunningEvent.instance.RemoveOnOnstacleCollisionListener(Game12_Player_Controller.instance.PlayerController_Obstacle_Listener);
			Game12_Player_RunningEvent.instance.RemoveOnOnstacleCollisionListener(Game12_GameManager.instance.GameManager_Obstacle_Listener);
			Game12_GUI_Manager.instance.RemoveStartGameListener(OnStartGameListener);
			Game12_GUI_Manager.instance.RemoveGameOverListener(OnGameOverListener);
			yield return new WaitForSeconds(3.0f);
		}
		Game12_GameManager.instance.SetGameOver();
	}
	//--< Ending


}
