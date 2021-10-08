using UnityEngine;
using System.Collections;

public class OilKingAnimDrill : OilKingAnimSprite {
	public override void Start()
	{
		base.Start();
		loopAnim = true;
	}

	public override void GetResourceFromAssetBundle()
	{
		lstImage = new Sprite[3];

		for (int i = 0; i < lstImage.Length; i++)
		{
			lstImage[i] = OilKingAssetLoader.s_Instance.getSpriteDrill(i);
		}

		base.GetResourceFromAssetBundle();
	}
}
