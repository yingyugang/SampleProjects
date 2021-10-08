using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum TypeOfCharacter{
	Digging, Spring,
}

public enum ColorItem{
	Red, Blue, Green, Violet, Yellow, Pink,
}

[System.Serializable]
public class CharacterOilKing : MonoBehaviour{
	public int id;
	public Image image;
	public TypeOfCharacter type;
	public ColorItem color;
}
