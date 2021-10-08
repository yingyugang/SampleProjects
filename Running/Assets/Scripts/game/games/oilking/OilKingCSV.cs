using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CSVSerifTalk
{
	public int ID;
	public int diggingid;
	public int throwingid;
	public string diggingname;
	public string throwingname;
	public string diggingSerif;
	public string throwingSerif;
	//0,1
	public int trick;
	//0,1
	public int anime;


	public static CSVSerifTalk Instance(CSVSerifTalk _csv)
	{
		CSVSerifTalk csv = new CSVSerifTalk();
		csv.ID = _csv.ID;
		csv.diggingid = _csv.diggingid;
		csv.throwingid = _csv.throwingid;
		csv.diggingname = _csv.diggingname;
		csv.throwingname = _csv.throwingname;
		csv.diggingSerif = _csv.diggingSerif;
		csv.throwingSerif = _csv.throwingSerif;
		csv.trick = _csv.trick;
		csv.anime = _csv.anime;
		return csv;
	}
}

[System.Serializable]
public class BlockAttribute
{
	public int ID;
	public string blockName;
	public Block typeBlock;
	public int timeBreak;
	public int brokenable;
	public int coinMin;
	public int coinMax;
	public bool moveable;
	public bool throwable;
	public Sprite imgBlock;
	public float Percent;
	public int index_percent;

	public static BlockAttribute Instance(BlockAttribute _blockattribute)
	{
		BlockAttribute block = new BlockAttribute();
		block.ID = _blockattribute.ID;
		block.typeBlock = _blockattribute.typeBlock;
		block.blockName = _blockattribute.blockName;
		block.timeBreak = _blockattribute.timeBreak;
		block.brokenable = _blockattribute.brokenable;
		block.coinMin = _blockattribute.coinMin;
		block.coinMax = _blockattribute.coinMax;
		block.moveable = _blockattribute.moveable;
		block.throwable = _blockattribute.throwable;
		block.imgBlock = _blockattribute.imgBlock;
		block.Percent = _blockattribute.Percent;
		block.index_percent = _blockattribute.index_percent;
		return block;
	}

	public override string ToString()
	{
		return string.Format("[BlockAttribute]: ID=" + ID + ",Block=" + typeBlock + ",HitPuch=" + timeBreak);
	}
}

[System.Serializable]
public class ItemAttribute
{
	//id,name,type,timebreak,throwable,score
	public int ID;
	public string name;
	public Block typeItem;
	public int timeBreak;
	public bool throwable;
	public int score;
	public Sprite imgSprite;
	public int index_percent;
	public float Percent;

	public static ItemAttribute Instance(ItemAttribute _itemAtribute)
	{
		ItemAttribute item = new ItemAttribute();
		item.ID = _itemAtribute.ID;
		item.name = _itemAtribute.name;
		item.typeItem = _itemAtribute.typeItem;
		item.timeBreak = _itemAtribute.timeBreak;
		item.throwable = _itemAtribute.throwable;
		item.score = _itemAtribute.score;
		item.imgSprite = _itemAtribute.imgSprite;
		item.index_percent = _itemAtribute.index_percent;
		item.Percent = _itemAtribute.Percent;
		return item;
	}
}

public class OilKingCSV : MonoBehaviour
{

	public static OilKingCSV s_Instance;

	public List<CSVSerifTalk> lstSerifTalk = new List<CSVSerifTalk>();
	public List<BlockAttribute> lstBlockAttribute = new List<BlockAttribute>();
	public List<ItemAttribute> lstItemAttribute = new List<ItemAttribute>();

	private Sprite[] arrSprite;
	private Sprite[] imageCharacter;
	// Use this for initialization
	void Awake()
	{
		s_Instance = this;

		if (arrSprite == null)
		{
			GetResourcesFromAssetBundle();
		}

		if (imageCharacter == null)
		{
			GetImageCharacterFromAssetBundle();
		}

		//read CSV seritalk
		List<Dictionary<string, object>> dataSerifTalk = CSVReader.Read(OilKingConfig.CSV_SERIF_TALK);
		if (dataSerifTalk != null)
		{
			for (int i = 0; i < dataSerifTalk.Count; i++)
			{
				CSVSerifTalk csv = new CSVSerifTalk();
				csv.ID = (int)dataSerifTalk[i]["id"];
				csv.diggingid = (int)dataSerifTalk[i]["diggingid"];
				csv.throwingid = (int)dataSerifTalk[i]["throwingid"];
				csv.diggingname = (string)dataSerifTalk[i]["diggingname"];
				csv.throwingname = (string)dataSerifTalk[i]["throwingname"];
				csv.diggingSerif = (string)dataSerifTalk[i]["digging_serif"];
				csv.throwingSerif = (string)dataSerifTalk[i]["throwing_serif"];
				csv.trick = (int)dataSerifTalk[i]["trick"];
				csv.anime = (int)dataSerifTalk[i]["anime"];

				lstSerifTalk.Add(csv);
			}
		}

		//read CSV block
		List<Dictionary<string, object>> dataBlock = CSVReader.Read(OilKingConfig.CSV_BLOCK_ATTRIBUTE);
		if (dataBlock != null)
		{
			for (int i = 0; i < dataBlock.Count; i++)
			{
				BlockAttribute csv = new BlockAttribute();
				csv.ID = (int)dataBlock[i]["id"];
				csv.blockName = (string)dataBlock[i]["name"];
				csv.typeBlock = (Block)((int)dataBlock[i]["type"]);

				csv.timeBreak = (int)dataBlock[i]["time_break"];
				csv.brokenable = (int)dataBlock[i]["brokenable"];
				csv.coinMin = (int)dataBlock[i]["coin_min"];
				csv.coinMax = (int)dataBlock[i]["coin_max"];
				csv.moveable = ((int)dataBlock[i]["moveable"]) == 1 ? true : false;
				csv.throwable = ((int)dataBlock[i]["throwable"]) == 1 ? true : false;

				csv.imgBlock = arrSprite[((int)dataBlock[i]["index_image"])];
				csv.index_percent = (int)dataBlock[i]["index_percent"];
				lstBlockAttribute.Add(csv);
			}
		}

		//read CSV item
		List<Dictionary<string, object>> dataItem = CSVReader.Read(OilKingConfig.CSV_ITEM_ATTRIBUTE);
		if (dataBlock != null)
		{
			for (int i = 0; i < dataItem.Count; i++)
			{
				ItemAttribute csv = new ItemAttribute();
				csv.ID = (int)dataItem[i]["id"];
				csv.name = (string)dataItem[i]["name"];
				csv.typeItem = (Block)((int)dataItem[i]["type"]);
				csv.timeBreak = (int)dataItem[i]["time_break"];
				csv.index_percent = (int)dataItem[i]["index_percent"];
				csv.throwable = ((int)dataItem[i]["throwable"]) == 1 ? true : false;
				csv.score = (int)dataItem[i]["score"];
				csv.imgSprite = arrSprite[((int)dataItem[i]["index_image"])];
				lstItemAttribute.Add(csv);
			}
		}
	}

	void GetResourcesFromAssetBundle()
	{
		arrSprite = new Sprite[17];
		arrSprite[0] = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.Stone);
		arrSprite[1] = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.Fossil);
		arrSprite[2] = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.Plaster);
		arrSprite[3] = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.Boom);
		arrSprite[4] = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.Treasures1);
		arrSprite[5] = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.Treasures2);
		arrSprite[6] = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.Treasures3);
		arrSprite[7] = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.Treasures4);
		arrSprite[8] = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.Treasures5);
		arrSprite[9] = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.Treasures6);
		arrSprite[10] = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.O_Item_1);
		arrSprite[11] = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.O_Item_2);
		arrSprite[12] = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.O_Item_3);
		arrSprite[13] = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.O_Item_4);
		arrSprite[14] = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.O_Item_5);
		arrSprite[15] = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.Boom);
		arrSprite[16] = OilKingAssetLoader.s_Instance.getSprite(TypeSprite.BlockFever);
	}

	void GetImageCharacterFromAssetBundle()
	{
		imageCharacter = new Sprite[12];

		for (int i = 0; i < 6; i++)
		{
			imageCharacter[i] = OilKingAssetLoader.s_Instance.getSpriteButtonDigging(i + 1);

			imageCharacter[i + 6] = OilKingAssetLoader.s_Instance.getSpriteButtonSpring(i + 1);
		}
	}

	public Sprite GetSpriteCharacterFromOilKingCSV(int i)
	{
		if (imageCharacter.Length == 0)
		{
			GetImageCharacterFromAssetBundle();
		}
		if (imageCharacter != null)
		{
			return imageCharacter[i];
		}
		else {
			return null;
		}
	}

	public BlockAttribute getBlockAttribute(Block _typeBlock)
	{

		BlockAttribute block = new BlockAttribute();
		for (int i = 0; i < lstBlockAttribute.Count; i++)
		{
			if (lstBlockAttribute[i].typeBlock == _typeBlock)
			{

				//percent
				float percentblock = 0;
				//chest ratio
				if (_typeBlock == Block.Treasure1 || _typeBlock == Block.Treasure2 || _typeBlock == Block.Treasure3 || _typeBlock == Block.Treasure4
					|| _typeBlock == Block.Treasure5 || _typeBlock == Block.Treasure6)
				{
					percentblock = ParameterServer.lstProbabilityBlock[4] / 6;
				}
				else {
					percentblock = ParameterServer.lstProbabilityBlock[lstBlockAttribute[i].index_percent];
				}

				lstBlockAttribute[i].Percent = percentblock;


				block = BlockAttribute.Instance(lstBlockAttribute[i]);
			}
		}
		return block;
	}

	 public CSVSerifTalk getSerifTalk(int _iddigging, int _idthrowing)
	{
		List<CSVSerifTalk> lstSerifChoise = new List<CSVSerifTalk>();

		//cheat
		bool isCheat = false;
		CheatData cheatData = CheatController.GetLastMatchCheat();
		if (cheatData != null)
		{
			//turn on
			isCheat = true;
		}
		else {
			isCheat = false;
		}

		for (int i = 0; i < lstSerifTalk.Count; i++)
		{
			//Debug.Log ("lstSerifTalk=" + _id);
			//			Debug.Log ("lstSerifTalk [i].diggingid=" + lstSerifTalk [i].diggingid + " lstSerifTalk [i].throwingid=" + lstSerifTalk [i].throwingid);
			if (lstSerifTalk[i].diggingid == _iddigging && lstSerifTalk[i].throwingid == _idthrowing)
			{
				//check trick
				if (lstSerifTalk[i].trick == 0 || (lstSerifTalk[i].trick == 1 && isCheat))
				{
					lstSerifChoise.Add(CSVSerifTalk.Instance(lstSerifTalk[i]));
				}

			}
		}
		Debug.Log("lstSerifChoise=" + lstSerifChoise.Count);
		int idSerifServer = 0;

		CSVSerifTalk serifTalk = lstSerifChoise[Random.Range(0, lstSerifChoise.Count)];

		return serifTalk;
	}
	public CSVSerifTalk GetSerifByID(int id)
	{
		CSVSerifTalk csvserif = new CSVSerifTalk();
		for (int i = 0; i < lstSerifTalk.Count; i++)
		{
			if (lstSerifTalk[i].ID == id)
			{
				csvserif = lstSerifTalk[i];
				return csvserif;
			}
		}
		return null;
	}
	public int[,] getArraysSerif()
	{
		int numCharacter = 6;
		int[,] arrSerif = new int[numCharacter, numCharacter];
		if (ParameterServer.lstSerifDone != null)
		{
			for (int i = 0; i < ParameterServer.lstSerifDone.Count; i++)
			{
				arrSerif[GetSerifByID(ParameterServer.lstSerifDone[i]).diggingid - 1, GetSerifByID(ParameterServer.lstSerifDone[i]).throwingid - 1]++;
			}
		}
		return arrSerif;
	}

	public ItemAttribute getItemAttribute(Block _typeItem)
	{
		ItemAttribute item = new ItemAttribute();
		for (int i = 0; i < lstItemAttribute.Count; i++)
		{
			if (lstItemAttribute[i].typeItem == _typeItem)
			{
				lstItemAttribute[i].Percent = ParameterServer.lstProbabilityChestItem[lstItemAttribute[i].index_percent];
				item = ItemAttribute.Instance(lstItemAttribute[i]);
			}
		}
		return item;
	}

	[ContextMenu("DebugAttribute")]
	public void DebugAttribute()
	{
		Debug.Log("block=" + getBlockAttribute(Block.Plaster).ToString());
	}
}