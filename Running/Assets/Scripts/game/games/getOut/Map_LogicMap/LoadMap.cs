using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public enum  TypeWall
{
	LineEnd = 0,
	Line = 1,
	Corner = 2,
	TheFork = 3,
	Block = 8,
	Door = 9
}






public class LoadMap : MonoBehaviour {
	public GameObject GateEndLeft;
	public GameObject background;
	public GameObject Special;
	public static Vector3 ScreenSize;
	public static MapNode map;
	public List<Wall> lstWall;
	public List<int> lstIndexCollumLineVertical;
	public GameObject[] arrayPrefabWall;
	public Wall[] doorLeft;
	public Wall[] doorRight;
	public GameObject[] objDoorRight;
	public GameObject[] objDoorLeft;
	public Sprite[] arraySpriteCorner;
	public Sprite[] arraySpriteLine;
	public Sprite[] arraySpriteTheFork;
	public Sprite[] arraySpriteLineEnd;
	private Wall wall;
	private float SizeBlock = 0.44f;
	private static LoadMap _instance;
	private int[] arrayEulurAngles = {90,-90,0,180,180,0,90,-90,0,90,90,-90,180,0};

	void Awake()
	{
		_instance = this;
	}

	public static LoadMap Instance()
	{
		return _instance;
	}

	//Init map
	public void Init()
	{
		string nameFileBackground = GetOut.GameParams.Instance.mapBackGround.nameFile;
		if(nameFileBackground != null)
			LoadImage(nameFileBackground);
		map = new MapNode (Map.s_Matrix);
		lstWall = new List<Wall> ();
		SetBlockImage();
		SetBlockID ();
		SetIdForLine ();
		Render();
		InitDoor();
		background.transform.position = new Vector3 (-LoadMap.ScreenSize.x + Map.SizeBlock * (Map.HEIGHT/2-0.5f), LoadMap.ScreenSize.y - Map.SizeBlock * (Map.WIDTH/2), 0f);
		Special.transform.position = new Vector3 (-LoadMap.ScreenSize.x + Map.SizeBlock * (Map.HEIGHT/2-0.5f), LoadMap.ScreenSize.y - Map.SizeBlock * (Map.WIDTH/2), 0f);
	}

	private float m_Special_Width = 1166f;
	private float m_Special_Heigh = 770f;

	private float m_Bg_Width = 2650f;
	private float m_Bg_Heigh = 1920f;

	private string m_Default_BG = "Background3";

	private void LoadImage(string nameFile)
	{
		Texture2D texture;
		texture = Resources.Load("games/getOut/"+nameFile) as Texture2D;
		texture.filterMode = FilterMode.Trilinear;
		Sprite sprite = Sprite.Create(texture, new Rect(0,0,texture.width, texture.height), new Vector2(0.5f,0.5f), 100.0f);


		if (texture.width <= m_Special_Width) {
			Special.GetComponent<SpriteRenderer> ().sprite = sprite;
			Special.transform.localScale = new Vector3 (m_Special_Width / texture.width, m_Special_Heigh / texture.height, 1f);
			Debug.Log ("Special stage !");
			/****/
			Texture2D texture_BG;
			texture_BG = Resources.Load("games/getOut/"+ m_Default_BG) as Texture2D;
			texture_BG.filterMode = FilterMode.Trilinear;
			Sprite sprite_BG = Sprite.Create(texture_BG, new Rect(0,0,texture_BG.width, texture_BG.height), new Vector2(0.5f,0.5f), 100.0f);
			background.GetComponent<SpriteRenderer> ().sprite = sprite_BG;
			background.transform.localScale = new Vector3 (m_Bg_Width / texture_BG.width, m_Bg_Heigh / texture_BG.height, 1f);
			/****/

		}
		else {
			Debug.Log ("Normal stage !");
			background.GetComponent<SpriteRenderer> ().sprite = sprite;
			background.transform.localScale = new Vector3 (m_Bg_Width / texture.width, m_Bg_Heigh / texture.height, 1f);
		}

	}


	private string RandomBackground()
	{
		int totalAppearPercent = 0;
		int percent = 0;
		for(int i = 0; i < GetOut.GameParams.Instance.arrayBackgroundData.Length; i++)
		{
			totalAppearPercent += GetOut.GameParams.Instance.arrayBackgroundData[i].appearProbability;
		}

		int random = Random.Range(0,totalAppearPercent);

		for(int i = 0; i < GetOut.GameParams.Instance.arrayBackgroundData.Length; i++)
		{
			if(random > percent && random > percent +GetOut.GameParams.Instance.arrayBackgroundData[i].appearProbability)
			{
				percent += GetOut.GameParams.Instance.arrayBackgroundData[i].appearProbability;
			}
			else
				return GetOut.GameParams.Instance.arrayBackgroundData[i].nameFile;
		}
		return null;
	}

	public void InitDoor()
	{
		//Door1 is the door on the left of the map
		doorLeft = new Wall[2];

		//Init Door Top Left
		doorLeft[0] = new Wall(new Vector2(Map.GateY1-2,Map.GateLeftX+0.5f));

		//Init Door Bot Left
		doorLeft[1] = new Wall(new Vector2(Map.GateY2+1,Map.GateLeftX+0.5f));


		//Door2 is the door on the right of the map
		doorRight = new Wall[2];

		//Init Door Top Right
		doorRight[0] = new Wall(new Vector2(Map.GateY1-2,Map.GateRightX+0.5f));

		//Init Door Bot Right
		doorRight[1] = new Wall(new Vector2(Map.GateY2+1,Map.GateRightX+0.5f));

		objDoorLeft = RenderDoor(doorLeft);
		objDoorRight = RenderDoor(doorRight);

		UnRenderDoor(objDoorRight);
		UnRenderDoor(objDoorLeft);
	}
		
	public GameObject[] RenderDoor(Wall [] wall)
	{
		GameObject[] door = new GameObject[wall.Length] ;
		for(int i = 0; i < wall.Length; i++)
			door[i] =((GameObject)Instantiate (GateEndLeft, new Vector3 (-ScreenSize.x + wall[i].Position.y * SizeBlock, ScreenSize.y - (wall[i].Position.x ) * SizeBlock, 0), transform.rotation));
		return door;
	}

	public void UnRenderDoor(GameObject[] door)
	{
		for(int i = 0; i < door.Length; i++)
		{
			door[i].SetActive(false);
		}
	}

	public void ActiveDoor(GameObject[] door)
	{
		for(int i = 0; i < door.Length; i++)
		{
			door[i].SetActive(true);
		}
	}
	//set block image
	public void SetBlockImage()
	{
		for (int i = 0; i < Map.WIDTH; i++) {
			for (int j = 0; j < Map.HEIGHT; j++) {
				if (!Map.IsNotWall (i, j)) {
					CheckBlockImage (i, j);
				}
			}
		}
	}

	//set ID image for block
	public void SetBlockID()
	{
		for (int i = 0; i < lstWall.Count; i++) {
			SetIdImageBlock (lstWall [i]);
		}
	}

	//Check 4 position around the wall
	public void CheckBlockImage(int i, int j)
	{

		wall = new Wall (new Vector2(i,j));
		if (i == 0 || i == Map.WIDTH - 1) {
			wall.idPrefabs = GameConfig.ID_LINE_HORIZONTAL;
			wall.idColor = Map.s_Matrix[i,j];
			wall.idSprite = GameConfig.ID_SPRITE_LINE;
		}

		if (j == 0 || j == Map.HEIGHT -1) {
			wall.idPrefabs = GameConfig.ID_LINE_VERTICLE;
			wall.idColor = Map.s_Matrix[i,j];
			wall.idSprite = GameConfig.ID_SPRITE_LINE;
		}
		//Up
		if(i >= 1)
			if (!Map.IsNotWall (i - 1, j)) {
				wall.haveWallUp = true;
				wall.TotalWallAround++;
			}
		//Down
		if(i <= Map.WIDTH-2)
			if (!Map.IsNotWall (i+1, j)) {
				wall.haveWallDown = true;
				wall.TotalWallAround++;
			}
		//Left
		if(j >= 1)
			if (!Map.IsNotWall (i, j-1)) {
				wall.haveWallLeft = true;
				wall.TotalWallAround++;
			}
		//Right
		if(j <= Map.HEIGHT-2)
			if (!Map.IsNotWall (i, j+1)) {
				wall.haveWallRight = true;
				wall.TotalWallAround++;
			}

		lstWall.Add (wall);
	}


	//Set ID image for the wall block
	public void SetIdImageBlock(Wall wall)
	{
		//Check the wall is fork
		if (wall.TotalWallAround == (int)TypeWall.TheFork) {
			if(Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y] != (int)TypeWall.Block && Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y] != (int)TypeWall.Door)
			{
			if (!wall.haveWallUp ) {
					wall.idPrefabs = GameConfig.ID_THE_FORK_DOWN;
					wall.idColor = Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y];
					wall.idSprite = GameConfig.ID_SPRITE_THE_FORK;
			} else if (!wall.haveWallDown) {
					wall.idPrefabs = GameConfig.ID_THE_FORK_UP;
					wall.idColor = Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y];
					wall.idSprite = GameConfig.ID_SPRITE_THE_FORK;
			} else if (!wall.haveWallLeft) {
					wall.idPrefabs = GameConfig.ID_THE_FORK_RIGHT;
					wall.idSprite = GameConfig.ID_SPRITE_THE_FORK;
					wall.idColor = Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y];
			} else
				{
					wall.idPrefabs = GameConfig.ID_THE_FORK_LEFT;
					wall.idColor = Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y];
					wall.idSprite = GameConfig.ID_SPRITE_THE_FORK;
				}
			}
			if(!lstIndexCollumLineVertical.Contains((int)wall.Position.y))
				lstIndexCollumLineVertical.Add ((int)wall.Position.y);
			return;
		}

		//check the wall is conner
		if (wall.TotalWallAround == (int)TypeWall.Corner) {
			if(Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y] != (int)TypeWall.Block && Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y] != (int)TypeWall.Door)
			{
				if (wall.haveWallUp) {
					if (wall.haveWallRight) {
						wall.idPrefabs = GameConfig.ID_CORNER_UP;
						wall.idColor = Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y];
						wall.idSprite = GameConfig.ID_SPRITE_CORNER;
					} else if (wall.haveWallLeft) {
						wall.idPrefabs = GameConfig.ID_CORNER_LEFT;
						wall.idColor = Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y];
						wall.idSprite = GameConfig.ID_SPRITE_CORNER;
					} 
					if(!lstIndexCollumLineVertical.Contains((int)wall.Position.y))
						lstIndexCollumLineVertical.Add ((int)wall.Position.y);
				} else if (wall.haveWallDown) {
					if (wall.haveWallRight) {
						wall.idPrefabs = GameConfig.ID_CORNER_RIGHT;
						wall.idColor = Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y];
						wall.idSprite = GameConfig.ID_SPRITE_CORNER;
					} else if (wall.haveWallLeft) {
						wall.idPrefabs = GameConfig.ID_CORNER_DOWN;
						wall.idColor = Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y];
						wall.idSprite = GameConfig.ID_SPRITE_CORNER;
					}
					if(!lstIndexCollumLineVertical.Contains((int)wall.Position.y))
						lstIndexCollumLineVertical.Add ((int)wall.Position.y);
				}
			}
			return;
		}

		//Check the line end
		if(wall.TotalWallAround ==(int) TypeWall.Line  ){
			if(Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y] != (int)TypeWall.Block && Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y] != (int)TypeWall.Door)
			{
				wall.idColor = Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y];
				wall.idSprite = GameConfig.ID_SPRITE_LINE_END;
				if(wall.haveWallUp)
				{
					wall.idPrefabs = GameConfig.ID_LINE_END_UP;
				}else
					if(wall.haveWallDown)
						wall.idPrefabs = GameConfig.ID_LINE_END_DOWN;
					else
						if(wall.haveWallLeft)
							wall.idPrefabs = GameConfig.ID_LINE_END_LEFT;
						else
							wall.idPrefabs = GameConfig.ID_LINE_END_RIGHT;

			}


			return;
		}

		//Check the line horizontal
		if(wall.TotalWallAround == (int) TypeWall.LineEnd)
		{
			if(Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y] != (int)TypeWall.Block && Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y] != (int)TypeWall.Door)
			{
				wall.idPrefabs = GameConfig.ID_LINE_HORIZONTAL;
				wall.idColor = Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y];
				wall.idSprite = GameConfig.ID_SPRITE_LINE;
			}
			return;
		}
	}

	//SetID for wall horizontal or vertical
	public void SetIdForLine()
	{
		Wall wall = null;
		for (int i = 0; i < lstWall.Count; i++) {
			wall = lstWall [i];
			if (wall.TotalWallAround == (int)TypeWall.Corner && wall.idPrefabs == 0) {
				if (lstIndexCollumLineVertical.Contains ((int)wall.Position.y) && (wall.haveWallUp||wall.haveWallDown)) {
					if(Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y] != (int)TypeWall.Block && Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y] != (int)TypeWall.Door)
					{
						wall.idPrefabs = GameConfig.ID_LINE_VERTICLE;
						wall.idColor = Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y];
						wall.idSprite = GameConfig.ID_SPRITE_LINE;
					}
				} else
					if(Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y] != (int)TypeWall.Block && Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y] != (int)TypeWall.Door)
					{
						wall.idPrefabs = GameConfig.ID_LINE_HORIZONTAL;
						wall.idColor = Map.s_Matrix[(int)wall.Position.x,(int)wall.Position.y];
						wall.idSprite = GameConfig.ID_SPRITE_LINE;
					}
			}
		}
	}

	//Render Map
	public void Render ()
	{
		Wall wall = null;
		for(int i = 0; i < lstWall.Count; i++)
		{
			wall = lstWall [i];
			InitWall(wall);
		}
	}





	//Render wall block
	public GameObject InitWall(Wall wall)
	{
		GameObject wallObject;
		Quaternion rotation = transform.rotation;
		wallObject = ((GameObject)Instantiate (arrayPrefabWall[wall.idSprite], new Vector3 (-ScreenSize.x + wall.Position.y * SizeBlock, ScreenSize.y - (wall.Position.x ) * SizeBlock, 0), transform.rotation));
		rotation = transform.rotation;
		if((wall.idPrefabs-1) >= 0 && (wall.idPrefabs-1) < arrayEulurAngles.Length)
			rotation.eulerAngles += new Vector3(0,0,arrayEulurAngles[wall.idPrefabs-1]);
		wallObject.transform.parent = transform;
		wallObject.transform.rotation = rotation;
		if((wall.idColor - 1) >= 0)
		switch(wall.idSprite)
		{
		case (int) TypeWall.LineEnd:
			wallObject.GetComponent<SpriteRenderer>().sprite = arraySpriteLineEnd[wall.idColor-1];
			break;
		case (int) TypeWall.Line:
			wallObject.GetComponent<SpriteRenderer>().sprite = arraySpriteLine[wall.idColor-1];
			break;
		case (int) TypeWall.Corner:
			wallObject.GetComponent<SpriteRenderer>().sprite = arraySpriteCorner[wall.idColor-1];
			break;
		case (int) TypeWall.TheFork:
			wallObject.GetComponent<SpriteRenderer>().sprite = arraySpriteTheFork[wall.idColor-1];
			break;
		}
		return wallObject;
	}
}
