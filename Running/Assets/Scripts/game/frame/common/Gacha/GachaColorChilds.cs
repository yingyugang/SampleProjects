using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GachaColorChilds : MonoBehaviour {

	public AnimationCurve curve = AnimationCurve.Linear(0,0,1,1);
	public Color from;
	public Color to;
	public float duration = 1;
	public Image targetImage;

	void Awake(){
		if(targetImage == null)
			targetImage = GetComponent<Image> ();
	}

	bool mIsPlaying;
	float t = 0;
	void Update(){
		if (targetImage == null)
			return;
		if(mIsPlaying){
			t += Time.deltaTime / duration;
			targetImage.color = Color.Lerp (from,to,curve.Evaluate(t));
			if(t>=1){
				mIsPlaying = false;
			}
		}
	}

	public bool IsPlaying{
		get{ 
			return mIsPlaying;
		}
	}

	public void Play(){
		if(mIsPlaying){
			return;
		}
		mIsPlaying = true;
		t = 0;
	}

	public void ResetToBegin(){
		mIsPlaying = false;
		t = 0;
		targetImage.color = from;
	}
}
