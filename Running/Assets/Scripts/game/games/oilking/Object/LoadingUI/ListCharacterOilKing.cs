using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ListCharacterOilKing{

	public List<CharacterOilKing> lstCharacter = new List<CharacterOilKing>();

	public CharacterOilKing GetCharacter(int id){
		if (lstCharacter != null) {	
			foreach (var item in lstCharacter) {
				if (item.id == id) {
					return item;
				}
			}
		}
		return null;
	}

	public int GetCharacterFromImage(string img){
		if (lstCharacter != null) {
			foreach (var item in lstCharacter) {
				if (item.image.name.Equals (img)) {
					return item.id;
				}
			}
		}
		return 0;
	}
}
