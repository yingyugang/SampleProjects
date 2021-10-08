using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestGacha : MonoBehaviour {
	public GachaController gacha;
	int gachaType = 1;
	int peopleCount = 1;
	public List<GachaItem> items;
	public List<GachaItem> selectItems;
	public bool reStart;
	bool isShowLED = true;
	void Start(){
		//gacha.gameObject.SetActive (true);
		//gacha.Play (gachaType,peopleCount,items);
		selectItems = new List<GachaItem>();
	}

	void Update(){
		if(reStart){
			reStart = false;
			gacha.Play (gachaType,peopleCount,items,isShowLED);
		}
	}

	bool isStart;
	//bool isShowGUI = false;
	//string btnTxt = "+";
	void OnGUI(){
		if (!isStart) {
			/*
			if(GUI.Button(new Rect(Screen.width - 30,0,30,30),btnTxt))
			{
				if (isShowGUI) {
					isShowGUI = false;
					btnTxt = "+";
				} else {
					isShowGUI = true;
					btnTxt = "-";
				}
			}
*/
			//if (!isShowGUI)
			//	return;
			GUI.color = Color.green;
			GUI.Label (new Rect(5,30,200,30),"Type:" + gachaType.ToString());
			GUI.color = Color.white;
			for(int i=1;i<9;i++){
				if(gachaType == i)
					GUI.color = new Color(1,Mathf.Sin(Time.time),Mathf.Cos(Time.time),1);
				if(GUI.Button(new Rect(5 + 25 * i,60,25,25),i.ToString())){
					gachaType = i;
				}
				GUI.color = Color.white;
			}

			GUI.color = Color.green;
			GUI.Label (new Rect(5,90,200,30),"Person:" + peopleCount.ToString());
			GUI.color = Color.white;
			for(int i=1;i<7;i++){
				if(peopleCount == i)
					GUI.color = new Color(1,Mathf.Sin(Time.time),Mathf.Cos(Time.time),1);
				if(GUI.Button(new Rect(5 + 25 * i,120,25,25),i.ToString())){
					peopleCount = i;
				}
				GUI.color = Color.white;
			}

			GUI.color = Color.green;
			GUI.Label (new Rect(5,150,200,30),"SelectItemCount:" + selectItems.Count.ToString());
			GUI.color = Color.white;
			for(int i=0;i<items.Count;i++){
				
				if(GUI.Button(new Rect(5 + 25 * (i+1),180,25,25),(i+1).ToString())){
					if (selectItems.Count >= 10)
						selectItems.RemoveAt (0);
					selectItems.Add (items[i]);
				}
			}


			GUI.color = Color.green;
			GUI.Label (new Rect(5,210,200,30),"ShowLED:" + isShowLED.ToString());
			GUI.color = Color.white;
			if(GUI.Button(new Rect(25,240,40,25),"Yes")){
				isShowLED = true;
			}
			if(GUI.Button(new Rect(70,240,40,25),"No")){
				isShowLED = false;
			}

			if (selectItems.Count == 0)
				GUI.color = Color.red;
			else
				GUI.color = Color.green;
			if(GUI.Button(new Rect(Screen.width - 110,270,50,30),"Start")){
				if (selectItems.Count > 0) {
					gacha.gameObject.SetActive (true);
					gacha.Play (gachaType,peopleCount,selectItems,isShowLED);
					gacha.onGachaFinish += Reset;
					isStart = true;
				}
			}
			GUI.color = Color.white;

			if(GUI.Button(new Rect(Screen.width - 55,270,50,30),"Reset")){
				selectItems.Clear ();
				gachaType = 1;
				peopleCount = 1;
				isStart = false;
			}
		}
	}

	void Reset(){
		isStart = false;
		selectItems.Clear ();
	}
}
