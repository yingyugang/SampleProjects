using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class InitGround : MonoBehaviour {

	public bool draw;
	public Material mat;
	public Vector3 p0;
	public Vector3 p1;
	public Vector3 p2;
	public Vector3 p3;


	GameObject go;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(draw){
			draw = false;
			DrawGround ();
		}
	}


	void DrawGround(){
		if (go != null)
			DestroyImmediate (go);
		GameObject go0 =  MeshUtility.GetMeshGameObject (mat,Color.white);
		this.go = go0;
		Vector3[] vs = new Vector3[]{p0,p1,p2,p3};
		Mesh mesh = new Mesh ();
		int[] triangles = new int[]{0,1,2,2,3,0};
		Vector2[] uvs = new Vector2[]{new Vector2(0,0),new Vector2(0,1),new Vector2(1,1),new Vector2(1,0)};
		mesh.vertices = vs;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();
		go.GetComponent<MeshFilter> ().mesh = mesh;
	}
}
