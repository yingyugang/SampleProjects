using UnityEngine;
using System.Collections;

public class BaseAnimation : MonoBehaviour
{
	public float speed = 0.1f;
	public const string isPlay = "isPlay";
	protected int currentIndex;
	protected int total;
	protected Coroutine coroutine;

	private void OnEnable ()
	{
		Init ();
	}

	virtual protected void Init ()
	{
		
	}

	protected void Show ()
	{
		coroutine = StartCoroutine (ShowOneAfterAnother ());
	}

	virtual protected IEnumerator ShowOneAfterAnother ()
	{
		yield return null;
	}

	virtual protected void OnDestroy ()
	{
		if(coroutine != null){
			StopCoroutine (coroutine);
		}
	}
}
