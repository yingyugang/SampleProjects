using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

public class Loading : MonoBehaviour
{
	protected const float TEXT_INTERVAL = 0.25f;
	protected string loadingText = "Loading";
	protected string dotText = ".";
	public Text text;

	private void OnEnable ()
	{
		StartCoroutine (Show ());
	}

	virtual protected IEnumerator Show ()
	{
		StringBuilder stringBuilder = new StringBuilder (loadingText);
		for (int i = 0; i < 4; i++) {
			text.text = stringBuilder.ToString ();
			yield return new WaitForSeconds (TEXT_INTERVAL);
			stringBuilder.Append (dotText);
		}
		StartCoroutine (Show ());
	}
}
