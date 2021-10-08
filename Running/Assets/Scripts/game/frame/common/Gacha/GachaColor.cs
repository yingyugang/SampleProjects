using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GachaColor : GachaTweenBase {

	public Color from = new Color(1,1,1,0);
	public Color to = Color.white;
	public Image targetImage;
	public Text text;

	protected override void Awake(){
		base.Awake ();
		if(targetImage == null)
			targetImage = GetComponent<Image> ();
		if(text==null)
			text = GetComponent<Text> ();
	}

	protected override void DoTween (float evaluate)
	{
		base.DoTween (evaluate);
		if(targetImage!=null)
			targetImage.color = Color.Lerp (from,to,evaluate);
		if(text!=null)
			text.color =  Color.Lerp (from,to,evaluate);
	}

	public override bool Play(){
		if (!base.Play ())
			return false;
		if(targetImage == null)
			targetImage = GetComponent<Image> ();
		if(targetImage!=null)
			targetImage.color = from;

		if(text!=null)
			text = GetComponent<Text> ();
		if(text!=null)
			text.color = from;
		return true;
	}

	public void ResetToBegin(){
		base.ResetToBegin ();
		if(targetImage!=null)
			targetImage.color = from;
		if (text != null)
			text.color = from;
	}
}
