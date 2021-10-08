using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Item_Manager : MonoBehaviour
{
	public float Item_Percentage;
	public float Item_PercentageDown;
	public float Boss_ItemPercentage;
	public float Boss_ItemPercentageDown;
	public int Item_DamageBoss;

	private int m_CountItem = 7;
	public Item[] ListItem;
	private float[] m_Percentage;
	private Vector2 m_Velocity;

	//--> Random Index Item
	int Random_IndexItem (bool isBoss)
	{
		float m_TotalProbality = 0;

		//-->Get appear_probality of Item
		float[] tempProbality = new float[m_CountItem];
		tempProbality [0] = m_Percentage [0];
		for (int i = 1; i < tempProbality.Length; i++) {
			tempProbality [i] = tempProbality [i - 1] + m_Percentage [i];
		}
		m_TotalProbality = tempProbality [tempProbality.Length - 1];
		//--<

		//-->Change Total Appear Probality when is boss stage
		if (isBoss) {
			Item boom = GetItemById (m_CountItem + 1);
			if (boom != null) {
				m_TotalProbality += boom.AppearProbality * 100;
			}
		}//--<

		//Random Item
		float rand = Random.Range (0, m_TotalProbality);
		for (int i = 0; i < tempProbality.Length; i++) {
			if (tempProbality [i] > rand)
				return i + 1;
		}
		if (isBoss)
			return m_CountItem + 1;

		return -1;
	}
	//--<

	//--> Choise Item after Random
	Item Chosen_Item (bool isBoss)
	{
		int randID = Random_IndexItem (isBoss);
		if (randID == -1)
			return null;
		foreach (Item item in ListItem) {
			if (item.Id == randID && !item.isChosen) {
				return item;
			}
		}
		return null;
	}
	//--<

	void Reduce_Percentage (int id, int percent)
	{
		if (id < m_Percentage.Length) {
			m_Percentage [id] = m_Percentage [id] * (100 - percent) / 100;
		}
	}

	//--> Drop item
	public void DropItem (Vector3 pos)
	{
		bool isBoss = Game8_Manager.instance.BlockManager.CheckBossStage (Game8_Manager.instance.Stage);
//		Debug.Log("Boss " + isBoss);
		if (!isBoss && !CheckAppearItem (Item_Percentage, Item_PercentageDown))
			return;
		if (isBoss && !CheckAppearItem (Boss_ItemPercentage, Boss_ItemPercentageDown))
			return;

		Item item = Chosen_Item (isBoss);
		if (item != null) {
			Reduce_Percentage (item.Id - 1, item.ReductionPercentage);
			item.DropItem (pos);
		}
	}
	//--<

	private float m_ProbalityItem = 1000;

	bool CheckAppearItem (float percent, float reduce)
	{	
		if (m_ProbalityItem > percent)	m_ProbalityItem = percent;
//		Debug.Log(percent + " Chosen " +m_ProbalityItem);
		if (Random.Range (0, 100) < m_ProbalityItem) {
			m_ProbalityItem -= reduce;
			return true;
		}
		return false;
	}

	//--> Reset Item
	public void ResetAllItem ()
	{
		
		m_ProbalityItem = 1000;
		for (int i = 0; i < ListItem.Length; i++) {
			ListItem [i].ResetItem ();
			ListItem [i].isRunning = false;
		}
	}
	//--<

	//--> Set Item from Csv file
	public void SetItemCsv (int id, int appear_probality, int effect_value, int reduce_percentage)
	{
		for (int i = 0; i < ListItem.Length; i++) {
			if (ListItem [i].Id == id) {
				ListItem [i].AppearProbality = appear_probality;
				ListItem [i].EffectValue = effect_value;
				ListItem [i].ReductionPercentage = reduce_percentage;
			}
		}
		if (m_Percentage == null)
			m_Percentage = new float[m_CountItem];		
		if (id <= m_CountItem)
			m_Percentage [id - 1] = appear_probality * 100;
	}
	//--<

	//-->get detail item by id
	Item GetItemById (int id)
	{
		foreach (Item item in ListItem) {
			if (item.Id == id) {
				return item;
			}
		}
		return null;
	}
	//--<
	public void CountPoint(int itemTree, int itemOden, int itemRice){
		itemTree *= ListItem [4].EffectValue;
		itemOden *= ListItem [5].EffectValue;
		itemRice *= ListItem [6].EffectValue;
		Game8_Manager.instance.CountScoreItem = itemTree + itemRice + itemOden;
	}

	public List<Item> GetRunningItem ()
	{
		List<Item> runningitem = new List<Item> ();
		foreach (Item item in ListItem) {
			if (item.isRunning) {
				runningitem.Add (item);
			}
		}
		return runningitem;
	}

	public void PauseAllItem(bool isPause){
		
		List<Item> runningitem = GetRunningItem ();
		foreach (Item item in runningitem) {
			if(item.GetComponent<Rigidbody2D> ().velocity != Vector2.zero){
				m_Velocity= item.GetComponent<Rigidbody2D> ().velocity;
			}
			if (isPause) {
				item.GetComponent<Rigidbody2D> ().isKinematic = true;
			} else {
				item.GetComponent<Rigidbody2D> ().isKinematic = false;
				item.GetComponent<Rigidbody2D> ().velocity = m_Velocity;
			}
		}
			
	}


}
