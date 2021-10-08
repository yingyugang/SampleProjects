using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Terrain_Elmp: MonoBehaviour  {
	public int ID;
	public float _xWDelta = .0f;
	public GameObject Cliff;
	public Terrain_Struct Terrain_Struct = new Terrain_Struct();
	public Transform HeightPoint;
	public GameObject Terrain_Collision;
	public List<Transform> ItemList = new List<Transform>();
	public List<Transform> ObstacleList = new List<Transform>();
	public List<EdgeCollider2D> AbyssList = new List<EdgeCollider2D>();
	public List<Transform> AbyssInvulnerableList = new List<Transform>();
	public List<Transform> EmptySlotList = new List<Transform>();
	//public List<Transform> PatternBorderList = new List<Transform>();

	public void RandomItem(){
		for(int i = 0; i < ItemList.Count; i++){
			ItemList[i].gameObject.SetActive(true);
			ItemList[i].GetComponent<Game12_Item_Component>().ShowItem();
		}
		for(int i = 0; i < ObstacleList.Count; i++){
			ObstacleList[i].gameObject.SetActive(true);
			ObstacleList[i].GetComponent<Game12_Item_Component>().ShowItem();
		}
	}
	public void ShowHitchhike(){
		for(int i = 0; i < ItemList.Count; i++){
			//ObstacleList[i].gameObject.SetActive(true);
			Game12_Item_Component Item = ItemList[i].GetComponent<Game12_Item_Component>();
			if (Item.itemProperties.m_ID == 1) {
				Debug.Log ("ShowHitchhike");
				Item.ShowItem ();
			}
		}
	}

	/*
	void OnEnable(){
		Game12_Item_Properties[] item_in_pattern;
	
		item_in_pattern = GetComponentsInChildren<Game12_Item_Component>();
		foreach (Game12_Item_Component item in item_in_pattern) {
			item.gameObject.SetActive(true);
		}


	}
	*/
}
