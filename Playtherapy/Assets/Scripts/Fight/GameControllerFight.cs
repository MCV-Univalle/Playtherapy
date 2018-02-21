using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameControllerFight : MonoBehaviour {



    public GameObject ParticlePunchRight;
    public GameObject ParticlePunchLeft; 

    public GameObject PlayerCenter;



	// Use this for initialization
	void Start () {


        Indicator();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Indicator() {


        GameObject Temporary_Bullet_Handler;

        System.Random handSelection = new System.Random();
        System.Random positionx = new System.Random();
        System.Random positiony = new System.Random();


        double posX = Math.Cos((0 + 90) * Math.PI / 180) ;
        double posY = Math.Sin((0 + 90) * Math.PI / 180) ;
        var vector = new Vector3((float)(PlayerCenter.transform.position.x - posX), (float)(PlayerCenter.transform.position.y - posY), (float)PlayerCenter.transform.position.z) ;//force-




        

        Temporary_Bullet_Handler = Instantiate(ParticlePunchRight, vector, PlayerCenter.transform.rotation) as GameObject;
      

    }
}
