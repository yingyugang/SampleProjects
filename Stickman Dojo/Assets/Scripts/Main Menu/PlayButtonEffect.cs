using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayButtonEffect : MonoBehaviour {
	public Transform effect1,effect2;
	Image effect1Img;
	public Color StartColor,EndColor;
	Color currentColor;
	float speed = 15;
	float fadeSpeed = 0.5f;
	float colorLerp;
	bool lerpSwitch;
	// Use this for initialization
	void Start () {
	
		effect1Img = effect1.gameObject.GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 euler1 = effect1.eulerAngles;
		Vector3 euler2 = effect2.eulerAngles;
		euler1.z += Time.deltaTime *speed ;
		euler2.z -= Time.deltaTime *speed;
		effect1.eulerAngles = euler1;
		effect2.eulerAngles = euler2;

		if (colorLerp < 1 && lerpSwitch == false) {
			colorLerp+=Time.deltaTime*fadeSpeed;
			if (colorLerp>1){
				lerpSwitch=true;
			}
		}
		if (colorLerp > 0 && lerpSwitch == true) {
			colorLerp-=Time.deltaTime*fadeSpeed;
			if (colorLerp<0){
				lerpSwitch=false;
			}
		}

		currentColor = Color.Lerp (StartColor, EndColor,colorLerp);
		effect1Img.color = currentColor;
	
	}
}
