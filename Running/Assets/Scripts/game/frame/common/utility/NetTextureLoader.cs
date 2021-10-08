using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class NetTextureLoader : MonoBehaviour
{
	public IEnumerator Load (string url, UnityAction<Texture2D> unityAction = null)
	{
		WWW www = new WWW (url + "?" + Random.Range (0, int.MaxValue));
		yield return www;
		if (www.isDone && string.IsNullOrEmpty (www.error)) {
			if (unityAction != null) {
				unityAction (www.texture);
			}
		}
		www.Dispose ();
		www = null;
	}
}
