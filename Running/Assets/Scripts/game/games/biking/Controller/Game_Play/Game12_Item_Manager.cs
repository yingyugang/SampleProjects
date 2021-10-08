using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Game12_Item_Manager : MonoBehaviour
{
	private bool m_TestItem = true;
	public Sprite[] ObstacleSprite;
	public GameObject[] ObstaclePrefabs;
	private bool IsCallItem = false;
	IEnumerator watingReturnPeople;
	float m_TitanSize;
	private int m_Distance = BikingKey.GameConfig.DistanceToReleaseItem;
	private static Game12_Item_Manager m_instance;

	public static Game12_Item_Manager instance {
		get {
			if (m_instance == null)
				m_instance = GameObject.FindObjectOfType<Game12_Item_Manager> ();
			return m_instance;
		}
	}

	void Awake(){
		m_instance = this;
	}
		
	public void ObstacleCreate (GameObject Obstacle, int itemID, int percent, Vector2 cordinate, float angel)
	{
		SpriteRenderer item = Obstacle.AddComponent<SpriteRenderer> ();
		ItemConfigByID (Obstacle, itemID, percent, cordinate);
		item.sortingOrder = 10;
		item.transform.localEulerAngles = new Vector3 (.0f, .0f, angel);
		if (itemID == 1) {
			Obstacle.name = BikingKey.GameConfig.Dekapan;
			Obstacle.transform.localScale = new Vector3 (.5f, .5f, .5f);
		}
		Terrain_Generation.instance.obstacles.Add (item);
	}

	/// <summary>
	/// We setup item properties before release them.
	/// </summary>
	/// <param name="item">Item.</param>
	/// <param name="itemID">Item I.</param>
	/// <param name="cordinate">Cordinate.</param>
	void ItemConfigByID (GameObject item, int itemID, int percent,  Vector2 cordinate)
	{
		switch (itemID) {
		case 0:
		case 1:
		case 4:
		//-->| titan items
		case 2:
		case 10: 
			ItemInit (item, itemID, percent, BikingKey.Tags.item, cordinate);
			break;
		//-->| score items
		case 3:
		case 8:
		case 9:
		case 11:
			cordinate =  new Vector2(cordinate.x, cordinate.y + 0.7f);
			ItemInit (item, itemID, percent, BikingKey.Tags.item, cordinate);
			break;
		//--<| score items
		case 5:
		case 6:
		case 7:
			ItemInit (item, itemID, percent, BikingKey.Tags.obstacle, cordinate);
			break;
			/*
		case 11:
			int[] listIDitem = {3, 8, 9};
			cordinate =  new Vector2(cordinate.x, cordinate.y + Random.Range(0.3f, 1.2f));
			ItemInit (item, itemID, percent, BikingKey.Tags.item, cordinate);
			break;
			*/
		default:
			break;
		}
	}
		
	void ItemInit (GameObject item, int itemID, int percent, string tab, Vector2 cordinate)
	{
		Game12_Item_Component item_comp = item.AddComponent<Game12_Item_Component> ();
		if (itemID == 1) {
		}
		else if (itemID == 4) {
			item.AddComponent<Game12_Item_Jump> ();
			item.AddComponent<BoxCollider2D> ().isTrigger = true;
			item.GetComponent<BoxCollider2D> ().size = new Vector2 (2.1f, 1.8f);
		} else {
			item.gameObject.AddComponent<PolygonCollider2D> ().isTrigger = true;
		}
		item.transform.position = new Vector3 (cordinate.x, cordinate.y, -1.0f);
		item_comp. GetItemProperties (itemID, percent);
		item.tag = tab;
		item.name = item_comp.itemProperties.GetImageSource ();
		item.GetComponent<SpriteRenderer> ().sprite = GetSpriteFromID (item_comp.itemProperties.m_ID);
	}
		
	//--< Render sprite with name, tab, cordinate, properties setup & lists divide

	public Sprite GetSpriteFromID(int itemID){
		if (itemID < ObstacleSprite.Length)
			return ObstacleSprite [itemID];
		return null;
	}

	private Sprite GetSpriteFromString (string spriteString)
	{
		for (int i = 0; i < ObstacleSprite.Length; i++) {
			if (spriteString == ObstacleSprite [i].name)
				return ObstacleSprite [i];
		}
		return null;
	}

	/// <summary>
	/// We will call item on the next pattern.
	/// Not yet to add 10.5%
	/// </summary>
	/// <param name="percentage">Percentage.</param>
	public void CallItem (float percentage)
	{
		ItemPercentage (Terrain_Controller.instance.Routes[Terrain_Controller.instance.currentPattern.ID].GetComponent<Terrain_Elmp> (),
		Game12_Player_Controller.instance.playerInfo);
	}
		


	/// <summary>
	/// We calculating the item' percentage to release them.
	/// </summary>
	/// <param name="items">Items.</param>
	/// <param name="playerInfo">Player info.</param>
	public void ItemPercentage (Terrain_Elmp Pattern, Game12_Player_Properties playerInfo)
	{
		Debug.Log ("ItemPercentage ");;
			// We check item, if item has yet showing, we continues with item percentage.
			if (IsCallItem) {
			
			for(int i = 0; i < Pattern.ItemList.Count; i++){
				// First we active item on choose pattern.
				Pattern.ItemList[i].gameObject.SetActive(true);
				int id = Pattern.ItemList[i].GetComponent<Game12_Item_Component> ().itemProperties.GetID ();

				if (id < 5) {
					// If the item is Rebirt, we shut it down, because we just call when Dekapan die, it's dynamically.
					if (id == 1 && Game12_Player_Controller.instance.playerInfo.GetID() == 1){
						Debug.Log(Pattern.ItemList[i].GetComponent<Game12_Item_Component> ().itemProperties.GetName());
						Debug.Log("No use Dekapan item");
						Pattern.ItemList[i].gameObject.SetActive (false);
					}
					else if(id == 2 && 
						Game12_Player_Controller.instance.IsTitan){
						Debug.Log("No use Titan item");
						Pattern.ItemList[i].gameObject.SetActive (false);
					}
					else{
						Debug.Log("Turn on item");
					}
					// Then we start calculating the percentage showing of all item.
					// We have: 20% for rebirth, 20% for titan buff, 50% for score item.
					PercentageCacl (playerInfo.GetItemRate (),
						Pattern.ItemList[i].gameObject.GetComponent<Game12_Item_Component> ().itemProperties.GetPercentage (),
						Pattern.ItemList[i].gameObject.GetComponent<SpriteRenderer> ());
				}
			}
			}

		IsCallItem = false;
	}
	//	public float DekapanDistance_Release(float where){
	//		return where += BikingKey.GameConfig.DistanceDekapan_Release;
	//	}
	void PercentageCacl (float percentagePlayer, int percentageItem, SpriteRenderer item)
	{
		int rd = Random.Range (1, 100);// 1 + Mathf.RoundToInt ((100 / percentageItem) / percentagePlayer));
		Debug.Log("Random rate = " +  rd.ToString() );
		Debug.Log("percentageItem = " +  percentageItem );
		Debug.Log("Player colect rate = " +  (float)(percentageItem) * percentagePlayer );


		if (rd <=  Mathf.RoundToInt ((float)(percentageItem) * percentagePlayer)) {
			item.gameObject.SetActive (true);
			IsCallItem = false;
			Debug.Log ("Release one" + item.name);

		} else {
			item.gameObject.SetActive (false);
			Debug.Log ("No item released");
		} 
	}

	/// <summary>
	/// Players the on dekapan hit.
	/// </summary>
	/// <param name="titanSize">Titan size.</param>
	/// <param name="duration">Duration.</param>
	public void PlayerOnDekapanHit (int itemID, int nextPlayerID)
	{
		Game12_Player_Controller.instance.PlayerHided();
		// Recall Dekapan
		StartCoroutine(Game12_GameManager.instance.CountdownOnDeadth(itemID, BikingKey.GameConfig.timerebirth, nextPlayerID));
		Game12_Player_Controller.instance.PlayerAvatar_Deadth (Game12_GUI_Manager.instance.Lifes[0], true);
	}

	/// <summary>
	/// This is Item ID: 2
	/// </summary>
	/// <param name="titanSize">Titan size.</param>
	/// <param name="duration">Duration.</param>
	public void PlayerOnTitan (float titanSize, float duration)
	{
		m_TitanSize = titanSize;
		Game12_Player_Controller.instance.TitanWheel.SetActive (true);
		Game12_Player_Controller.instance.Player.GetComponent<BoxCollider2D> ().enabled = false;
		Game12_Player_Controller.instance.IsTitan = true;
		Vector3 titan = (Game12_Player_Controller.instance.Player.transform.localScale) * titanSize;
		//-->| by anhgh
		Game12_Player_Controller.instance.Player.transform.position += Vector3.up * titanSize * 0.5f; // it will help player wont die
		//--<| by anhgh
		Game12_Player_Controller.instance.Player.transform.DOScale (titan, 1.0f);
		watingReturnPeople = ReturnPeople(duration);
		StartCoroutine(watingReturnPeople);
	}

	public void CancelReturnPeople(){
		if(watingReturnPeople != null){
			StopCoroutine(watingReturnPeople);
		}
	}
	public IEnumerator ReturnPeople (float duration)
	{
		yield return new WaitForSeconds(duration);
		Game12_Player_Controller.instance.Player.transform.DOScale (Vector3.one, 1.0f);
		Game12_Player_Controller.instance.IsTitan = false;
		Game12_Player_Controller.instance.TitanWheel.SetActive (false);
	}

	public void ObstacleExplorerOnTitanPower (Transform transform, int index)
	{
		// Play sound fx
		Game12_GUI_Manager.instance.PlaySound(SoundEnum.SE35_bike_explosion);
		GameObject clone = Instantiate (ObstaclePrefabs [index], transform.position, transform.rotation) as GameObject;
		if (clone.gameObject.name == "Root(Clone)") {
			clone.GetComponent<SpriteRenderer> ().sortingOrder = 10;
		}
		clone.transform.localScale = new Vector3 (.7f, .7f, .7f);
		Game12_Player_Controller.instance.TitanWheel.SetActive (true);
	}
}
