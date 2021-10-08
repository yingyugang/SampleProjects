using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Terrain_Generation : MonoBehaviour
{
	public Material Unlit_Transparent;
	public Material Unlit_TransparentCutout;
	public Material Unlit_Texture;
	//public List<Vector2> pointForBorder;
	private int resolution = BikingKey.Terrain.Resolution;
	private int hillNumber = -2;  //if -2 it will draw border
 	private float[] m_heightmapTemp;
	private float[] m_heightmapTempMain;
	private float[] m_terrainAbyssClone;
	public Texture[] groundTextures;
	public Texture[] borderLeftTextures;
	public Texture[] borderRightTextures;
	public Texture[] blurTextures;
	public List<SpriteRenderer> obstacles;
	public Sprite[] ObstacleSprite;
	public Sprite ObstacleSpring_Sprite;
	private static Terrain_Generation m_instance;
	public Terrain_Procedural Procedure;
	public GameObject StandardPattern;
	public static Terrain_Generation instance {
		get {
			if (m_instance == null)
				m_instance = GameObject.FindObjectOfType<Terrain_Generation> ();
			return m_instance;
		}
	}
	void Awake(){
		m_instance = this;
	}
	void Start ()
	{
		for (int i = 0; i < Game12_GameParams.instance.patternList.Count; i++) {
			if (i > 0) {
				CreatePattern (true, Game12_GameParams.instance.patternList [i].points.ToArray (), 
					Game12_GameParams.instance.patternList [i].itemPattern,
					Game12_GameParams.instance.patternList [i].itemPercent,
					groundTextures [0], groundTextures [1]);
			} else {
				Debug.Log ("Create StandardPattern");
				CreatePattern (false, Game12_GameParams.instance.patternList [i].points.ToArray (), 
					Game12_GameParams.instance.patternList [i].itemPattern,
					Game12_GameParams.instance.patternList [i].itemPercent,
					borderLeftTextures [0], borderLeftTextures [1]);
			}
		}

		Terrain_Controller.instance.countParttern = Terrain_Controller.instance.Routes.Count;
//		Debug.Log ("partten: " + Terrain_Controller.instance.countParttern);
		DuplicatePattern ();
		Terrain_Controller.instance.HideAllPattern ();
//		Terrain_Controller.instance.CreateMapWithRandomPattern ();
	}

	IEnumerator watingDulicate ()
	{
		yield return new WaitForEndOfFrame ();

	}

	public void DuplicatePattern ()
	{
		int count = Terrain_Controller.instance.Routes.Count;
		for (int i = 0; i < count; i++) {
			GameObject pattern = Instantiate (Terrain_Controller.instance.Routes [i], Vector3.zero, Quaternion.identity) as GameObject;
			//-->add ground, grass
			Terrain_Elmp terrainelmp = pattern.GetComponent<Terrain_Elmp> ();
			if (terrainelmp != null && terrainelmp.Terrain_Struct.m_grass == null) {
				terrainelmp.Terrain_Struct.m_grass = terrainelmp.transform.GetChild (0).FindChild ("grass").GetComponent<MeshRenderer> ();
				terrainelmp.Terrain_Struct.m_ground = terrainelmp.transform.GetChild (0).FindChild ("ground").GetComponent<MeshRenderer> ();
			}
			/*
			if (terrainelmp.Terrain_Struct.BorderGrassClone.Count == 0) {
				for (int j = 0; j < pattern.transform.GetChild (0).childCount; j++) {
					if (pattern.transform.GetChild (0).GetChild (j).transform.name.Contains (BikingKey.Terrain.BorderRight)) {
						terrainelmp.Terrain_Struct.BorderGrassClone.Add (pattern.transform.GetChild (0).GetChild (j).GetComponent<MeshRenderer> ());
						terrainelmp.Terrain_Struct.BorderClone.Add (pattern.transform.GetChild (0).GetChild (j).GetChild (0).GetComponent<MeshRenderer> ());

					}

				}
			}
			*/
			//hard code fix border

			//--<
			Terrain_Controller.instance.Routes.Add (pattern);
		}

	}

	/// <summary>
	/// We modify border pattern, just take what we need: ground + grass.
	/// We set grass is parent of ground.
	/// We change shader + sorting layer to show front of background.
	/// </summary>
	/// <param name="border">Border.</param>
	void BorderModify (GameObject border)
	{
		Debug.Log ("BorderModify");
		foreach (Transform child in border.transform.GetChild(0)) {
			if (child.name == BikingKey.Terrain.Border_Ground || child.name == BikingKey.Terrain.Border_Grass) {
			} else
				GameObject.Destroy (child.gameObject);
		}
		Transform[] borderRight = border.transform.GetChild (0).GetComponentsInChildren<Transform> ();
		//borderRight [1].gameObject.AddComponent<BoxCollider2D> ();
		//borderRight [1].gameObject.GetComponent<BoxCollider2D> ().offset = new Vector2 (0.4f, 12.5f);
		//borderRight [1].gameObject.GetComponent<BoxCollider2D> ().size = new Vector2 (0.4f, 25f);
		borderRight [1].SetParent (borderRight [2], false);
		borderRight [2].gameObject.name = BikingKey.Terrain.BorderRight;
		borderRight [2].GetComponent<Renderer> ().material = Unlit_Transparent;
		borderRight [2].GetComponent<Renderer> ().material.mainTexture = borderLeftTextures [1];
		borderRight [2].GetComponent<Renderer> ().sortingOrder = 5;
		borderRight [2].GetChild (0).GetComponent<Renderer> ().material = Unlit_Transparent;
		borderRight [2].GetChild (0).GetComponent<Renderer> ().material.mainTexture = borderLeftTextures [0];
		borderRight [2].GetChild (0).GetComponent<Renderer> ().sortingOrder = 5;
		borderRight [2].transform.parent = null;
		if (Terrain_Controller.instance.PrefabBorderList.Count == 0)
			Terrain_Controller.instance.PrefabBorderList.Add (borderRight [2]);

		Destroy (border);
	}

	/// <summary>
	/// We create pattern here.
	/// </summary>
	/// <param name="isMainTerrain">If set to <c>true</c> is main terrain.</param>
	/// <param name="terrain">Terrain.</param>
	/// <param name="itemPattern">Item pattern.</param>
	/// <param name="evrGround">Evr ground.</param>
	/// <param name="evrGrass">Evr grass.</param>
	/// <param name="leftGrass">Left grass.</param>
	GameObject CreatePattern (bool isMainTerrain, Vector2[] terrain, List<KeyValuePair<int, Vector2>> itemPattern,
		int[] itemPercent, Texture evrGround, Texture evrGrass)
	{
		hillNumber++;
		if (hillNumber > -1) {
			TerrainBuilder (isMainTerrain, terrain, itemPattern, itemPercent, evrGround, evrGrass, BikingKey.Terrain.Width, false, 50);
		} else {
			TerrainBuilder (isMainTerrain, terrain, itemPattern, itemPercent, evrGround, evrGrass, BikingKey.Terrain.Width, true, 50);
		}
		GameObject TheTerrain = GameObject.Find ("Terrain");
		GameObject TheHills = new GameObject ("Hills" + hillNumber.ToString ());
		TheTerrain.transform.SetParent (TheHills.transform, false);
		TheTerrain.name = "Terrain" + hillNumber.ToString ();
		TheHills.AddComponent<Terrain_Elmp> ();
		TheHills.GetComponent<Terrain_Elmp> ().ID = hillNumber;
		TerrainAttribute_AddPermanently (TheTerrain, TheHills.GetComponent<Terrain_Elmp> ());
		TerrainSetLayer(TheHills);
		if (hillNumber == -1) {
			TheHills.gameObject.name = BikingKey.Terrain.Border;
			//BorderModify (TheHills.gameObject);
			TheHills.gameObject.SetActive(false);
		} else if (hillNumber == 0) {
			TheHills.gameObject.name = BikingKey.Terrain.Standard;
			StandardPattern = TheHills;
		}
		else {
		//	BorderModify (TheHills.gameObject);
			Terrain_Controller.instance.Routes.Add (TheHills);
		} 
		TheHills.transform.position = BikingKey.Terrain.MeshStartPosition;
		return TheHills;
	}
		
	void TerrainSetLayer(GameObject terrain){
		Transform[] tf = terrain.GetComponentsInChildren<Transform>();
		foreach(Transform child in tf){
			if(child.name == BikingKey.Terrain.Terrain_Collision){
				child.gameObject.layer = LayerMask.NameToLayer("TransparentFX");
			}

		}
	}

	/// <summary>
	/// We build pattern at mesh.
	/// </summary>
	/// <param name="isMainTerrain">If set to <c>true</c> is main terrain.</param>
	/// <param name="terrain">Terrain.</param>
	/// <param name="itemPattern">Item pattern.</param>
	/// <param name="evrGround">Evr ground.</param>
	/// <param name="evrGrass">Evr grass.</param>
	/// <param name="mat">Mat.</param>
	/// <param name="terrain_width_default">Terrain width default.</param>
	void TerrainBuilder (bool isMainTerrain, Vector2[] terrain, List<KeyValuePair<int, Vector2>> itemPattern, int [] itemPercent,
		Texture evrGround, Texture evrGrass, int terrain_width_default, bool isBorder, int vecticesCount)
	{
		// Generate the heightmap
		float[] heightmap = GenerateHeightMap (terrain, itemPattern, itemPercent, terrain_width_default, isMainTerrain);
		// Create vertices, uv's and triangles
		Vector2[] terrainVertices = Procedure.CreateTerrainVertices (heightmap, resolution, vecticesCount);
		Vector2[] terrainUV = Procedure.GenerateTerrainUV (heightmap, vecticesCount);
		int[] terrainTriangles = Procedure.Triangulate (terrainVertices.Length);
		// Create parent
		GameObject parent = new GameObject ("Terrain");
		// Create the mesh
		Procedure.GenerateMesh (terrainVertices, terrainTriangles, terrainUV, evrGround, 0, parent, Unlit_Texture, isBorder);
		// Repeat the process for grass
		Vector2[] grassVertices = Procedure.CreateGrassVertices (heightmap, resolution, vecticesCount);
		Vector2[] grassUV = Procedure.GenerateGrassUV (heightmap, terrain_width_default, vecticesCount);
		int[] grassTriangles = Procedure.Triangulate (terrainVertices.Length);
		Procedure.GenerateMesh (grassVertices, grassTriangles, grassUV, evrGrass, -1, parent, Unlit_Transparent, isBorder);
		// Add edge coliider2D for terrain
		PatternCollider (TerrainSpecialHeightMap (terrain), parent);
		//if (hillNumber > 0)
		//	CheckCliff (terrain, parent);
		// Set parent into parent each hill
		for (int i = 0; i < obstacles.Count; i++) {
			obstacles [i].transform.SetParent (parent.transform, false);
		}
		obstacles.Clear ();
	}
		
	/// We release item here with angle.

	private void ItemRelativeCordinate (int itemID, int percent, Vector3 itemStartPosition, Vector3 itemEndPosition)
	{
		Vector3 m_direction = itemEndPosition - itemStartPosition;
		if (itemID == 3 || itemID == 8 || itemID == 9 || itemID == 11)
			m_direction = Vector3.right;
		if (m_direction.magnitude <= 0.001f)
			m_direction = Vector3.right;
			//Debug.Log ("??");
		float m_angle = 0;
		m_angle = Vector3.Angle (m_direction, Vector3.right);
		if (m_direction.y < 0)
			m_angle = -m_angle;
			//Debug.Log (m_angle);
		if(itemID != 0){
			GameObject obstacle = new GameObject ("Obstacle");
			Game12_Item_Manager.instance.ObstacleCreate (obstacle, itemID, percent, itemStartPosition, m_angle);
		}
	}
		
	/// Generates the height map for terrain builder.
	public float[] GenerateHeightMap (Vector2[] terrain, List<KeyValuePair<int, Vector2>> itemPattern, int [] itemPercent, int terrainWidth, bool isMainTerrain)
	{
		float distance;
		int itemCount = 0;
		float[] heightmap = new float[terrainWidth + 1];
		for (int j = 0; j < terrain.Length; j += 2) {
			heightmap [(int)terrain [j].x] = terrain [j].y;
			if (j >= 2) {
				distance = (terrain [j].y - terrain [j - 2].y) / (int)(terrain [j].x - terrain [j - 2].x);
				HeightMapValue (terrain [j - 2], terrain [j], heightmap, distance);
			}
			// Add item on pattern
//			if (itemPattern [itemCount].Key == 4) {
//				if (terrain [j].y == 0) {
//					ItemRelativeCordinate (itemPattern [itemCount].Key, VRCI (itemPattern [itemCount].Value * BikingKey.Terrain.cordinateMulti), VRCI (terrain [j - 1]));
//				} else {
//					ItemRelativeCordinate (itemPattern [itemCount].Key, VRCI (itemPattern [itemCount].Value * BikingKey.Terrain.cordinateMulti), VRCI (terrain [j - 2]));
//				}
//			} else {
			if (j == terrain.Length - 2) {
				ItemRelativeCordinate (itemPattern [itemCount].Key, itemPercent[itemCount],  VRCI (itemPattern [itemCount].Value * BikingKey.Terrain.CordinateMulti), VRCI (terrain [j + 1]));
			} else {
				ItemRelativeCordinate (itemPattern [itemCount].Key, itemPercent[itemCount], VRCI (itemPattern [itemCount].Value * BikingKey.Terrain.CordinateMulti), VRCI (terrain [j + 2]));
			}
//			}
			itemCount++;
		}
		heightmap [heightmap.Length - 1] = terrain [terrain.Length - 1].y;
		distance = (terrain [terrain.Length - 1].y - terrain [terrain.Length - 2].y) / (terrain [terrain.Length - 1].x - terrain [terrain.Length - 2].x);
		HeightMapValue (terrain [terrain.Length - 2], terrain [terrain.Length - 1], heightmap, distance);
		for (int j = 0; j < terrain.Length; j += 2) {
			if (terrain [j].y != 0)
				heightmap [(int)terrain [j].x] = terrain [j].y;
		}
		// We check this heightmap, if it's a border, we just collect mesh 1->60
		if (!isMainTerrain) {
			for (int i = 30; i < heightmap.Length; i++) {
				heightmap [i] = 0;
			}
			heightmap [0] = 0;
		}

		//-->Fill border
		for(int i = 1; i < heightmap.Length; i++){
			if(heightmap[i] == 0 && heightmap[i-1] != 0){
				heightmap[i] = heightmap[i-1];
			}
			i++;
		}
		//--<
		return HMRC (heightmap);
	}

	/// <summary>
	/// The distance bettwen height map.
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	/// <param name="heightmap">Heightmap.</param>
	/// <param name="distances">Distances.</param>
	void HeightMapValue (Vector2 start, Vector2 end, float[] heightmap, float distances)
	{
		for (int i = (int)start.x + 1; i <= (int)end.x; i++)
			heightmap [i] = heightmap [i - 1] + distances;
	}

	/// <summary>
	/// We get BikingKey.Terrain.CSVMeshPoint(31) point to create the mesh, no need same point.
	/// </summary>
	/// <returns>The special height map.</returns>
	/// <param name="terrain">Terrain.</param>
	Vector2[] TerrainSpecialHeightMap (Vector2[] terrain)
	{
		int count = 2;
		Vector2[] vectorSpilit = new Vector2[terrain.Length / 2 + 1];
		for (int i = 0; i < terrain.Length; i++) {
			if (i > 1) {
				if (i % 2 != 0 && count < BikingKey.Terrain.CSVMeshPoint + 1) {
					vectorSpilit [count] = terrain [i];
					count++;
				}
			} else
				vectorSpilit [i] = terrain [i];
		}
		return vectorSpilit;
	}

	/// <summary>
	/// We need convert your parameter to true heightmap to use.
	/// </summary>
	/// <param name="pattern">Pattern.</param>
	float[] HMRC (float[] heightmap)
	{
		float[] heightmapClone = (float[])heightmap.Clone ();
		for (int i = 0; i < heightmapClone.Length; i++) {
			heightmapClone [i] /= BikingKey.Terrain.Resolution;

		}
		return heightmapClone;
	}

	/// <summary>
	/// We need convert your parameter to true vector in Unity.
	/// </summary>
	/// <param name="pattern">Pattern.</param>
	Vector2[] VRC (Vector2[] pattern)
	{
		Vector2[] patternClone = (Vector2[])pattern.Clone ();
		//Debug.Log (patternClone.Length);
		for (int i = 0; i < patternClone.Length; i++) {
			patternClone [i].x /= BikingKey.Terrain.Resolution;
			patternClone [i].y /= BikingKey.Terrain.Resolution;
		}
		return patternClone;
	}

	/// <summary>
	/// And here for one point when we need to use
	/// </summary>
	/// <param name="item">Item.</param>
	Vector2 VRCI (Vector2 item)
	{
		item.x /= BikingKey.Terrain.Resolution;
		item.y /= BikingKey.Terrain.Resolution;
		return item;
	}

	/*
	void CheckCliff (Vector2[] pattern, GameObject parent)
	{
		GameObject Cliffs = new GameObject (BikingKey.Terrain.Cliff);
		Cliffs.AddComponent<Game12_Cliff> ();
		Cliffs.transform.SetParent (parent.transform, false);
		for (int i = 0; i < m_VRC.Length; i += 2) {
			if (i == m_VRC.Length - 1)
				break;
			if (m_VRC [i].x == m_VRC [i + 1].x && (m_VRC [i].y != 0 && m_VRC [i + 1].y != 0)) {
				GameObject cliff = new GameObject ("cliff");
				cliff.transform.SetParent (Cliffs.transform, false);
				cliff.AddComponent<EdgeCollider2D> ();
				List<Vector2> point = new List<Vector2> ();
				Vector2 middle;
				// Check right
				if (m_VRC [i].y > m_VRC [i + 1].y) {
					point.Add (new Vector2 (m_VRC [i].x + .70f, m_VRC [i].y - BikingKey.Terrain.CliffDeep * 3));
					point.Add (new Vector2 (m_VRC [i + 1].x + .70f, m_VRC [i + 1].y));
					middle = new Vector2 (m_VRC [i].x, m_VRC [i].y);
				}
				// Check left
				else {
					point.Add (new Vector2 (m_VRC [i].x - 0.70f, m_VRC [i].y));
					point.Add (new Vector2 (m_VRC [i + 1].x - 0.70f, m_VRC [i + 1].y - 0.2f));
					middle = new Vector2 (m_VRC [i].x - .070f, m_VRC [i].y);
				}
				point.Add (middle);
				cliff.GetComponent<EdgeCollider2D> ().points = point.ToArray ();
				cliff.GetComponent<EdgeCollider2D> ().isTrigger = true;
				cliff.tag = BikingKey.Tags.obstacle;
				cliff.AddComponent<Game12_Item_Component> ();
				// Set temp for variable
				cliff.GetComponent<Game12_Item_Component> ().itemProperties.SetName (BikingKey.Terrain.Cliff);
				cliff.GetComponent<Game12_Item_Component> ().itemProperties.SetImageSource (BikingKey.Terrain.Cliff);
				cliff.GetComponent<Game12_Item_Component> ().itemProperties.SetDescription (BikingKey.Terrain.Cliff);
				if (m_VRC [i].y > m_VRC [i + 1].y) {
					Cliffs.GetComponent<Game12_Cliff> ().CliffLeft.Add (cliff.transform);
				} else {
					Cliffs.GetComponent<Game12_Cliff> ().CliffRight.Add (cliff.transform);
				}
			}
		}
	}
	*/


	void PatternCollider (Vector2[] pattern, GameObject parent)
	{
		GameObject AbyssTrigger = new GameObject (BikingKey.Terrain.AbyssInvulnerable);
		Game12_Abyss Aby = AbyssTrigger.AddComponent<Game12_Abyss> ();
		AbyssTrigger.AddComponent<EdgeCollider2D> ();
		AbyssTrigger.transform.position = Vector3.zero;
		AbyssTrigger.transform.SetParent (parent.transform, false);
		GameObject CollisionObj = new GameObject (BikingKey.Terrain.Terrain_Collision);
		List<Vector2> Collision = new List<Vector2> ();
		CollisionObj.AddComponent<EdgeCollider2D> ();
		CollisionObj.AddComponent<Terrain_Collision> ();
		CollisionObj.transform.position = Vector3.zero;
		CollisionObj.transform.SetParent (parent.transform, false);
		// Start pos
		Collision.Add (new Vector2 (.0f, .0f));
		// Detect special pattern
		Vector2 [] m_VRC = VRC (pattern);

		if (m_VRC [0].x == 0 && m_VRC [0].y == 0) {
			for (int i = 2; i < m_VRC.Length; i++)
				Collision.Add (m_VRC [i]);
			// Get trigger
			Aby.TriggerPoints.Add (new Vector2 (m_VRC [0].x, m_VRC [3].y));
			Aby.TriggerPoints.Add (new Vector2 (m_VRC [0].x, m_VRC [1].y + BikingKey.Terrain.AbyssDeep * 2));
			Aby.TriggerPoints.Add (new Vector2 (m_VRC [2].x, m_VRC [2].y + BikingKey.Terrain.AbyssDeep * 2));
			Aby.TriggerPoints.Add (new Vector2 (m_VRC [3].x, m_VRC [3].y));
		}
		// Main pos
		else {
			for (int i = 0; i < m_VRC.Length; i++) {
				if (m_VRC [i].y == 0) {
					// Left abyss
					if (m_VRC [i - 1].y != 0) {
						Collision.Add (new Vector2 (m_VRC [i].x, -1f));
						Aby.TriggerPoints.Add (new Vector2 (m_VRC [i - 1].x, m_VRC [i - 1].y));
						Aby.TriggerPoints.Add (new Vector2 (m_VRC [i].x, m_VRC [i - 1].y - BikingKey.Terrain.AbyssDeep * 2));
					}
					// Right abyss
					else {
						if (i < m_VRC.Length - 1) {
							Collision.Add (new Vector2 (m_VRC [i].x,  -1f));
							Aby.TriggerPoints.Add (new Vector2 (m_VRC [i].x, m_VRC [i + 1].y - BikingKey.Terrain.AbyssDeep * 2));
							Aby.TriggerPoints.Add (new Vector2 (m_VRC [i + 1].x, m_VRC [i + 1].y));
						}

					}
				} else {
					Collision.Add (m_VRC [i]);
				}
			}
		}
		// End pos
		Collision.Add (new Vector2 (m_VRC [m_VRC.Length - 1].x, .0f));
		// Finish pos
		if (m_VRC [0].x == 0 && m_VRC [0].y == 0)
			Collision.Add (new Vector2 (pattern [2].x, pattern [2].y));
		else
			Collision.Add (new Vector2 (.0f, .0f));
		CollisionObj.GetComponent<EdgeCollider2D> ().points = Collision.ToArray ();
		CollisionObj.GetComponent<EdgeCollider2D> ().isTrigger = false;
		if (Aby.TriggerPoints.Count > 2) {
			List<Vector2> abysInvulnerable = new List<Vector2> ();
			for (int i = 0; i < Aby.TriggerPoints.Count; i++) {
				abysInvulnerable.Add (Aby.TriggerPoints [i]);
				if (i % 4 == 3) {
					abysInvulnerable.Add (Aby.TriggerPoints [i - 3]);
					//-->| by anhgh -> that's buggy, need to be fixed
					AbyssBox (abysInvulnerable, AbyssTrigger);  
					//--<| by anhgh
					abysInvulnerable.Clear ();
				}
			}
		}
		if (Aby.TriggerPoints.Count == 0) {
			GameObject heightPoint = new GameObject (BikingKey.Terrain.HeightLifePoint);
			heightPoint.name = "None";
			heightPoint.transform.SetParent (AbyssTrigger.transform.parent.transform, false);
		}
	}

	///

	GameObject AbyssBox (List<Vector2> points, GameObject parent)
	{
		// Abyss invulnerable
		GameObject abyssInvulnerableBox;
		if (points [0].x == 0 && points [1].x == 0)
			abyssInvulnerableBox = new GameObject (BikingKey.Terrain.AbyssInvulnerableHeader);
		else
			abyssInvulnerableBox = new GameObject (BikingKey.Terrain.AbyssInvulnerable);
		abyssInvulnerableBox.AddComponent<EdgeCollider2D> ();
		// We collect point to put border
		// We add border here
		//pointForBorder.Add (abyssInvulnerableBox.GetComponent<EdgeCollider2D> ().points [0]);
		abyssInvulnerableBox.GetComponent<EdgeCollider2D> ().points = points.ToArray ();
		abyssInvulnerableBox.GetComponent<EdgeCollider2D> ().isTrigger = false;
		abyssInvulnerableBox.transform.position = Vector3.zero;
		abyssInvulnerableBox.transform.SetParent (parent.transform, false);
		//Terrain_Controller.instance.InvulnerableList.Add (abyssInvulnerableBox);
		// Create trigger for obstacle 
		GameObject triggerBox = Instantiate (
			abyssInvulnerableBox, 
			new Vector3 (abyssInvulnerableBox.transform.position.x, abyssInvulnerableBox.transform.position.y, abyssInvulnerableBox.transform.position.z), 
			abyssInvulnerableBox.transform.rotation) as GameObject;
		triggerBox.transform.SetParent (parent.transform.parent.transform, false);
		triggerBox.name = BikingKey.Terrain.Terrain_Abyss;
		//================

		GameObject heightPoint = new GameObject (BikingKey.Terrain.HeightLifePoint);
		heightPoint.transform.position = new Vector3 (
			abyssInvulnerableBox.GetComponent<EdgeCollider2D> ().points [0].x,
			abyssInvulnerableBox.GetComponent<EdgeCollider2D> ().points [0].y + BikingKey.Terrain.HeightLifePointValue, 
			abyssInvulnerableBox.transform.position.z);
		heightPoint.transform.SetParent (parent.transform.parent.transform, false);

		//================
		List<Vector2> triggerPoints = new List<Vector2> ();
		triggerPoints.Add (new Vector2 (triggerBox.GetComponent<EdgeCollider2D> ().points [0].x,
			triggerBox.GetComponent<EdgeCollider2D> ().points [0].y - BikingKey.Terrain.AbyssDeep));
		triggerPoints.Add (triggerBox.GetComponent<EdgeCollider2D> ().points [1]);
		triggerPoints.Add (triggerBox.GetComponent<EdgeCollider2D> ().points [2]);
		triggerPoints.Add (new Vector2 (
				triggerBox.GetComponent<EdgeCollider2D> ().points [3].x,
				triggerBox.GetComponent<EdgeCollider2D> ().points [3].y - 0.70f));
		triggerBox.GetComponent<EdgeCollider2D> ().points = triggerPoints.ToArray ();
		triggerBox.GetComponent<EdgeCollider2D> ().isTrigger = true;
		//triggerBox.SetActive (true);
		// Set temp for variable
		Game12_Item_Component aby = triggerBox.AddComponent<Game12_Item_Component> ();
		aby.itemProperties.SetName (BikingKey.Terrain.Terrain_Abyss);
		aby.itemProperties.SetImageSource (BikingKey.Terrain.Terrain_Abyss);
		aby.itemProperties.SetDescription (BikingKey.Terrain.Terrain_Abyss);
		triggerBox.tag = BikingKey.Tags.obstacle;
		// Deactive invulnerable box, just use when need
		abyssInvulnerableBox.SetActive (false);
		return abyssInvulnerableBox;
	}


	/// <summary>
	/// We add and remove unnecessary attribute for all mesh.
	/// </summary>
	/// <param name="m">M.</param>
	/// <param name="terrain_elmp">Terrain elmp.</param>
	void TerrainAttribute_AddPermanently (GameObject m, Terrain_Elmp terrain_elmp)
	{
		Transform[] transforms = m.GetComponentsInChildren<Transform> ();
		MeshRenderer hillMeshGround = transforms [1].GetComponent<MeshRenderer> ();
		MeshRenderer hillMeshGrass = transforms [2].GetComponent<MeshRenderer> ();
		hillMeshGround.sortingOrder = 5;
		hillMeshGrass.sortingOrder = 5;
		hillMeshGround.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		hillMeshGround.receiveShadows = false;
		hillMeshGround.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
		hillMeshGround.useLightProbes = false;
		hillMeshGrass.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		hillMeshGrass.receiveShadows = false;
		hillMeshGrass.useLightProbes = false;
		hillMeshGrass.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
		terrain_elmp.Terrain_Struct.m_ground = hillMeshGround;
		terrain_elmp.Terrain_Struct.m_grass = hillMeshGrass;

		//edit by HoanTruong
//		terrain_elmp.Terrain_Struct.BorderClone = new List<MeshRenderer> ();
//		if (GameObject.Find (BikingKey.Terrain.BorderRight) != null) {
//			terrain_elmp.Terrain_Struct.BorderClone.Add (GameObject.Find (BikingKey.Terrain.BorderRight).transform.GetChild (0).GetComponent<MeshRenderer> ());
//		}
//		terrain_elmp.Terrain_Struct.BorderGrassClone = new List<MeshRenderer> ();
//		if (GameObject.Find (BikingKey.Terrain.BorderLeft) != null) {
//			terrain_elmp.Terrain_Struct.BorderGrassClone.Add (GameObject.Find (BikingKey.Terrain.BorderLeft).transform.GetChild (0).GetComponent<MeshRenderer> ());
//		} else {
//			Debug.Log ("null terrain border");
//			Debug.Break ();
//		}

		terrain_elmp._xWDelta = hillMeshGround.bounds.size.x - BikingKey.Terrain.TerrainPixel_Standard;
		for (int i = 0; i < transforms.Length; i++) {
			// Collect emptyslot
			if (transforms [i].name == BikingKey.Terrain.EmptySlot)
				terrain_elmp.EmptySlotList.Add (transforms [i]);
			// Collect abysss 
			if (transforms [i].name == BikingKey.Terrain.Terrain_Abyss) {
				transforms [i].tag = BikingKey.Tags.obstacle;
				terrain_elmp.AbyssList.Add (transforms [i].GetComponent<EdgeCollider2D> ());
			//	transforms [i].gameObject.SetActive (true);
			}
			// Collect abyss invulnerable
			if (transforms [i].name == BikingKey.Terrain.AbyssInvulnerable) {
				foreach (Transform child in transforms[i]) {
					terrain_elmp.AbyssInvulnerableList.Add (child);
				}
			}
			// Collect item
			if (transforms [i].CompareTag (BikingKey.Tags.item)) {
				terrain_elmp.ItemList.Add (transforms [i]);
			}
			// Collect collision
			if (transforms [i].name == BikingKey.Terrain.Terrain_Collision) {
				terrain_elmp.Terrain_Collision = transforms [i].gameObject;
			}
			// Collect Cliff
			if (transforms [i].GetComponent<Game12_Cliff> ()) {
				terrain_elmp.Cliff = transforms [i].gameObject;
			}
			// Collect obstacle
			if (transforms [i].CompareTag (BikingKey.Tags.obstacle) && (transforms [i].name != BikingKey.Terrain.Terrain_Abyss)) {
				terrain_elmp.ObstacleList.Add (transforms [i]);
			}
			// Add height point
			if (transforms [i].name == BikingKey.Terrain.HeightLifePoint ||
			    transforms [i].name == "None")
				terrain_elmp.HeightPoint = transforms [i];
		}

	}
}
