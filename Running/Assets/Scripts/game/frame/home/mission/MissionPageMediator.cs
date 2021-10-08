using UnityEngine;
using System.Collections;

public class MissionPageMediator : PageMediator
{
	protected override void CheckResources ()
	{
		pageNumber = 4;
		ShowWindow ();
	}
}
