using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	// sprite effects for wall blood
	SpriteEffects sprEf;


	// Sounds
	SoundSet soundSet;

	// sword collider for death detection
	public Collider swordCollider;

	// used components
	BotSpawner spawn;
	Rigidbody rigid;
	BotAi botAi;
	Commands comms;
	Animator anim;
	GameOverHandler gameOverH; 


	// Use this for initialization
	void Start () {

		sprEf = FindObjectOfType<SpriteEffects> ();
		gameOverH = FindObjectOfType<GameOverHandler> ();

		soundSet = FindObjectOfType<SoundSet>();
		spawn = FindObjectOfType<BotSpawner> ();
		anim = GetComponent<Animator> ();
		rigid = GetComponent<Rigidbody> ();

		// getting the AI if it's a BOT or the Commands for the Player
		if (gameObject.tag == "Bot") {
			botAi = GetComponent<BotAi> ();
		} else {
			comms = GetComponent<Commands>();
		}

	}
	
	void Update(){
		if (transform.position.y <= -1) {
			Respawn();
		
		}

	}

	void Respawn(){
		rigid.velocity = Vector3.zero;

		spawn.SpawnBot (this.transform);

	}

	void OnTriggerEnter(Collider collider){

		if (collider.tag == "Sword"  && collider.transform.root!=this.transform.root) {
			Death();
		}

	} 
	void InstantiateEffect(string effect,float x,float y,float z,float rx,float ry,float rz,int probability){

		int prob = Random.Range (0, probability);
		if (prob != 0)
			return;

		Vector3 pos;

		pos.x = x;
		pos.y = y;
		pos.z = z;

		Vector3 rotEuler = Vector3.zero;

		rotEuler.x = rx;
		rotEuler.y = ry;
		rotEuler.z = rz;

		Quaternion rot = Quaternion.Euler (rotEuler);

		GameObject effectObj = (GameObject)Instantiate (Resources.Load (effect), pos, rot);
	}


	void InstantiateSpriteEffect(int number,float x,float y,float z,float rx, float ry, float rz, int probability){
		int prob = Random.Range (0, probability);
		if (prob != 0)
			return;

		Vector3 pos;
		pos.x = x;
		pos.y = y;
		pos.z = z;

		Vector3 rotEuler = Vector3.zero;
		
		rotEuler.x = rx;
		rotEuler.y = ry;
		rotEuler.z = rz;
		Quaternion rot = Quaternion.Euler (rotEuler);


		GameObject effectObj = (GameObject)Instantiate (sprEf.spritesEffects[number].gameObject, pos, rot);
		effectObj.tag = "Cleanable";
	
	}


	void Death(){
		// instantiate effects on death
		InstantiateEffect ("blood",transform.position.x,-0.55f,0.3f,55,0,Random.Range (0, 361),1);
		InstantiateEffect ("deadBody",transform.position.x,transform.position.y,-0.095f,0,0,0,1);

		InstantiateSpriteEffect (Random.Range(0,sprEf.spritesEffects.Length),transform.position.x,transform.position.y+Random.Range(0.5f,1.0f),0.3f,0,0,0,2);

		//randomizing hit sound and play
		soundSet.Sounds[0].pitch = Random.Range (0.5f, 1.5f);
		soundSet.Sounds[0].Play ();

		// if this is  a BOT disable the AI, set a position on the background and Spawn an other Bot

		if (botAi != null) {
		
			Respawn();


			return;
			
		} else {

			// The death occurred on the Player : GameOver
			gameOverH.GameOver();


		}



	}


}
