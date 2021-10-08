using UnityEngine;
using System.Collections;

public class ApManager : MonoBehaviour
{
	public ApMediator ap;

	public void UpdateApRecoveryTime ()
	{
		ap.SetApWithServerData ();
	}
}
