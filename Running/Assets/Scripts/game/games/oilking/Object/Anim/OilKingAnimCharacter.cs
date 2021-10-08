using UnityEngine;
using System.Collections;

public enum CharacterType
{
	Hit,
	Throw
}

public class OilKingAnimCharacter : OilKingAnimSprite {
	public CharacterType charType;

	public override void Start()
	{
		base.Start();
		loopAnim = false;
	}

	public override void GetResourceFromAssetBundle()
	{
		

		if(charType == CharacterType.Hit)
		{
			lstImage = new Sprite[4];
			for (int i = 0; i < lstImage.Length; i++)
			{
				lstImage[i] = OilKingAssetLoader.s_Instance.getSpriteCharacter( LoadingUIOilKing.s_Instance.idHit + 1,TypeSprite.SuffixHit,i);
			}
			lstImage[3] = OilKingAssetLoader.s_Instance.getSpriteCharacter(LoadingUIOilKing.s_Instance.idHit + 1, TypeSprite.SuffixHit, 2);
		}

		if (charType == CharacterType.Throw)
		{
			lstImage = new Sprite[3];
			for (int i = 0; i < lstImage.Length; i++)
			{
				lstImage[i] = OilKingAssetLoader.s_Instance.getSpriteCharacter(LoadingUIOilKing.s_Instance.idThrow + 1, TypeSprite.SuffixThrow, i);
			}
		}

		base.GetResourceFromAssetBundle();
	}

	public void SetSpriteWhenHitBoom()
	{
		switch (charType)
		{
			case CharacterType.Hit:
				StopAnim();
				imgTarget.sprite = OilKingAssetLoader.s_Instance.getSpriteCharacter(LoadingUIOilKing.s_Instance.idHit + 1, TypeSprite.SuffixHit, 3);
				break;
			case CharacterType.Throw:
				imgTarget.sprite = OilKingAssetLoader.s_Instance.getSpriteCharacter(LoadingUIOilKing.s_Instance.idThrow + 1, TypeSprite.SuffixThrow, 3);
				break;
		}
	}
}
