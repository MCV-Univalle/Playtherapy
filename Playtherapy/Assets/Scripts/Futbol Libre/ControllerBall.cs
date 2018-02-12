using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerBall : MonoBehaviour {

	public Rigidbody ball;
	public float velocity;
	public float velocidadGravedad;
	public PlayerFL jugador;
	public bool puede_levantarse=false;


	// Use this for initialization
	void Start () {

	}
	public void patear()
	{
		if (ball!=null) {
			ball.velocity= (Vector3.up*velocity);
		}
	}

	public void OnTriggerEnter(Collider other){
		print ("colision! 1");
		print ("colision 1-2 con"+other.gameObject.name);
		if (other.gameObject.name.Equals("ZonaPateo")) {
			print ("colision! 2");
			puede_levantarse = true;
			print ("puedo patear");
		}
	}

	public void OnTriggerExit(Collider other){
		print ("colision! 3");
		print ("colision 3-4 con"+other.gameObject.name);
		if (other.gameObject.name.Equals("ZonaPateo")) {
			print ("colision! 4");
			puede_levantarse = false;
			print ("no puedo patear");
		}
	}
}
