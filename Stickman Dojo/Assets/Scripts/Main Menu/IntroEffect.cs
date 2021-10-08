using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroEffect : MonoBehaviour {
	public GameObject write1,write2,blood,PlayComplex,BotComplex;
	// this are the appear timers
	SpriteRenderer[]spriteFade;
	Image []fadeColors;
	float timer;
	float step1 = 0.5f;
	float step2 = 1.0f;
	float step3 = 1.5f;
	float step4 = 2.5f;
	float step5 = 3.5f;
	int step=0;
	public AudioSource audio1,audio2,audio3;
	// Use this for initialization
	void StartComplex () {
		fadeColors = PlayComplex.GetComponentsInChildren<Image> ();
		int i = 0;
		while (i<fadeColors.Length) {
		
			Color col = fadeColors[i].color;
			col.a = 0;
			fadeColors[i].color = col;
			i++;

		}
		spriteFade = BotComplex.GetComponentsInChildren<SpriteRenderer> ();




	}

	void FadeComplex(){
		foreach (Image fade in fadeColors) {
			Color col = fade.color;
			col.a += Time.deltaTime;
			fade.color = col;
		}
		foreach (SpriteRenderer sprite in spriteFade) {
			Color col = sprite.color;
			col.a += Time.deltaTime;
			sprite.color = col;
		}


	}
	
	// Update is called once per frame
	void Update () {
	
		timer += Time.deltaTime;
		if (timer > step1 && step == 0) {
			write1.SetActive(true);
			audio1.Play();
			step++;
		}
		if (timer > step2 && step == 1) {
			write2.SetActive(true);
			audio1.Play();

			step++;
		}
		if (timer > step3 && step == 2) {
			blood.SetActive(true);
			audio2.Play();

			step++;
		}
		if (timer > step4 && step == 3) {
			PlayComplex.SetActive(true);
			BotComplex.SetActive(true);
			StartComplex();
			step++;
			audio3.Play();
		}
		if (step == 4) {
			FadeComplex();
		}
		if (timer>step5){
			PlayButtonEffect playButton = FindObjectOfType<PlayButtonEffect>();
			playButton.enabled= true;
		}
	}




}
