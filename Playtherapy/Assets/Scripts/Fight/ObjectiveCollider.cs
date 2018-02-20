using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveCollider : MonoBehaviour {

    public GameObject Impact;

	// Use this for initialization
	void Start () {
		
	}

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Robot")
        {


            Destroy(gameObject);
            Destroy(Instantiate(Impact, transform.position, transform.rotation), 5.0f);



        }
    }
}
