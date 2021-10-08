using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Terrain_Procedural : MonoBehaviour {
	private static Terrain_Procedural m_instance;
	public static Terrain_Procedural instance{
		get{
			if(m_instance == null) m_instance = GameObject.FindObjectOfType<Terrain_Procedural>();
			return m_instance;
		}
	}
	void Awake(){

	}
	//--> Terrain mesh generation
	public MeshRenderer GenerateMesh(Vector2[] vertices, int[] triangles, Vector2[] uv, Texture texture, float zOffset, GameObject parent, Material mat, bool isBorder) {
		List<Vector3> meshVertices = new List<Vector3>();
		// Convert our Vector2's to Vector3
		foreach (Vector2 vertex in vertices) {
			meshVertices.Add(new Vector3(vertex.x, vertex.y, transform.position.z + zOffset));
		}
		// Create a new mesh and set the vertices, uv's and triangles
		Mesh mesh = new Mesh();
		mesh.vertices = meshVertices.ToArray();
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		GameObject meshObj = new GameObject(texture.name);
		// Add the mesh to the object
		meshObj.AddComponent<MeshRenderer>();
		MeshFilter filter = meshObj.AddComponent<MeshFilter>();
		filter.mesh = mesh;
		// Add a texture  
		//meshObj.AddComponent<Terrain_AbyssTrigger>();
		meshObj.GetComponent<Renderer>().material = mat;
		meshObj.GetComponent<Renderer>().material.mainTexture = texture;
		// Reparent as a child of this game object
		//--> Duplicate terrain border for blur effect, just change texture
		//--< Duplicate terrain border for blur effect, just change texture
		meshObj.transform.SetParent(parent.transform, false);
		return meshObj.GetComponent<MeshRenderer>();
	}
	//--< Terrain mesh generation
	private float ESP = 0.001f;
	private bool CompareValue(float v1, float v2, float v3){
		return (Mathf.Abs ((v1 - v2) - (v2 - v3)) > ESP);
	}
	//--> Create vertices for ground
	public Vector2[] CreateTerrainVertices (float[] heightmap, float resolution, int vecticesCount){
		// The minimum resolution is 1
		//Debug.Log("CreateTerrainVertices");
		//Debug.Log(heightmap.Length);
		List<Vector2> vertices = new List<Vector2>();
		// For each point, in the heightmap, create a vertex for the top and the bottom of the terrain.
		for (int i = 0; i < heightmap.Length; i ++) {
		//	Debug.Log (i + " -> " + heightmap [i]);
			vertices.Add (new Vector2 (i / resolution, heightmap [i]));
			vertices.Add (new Vector2 (i / resolution, 0));
			int j;
			for (j = i + 1; j < heightmap.Length; j++) {
				if (j == heightmap.Length-1 || CompareValue(heightmap [j+1], heightmap [j], heightmap [j-1])) {
					i = j-1;
//					Debug.Log (i + " <- " + heightmap [i]);
					break;
				}
			}
		}
		vertices.Add(new Vector2((BikingKey.Terrain.Width ) /resolution, heightmap[heightmap.Length-1]));
		vertices.Add(new Vector2((BikingKey.Terrain.Width ) /resolution, 0));
		return vertices.ToArray();
	}
	//--< Create vertices for ground

	//--> Generate ground'UV
	public Vector2[] GenerateTerrainUV(float[] heightmap, int vecticesCount) {
		List<Vector2> uv = new List<Vector2>();
		float factor = 1f / heightmap.Length;
		float blockW = BikingKey.Terrain.Factor / BikingKey.Terrain.Resolution * 2f;
		float block = BikingKey.Terrain.Width / 50f * factor;
		for (int i =0; i < heightmap.Length; i ++) {
			uv.Add (new Vector2 (i * block, heightmap [i] / blockW));
			uv.Add (new Vector2 (i * block, 0));
			int j;
			for (j = i + 1; j < heightmap.Length; j++) {
				if (j == heightmap.Length-1 || CompareValue(heightmap [j+1], heightmap [j], heightmap [j-1])) {
					i = j-1;
					break;
				}
			}
		}
		uv.Add(new Vector2((BikingKey.Terrain.Width ) * block, heightmap [heightmap.Length-1] / blockW));
		uv.Add(new Vector2((BikingKey.Terrain.Width ) * block, 0));
		return uv.ToArray();
	}
	//--< Generate ground'UV

	//--> Create vertices for grass
	public Vector2[] CreateGrassVertices(float[] heightmap, float resolution, int vecticesCount ) {
		resolution = Mathf.Max(1, resolution);
		List<Vector2> vertices = new List<Vector2>();
		for (int i = 0; i < heightmap.Length; i ++) {
				vertices.Add (new Vector2 (i / resolution, heightmap [i]));
				vertices.Add (new Vector2 (i / resolution, heightmap [i] - 1f));
			int j;
			for (j = i + 1; j < heightmap.Length; j++) {
				if (j == heightmap.Length-1 || CompareValue(heightmap [j+1], heightmap [j], heightmap [j-1])) {
					i = j-1;
					break;
				}
			}
		}
	//	vertices.Add(new Vector2((BikingKey.Terrain.Width + 1) /resolution, heightmap[heightmap.Length-1]));
	//	vertices.Add(new Vector2((BikingKey.Terrain.Width + 1) /resolution, heightmap[heightmap.Length-1]-1));
		return vertices.ToArray();
	}
	//--< Create vertices for grass

	//--> Generate grass'UV
	public Vector2[] GenerateGrassUV(float[] heightmap, float terrainWidth, int vecticesCount) {
		List<Vector2> uv = new List<Vector2>();
		float factor = 1f / heightmap.Length;
//		float blockW = (float)(BikingKey.Terrain.Factor/(BikingKey.Terrain.Resolution/2));
		float block = terrainWidth/50f * factor;
		for (int i = 0; i < heightmap.Length; i ++) {
			uv.Add(new Vector2(i * block, 1f));
			uv.Add(new Vector2(i * block, 0.1f));
			int j;
			for (j = i + 1; j < heightmap.Length; j++) {
				if (j == heightmap.Length-1 || CompareValue(heightmap [j+1], heightmap [j], heightmap [j-1])) {
					i = j - 1;
					break;
				}
			}
		}
	//	uv.Add(new Vector2((BikingKey.Terrain.Width + 1) * block, 1f));
	//	uv.Add(new Vector2((BikingKey.Terrain.Width + 1) * block, 0.1f));
		return uv.ToArray();
	}
	//--< Generate grass'UV

	//--> Triangle
	public int[] Triangulate(int count){
		List<int> indices = new List<int>();
		for (int i = 0; i < count - 4; i += 2) {
			indices.Add(i);      // 0
			indices.Add(i + 3);  // 3
			indices.Add(i + 1);  // 1
			indices.Add(i + 3);  // 3
			indices.Add(i);      // 0
			indices.Add(i + 2);  // 2
		}
		return indices.ToArray();
	}
	//--< Triangle
	List<int> TrisDuplicateRemoving(List<int> tris){
		for(int i = 3; i < tris.Count - 4; i += 3){
			if(tris[i] == tris[i - 3] && tris[i+1] == tris[i - 2] && tris[i+ 2] == tris[i - 1]){
				tris.Remove(tris[i]);
				tris.Remove(tris[i+1]);
				tris.Remove(tris[i+2]);
			}
		}
		return tris;
	}

}
