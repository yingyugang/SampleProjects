using UnityEngine;
using System.Collections;

public delegate void OnTweenDone();
public class GachaTweenBase : MonoBehaviour {

	public OnTweenDone onTweenDone;
	public AnimationCurve curve = AnimationCurve.Linear(0,0,1,1);
	public int loopType = 0;
	public float duration = 1;
	public float delay = 0;
	public bool isLoop;
	public bool isPlayOnEnable;
	public bool isAutoHide;
	public bool isIgnoreTimeScale;
	protected Transform mTrans;

	protected virtual void Awake(){
		mTrans = transform;
	}

	protected virtual void OnEnable(){
		if (isPlayOnEnable) {
			ResetToBegin ();
			Play ();
		}
	}

	public bool IsPlaying(){
		return isPlaying;
	}

	public virtual bool Play(){
		if(isPlaying){
			return false;
		}
		isPlaying = true;
		mBeginTime = Time.time + delay;
		mTime = 0;
		return true;
	}

	public virtual void ResetToBegin(){
		isPlaying = false;
		mTime = 0;
	}

	bool isPlaying;
	float mTime=0;
	int dir = 1;
	float mBeginTime = 0;
	float deltaTime = 0;
	void Update(){
		if (isIgnoreTimeScale)
			deltaTime = Time.unscaledDeltaTime;
		else
			deltaTime = Time.deltaTime;
		if(isPlaying && mBeginTime<=Time.time){
			mTime += deltaTime / duration * dir;
			DoTween (curve.Evaluate(mTime));
			if (isLoop) {
				if (loopType == 0) {
					if (mTime >= 1) {
						dir = -1;
					}
					if (mTime <= 0) {
						dir = 1;
					}
				} else {
					if (mTime >= 1) {
						mTime = 0;
					}
				}
			} else {
				if (mTime >= 1 || mTime <= 0) {
					isPlaying = false;
					if (onTweenDone != null)
						onTweenDone ();
					if (isAutoHide)
						gameObject.SetActive (false);
				} 
			}
		}
	}

	//tween logic
	protected virtual void DoTween(float evaluate){
	
	}


}
