using UnityEngine;
using System.Collections;

public class Instantiator
{
	private static Instantiator INSTANTIATOR;

	public static Instantiator GetInstance ()
	{
		if (INSTANTIATOR == null) {
			INSTANTIATOR = new Instantiator ();
		}
		return INSTANTIATOR;
	}

	public T Instantiate<T> (T t, Vector2 localposition, Vector3 localScale, Transform parent) where T : Component
	{
		T newT = GameObject.Instantiate<T> (t);
		newT = SetParent<T> (newT, localposition, localScale, parent);
		return newT;
	}

	public T SetParent<T> (T t, Vector2 localposition, Vector3 localScale, Transform parent) where T : Component
	{
		SetParent (t.transform, localposition, localScale, parent);
		return t;
	}

	public GameObject InstantiateGameObject (GameObject go, Vector2 localposition, Vector3 localScale, Transform parent)
	{
		GameObject newGo = GameObject.Instantiate (go);
		SetParent (newGo.transform, localposition, localScale, parent);
		return newGo;
	}

	public GameObject InstantiateGameObjectFromPath (string path, Vector2 localposition, Vector3 localScale, Transform parent)
	{
		GameObject go = GameObject.Instantiate (Resources.Load<GameObject> (path));
		SetParent (go.transform, localposition, localScale, parent);
		return go;
	}

	private void SetParent (Transform trans, Vector2 localposition, Vector3 localScale, Transform parent)
	{
		trans.SetParent (parent);
		trans.localPosition = localposition;
		trans.localScale = localScale;
	}
}
