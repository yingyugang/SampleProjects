using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OilKing_RedBackGround : GachaTweenBase
{
	public Color from = new Color (1, 1, 1, 0);
	public Color to = Color.white;
	public Image targetImage;


	protected override void Awake ()
	{
		base.Awake ();
		if (targetImage == null)
			targetImage = GetComponent<Image> ();
	}

	protected override void DoTween (float evaluate)
	{
		base.DoTween (evaluate);
		if (targetImage != null) { 
			targetImage.color = Color.Lerp (from, to, evaluate); 
		}
	}

	public override bool Play ()
	{
		if (!base.Play ())
			return false;
		if (targetImage == null)
			targetImage = GetComponent<Image> ();
		if (targetImage != null)
			targetImage.color = from;
		return true;
	}

	public void ResetToBegin ()
	{
		base.ResetToBegin ();
		if (targetImage != null)
			targetImage.color = from;
	}
}
