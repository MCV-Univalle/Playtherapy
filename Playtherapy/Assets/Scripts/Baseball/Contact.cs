using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Contact : MonoBehaviour {

	public int scorevalue;
	private GameController gameController;
	public GameObject ball_particles;
    private AudioSource source;
	private bool pivote;

	void Start()
	{
		pivote = false;
        
       
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if (gameControllerObject != null) {

			gameController = gameControllerObject.GetComponent<GameController>();
		}
		if (gameController == null) {

			Debug.Log("Cannot find GameController script");
		}
		
	}

	void OnTriggerEnter(Collider other){
        
        if (other.tag == "RightHand" && GameController.gc.selectArm <= 50) {
            
          
            Destroy(Instantiate (ball_particles, transform.position, transform.rotation),2.0f);
			GameController.gc.AddScore (scorevalue);
			//Debug.Log (GameController.gc.selectArm );

            


            Destroy (gameObject);
		}
        if (other.tag == "LeftHand" && GameController.gc.selectArm > 50) {
    
            Destroy(Instantiate (ball_particles, transform.position, transform.rotation),2.0f);
			GameController.gc.AddScore (scorevalue);
			//Debug.Log (GameController.gc.selectArm );
            



            Destroy (gameObject);
		}
		if (other.tag == "Wall") {
			
			

			Destroy (gameObject);




		}
		//Destroy (gameObject);
	}
}
