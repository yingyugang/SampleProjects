using UnityEngine;
using System.Collections;

public class CardPageMediator : PageMediator
{
	public CardTopMediator cardTopMediator;

	protected override void CheckResources ()
	{
		pageNumber = 2;
		cardTopMediator.hasInit = false;
		ShowWindow ();
	}
}
