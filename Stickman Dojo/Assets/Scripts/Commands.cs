using UnityEngine;
using System.Collections;

public class Commands : MonoBehaviour {
	public Transform ArmPivot;

	float Playerspeed=5f;
	float JumpPower = 5f;
	float jumpHeightMin = 0.1f; 	// minimum Y position to jump
	float JumpCooldown;

	float armAngle;
	float ArmVariable;

	Animator anim;
	Rigidbody rigid;
	public CNJoystick leftJoystick;
	public CNTouchpad rightJoystick;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rigid = GetComponent<Rigidbody> ();

	}
	
	// Update is called once per frame
	void Update () {
	
		if (rightJoystick.IsCurrentlyTweaking) {
			ArmVariable += Time.deltaTime*Sword.Armspeed;
		} else {
			ArmVariable -= Time.deltaTime*Sword.Armspeed;
		}
		ArmVariable = Mathf.Clamp (ArmVariable, 0, 1);




		armAngle = ArmVariable+Input.GetAxis("Vertical")*2;
				
			



		// Return the correct sword eulerangle by giving a value (from 0 to 1)

		ArmPivot.eulerAngles = Sword.ArmPivotRotation(ArmPivot.eulerAngles,armAngle);


		// Clamp the value from 0 to 1 and avoid overcharge
		armAngle = Sword.ClampArmAngle (armAngle);


		JumpCooldown += Time.deltaTime;
		if ((Input.GetKeyDown (KeyCode.Space) || leftJoystick.GetAxis("Vertical")>0.5f) && transform.position.y<jumpHeightMin && JumpCooldown>0.5f) {
			JumpCooldown=0;

			rigid.AddForce (Vector3.up*JumpPower, ForceMode.Impulse);	

		}



	}



	void FixedUpdate(){
		float inputValue = leftJoystick.GetAxis ("Horizontal") + Input.GetAxis ("Horizontal");
		Movement(inputValue);			
	}



	void Movement(float inputH){

		Vector3 rotation = transform.eulerAngles;

		if (inputH != 0) {
			anim.SetBool("Run",true);

			//Face the player left/right
			if (inputH<0){rotation.y =0;}else{
				rotation.y =180;
			}
			// move 
			Vector3 movementVelocity  = rigid.velocity;
			movementVelocity.x = inputH*Playerspeed;

			// check if the character is gone over screenBounds
			Vector3 xPosit = transform.position;
			xPosit = Camera.main.WorldToScreenPoint(xPosit);
			float xPos = xPosit.x/Screen.width;

			// apply the normal force if the character is in screenbounds, otherwise apply a force to repel it
			if (xPos>0.025f && inputH<0 || xPos<0.9725f && inputH>0){
				rigid.velocity = movementVelocity;}
			else {
				rigid.velocity=transform.right+Physics.gravity/2;
			}


			// Updating Rotation

			if (rotation.y!=transform.rotation.y){
				transform.eulerAngles = rotation;}


		} else {
			anim.SetBool("Run",false);
			Vector3 movementVelocity  = rigid.velocity;
			movementVelocity.x = 0;
			rigid.velocity = movementVelocity;
		}







	}




}
