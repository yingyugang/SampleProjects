using UnityEngine;
using System.Collections;

public class OtherPageMediator : PageMediator
{
	protected override void CheckResources ()
	{
		pageNumber = 5;
		ShowWindow ();
	}
}
