using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace SixRun{
	public class Item : MonoBehaviour {

		public Animator anim;
		SpriteRenderer sr;
		Transform trans;
		public bool isCollected;
		public BoxCollider coll;
		public Transform shadowPos;
		public Transform bodyPos;

		public Transform miniJump;
		public Transform bigJump;

		public MapDetailData mapDetailData;
		public ItemData itemData;

		//public int heightOffset = 0;
		//public int height = 2;
		//public int width = 3;
		static Mesh mesh;

		float characterZ = -1.5f;
		Dictionary<SpriteRenderer,int> defaultBodyOrders;
		Dictionary<SpriteRenderer,int> overBodyOrders;
		bool isOver;
		SpriteRenderer[] bodyRenderers;


		void Awake(){
			trans = transform;
			coll = GetComponentInChildren<BoxCollider> ();
			coll.gameObject.layer = GameManager.OBSTACLE_LAYER;
			shadowPos = trans.FindChild ("shadowPos");
			bodyPos= trans.FindChild ("bodyPos");
			if (bodyPos != null) {
				bodyRenderers = bodyPos.GetComponentsInChildren<SpriteRenderer> ();
				defaultBodyOrders = new Dictionary<SpriteRenderer, int> ();
				overBodyOrders = new Dictionary<SpriteRenderer, int> ();
				if (bodyRenderers != null && bodyRenderers.Length > 0) {
					for(int i=0;i<bodyRenderers.Length;i++){
						defaultBodyOrders.Add (bodyRenderers[i],bodyRenderers[i].sortingOrder);
						overBodyOrders.Add (bodyRenderers[i],bodyRenderers[i].sortingOrder + 6);
					}
				}
			}
			if(shadowPos!=null){
				miniJump = shadowPos.FindChild ("miniJump");
				bigJump = shadowPos.FindChild ("bigJump");
			}
			if (mesh == null) {
				GameObject obj = GameObject.CreatePrimitive (PrimitiveType.Cube);
				obj.SetActive (false);
				mesh = obj.GetComponent<MeshFilter> ().sharedMesh;
			}
		}

		void OnEnable(){
			isOver = false;
			isCollected = false;

			if (bodyRenderers != null && bodyRenderers.Length > 0) {
				for(int i=0;i<bodyRenderers.Length;i++){
					bodyRenderers[i].sortingOrder = defaultBodyOrders[bodyRenderers[i]];
				}
			}
			if(miniJump!=null){
				miniJump.GetComponent<SpriteRenderer> ().sortingOrder = 0;
			}
			if(bigJump!=null){
				bigJump.GetComponent<SpriteRenderer> ().sortingOrder = 0;
			}
		}
		public void SetPos(float heightOffset,int height,int width){
			if (heightOffset < 0) {
				heightOffset = 0;
			}
			float realHeight = height * GroundSpawner.GetInstance ().defaultWidth * GroundSpawner.GetInstance ().scale;
			float realWidth = (GroundSpawner.GetInstance ().defaultWidth * width + GroundSpawner.GetInstance ().border * (width - 1)) * GroundSpawner.GetInstance ().scale;
			float offsetX = realWidth / 2 - GroundSpawner.GetInstance ().defaultWidth / 2 * GroundSpawner.GetInstance ().scale;
			coll.center = new Vector3 (0,realHeight/2,0);
			coll.size = new Vector3 (realWidth, realHeight, 0.5f) - new Vector3 (2,2,0);
			if(bodyPos)bodyPos.transform.localPosition = new Vector3 (offsetX,0,0);
			if(shadowPos)shadowPos.transform.localPosition = new Vector3 (offsetX,0,0);
			if (heightOffset > 0) {
				float realHeightOffset= heightOffset + 0.5f;
				bodyPos.transform.localPosition += new Vector3(0,realHeightOffset,0) * GroundSpawner.GetInstance ().scale;
			}
			if (heightOffset >= 1 && heightOffset <= 2) {
				if (miniJump != null) {
					miniJump.gameObject.SetActive (true);
				}
				if (bigJump != null) {
					bigJump.gameObject.SetActive (false);
				}
			} else if (heightOffset > 2 && heightOffset <= 4) {
				if (miniJump != null) {
					miniJump.gameObject.SetActive (false);
				}
				if (bigJump != null) {
					bigJump.gameObject.SetActive (true);
				}
			} else {
				if (miniJump != null) {
					miniJump.gameObject.SetActive (false);
				}
				if (bigJump != null) {
					bigJump.gameObject.SetActive (false);
				}
			}
		}
		int order = 0;
		void Update () {
			if(GameManager.GetInstance().isPaused){
				return;
			}
			trans.position += GameManager.GetInstance ().forward * (GameManager.GetInstance().RealSpeed() + itemData.moveSpeed * GameManager.GetInstance().gridLength) * Time.deltaTime;
			order = 100 - (int)trans.position.z;
			if (trans.position.z <= characterZ) {
				//isOver = true;
				if (bodyRenderers != null && bodyRenderers.Length > 0) {
					
					for (int i = 0; i < bodyRenderers.Length; i++) {
						bodyRenderers [i].sortingOrder = defaultBodyOrders [bodyRenderers [i]] + order * 3;
					}
					if (miniJump != null) {
						miniJump.GetComponent<SpriteRenderer> ().sortingOrder = order * 3;
					}
					if (bigJump != null) {
						bigJump.GetComponent<SpriteRenderer> ().sortingOrder = order * 3;
					}

				}
			} else {
				for(int i=0;i<bodyRenderers.Length;i++){
					bodyRenderers[i].sortingOrder = defaultBodyOrders[bodyRenderers[i]] + order * 2;
					if(miniJump!=null){
						miniJump.GetComponent<SpriteRenderer> ().sortingOrder =  order * 2;
					}
					if(bigJump!=null){
						bigJump.GetComponent<SpriteRenderer> ().sortingOrder =  order * 2;
					}
				}
			}
			
		}
	}
}
