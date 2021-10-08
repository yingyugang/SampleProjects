using UnityEngine;
using System.Collections;

public class Sword{
	public static float Armspeed=5f;

	public static float minArmAngle = -120f;
	public static float maxArmAngle = 45;

	public static float ConvertSwordAngle(float angle){
		angle = ClampArmAngle (angle);
		float lerper = Mathf.Lerp (maxArmAngle, minArmAngle, angle);
		return lerper;

	}

	public static float ClampArmAngle(float angle){
		angle = Mathf.Clamp (angle, 0, 1);
		return angle;
	}


	public static Vector3 ArmPivotRotation(Vector3 pivotRot,float angle){

		float ClampedValue = ConvertSwordAngle (angle);
		Vector3 euler = pivotRot;
		euler.z = ClampedValue;
		return euler;
	
	}




}
