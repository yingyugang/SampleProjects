using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace SixRun{
	public class GroundSpawner : SixRunSingleMono<GroundSpawner> {

		Mesh mMesh;
		MeshFilter mMeshFilter;
		public List<Vector3> verticeList;
		public List<Color> colorList;
		public List<int> triangleList;
		public List<Vector2> uvList;
		public Color[] colors;
		public Color lineColor;
		public List<Vector3> startPosList;
		public const int startGridCount = 24;


		protected override void Awake(){
			base.Awake ();
			verticeList = new List<Vector3> ();
			colorList = new List<Color> ();
			triangleList = new List<int> ();
			uvList = new List<Vector2> ();

			mMesh = new Mesh ();
			mMeshFilter = GetComponent<MeshFilter> ();
			mMesh.vertices = new Vector3[0];
			for(int i=0;i<6;i++){
				GameObject go = GameObject.CreatePrimitive (PrimitiveType.Capsule);
				go.transform.SetParent (transform);
				go.transform.localPosition = new Vector3((i + 1)* (defaultWidth + border) - defaultWidth/2,0, defaultWidth/2) * scale;
				go.SetActive (false);
				startPosList.Add (go.transform.position);
			}
			for(int i=0;i< startGridCount;i++){
				SpawnRow(null);
			}
			StartCoroutine (_CleanRow());
		}

		int mCurrentRowIndex;
		public int spawnCount = 10;
		public float scale = 10;
		public float border = 0.05f;
		public float defaultWidth = 1f;
		public void SpawnRow(HashSet<int> holeIndex){
			if(verticeList.Count > 0 && transform.TransformPoint(verticeList[verticeList.Count-1]).z > 120){
				return;
			}

			//for(int j = 0;j < spawnCount ;j++){
			float x = 0;
			for (int i = 0; i < 7; i++) {
				CreateTriangle (new Rect(i* (defaultWidth + border),-mCurrentRowIndex * (defaultWidth + border) ,border,defaultWidth),lineColor);
			}
			for(int i =0;i<6;i++){
				if (holeIndex!=null && holeIndex.Contains(i)) {
					CreateTriangle (new Rect(i* (defaultWidth + border) + border,-mCurrentRowIndex * (defaultWidth + border),defaultWidth,defaultWidth),new Color(0,0,0,0));
				} else {
					CreateTriangle (new Rect(i* (defaultWidth + border) + border,-mCurrentRowIndex * (defaultWidth + border),defaultWidth,defaultWidth),colors[5-i]);
				}

			}
			CreateTriangle (new Rect(0,-mCurrentRowIndex * (defaultWidth + border)-border,(defaultWidth + border) * 6 + border,border),lineColor);
			mCurrentRowIndex++;
			if(mCurrentRowIndex>30){
				int count = 56;
				verticeList.RemoveRange (0,count);
				triangleList.RemoveRange (0,84);
				for(int i=0;i<triangleList.Count;i++){
					triangleList [i] = triangleList [i] - count;
				}
				colorList.RemoveRange (0,count);
				uvList.RemoveRange(0,count);
			}

			mMesh.vertices = verticeList.ToArray ();
			mMesh.uv = uvList.ToArray ();
			mMesh.colors = colorList.ToArray ();
			mMesh.triangles = triangleList.ToArray ();
			mMesh.RecalculateBounds ();
			mMeshFilter.mesh = mMesh;

		}

		IEnumerator _CleanRow(){
			while (true) {
				yield return new WaitForSeconds (5);
				CleanRow ();
			}
		}

		void CleanRow(){
			if(verticeList.Count>2400){
				int count = verticeList.Count - 2400;
				verticeList.RemoveRange (0,count);
				triangleList.RemoveRange (0,triangleList.Count - 3600);
				for(int i=0;i<triangleList.Count;i++){
					triangleList [i] = triangleList [i] - count;
				}
				colorList.RemoveRange (0,count);
				uvList.RemoveRange(0,count);
			}
			mMesh.vertices = verticeList.ToArray ();
			mMesh.uv = uvList.ToArray ();
			mMesh.colors = colorList.ToArray ();
			mMesh.triangles = triangleList.ToArray ();
			mMesh.RecalculateBounds ();
			mMeshFilter.mesh = mMesh;
		}

		void CreateTriangle(Rect rect,Color color){
			Vector3 vertic = new Vector3 (rect.x * scale,0,rect.y* scale);
			verticeList.Add (vertic);
			int t0 = verticeList.Count - 1;
			Vector3 vertic1= new Vector3 (rect.x* scale ,0,(rect.y + rect.height)* scale);
			verticeList.Add (vertic1);
			int t1 = verticeList.Count - 1;
			Vector3 vertic2 = new Vector3 ((rect.x + rect.width)* scale ,0,(rect.y + rect.height)* scale);
			verticeList.Add (vertic2);
			int t2 = verticeList.Count - 1;
			Vector3 vertic3 = new Vector3 ((rect.x + rect.width)* scale ,0,rect.y* scale);
			verticeList.Add (vertic3);
			int t3 = verticeList.Count - 1;

			triangleList.Add (t0);
			triangleList.Add (t1);
			triangleList.Add (t3);
			triangleList.Add (t1);
			triangleList.Add (t2);
			triangleList.Add (t3);

			colorList.Add (color);
			colorList.Add (color);
			colorList.Add (color);
			colorList.Add (color);

			uvList.Add (new Vector2(0,0));
			uvList.Add (new Vector2(0,1));
			uvList.Add (new Vector2(1,1));
			uvList.Add (new Vector2(1,0));
		}
	}
}