using UnityEngine;
using System.Collections;

public class BotAi : MonoBehaviour {
	// sounds
	SoundSet soundset;


	// the player to attack
	Transform player;

	// used components for the movement/animation
	Animator anim;
	Rigidbody rigid;

	//walk speed of the Bot
	float Botspeed=2f;

	// the minimum Y position to Jump
	float jumpHeightMin = 0.1f;

	public Transform ArmPivot;

	// attack Randomization Variables
	float changeSwingCheck=0.45f;
	float changeSwingCooldown;
	bool attackUpDown;
	float swingLerper;

	// walk Randomization
	float changeWalkCheck=0.5f;
	float changeWalkCooldown;
	int speedModder=-1;

	// jump Randomization
	float changeJumpCheck= 1f;
	float changeJumpCooldown;
	float JumpPower = 5f;


	// Use this for initialization
	void Start () {

		//finding used components
		anim=GetComponent<Animator> ();
		anim.SetBool ("Run", true);

		rigid = GetComponent<Rigidbody> ();

		soundset = FindObjectOfType<SoundSet> ();

		//finding player transform
		player = GameObject.FindGameObjectWithTag ("PG").gameObject.transform;
	
	}
	

	void FixedUpdate () {
		MoveTowardPlayer ();	
	}

	void Update(){
		LookAtPlayer ();


		//randomize swings

		changeSwingCooldown += Time.deltaTime;
		if (changeSwingCooldown > changeSwingCheck) {
			SwingChanger();
		}
		Swing ();

	
		//randomize walk

		changeWalkCooldown += Time.deltaTime;
		if (changeWalkCooldown > changeWalkCheck) {
			WalkChanger();
		}


		//randomize jump
		changeJumpCooldown += Time.deltaTime;
		if (changeJumpCooldown > changeJumpCheck) {
			JumpChanger();
		}


	}

	void JumpChanger(){
		changeJumpCooldown = 0;

	
		int randomize = Random.Range (0, 2);
		
		if (randomize == 1 && transform.position.y<jumpHeightMin) {
			rigid.AddForce (Vector3.up*JumpPower, ForceMode.Impulse);


		} 


	}




	void WalkChanger(){
		int randomize = Random.Range (0, 2);
		
		if (randomize == 0) {
			speedModder=1;
		} else {
			speedModder=-1;
			
		}
		changeWalkCooldown = 0;

	}


	void Swing(){
		if (attackUpDown) {
			swingLerper+=Time.deltaTime*Sword.Armspeed;

		} else {
			swingLerper-=Time.deltaTime*Sword.Armspeed;

		
		}

		// Return the correct sword eulerangle by giving a value (from 0 to 1)
		
		ArmPivot.eulerAngles = Sword.ArmPivotRotation(ArmPivot.eulerAngles,swingLerper);
		
		
		// Clamp the value from 0 to 1 and avoid overcharge
		swingLerper = Sword.ClampArmAngle (swingLerper);



	}

	void SwingChanger(){
		int randomize = Random.Range (0, 2);


		if (randomize == 1 && attackUpDown==false) {
			attackUpDown=true;
			PlaySwingSounds();
		} 

		if (randomize ==0 && attackUpDown==true){
			attackUpDown=false;
			PlaySwingSounds();
		}

		changeSwingCooldown = 0;

	}

	void PlaySwingSounds(){

		// randomizing swing sound and play
		soundset.Sounds [1].pitch = Random.Range (0.8f, 1.2f);
		soundset.Sounds [1].Play ();

	}






	void MoveTowardPlayer(){
		Vector3 movementVelocity  = rigid.velocity;

		// check if the character is gone over screenBounds
		Vector3 xPosit = Camera.main.WorldToScreenPoint(transform.position);
		float xPos = xPosit.x / Screen.width;

	
		//forcing the direction Towards if the BOT is out the screen bounds
		if (xPos > 0.8f ) {
			speedModder = 1;
		}
		if (xPos < 0.2f ) {
			speedModder = -1;
		}


		if (transform.rotation.y == 0 ) {
		movementVelocity.x = Botspeed*speedModder;
		} 

		else {
		movementVelocity.x = -Botspeed*speedModder;
		}


	
		rigid.velocity = movementVelocity;

	}





	void LookAtPlayer(){

		
		float playerX = player.position.x;
		float botX = transform.position.x;
		Vector3 euler = transform.eulerAngles;

		if (botX > playerX) {
			euler.y = 0;
		} else {
			euler.y = 180f;			
		}

		if (euler.y != transform.rotation.y) {
			// apply changes only if the rotation is different (it saves memory usage)
			transform.eulerAngles = euler;
		}

	}






}
