using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GachaRandomBallColor : MonoBehaviour {

	public bool loadImage;
	public bool setImage;
	public int[] types;
	public Image[] imgs;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Application.isPlaying)
			return;
		if(loadImage){
			loadImage = false;
			LoadImage ();
		}
		if (setImage) {
			setImage = false;
			SetImages ();
		}
	}

	void LoadImage(){
		imgs = GetComponentsInChildren<Image> ();
		types = new int[imgs.Length];
		GachaController gc = FindObjectOfType<GachaController> ();
		List<Sprite> ballTypes = new List<Sprite> (gc.ballTypes);
		for(int i=0;i<imgs.Length;i++){
			types [i] = ballTypes.IndexOf(imgs[i].sprite);
		}
	}

	void SetImages(){
		GachaController gc = FindObjectOfType<GachaController> ();
		for(int i=0;i<imgs.Length;i++){
			imgs [i].sprite = gc.ballTypes [types [i]];
		}
	}

}
