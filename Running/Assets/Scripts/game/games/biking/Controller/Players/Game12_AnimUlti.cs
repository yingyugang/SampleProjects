	using UnityEngine;
using System.Collections;

public class Game12_AnimUlti : MonoBehaviour {
	//-->  Fade Effects
	public void SpriteRenderGroupFade(Transform gr, bool fadein, float speed, float delay){
		foreach(Transform tf in gr){
			if(tf.GetComponent<SpriteRenderer>()){
//				ChangeBackground(tf, isFadeIn, .01f);
				StartCoroutine(IenumFade(tf, fadein, speed, delay));
			}
		}

	}
	//--<
	IEnumerator IenumFade(Transform tf, bool fadein, float speed, float delay){
		yield return new WaitForSeconds(delay);
		SpriteRenderer sprite = tf.GetComponent<SpriteRenderer>();
		bool running = true;
		sprite.enabled = true;
		Color clr = sprite.color;
		if(fadein)clr = new Color(255, 255, 255, 0);
		else clr = new Color(255, 255, 255, 1);
		sprite.color =clr;
		while(running){
			if(fadein){
				clr.a +=speed;
				sprite.color = clr;
				if(clr.a >= 1){
					running = false;
					sprite.enabled = true;
				}
			}
			else{
				clr.a -=speed;
				sprite.color = clr;
				if(clr.a <= 0){
					running = false;
					sprite.enabled = false;
				}
			}
			yield return null;
		}
	}
}
