using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
public class Global_GUI_Ent : MonoBehaviour {

	//-->Coroutine fix on time scale
	public static class CoroutineUtil{
		public static IEnumerator WaitForRealSeconds(float t){
			float s = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < s + t) yield return null;
		}
	}
	//--<Coroutine fix on time scale
	public void DoTweenFix(float t){
		DOTween.To(()=> Time.timeScale, x=> Time.timeScale = x, t, 1.0f).SetEase(Ease.Unset).SetUpdate(true);
	}

	//-->Auto type
	// Text free stype effect
	public IEnumerator AutoType (string ts, Text tg, float delay, string onFinish) {
		tg.text = "";
		foreach (char letter in ts.ToCharArray()) {
			tg.text += " " + letter;
			yield return 0;
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(delay));
		}   
		if(onFinish != "") Global_GameOver.instance.Invoke(onFinish, .0f);
	}
	//--<Auto type

	//-->Auto type with border
	// Text free stype with border delta size effect
	public IEnumerator AutoTypeBorder (string ts, Text tg, RectTransform trect, float delay, string onFinish) {
		tg.text = "";
		foreach (char letter in ts.ToCharArray()) {
			tg.text += " " + letter;
			trect.sizeDelta = new Vector2(tg.preferredWidth + 50.0f, trect.rect.height);
			yield return 0;
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(delay));
		}      
		if(onFinish != "") Global_GameOver.instance.Invoke(onFinish, .0f);
	}
	//--<Auto type with border

	//-->Text timer
	// Score incr effect
	public IEnumerator TextTimer (float score, Text tg, string onFinish) {
		// Need modify score'count time 
		float count = 0;
		float _t = score/10;
		float _d = score%10;
		while(count <= score){
			count+=10;
			tg.text = count.ToString();
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(.01f));
		}
		count = count - 10 + _d; 
		tg.text = count.ToString();
		if(onFinish != "") Global_GameOver.instance.Invoke(onFinish, .0f);
	}
	//--<Text timer

	//-->Gameobject Shak
	// Vibrating game object effect
	public IEnumerator ObjShaking(Transform tf) {
		float shake = 2.0f;
		float amount = 8.30f;
		float df = 0.3f;
		while(shake > 0){
			tf.localPosition = Random.insideUnitSphere * amount;
			shake -= .5f * df;
			yield return 0;
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(.01f));
		}
	}
	//--<Gameobject Shak

	//-->EXP bar calc
	// Bar pregress animation
	// Input: Imagebar, game playing exp, player exp, fill speed & finish call string
	public IEnumerator BarProgress(Image img, float exp, float currentExp, float speed, string onFinish) {
		float count = currentExp;
		exp = exp + currentExp;
		while(count <= exp){
			count+=Time.deltaTime*speed;
			img.fillAmount = count;
			yield return 0;
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(.01f));
			if(count > 1f) break;
		}
		img.fillAmount = exp;
		if(count > 1){
			if(exp == 1){
				img.fillAmount = 1.0f;
				yield return null;
			} 
			else{
				img.fillAmount = .0f;
				Global_GameOver.instance.TxtExpLvl.text = "LV " + (PlayerPrefs.GetInt(BikingKey.SSTring.PlayerCurrentLvl) + 1);
				PlayerPrefs.SetInt(BikingKey.SSTring.PlayerCurrentLvl, PlayerPrefs.GetInt(BikingKey.SSTring.PlayerCurrentLvl) + 1);
				float _preExpUp = ExpLvlUp(PlayerPrefs.GetInt(BikingKey.SSTring.PlayerCurrentLvl)-1);
				float _expUp = ExpLvlUp(PlayerPrefs.GetInt(BikingKey.SSTring.PlayerCurrentLvl));
				exp =  Mathf.RoundToInt(((exp - 1.0f)*100)*_preExpUp/100);
				float _q = ExpLvlFill(exp, _expUp);
				float _sp = _expUp*0.4f/100;
				StartCoroutine(BarProgress(img, _q, 0, _sp, ""));
			}
		}
	}
	private float ExpLvlFill(float exp, float expLvlUp){
		float _expFill = exp/expLvlUp;
		return _expFill;
	}
	private float ExpLvlUp(int playerLvl){
		float _exp = 0;
		if(playerLvl == 1)_exp = Global_GameOver.instance.GameOver_Element.ExpLevelTarget[playerLvl-1]; else{
			_exp = Global_GameOver.instance.GameOver_Element.ExpLevelTarget[playerLvl-1]
				- Global_GameOver.instance.GameOver_Element.ExpLevelTarget[playerLvl - 2];
		}
		return Mathf.RoundToInt(_exp);
	}
	//--<EXP bar calc
}
