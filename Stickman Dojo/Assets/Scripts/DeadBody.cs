using UnityEngine;
using System.Collections;

public class DeadBody : MonoBehaviour {

	float cooldown;
	public float timer=10f;
	public Rigidbody rigid;
	public Collider coll;
	public SpriteRenderer spr;
	public Sprite[]sprites = new Sprite[10];

	void Start(){
		spr.sprite = sprites [Random.Range (0, sprites.Length)];

		Vector3 rotEuler = transform.eulerAngles;
		
	
		rotEuler.z = Random.Range (-90, 90);
		if (rotEuler.z > -45 || rotEuler.z < 45)
			rotEuler.z = Random.Range (45, 90);
		transform.eulerAngles = rotEuler;


	}

	// Update is called once per frame
	void Update () {

		cooldown += Time.deltaTime;
		if (cooldown >= timer) {
			Destroy(rigid);
			Destroy(coll);
			Destroy(this);


			}
		}

	}

