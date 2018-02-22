using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameControllerFight : MonoBehaviour {

    public GameObject ParticlesParent;

    public GameObject ParticlePunchRight;
    public GameObject ParticlePunchLeft; 

    public GameObject PlayerCenter;

    float appearTime = 4.0f;
    float rate = 4;

  

	// Use this for initialization
	void Start () {


        //InvokeRepeating("ShowObjective", 0f, 0f);

    }

    // Update is called once per frame
    void Update() {


        Time.timeScale = 1;
        if (Time.time > appearTime && ParticlesParent.transform.childCount < 1)
        {

            InvokeRepeating("ShowObjective", 0f, 0f);


            appearTime = appearTime + rate;
           
            

        }

    }



    void ShowObjective() {

        StartCoroutine(Indicator());
    }


    IEnumerator Indicator() {


       

        GameObject Temporary_Bullet_Handler;
        

        System.Random handSelection = new System.Random();
        System.Random positionZ = new System.Random();
        System.Random Angulo = new System.Random();

        yield return new WaitForSeconds(0f);

        int hand_selected = handSelection.Next(1, 100);


        if (hand_selected < 50)
        {
            //LEFT
            double RandomAngle = (25 + Angulo.NextDouble() * (180 - 0));


            double posX = Math.Cos((RandomAngle + 90) * Math.PI / 180) * 2;
            double posY = Math.Sin((RandomAngle + 90) * Math.PI / 180) * 2;

            double posZ = positionZ.NextDouble()*(1.6-0.7)+0.7;
            // posZ debe variar de 0.7-1.7 para estar a distancia del jugador 
            //posX debe variar de 25-180 y negativo 


            var vector = new Vector3((float)(PlayerCenter.transform.position.x - posX), (float)(PlayerCenter.transform.position.y - posY), (float)(PlayerCenter.transform.position.z - posZ));//force-


            Temporary_Bullet_Handler = Instantiate(ParticlePunchLeft, vector, PlayerCenter.transform.rotation) as GameObject;
            Temporary_Bullet_Handler.transform.parent = ParticlesParent.transform;

        }
        else {
            //RIGHT
            double RandomAngle = -((25 + Angulo.NextDouble() * (180 - 0)));


            double posX = Math.Cos((RandomAngle + 90) * Math.PI / 180) * 2;
            double posY = Math.Sin((RandomAngle + 90) * Math.PI / 180) * 2;

            double posZ = positionZ.NextDouble()*(1.6-0.7)+0.7;
            // posZ debe variar de 0.7-1.7 para estar a distancia del jugador 
            //posX debe variar de 25-180 y negativo 


            var vector = new Vector3((float)(PlayerCenter.transform.position.x - posX), (float)(PlayerCenter.transform.position.y - posY), (float)(PlayerCenter.transform.position.z - posZ));//force-


            Temporary_Bullet_Handler = Instantiate(ParticlePunchRight, vector, PlayerCenter.transform.rotation) as GameObject;
            Temporary_Bullet_Handler.transform.parent = ParticlesParent.transform;

        }


        



    }
}
