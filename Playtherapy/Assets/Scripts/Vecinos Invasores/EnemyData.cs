using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
public class EnemyData : MonoBehaviour {



	public int ship_random_index;//describes the type of the ship small,medium or big 
	// Use this for initialization
	public Finger.FingerType finger_to_destroy;
	public void setMaterialFinger(Material mat,Finger.FingerType finger)
	{
		this.transform.GetChild(0).GetComponent<Renderer> ().material = mat;
        finger_to_destroy = finger;
	}

}
