using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpDelete : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Ball")
        {

            Destroy(gameObject);
        }
        
           




    }
    }
