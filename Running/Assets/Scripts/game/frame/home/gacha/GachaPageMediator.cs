using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GachaPageMediator : PageMediator
{
	public GachaAnimationMediator gachaAnimationMediator;

	override protected void CheckResources ()
	{
		pageNumber = 1;
		ShowWindow ();
	}
}
