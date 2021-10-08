using UnityEngine;
using System.Collections;

public class EndingGame : MonoBehaviour {
	
	// Update is called once per frame
	public void GameOver()
	{
		UIManager.Instance.CloseEnding();
		GetOut.GameManager.Instance().GameOver();
	}
}
