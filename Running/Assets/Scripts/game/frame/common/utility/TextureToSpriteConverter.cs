using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureToSpriteConverter
{
	public static Sprite ConvertToSprite (Texture2D texture2d)
	{
		if (texture2d == null) {
			return null;
		}
		return Sprite.Create (texture2d, new Rect (0, 0, texture2d.width, texture2d.height), new Vector2 (0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect);
	}

	public static List<Sprite> ConvertToSpriteList (List<Texture2D> texture2DList)
	{
		int length = texture2DList.Count;
		List<Sprite> list = new List<Sprite> ();
		for (int i = 0; i < length; i++) {
			list.Add (TextureToSpriteConverter.ConvertToSprite (texture2DList [i]));
		}
		return list;
	}

	public static Dictionary<string,Sprite> ConvertToSpriteDictionary (List<Texture2D> texture2DList)
	{
		int length = texture2DList.Count;
		Dictionary<string,Sprite> dictionary = new Dictionary<string, Sprite> ();
		for (int i = 0; i < length; i++) {
			Texture2D texture2d = texture2DList [i];
			dictionary.Add (texture2d.name, TextureToSpriteConverter.ConvertToSprite (texture2d));
		}
		return dictionary;
	}
}
