using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Item_Boom : Item
{

	void Start ()
	{
		Rigid = GetComponent<Rigidbody2D> ();
	
	}

	public override void Effect ()
	{
		if (Game8_Manager.instance.GameState != GAMESTATE.START) {
			Game8_Manager.instance.PlaySound (SoundEnum.se06_timeup_countdown);	
			Game8_Manager.instance.Racket.RacketBlinking ();
		}
	}
		

}
