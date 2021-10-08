using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Game12_Footer : MonoBehaviour {

	public Image[] itemImage;
	public void AddItemScore(int score, string spriteName){
//		Debug.Log("Wth? " + spriteName);
//		PlayerPrefs.SetInt(BikingKey.GameConfig.Score, score);
		for(int i = 0; i < itemImage.Length; i++){
			if(itemImage[i].sprite.name == spriteName){
//				PlayerPrefs.SetInt(spriteName, PlayerPrefs.GetInt(spriteName) + 1);
				Game12_GameManager.instance.total_score_item += score;
				Text itemTxt = itemImage[i].transform.GetChild(0).GetComponent<Text>();
				itemTxt.text = int.Parse(itemTxt.text) + 1 + "";
			}
		}
	}
}
