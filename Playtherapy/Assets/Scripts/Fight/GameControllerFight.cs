using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameControllerFight : MonoBehaviour {
    //Personaje 

    public static GameControllerFight gc;
    public GameObject ParticlesParent;

    public GameObject ParticlePunchRight;
    public GameObject ParticlePunchLeft;

    public GameObject ParticleSwordRight;
    public GameObject ParticleSwordLeft;

    public GameObject PlayerCenter;
    public int score;
    PutDataResults results;

    private float Minangle;
    private float Maxangle;


    // TEST



    public string name = "si lo muestra prro";



    //Game


    float appearTime =0.5f ;
    float rate = 4;

    float currentTime;
    private float timeMillis;
    public float totalTime;

    float GameType;
    float currentRepetitions;
    public Text textCurrentTime;
    public GameObject MainPanel;
    public bool InGame;


    public Text ScoreText;
    // Giant Robot

    public Animator GiantRobot;

    //EndGame

    public GameObject ResultPanel;




	// Use this for initialization
	void Start () {
        
        gc = gameObject.GetComponent<GameControllerFight>();
        MainPanel.SetActive(false);
        results = ResultPanel.GetComponent<PutDataResults>();
        ResultPanel.SetActive(false);

        InGame = false;

      

    }

    // Update is called once per frame
    void Update() {

        
        if (InGame)
        {


            if (GameType == 0)
            {

                currentTime -= Time.deltaTime;
                if (currentTime > 0 && GameType == 0)
                {
                    timeMillis -= Time.deltaTime * 1000;
                    if (timeMillis < 0)
                        timeMillis = 1000f;
                    textCurrentTime.text = (((int)currentTime) / 60).ToString("00") + ":"
                        + (((int)currentTime) % 60).ToString("00") + ":"
                        + ((int)(timeMillis * 60 / 1000)).ToString("00");
                   // sliderCurrentTime.value = currentTime * 100 / totalTime;


                }
                else
                {


                    textCurrentTime.text = "00:00:00";
                }
            }

            if (GameType == 1)
            {

                textCurrentTime.text = currentRepetitions + " Restante";


            }

            if (GameType == 1)
            {
                if (currentRepetitions <= 0 && ParticlesParent.transform.childCount == 0)
                {


                    EndGame();


                }
            }
            else
            {

                if (currentTime <= 0 && ParticlesParent.transform.childCount == 0)
                {
                    EndGame();

                }
            }

            Time.timeScale = 1;
            if (Time.time > appearTime && ParticlesParent.transform.childCount < 1)
            {

                InvokeRepeating("ShowObjective", 0f, 0f);


                appearTime = Time.time + 2;



            }

        }
        else {

            

        }



    }


    public void StartGame(float minimo, float maximo,int number_repetitions,float count) {

        GiantRobot.enabled = true;
        score = 0;
        UpdateScore();
        MainPanel.SetActive(true);
        InGame = true;

        Maxangle = maximo;
        Minangle = minimo;

        if (number_repetitions == 0)
        {

           

            if (count == 0) {

                count = 60;
            }
            currentTime = count;
            GameType = number_repetitions;
        }
        else {

            if (count == 0)
            {

                count = 1;
            }

            currentRepetitions = count;
            GameType = number_repetitions;
        }
        

    }
    public void EndGame()
    {

        MainPanel.SetActive(false);
        //pausa.SetActive(false);
        ResultPanel.SetActive(true);


        StopAllCoroutines();
        GiantRobot.enabled = false;
        InGame = false;
        int result = Mathf.RoundToInt((score / score) * 100);
        results = ResultPanel.GetComponent<PutDataResults> ();
        print(result);
        results.updateData(result, 0);







    }

    void ShowObjective() {

        StartCoroutine(Indicator());
    }


    IEnumerator Indicator() {


       

        GameObject Temporary_Bullet_Handler;

        System.Random typeSelection = new System.Random();

        GiantRobot.Play("Punch");
        
        

        yield return new WaitForSeconds(0f);

        int type = typeSelection.Next(0, 100);

        if (type < 50)
        {
            System.Random positionZ = new System.Random();
            System.Random Angulo = new System.Random();
            System.Random handPunchSelection = new System.Random();
            int hand_selected = handPunchSelection.Next(0, 100);


            if (hand_selected < 50)
            {
                //LEFT
                double RandomAngle = (Minangle + Angulo.NextDouble() * (Maxangle - Minangle));


                double posX = Math.Cos((RandomAngle + 90) * Math.PI / 180) * 2;
                double posY = Math.Sin((RandomAngle + 90) * Math.PI / 180) * 2;

                double posZ = positionZ.NextDouble() * (1.6 - 0.7) + 0.7;
                // posZ debe variar de 0.7-1.7 para estar a distancia del jugador 
                //posX debe variar de 25-180 y negativo 


                var vector = new Vector3((float)(PlayerCenter.transform.position.x - posX), (float)(PlayerCenter.transform.position.y - posY), (float)(PlayerCenter.transform.position.z - posZ));//force-


                Temporary_Bullet_Handler = Instantiate(ParticlePunchLeft, vector, PlayerCenter.transform.rotation) as GameObject;
                Temporary_Bullet_Handler.transform.parent = ParticlesParent.transform;
                ModifyRepetitions();

            }
            else
            {
                //RIGHT
                double RandomAngle = -((Minangle + Angulo.NextDouble() * (Maxangle - Minangle)));


                double posX = Math.Cos((RandomAngle + 90) * Math.PI / 180) * 2;
                double posY = Math.Sin((RandomAngle + 90) * Math.PI / 180) * 2;

                double posZ = positionZ.NextDouble() * (1.6 - 0.7) + 0.7;
                
                // posZ debe variar de 0.7-1.7 para estar a distancia del jugador 
                //posX debe variar de 25-180 y negativo 


                var vector = new Vector3((float)(PlayerCenter.transform.position.x - posX), (float)(PlayerCenter.transform.position.y - posY), (float)(PlayerCenter.transform.position.z - posZ));//force-


                Temporary_Bullet_Handler = Instantiate(ParticlePunchRight, vector, PlayerCenter.transform.rotation) as GameObject;
                Temporary_Bullet_Handler.transform.parent = ParticlesParent.transform;
                ModifyRepetitions();

            }






        }
        else {
            System.Random positionZ = new System.Random();
            System.Random Angulo = new System.Random();
            System.Random handSwordSelection = new System.Random();


            int hand_selected = handSwordSelection.Next(0, 100);


               if (hand_selected < 50)
               {
                   //LEFT SWORD
                   double RandomAngle = (Minangle + Angulo.NextDouble() * (Maxangle - Minangle));


                   double posX = Math.Cos((RandomAngle + 90) * Math.PI / 180) * 2;
                   double posY = Math.Sin((RandomAngle + 90) * Math.PI / 180) * 2;

                   double posZ = 4;
                   // posZ debe variar de 0.7-1.7 para estar a distancia del jugador 
                   //posX debe variar de 25-180 y negativo 
                   


                   var vector = new Vector3((float)(PlayerCenter.transform.position.x - posX), (float)(PlayerCenter.transform.position.y - posY), (float)(PlayerCenter.transform.position.z - posZ));//force-


                   Temporary_Bullet_Handler = Instantiate(ParticleSwordLeft, vector, PlayerCenter.transform.rotation) as GameObject;
                   Temporary_Bullet_Handler.transform.parent = ParticlesParent.transform;
                ModifyRepetitions();

            }
               else {
                   //RIGHT SWORD
                   double RandomAngle = -((Minangle + Angulo.NextDouble() * (Maxangle- Minangle)));


                   double posX = Math.Cos((RandomAngle + 90) * Math.PI / 180) * 2;
                   double posY = Math.Sin((RandomAngle + 90) * Math.PI / 180) * 2;

                   double posZ = 4;
                   
                   // posZ debe variar de 0.7-1.7 para estar a distancia del jugador 
                   //posX debe variar de 25-180 y negativo 


                   var vector = new Vector3((float)(PlayerCenter.transform.position.x - posX), (float)(PlayerCenter.transform.position.y - posY), (float)(PlayerCenter.transform.position.z - posZ));//force-


                   Temporary_Bullet_Handler = Instantiate(ParticleSwordRight, vector, PlayerCenter.transform.rotation) as GameObject;
                   Temporary_Bullet_Handler.transform.parent = ParticlesParent.transform;
                ModifyRepetitions();

               }



           


        }

    }


    public void ChangeScore (int ScoreObject)
    {
        score += ScoreObject;
        UpdateScore();
    }
    void UpdateScore()
    {

        ScoreText.text = "" + score;
    }



    public void ModifyRepetitions() {

        currentRepetitions = currentRepetitions - 1;

    }
}



