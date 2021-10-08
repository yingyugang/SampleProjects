using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Splash : SceneInitializer
{
	public Image logo;
	public float delay = 4;
	private const string LOGO = "logo";

	protected override void Init ()
	{
		StartCoroutine (ShowSplash ());
	}

	private IEnumerator ShowSplash ()
	{
		yield return new WaitForSeconds (delay);
		ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene (SceneEnum.Main);
	}

	override protected IEnumerator CheckSceneResources ()
	{
		yield return null;
		Init ();
	}
}
