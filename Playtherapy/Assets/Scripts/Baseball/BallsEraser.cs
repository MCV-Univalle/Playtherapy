using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsEraser : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Ball" )
        {

            Destroy(other);

        }
    }
}
