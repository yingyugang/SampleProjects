using UnityEngine;
using System.Collections;

public class Game12_Player_RunningEvent : MonoBehaviour
{
	public delegate void OnOnstacleCollisionDelegate (GameObject obj, int itemID, string name, string imageResources, int itemType, float effect_Value1,
		float effect_Value2, int percentage, int percentageReduction, string desciption);

	public event OnOnstacleCollisionDelegate OnstacleCollisionEvent;

	public delegate void OnItemCollisionDelegate (GameObject obj, int itemID, string name, string imageResources, int itemType, float effect_Value1,
		float effect_Value2, int percentage, int percentageReduction, string desciption);

	public event OnItemCollisionDelegate OnItemCollisionEvent;

	public static Game12_Player_RunningEvent _instance;

	public static Game12_Player_RunningEvent instance {
		get {
			if (_instance == null)
				Debug.Log ("Game12_Player_RunningEvent is null");
			//	_instance = GameObject.FindObjectOfType<Game12_Player_RunningEvent> ();
			return _instance;
		}
	}

	void Awake ()
	{
		_instance = this;
	}

	public void AddOnstacleCollisionListener (OnOnstacleCollisionDelegate callback)
	{
		OnstacleCollisionEvent += callback;
	}

	public void RemoveOnOnstacleCollisionListener (OnOnstacleCollisionDelegate callback)
	{
		OnstacleCollisionEvent -= callback;
	}

	public void AddItemCollisionListener (OnItemCollisionDelegate callback)
	{
		OnItemCollisionEvent += callback;
	}

	public void RemoveItemCollisionListener (OnItemCollisionDelegate callback)
	{
		OnItemCollisionEvent -= callback;
	}

	Game12_Item_Properties GetItemPropertiesByID (GameObject item)
	{
		return item.GetComponent<Game12_Item_Component> ().itemProperties;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == BikingKey.Tags.item) {
			// Send event to game manager, gui manager
			Game12_Item_Properties m_item_p = other.gameObject.GetComponent<Game12_Item_Component> ().itemProperties;

			OnItemCollisionEvent (other.gameObject,
				m_item_p.m_ID,
				m_item_p.m_Name,
				m_item_p.m_ImageSource,
				m_item_p.m_Type,
				m_item_p.m_Effect_Value1,
				m_item_p.m_Effect_Value2,
				m_item_p.m_Percentage,
				m_item_p.m_PercentageReduction,
				m_item_p.m_Desciption);
			return;
		}
		if(other.gameObject.name == BikingKey.Terrain.Terrain_Collision){
			//OnstacleCollisionEvent(other.gameObject, 0, "", "", 0, .0f, .0f, 0, 0, "");
			return;
		}
		if (other.gameObject.tag == BikingKey.Tags.obstacle) {
			if (!Game12_Player_Controller.instance.IsInvulnerable && !Game12_Player_Controller.instance.IsTitan) {
				// Send event to game manager, gui manager
				Game12_Item_Properties m_item_p = other.gameObject.GetComponent<Game12_Item_Component> ().itemProperties;
				OnstacleCollisionEvent (other.gameObject,
					m_item_p.m_ID,
					m_item_p.m_Name,
					m_item_p.m_ImageSource,
					m_item_p.m_Type,
					m_item_p.m_Effect_Value1,
					m_item_p.m_Effect_Value2,
					m_item_p.m_Percentage,
					m_item_p.m_PercentageReduction,
					m_item_p.m_Desciption);

			} else if (other.gameObject.name == BikingKey.Terrain.Terrain_Abyss || other.gameObject.name == BikingKey.Terrain.Cliff) {
				OnstacleCollisionEvent(other.gameObject, 0, "", "", 0, .0f, .0f, 0, 0, "");
			}
			return;
		}

		if (other.gameObject.name.Contains (BikingKey.GameConfig.NameItemReborn)) {
			Debug.Log (BikingKey.GameConfig.NameItemReborn);
			Game12_GUI_Manager.instance.PlaySound(SoundEnum.SE32_bike_hitchhike);
			other.gameObject.SetActive (false);
			Game12_Player_Controller.instance.PlayerInsertLife ();
			if (Game12_Player_Controller.instance.playerInfo.GetID () == (int)Game12_Character.dekapan) {
				Game12_GameManager.instance.hitchhike1 = 1;
			} else {
				Game12_GameManager.instance.hitchhike2 = 1;
			}
		}

	}
}
