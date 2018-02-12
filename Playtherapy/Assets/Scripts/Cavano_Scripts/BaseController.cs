using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour {
	GameManagerCavano manager;
	// Use this for initialization
	void Start () {
		manager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManagerCavano> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag ("Coins")) {
			manager.SetScore (3);
			manager.SetStatusCava (false);
		}
	}
}
