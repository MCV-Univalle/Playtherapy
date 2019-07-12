using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallFL : MonoBehaviour {
	
	ManagerFL manager;

	void Start () {
		manager = GameObject.FindObjectOfType<ManagerFL> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnCollisionEnter(Collision col){
        manager.repeticionesRestantes++;

        if (col.gameObject.transform.tag=="Terrain") {
			this.transform.position = new Vector3 (0, 6, 2);
			manager.golpeFallido.Play ();
		}
	}


}
