using UnityEngine;
using System.Collections;

//public enum ItemType
//{
//
//	Tree = 5,
//	Oden,
//	Rice
//}
public class Item_Point : Item
{
	//public ItemType type;
	void Start ()
	{
		Rigid = GetComponent<Rigidbody2D> ();
	}

	public override void Effect ()
	{
		Game8_Manager.instance.PlaySound (SoundEnum.se11_scoreitem);
		{
			switch (Id ) {
			case 5:
				//if (Game8_Manager.instance.ItemScoreTree >= BreackoutConfig.MAXITEM)
				//	return;
				Game8_Manager.instance.ItemScoreTree++;
				break;
				
			case 6: 
				//if (Game8_Manager.instance.ItemScoreOden >= BreackoutConfig.MAXITEM)
				//	return;
				Game8_Manager.instance.ItemScoreOden++;
				break;
	
			case 7: 
				//if (Game8_Manager.instance.ItemScoreRice >= BreackoutConfig.MAXITEM)
				//	return;
				Game8_Manager.instance.ItemScoreRice++;
				break;
			}
		}

	}



}
