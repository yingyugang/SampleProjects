using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Terrain_Skin
{
	Desert = 1,
	Forest,
	Akatsuka,
	none
}

public enum Game12_level
{
	desert = 0,
	forest,
	akatsuka,
	maxVal
}

public class MapBuilder
{
	public float distance;
	public float[] percentage;
	private float m_TotalPercent;
	public List<Terrain_RenderMap> listEnable;

	public MapBuilder(){
		distance = -1f;
		listEnable = new List<Terrain_RenderMap> ();
	}

	public void UpdateListEnable (Terrain_RenderMap terrent)
	{
		if (listEnable == null)
			listEnable = new List<Terrain_RenderMap> ();
		listEnable.Add (terrent);
	}

	public void UpdateDistance (float distance)
	{
		this.distance = distance;
	}

	public int RandomID ()
	{
		//return 0;
		//--> Set percent
		percentage = new float[listEnable.Count];
		if (percentage.Length <= 0)
			return 1;
		percentage [0] = listEnable [0].Percentage;
		for (int i = 1; i < percentage.Length; i++) {
			percentage [i] = percentage [i - 1] + listEnable [i].Percentage;
		}
		m_TotalPercent = percentage [percentage.Length - 1];
		//--<

		//-->Random by percent
		float rand = Random.Range (0, m_TotalPercent);
		for (int i = 0; i < percentage.Length; i++) {
			if (rand < percentage [i])
				return listEnable [i].ID;
		}
		return listEnable [0].ID;
		//--<
	}
}

public class Terrain_Controller : MonoBehaviour
{
	protected Terrain_Skin SKIN = Terrain_Skin.none;
	private static Terrain_Controller m_instance;

	public static Terrain_Controller instance {
		get {
			return m_instance;
		}
	}
	void Awake(){
		m_instance = this;
	}

	public List<GameObject> Routes = new List<GameObject> ();
	private Terrain_Elmp[] Terrain_Elmp_List;
	//	public List<GameObject> TerrainList = new List<GameObject> ();
	//public List<GameObject> InvulnerableList = new List<GameObject> ();
	//public List<SpriteRenderer> ObstacleList = new List<SpriteRenderer> ();
	//public List<SpriteRenderer> ItemList = new List<SpriteRenderer> ();
	public List<Transform> PrefabBorderList = new List<Transform> ();
	public List<Transform> BorderList = new List<Transform> ();
	public List<GameObject> Rendering_Pattern = new List<GameObject> ();
	public List<Terrain_RenderMap> ListDashMap = new List<Terrain_RenderMap> ();
	public Game12_BGLoop[] BackgroundGroup;
	public Transform Player;
	public Transform RouteGroup_1;
	public Transform RouteGroup_2;
	public Texture Ground;
	public Texture Grass;
	public Texture BorderLeftGround;
	public Texture BorderLeftGrass;
	public Texture BorderRightGround;
	public Texture BorderRightGrass;
	public int indexMap = 0;
	[HideInInspector]public Transform Standard_Terrain;
	[HideInInspector]public Transform Border_Terrain;
	[HideInInspector]public int PatternID;
	[HideInInspector]public int TempPatternID;
	[HideInInspector]public float pattern_width_group = .0f;
	[HideInInspector]public int countParttern = 0;
	public int Automatic_change_BG_distance = 600;
	// Private
	private bool m_IsTimeLoop;
	private int m_Distance;
	int widthcount = 0;
	int oldIndexPatter = -1;

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update ()
	{
		// Pattern loop
		if (Routes.Count >= 1) {
			InitNextPattern ();
		}

	}

	void ChangeMapSkind ()
	{
		if (SKIN == Terrain_Skin.Desert) {
			SetPatternTexture (2, 3);
			SKIN = Terrain_Skin.Forest;
	
		} else if (SKIN == Terrain_Skin.Forest) {
			SetPatternTexture (4, 5);
			SKIN = Terrain_Skin.Akatsuka;

		} else {
			SetPatternTexture (0, 1);
			SKIN = Terrain_Skin.Desert;
		}
		StartCoroutine ("ChangeTexture");
	}
	//->|by anhgh
	private List<int> Distance_List_To_Random = new List<int>();
	private List<int> Route_ID_List = new List<int>();
	private int[]  Background_appear_distance = {0,300,600};
	public bool[]  Background_appear = {false,false,false};
	private int currentBackground = 0;
	private int currentDistance = -300;
	private int lastpick = -40;
	private int start_generate_distance = 40;
	//int oldIndexPatter = -1;
	void PickFirstPattern(int distance){
		Debug.Log ("Find next pattern for distance -> " + currentDistance + start_generate_distance);
		int total_percent = 0;
		for (int i = 0; i < Route_ID_List.Count; i++) {
//			Debug.Log (ID_to_pattern [Route_ID_List [i]].Appear_Distance);

			if (ID_to_pattern [Route_ID_List [i]].Appear_Distance <= currentDistance + start_generate_distance
				&& ID_to_pattern [Route_ID_List [i]].Appear_Distance_End > currentDistance + start_generate_distance
				&& ID_to_pattern [Route_ID_List [i]].Percentage > 0) 
			{
				Debug.Log ("Can pick ID -> " + Route_ID_List[i]);
				total_percent += ID_to_pattern [Route_ID_List [i]].Percentage;
			}
		}
		Debug.Log ("total_percent -> " + total_percent);
		int local_percent = total_percent;
		int per = Random.Range (0,total_percent);
		Debug.Log ("percent -> " + per);
		for (int i = 0; i < Route_ID_List.Count; i++) {
			if (ID_to_pattern [Route_ID_List [i]].Appear_Distance <= currentDistance + start_generate_distance
				&& ID_to_pattern [Route_ID_List [i]].Appear_Distance_End > currentDistance + start_generate_distance
				&& ID_to_pattern [Route_ID_List [i]].Percentage > 0) 
			{
				local_percent -= ID_to_pattern [Route_ID_List [i]].Percentage;
				per -= ID_to_pattern [Route_ID_List [i]].Percentage;

				Debug.Log (per + " / " + local_percent);
				if (per < 0 || local_percent == 0) { //ID_to_pattern [Route_ID_List [i]].Percentage
					/*
					 */
					if (oldIndexPatter >=0)
						Routes [oldIndexPatter].SetActive (false); //free pool

					if (local_percent == 0)
						Debug.Log ("Must pick now.");
					
					oldIndexPatter = currentPattern.ID;
					Debug.Log ("Now pick ID -> " + Route_ID_List [i]);
					int randID = Route_ID_List [i] - 1;
					if (currentPattern.ID == randID) {
						randID += countParttern;
						Debug.Log ("Same pattern, now use from pool");
					}
					currentPattern.ID = randID;
					GrabPattern ();
					return;
				}
			}
		}

	}
	//--<|by anhgh
	//-->|by anhgh 
	void InitNextPattern ()
	{
		
		//make bg change via param in csv
		if (!Game12_GameManager.instance)
			return;
		int dis = (int)Game12_GameManager.instance.Distance;// (int)Game12_Player_Controller.instance.Player.transform.position.x;
		//Debug.Log ("Distance = " + dis);
		if (dis > currentDistance) {
			currentDistance = dis;
			if (dis >= lastpick) {
				PickFirstPattern (dis);
				lastpick += m_Distance;
			}

			if (currentBackground < Background_appear_distance.Length
				&& !Background_appear[currentBackground] 
				&& currentDistance >= Background_appear_distance [currentBackground] - start_generate_distance) {
				Background_appear [currentBackground] = true;
				if (Background_appear_distance [currentBackground] > 0) {
					EvrLvlChange ((Game12_level)(currentBackground % 3));
					ChangeMapSkind ();
				}
				currentBackground++;
			}
			if (currentBackground > 1)
				Terrain_Generation.instance.StandardPattern.SetActive (false); //hide standard ground
		}
	}
	//--<|by anhgh 
	private static int MAX_PATTERN = 6000;
	void Start ()
	{
		LoadDashMapData ();
		m_Distance = (int)Game12_GameManager.instance.DistancePerPatern;// BikingKey.GameConfig.DistanceToChangePattern;
		Debug.Log("Distance Per Patern : " +m_Distance);
		currentDistance = -m_Distance;
		lastpick = - (int)(Game12_GameManager.instance.DistancePerPatern / 5);
		start_generate_distance = (int)(Game12_GameManager.instance.DistancePerPatern / 5);
		SKIN = Terrain_Skin.Desert;
		Terrain_Init ();
		Background_appear_distance = new int [MAX_PATTERN];
		Background_appear = new bool[MAX_PATTERN];

		Debug.Log (Game12_GameParams.instance.terrainAbyssData.Count);
		for (int i = 0; i < Game12_GameParams.instance.terrainAbyssData.Count; i++) {
			Background_appear_distance [i] = (int)Game12_GameParams.instance.terrainAbyssData [i] ["appear_distance"];
		//	Debug.Log (Game12_GameParams.instance.terrainAbyssData [i] ["name"] + " --> " + Background_appear_distance [i] );
		}
		for (int i = Game12_GameParams.instance.terrainAbyssData.Count; i < Background_appear_distance.Length; i++) {
			Background_appear_distance [i] = Background_appear_distance [i - 1] + Automatic_change_BG_distance; 
		}
			
		currentBackground = 0;
		currentPattern.ID = -1;
	}
	private Terrain_RenderMap[] ID_to_pattern;
	void LoadDashMapData ()
	{
		ID_to_pattern = new Terrain_RenderMap[BikingKey.GameConfig.MaxMapID];
		float current_distance = -1f;
		//-->|by anhgh
		for (int i = 0; i < Game12_GameParams.instance.dashMap.Count; i++) {
			Terrain_RenderMap pattern;
			int ID = (int)Game12_GameParams.instance.dashMap [i] ["id"];
			string Name = Game12_GameParams.instance.dashMap [i] ["name"].ToString ();
			int Percentage = (int)(Game12_GameParams.instance.dashMap [i] ["percentage"]);
			int Distance = (int)(Game12_GameParams.instance.dashMap [i] ["appear_distance"]);
			int Max_Y = 0;// = (int) (Game12_GameParams.instance.dashMap [i] ["max_y"]);
			int Min_Y = 0;// = (int)(Game12_GameParams.instance.dashMap [i] ["min_y"]);

			int Distance_end = (int)(Game12_GameParams.instance.dashMap [i] ["appear_distance_end"]);


			Route_ID_List.Add(ID);
			pattern = new Terrain_RenderMap(ID, Name, Percentage, Distance, Max_Y, Min_Y, Distance_end);
			ID_to_pattern [ID] = pattern;
			//-<|by anhgh

			//terrent = new Terrain_RenderMap (ID, Name, Percentage, Distance, Max_Y, Min_Y);
			//ListDashMap.Add (terrent);
			//Debug.Log ("Map ID " + ID + " appear_distance " + Distance);
		}
		//--<|by anhgh
	}


	//public List<MapBuilder> listMap = new List<MapBuilder> ();
	public Terrain_RenderMap currentPattern;

	void GrabPattern ()
	{
		Debug.Log ("Use pattern " + Routes [currentPattern.ID].name);
		Routes [currentPattern.ID].transform.position = new Vector3 (Standard_Terrain.position.x + Standard_Terrain.GetComponent<Terrain_Elmp> ()._xWDelta + pattern_width_group * widthcount, Standard_Terrain.position.y, .0f);
		widthcount++;

		Routes [currentPattern.ID].SetActive (true);
		Terrain_Elmp_List[currentPattern.ID].RandomItem();
		StartCoroutine ("ChangeTexture");
	}


	/// <summary>
	/// We random pattern every clash and add into Rendering_Pattern to create the map.
	/// </summary>
	/// <param name="pattern">Pattern.</param>
	/// <param name="take">Take.</param>
	void PatternRandom (List<GameObject> pattern, int take)
	{
		List<int> numbers = new List<int> ();
		for (int i = 0; i < 10; i++)
			numbers.Add (i);
		for (int i = 0; i < pattern.Count; i++) {
			if (i > take - 1)
				break;
			int rd = numbers [Random.Range (0, numbers.Count)];
			GetChildPattern (pattern [rd]);
			numbers.Remove (rd);
		}
	}

	/// <summary>
	/// We get child every pattern.
	/// </summary>
	/// <param name="pattern">Pattern.</param>
	void GetChildPattern (GameObject pattern)
	{
		pattern.SetActive (true);
		foreach (Transform tf in pattern.transform) {
			Rendering_Pattern.Add (tf.gameObject);
		}
	}

	/// <summary>
	/// Hides the big pattern.
	/// </summary>
	/// <param name="pattern">Pattern.</param>
	void HideBigPattern (List<GameObject> pattern)
	{
		for (int i = 0; i < pattern.Count; i++) {
			pattern [i].SetActive (false);
		}
	}

	public void HideAllPattern ()
	{
		Debug.Log ("Now hide all pattern before game start : " + Routes.Count);
		Terrain_Elmp_List = new Terrain_Elmp[Routes.Count];
		for (int i = 0; i < Routes.Count; i++) {
			Terrain_Elmp_List [i] = Routes [i].GetComponent<Terrain_Elmp>();
			Routes [i].SetActive (false);
		}
	}

	/// <summary>
	/// Init the terrains
	/// </summary>
	private void Terrain_Init ()
	{

		//For standard
		if (GameObject.Find (BikingKey.Terrain.Standard)) {
			Standard_Terrain = GameObject.Find (BikingKey.Terrain.Standard).transform;
			pattern_width_group = Standard_Terrain.GetComponent<Terrain_Elmp> ()._xWDelta;
			Standard_Terrain.transform.position = new Vector3 (Standard_Terrain.position.x - pattern_width_group,
				Standard_Terrain.transform.position.y, .0f);
		}

//		//For border
		if (GameObject.Find (BikingKey.Terrain.BorderRight)) {
			Border_Terrain = GameObject.Find (BikingKey.Terrain.BorderRight).transform;
			Border_Terrain.position = new Vector3 (-pattern_width_group * 3, .0f, .0f);
		}

		// Hide border. Why? Because i just use it for Instantiate.
//		PrefabBorderList [0].position = new Vector3 (Standard_Terrain.position.x - 20, Standard_Terrain.position.y, Standard_Terrain.position.z);
		CreateBorder ();
//		Terrain_Group ();
	}
		
	/// <summary>
	/// We add border later for each terrain.
	/// We set position at the border's abyss. Why? Because the border just only appear when the abyss available on pattern.
	/// </summary>
	void CreateBorder ()
	{
		//For main terrain
		for (int i = 0; i < Routes.Count; i++) {
			// Abyss border
			if (Terrain_Elmp_List[i].HeightPoint.name != "None") {
				for (int j = 0; j < Terrain_Elmp_List[i].AbyssInvulnerableList.Count; j++) {
					// For right
					Transform borderRight = Instantiate (
						PrefabBorderList [0], 
						new Vector3 (Terrain_Elmp_List[i].AbyssInvulnerableList [j].GetComponent<EdgeCollider2D> ().points [3].x,
							Terrain_Elmp_List[i].AbyssInvulnerableList [j].GetComponent<EdgeCollider2D> ().points [3].y // - (PrefabBorderList [0].GetChild (0).GetComponent<MeshRenderer> ().bounds.size.y)
						                       -38f - BikingKey.Terrain.CliffTriggerHeight, -.1f),
						Terrain_Elmp_List[i].AbyssInvulnerableList [j].transform.rotation
					) as Transform;
					ModifyBorder (borderRight, Terrain_Elmp_List[i].AbyssInvulnerableList [j].parent.parent.parent, false);

//					borderRight.GetChild (0).GetComponent<MeshRenderer>().material.mainTextureScale=new Vector2(borderRight.GetChild (0).GetComponent<MeshRenderer>().material.mainTextureScale.x*-1,
//						borderRight.GetChild (0).GetComponent<MeshRenderer>().material.mainTextureScale.y);

					//Terrain_Elmp_List[i].Terrain_Struct.BorderClone.Add (borderRight.GetChild (0).GetComponent<MeshRenderer> ());
					//Terrain_Elmp_List[i].Terrain_Struct.BorderGrassClone.Add (borderRight.GetComponent<MeshRenderer> ());


					// For left
					if (Terrain_Elmp_List[i].AbyssInvulnerableList [j].name != BikingKey.Terrain.AbyssInvulnerableHeader) {
						Transform borderLeft = Instantiate (
							PrefabBorderList [0], 
							new Vector3 (Terrain_Elmp_List[i].AbyssInvulnerableList [j].GetComponent<EdgeCollider2D> ().points [0].x + 0.03f,
							Terrain_Elmp_List[i].AbyssInvulnerableList [j].GetComponent<EdgeCollider2D> ().points [0].y //- (PrefabBorderList [0].GetChild (0).GetComponent<MeshRenderer> ().bounds.size.y)
							                      -38f , -.1f),
							Terrain_Elmp_List[i].AbyssInvulnerableList [j].transform.rotation) as Transform;
						
						ModifyBorder (borderLeft, Terrain_Elmp_List[i].AbyssInvulnerableList [j].parent.parent.parent, true);

					//	Terrain_Elmp_List[i].Terrain_Struct.BorderClone.Add (borderLeft.GetChild (0).GetComponent<MeshRenderer> ());
					//	Terrain_Elmp_List[i].Terrain_Struct.BorderGrassClone.Add (borderLeft.GetComponent<MeshRenderer> ());
					}

				}
			}

			// Cliff border
			/*
			if (Routes [i].GetComponent<Terrain_Elmp> ().Cliff.transform.childCount > 0) {
				// For left
				for (int k = 0; k < Routes [i].GetComponent<Terrain_Elmp> ().Cliff.GetComponent<Game12_Cliff> ().CliffLeft.Count; k++) {
		
					Transform borderLeft = Instantiate (PrefabBorderList [0], new Vector3 (Routes [i].GetComponent<Terrain_Elmp> ().Cliff.GetComponent<Game12_Cliff> ().CliffLeft [k]
						.GetComponent<EdgeCollider2D> ().points [0].x,
						                       Routes [i].GetComponent<Terrain_Elmp> ().Cliff.GetComponent<Game12_Cliff> ().CliffLeft [k]
						.GetComponent<EdgeCollider2D> ().points [0].y + BikingKey.Terrain.CliffDeep * 3
						                       - (PrefabBorderList [0].GetChild (0).GetComponent<MeshRenderer> ().bounds.size.y), 1f), Quaternion.identity) as Transform;
					ModifyBorder (borderLeft, Routes [i].GetComponent<Terrain_Elmp> ().Cliff.GetComponent<Game12_Cliff> ().CliffLeft [k].parent.parent.parent, true);

				}
				// For right
				for (int k = 0; k < Routes [i].GetComponent<Terrain_Elmp> ().Cliff.GetComponent<Game12_Cliff> ().CliffRight.Count; k++) {
					Transform borderRight = Instantiate (PrefabBorderList [0], new Vector3 (Routes [i].GetComponent<Terrain_Elmp> ().Cliff.GetComponent<Game12_Cliff> ().CliffRight [k]
						.GetComponent<EdgeCollider2D> ().points [1].x,
						                        Routes [i].GetComponent<Terrain_Elmp> ().Cliff.GetComponent<Game12_Cliff> ().CliffRight [k]
						.GetComponent<EdgeCollider2D> ().points [1].y + 0.2f
						                        - (PrefabBorderList [0].GetChild (0).GetComponent<MeshRenderer> ().bounds.size.y), 1f), Quaternion.identity) as Transform;
					ModifyBorder (borderRight, Routes [i].GetComponent<Terrain_Elmp> ().Cliff.GetComponent<Game12_Cliff> ().CliffRight [k].parent.parent.parent, false);
				}
			}
			*/
		}
	}

	void ModifyBorder (Transform border, Transform parent, bool isLeft)
	{
		if (isLeft)
			border.transform.localScale = new Vector3 (-1, 1, 1);
		border.SetParent (parent.GetChild (0).transform, false);
		border.gameObject.SetActive (true);
		// We add into elmp.
		//parent.GetComponent<Terrain_Elmp> ().PatternBorderList.Add (border);
		// We add to a general variable to control global.

		//-->Hard code translate border
		/*
		if(isLeft){
			border.localPosition += - 0.15f * Vector3.right;
		}else{
			border.localPosition += 0.5f * Vector3.right;
		}
*/
		//--<
		BorderList.Add (border);
	}
		
	/// <summary>
	/// We have 30 pattern, group them every 15 pattern.
	/// </summary>

	int GetIDPatternCenterOfGroup (GameObject parent)
	{
		return parent.transform.GetChild (2).GetComponent<Terrain_Elmp> ().ID;
	}

	/// <summary>
	/// Changes the texture all of pattern
	/// </summary>
	/// <param name="evrGround">Evr ground.</param>
	/// <param name="evrGrass">Evr grass.</param>
	IEnumerator ChangeTexture ()
	{
		Terrain_Elmp_List[currentPattern.ID].Terrain_Struct.m_ground.material.mainTexture = Ground;
		Terrain_Elmp_List[currentPattern.ID].Terrain_Struct.m_grass.material.mainTexture = Grass;
		/*
		if (Terrain_Elmp_List[currentPattern.ID].Terrain_Struct.BorderClone.Count > 0) {
			for (int i = 0; i < Terrain_Elmp_List[currentPattern.ID].Terrain_Struct.BorderClone.Count; i++) {
				Terrain_Elmp_List[currentPattern.ID].Terrain_Struct.BorderClone [i].material.mainTexture = BorderLeftGround;
			}
		}
		if (Terrain_Elmp_List[currentPattern.ID].Terrain_Struct.BorderGrassClone.Count > 0) {
			for (int i = 0; i < Terrain_Elmp_List[currentPattern.ID].Terrain_Struct.BorderGrassClone.Count; i++) {
				Terrain_Elmp_List[currentPattern.ID].Terrain_Struct.BorderGrassClone [i].material.mainTexture = BorderLeftGround;
			}
		}
		*/
		yield return null;
	}

	/// <summary>
	/// Build abyss invulnerable now
	/// </summary>
	/// <param name="isInvulnerable">If set to <c>true</c> is invulnerable.</param>
	public void AbyssOnInvulnerable (bool isInvulnerable)
	{
		//Debug.Log ("AbyssOnInvulnerable " + InvulnerableList.Count);
		//for (int i = 0; i < InvulnerableList.Count; i++) {
		//}
		//InvulnerableList [i].SetActive (isInvulnerable);
	}

	/// <summary>
	/// Terrains the switched random child each other every loop
	/// </summary>
	/// <param name="p">P.</param>
	private void Terrain_Switcher (Transform p)
	{
		List<Vector3> v = new List<Vector3> ();
		foreach (Transform tf in p)
			v.Add (tf.position);
		foreach (Transform tf in p) {
			int _rd = Random.Range (0, v.Count);
			tf.position = v [_rd];
			v.RemoveAt (_rd);
		}
	}

	/// <summary>
	/// Get position center of a group
	/// </summary>
	/// <returns>The of group.</returns>
	/// <param name="p">P.</param>
	public Vector3 CenterOfGroup (Transform p)
	{
		Vector3 com;
//		Vector3 gr = Vector3.zero;
//		int index = 0;
//		foreach (Transform tf in p) {
//			gr += tf.position;
//			index++;
//		}
//		com = gr / index;
		com = p.position + new Vector3 (pattern_width_group / 2, 0f, 0f);
		return com;
	}

	bool isLoopPattern;

	/// <summary>
	/// Terrains the looper.
	/// </summary>
	void Terrain_Looper (GameObject route, List<GameObject> PatternGroup)
	{
		if (Player.position.x > CenterOfGroup (route.transform).x) {
			if (!isLoopPattern) {
				int rd = ChooseNextPattern (PatternGroup);
				PatternGroup [rd].transform.position = new Vector3 (route.transform.position.x + pattern_width_group, route.transform.position.y, 
					route.transform.position.z);
				PatternGroup.RemoveAt (rd);
				isLoopPattern = true;
			}
		}
	}

	/// <summary>
	/// We will chose one of 9 pattern remain.
	/// </summary>
	int ChooseNextPattern (List<GameObject> PatternGroup)
	{
		int rd = Random.Range (0, PatternGroup.Count);
		return rd;
	}

	/// <summary>
	/// We get terrain group size
	/// </summary>
	/// <returns>The size of one pattern.</returns>
	private float Terrain_Size ()
	{
		float w = .0f;
		foreach (Transform tf in RouteGroup_1) {
			Terrain_Elmp te = tf.GetComponent<Terrain_Elmp> ();
			w += te._xWDelta;
		}
		return w;
	}

	/// <summary>
	/// We change game background here.
	/// </summary>
	/// <param name="lvl">Lvl.</param>
	public void EvrLvlChange (Game12_level lvl)
	{
		switch (lvl) {
		case Game12_level.desert:
			BackgroundGroup [0].Show ();
			BackgroundGroup [2].Hide ();
			Game12_GameManager.instance.desert_reach++;
			break;
		case Game12_level.forest:
			BackgroundGroup [1].Show ();
			BackgroundGroup [0].Hide ();
			Game12_GameManager.instance.jungle_reach++;
			break;
		case Game12_level.akatsuka:
			BackgroundGroup [2].Show ();
			BackgroundGroup [1].Hide ();
			Game12_GameManager.instance.akatsuka_reach++;
			break;
		default:
			break;
		}
	}

	/// <summary>
	/// Fade in/out background effect
	/// </summary>
	/// <param name="tf">Tf.</param>
	/// <param name="fadein">If set to <c>true</c> fadein.</param>
	/// <param name="speed">Speed.</param>
	void FadeInOutBackground (Transform tf, bool fadein, float speed)
	{
		GetComponent<Game12_AnimUlti> ().SpriteRenderGroupFade (tf, fadein, speed, BikingKey.GameConfig.BackgroundFadeDelay);
	}

	/// <summary>
	/// We setup new texture pattern& next distances target.
	/// </summary>
	/// <param name="groundIndex">Ground index.</param>
	/// <param name="grassIndex">Grass index.</param>
	void SetPatternTexture (int groundIndex, int grassIndex)
	{
		Ground = Terrain_Generation.instance.groundTextures [groundIndex];
		Grass = Terrain_Generation.instance.groundTextures [grassIndex];
		BorderLeftGround = Terrain_Generation.instance.borderLeftTextures [groundIndex];
		BorderLeftGrass = Terrain_Generation.instance.borderLeftTextures [grassIndex];
		BorderRightGround = Terrain_Generation.instance.borderRightTextures [groundIndex];
		BorderRightGrass = Terrain_Generation.instance.borderRightTextures [grassIndex];
		TempPatternID = PatternID + 4;
	}
}
