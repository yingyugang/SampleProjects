using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;

public class Block_Manager : MonoBehaviour
{
	public GameObject[] BasicBlocks;
	public Sprite[] ListSprite;
	public Sprite[] SpriteBlock;
	public Game8_LoadData Data;
	private int m_IndexBasicBlock = 0;
	private List<GameObject> LiveBlock = new List<GameObject> ();
	private List<Stage> m_ListStage;

	public void ResetBlock ()
	{
		foreach (GameObject block in LiveBlock) {
			Destroy (block);
		}
		LiveBlock.Clear ();
	}

	Block_Detail GetBasicBlock ()
	{
		return BasicBlocks [m_IndexBasicBlock++].transform.GetChild (0).gameObject.GetComponent<Block_Detail> ();
	}

	//return Block
	GameObject GetBasicBlockById (int _id)
	{
		for (int i = 0; i < BasicBlocks.Length; i++) {
			if (BasicBlocks [i].transform.GetChild (0).gameObject.GetComponent<Block_Detail> ().Id == _id) {
				return BasicBlocks [i];
			}
		}
		return BasicBlocks [0];
	}


	/**********************/
	/* create basic block, read in csv file */
	/* id : ID of Block */
	/* rotation :  rotation of block */
	/* width, height :  size of block */
	/* is_key :  is key block */
	/**********************/
	public void CreateBlock (int id, string image_name, float width, float height, float rotation, int is_key, int is_boss, string face_image)
	{
		Block_Detail block = GetBasicBlock ();
		SpriteRenderer _mysprite = block.gameObject.GetComponent<SpriteRenderer> ();

		//-->set sprite, id, collider, keyblock
		block.Id = id;
		_mysprite.sprite = CheckSpriteBlock (ListSprite, image_name);
		if (_mysprite.sprite == null)
			return;
		block.gameObject.AddComponent<PolygonCollider2D> ();
		block.IsKeyBlock = is_key > 0;

		//--<

		//-->scale size sprite
		float xScale = Mathf.Abs (width / FloatFormat (_mysprite.bounds.size.x, 2));
		float yScale = Mathf.Abs (height / FloatFormat (_mysprite.bounds.size.y, 2));
		float childScale = xScale < yScale ? yScale : xScale;
		block.transform.localScale = new Vector3 (xScale * Mathf.Sign (width), yScale * Mathf.Sign (height), 1f);
		if (block.IsKeyBlock && is_boss == 0) {
			face_image = BreackoutConfig.IMAGEKEY;
		}

	
		SetImgBlock (block.gameObject, is_boss, is_key, face_image, childScale);

	}

	Sprite CheckSpriteBlock (Sprite[] listSprite, string name)
	{
		for (int i = 0; i < listSprite.Length; i++) {
			if (name.Equals (listSprite [i].name)) {
				return listSprite [i];
			}
		}
		return null;
	}

	void SetImgBlock (GameObject block, int isBos, int isKey, string nameface, float parentScale)
	{
		
		float _ratioScale = 0.45f;
		Transform child = block.transform.GetChild (0);
		SpriteRenderer _myspriteChild = child.GetComponent<SpriteRenderer> ();
		if (CheatController.IsCheated (0)) {
			if (nameface == "karashi_iyami")
				nameface = "karashi_chibita";
		}
		_myspriteChild.sprite = CheckSpriteBlock (SpriteBlock, nameface);
		if (_myspriteChild.sprite == null)
			return;

		//-->Scale karashi sprite 
		Bounds boundCol = block.gameObject.GetComponent<PolygonCollider2D> ().bounds;

		if (block.GetComponent<SpriteRenderer> ().sprite.name.Equals (BreackoutConfig.BLOCKHAMPEN)) {
			float ChildScalex = boundCol.size.x / FloatFormat (_myspriteChild.bounds.size.x, 2);
			child.position = boundCol.center - boundCol.size / 6;
			child.localScale = Vector3.one * ChildScalex * 0.4f;
			return;

		}
		child.position = boundCol.center;
		if (isBos <= 0) {

			float xChildScale = boundCol.size.x / FloatFormat (_myspriteChild.bounds.size.x, 2);
			float yChildScale = boundCol.size.y / FloatFormat (_myspriteChild.bounds.size.y, 2);
			float childScale = xChildScale > yChildScale ? yChildScale : xChildScale;
			child.localScale = Vector3.one * childScale * _ratioScale;
		}
		child.localScale = new Vector3 (child.localScale.x / block.transform.localScale.x * parentScale, child.localScale.y / block.transform.localScale.y * parentScale, 1f);
		//--<
	}
		
	/**********************/
	/* locate block in map, read in csv file */
	/* id : ID of Block */
	/* rotation :  rotation of block */
	/* x,y :  position of block */
	/* attack_num :  strong of block */
	/**********************/
	public void MappingBlock (int id, float rotation, int x, int y, int attack_num)
	{
		//-->instance, locate block in stage
		GameObject newblock = Instantiate (GetBasicBlockById (id)) as GameObject;
		newblock.transform.SetParent (this.gameObject.transform);
		float cell = 1.08f;
		newblock.transform.localPosition = new Vector2 (x * cell, -y * cell);
		//--<

		//-->set Strong and KeyBlock in Map
		GameObject child = newblock.transform.GetChild (0).gameObject;
		child.GetComponent<Block_Detail> ().Strong = attack_num;
		if (child.GetComponent<Block_Detail> ().IsKeyBlock)
			Game8_Manager.instance.BlockKey++;
		//Rotate	
		RotateBlock (child, rotation);
		//--<
		LiveBlock.Add (newblock);
	}

	public void CreateBoss (int idStage, int idBlock, float rotation, int attack_num)
	{
		//-->instance boss
		GameObject newblock = Instantiate (GetBasicBlockById (idBlock)) as GameObject;
		newblock.transform.SetParent (this.gameObject.transform);
		newblock.AddComponent<BossController> ().InitBoss (GetBossPattern (idStage));
		//--<

		//-->set Strong and Rotation
		Transform child = newblock.transform.GetChild (0);
		child.eulerAngles = new Vector3 (0f, 0f, rotation);
		child.GetChild (0).eulerAngles = new Vector3 (0f, 0f, 0f);
		child.GetComponent<Block_Detail> ().Strong = attack_num;
		//--<
		LiveBlock.Add (newblock);
		Transform childImg = child.GetChild (0).transform;
		float imgScale = (childImg.lossyScale.x + childImg.lossyScale.y) / 2;
		childImg.SetParent (newblock.transform.parent);
		childImg.localScale = Vector3.one * imgScale;
		childImg.SetParent (child.transform);
	}


		

	/**********************/
	/* rotate block and change top-left pivot */
	/* child : Block_Detail */
	/* rot :  rotation of block */
	/**********************/
	void RotateBlock (GameObject child, float rot)
	{		

		child.transform.eulerAngles = new Vector3 (0f, 0f, -rot);
		SpriteRenderer _mysprite = child.gameObject.GetComponent<SpriteRenderer> ();
		child.transform.localPosition += new Vector3 (_mysprite.bounds.size.x / 2, -_mysprite.bounds.size.y / 2, .0f);

		//-->Roate child Img 
		Transform childImg = child.transform.GetChild (0).transform;
		childImg.SetParent (child.transform.parent);
		childImg.localEulerAngles = Vector3.zero;
		childImg.localScale = new Vector3 (Mathf.Abs (childImg.localScale.x), Mathf.Abs (childImg.localScale.y), 1f);
		childImg.SetParent (child.transform);
		//--<

	}

	float FloatFormat (float x, int n)
	{
		return ((float)Mathf.RoundToInt (x * n * 10) / (n * 10));
	}

	public void SavingStage (int id, int isboss, string pattern)
	{
		if (m_ListStage == null)
			m_ListStage = new List<Stage> ();
		m_ListStage.Add (new Stage (id, isboss, pattern));
	}

	public bool CheckBossStage (int id)
	{
		foreach (Stage stage in m_ListStage) {
			if (stage.CheckStage (id))
				return true;
		}
		return false;
	}

	public int[] GetBossPattern (int id)
	{
		foreach (Stage stage in m_ListStage) {
			if (stage.CheckStage (id))
				return stage.GetPattern ();
		}
		return null;
	}

	private class Stage
	{
		private int m_Id;
		private bool m_IsBoss;
		private string m_PatternAction;

		public Stage (int id, int isboss, string pattern)
		{
			m_Id = id;
			m_IsBoss = isboss > 0;
			m_PatternAction = pattern;
		}

		public bool CheckStage (int id)
		{
			return (m_IsBoss && (m_Id == id));
		}

		public int[] GetPattern ()
		{
			string[] mypattern = m_PatternAction.Trim ().Split (new char[] { '#' }, System.StringSplitOptions.RemoveEmptyEntries);
			int[] actions = new int[mypattern.Length];
			for (int i = 0; i < actions.Length; i++) {
				actions [i] = int.Parse (mypattern [i]);
			}
			return actions;
		}
	}
		
}
