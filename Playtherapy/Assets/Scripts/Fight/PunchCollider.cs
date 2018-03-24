using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class PunchCollider : MonoBehaviour {



    public GameObject BuildingParticles;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame


    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Building"){

            
            Destroy(Instantiate(BuildingParticles, transform.position, transform.rotation), 5.0f);
           
            
            
        }
    }
}
