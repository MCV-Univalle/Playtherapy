using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameControllerFight : MonoBehaviour {
    //Camara 

    public GameObject Camera;



    //Personaje 

    public static GameControllerFight gc;
    public GameObject ParticlesParent;

    public GameObject ParticlePunchRight;
    public GameObject ParticlePunchLeft;

    public GameObject ParticleSwordRight;
    public GameObject ParticleSwordLeft;

    public GameObject PlayerCenter;
    public float score;
    public float total;
    PutDataResults results;
    //Parameters

    private float Minangle;
    private float Maxangle;
    private float spawnTime; 
    private bool IsFlexion;
    private bool IsExtension;
    private bool IsCombined;
    private float ShoulderSelection;



    // TEST
    public bool clean;

    public bool GameOverBool;
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
    public Slider Robot;
    public GameObject MainPanel;
    public GameObject PauseButton;
    public GameObject Retry;
    public bool InGame;
    public GameObject Eraser;
    public GameObject RightShoulder;
    public GameObject LeftShoulder;

    public Text ScoreText;
    public float TotalRepetitions;
    // Giant Robot

    public Animator GiantRobot;

    //EndGame

    public GameObject ResultPanel;


    //righthand(139.95,75.04,727.75)
    //lefthand(157.11,64.81,231.29)

	// Use this for initialization
	void Start () {
        
        gc = gameObject.GetComponent<GameControllerFight>();
        MainPanel.SetActive(false);
        PauseButton.SetActive(false);
        results = ResultPanel.GetComponent<PutDataResults>();
        ResultPanel.SetActive(false);
        Camera.transform.position = new Vector3(149f,84.38f,259.68f);
        clean = true;
        InGame = false;
        GameOverBool = false;
        currentTime = 0;
        currentRepetitions = 0;

      

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
                    Robot.value = currentTime * 100 / TotalRepetitions;


                }
                else
                {


                    textCurrentTime.text = "00:00:00";

                }
            }

            if (GameType == 1)
            {

                textCurrentTime.text = currentRepetitions + " Restante";
                Robot.value = currentRepetitions * 100 / TotalRepetitions;


            }

            if (GameType == 1)
            {
                if (currentRepetitions <= 0 && ParticlesParent.transform.childCount == 0)
                {
                    InGame = false;
                    GameOverBool = false;

                    Camera.transform.position = new Vector3(149f, 74.38f, 245.68f);
                    StopAllCoroutines();

                    InvoqueGameOver();
                    


                }
            }
            else
            {

                if (currentTime <= 0 && ParticlesParent.transform.childCount == 0)
                {
                    InGame = false;
                    GameOverBool = false;
                    Camera.transform.position = new Vector3(149f, 74.38f, 245.68f);
                    StopAllCoroutines();
                    InvoqueGameOver();
                    
                    


                }
            }
            

            Time.timeScale = 1;
            if (Time.time > appearTime && ParticlesParent.transform.childCount == 0  && (currentRepetitions > 0 || currentTime > 0 ) && GameOverBool )
            {
                

                InvokeRepeating("ShowObjective", 0f, 0f);
                

                appearTime = Time.time + spawnTime;



            }


           

        }
        else {

            

        }

       



    }

    public float life;
    public float LifeControl
    {

        get
        {
            return life;
        }
        set
        {
            life = value;

            
        }


    }





    public void StartGame(float minimo, float maximo,int number_repetitions,float count,float time,bool flexion,bool Extension,bool comb,float shoulder) {

            

        Camera.transform.position = new Vector3(149f, 84.38f, 259.68f);
        
        Eraser.SetActive(false);
        spawnTime = time;
        IsFlexion = flexion;
        IsExtension = Extension;
        IsCombined = comb;
        ShoulderSelection = shoulder;
        GiantRobot.enabled = true;
        score = 0;
        total = 0;
        UpdateScore();
        MainPanel.SetActive(true);
        PauseButton.SetActive(true);
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
            totalTime = count;
            TotalRepetitions = count;

        }
        else {

            if (count == 0)
            {

                count = 1;
            }

            currentRepetitions = count;
            GameType = number_repetitions;
            TotalRepetitions = count;
        }
        GameOverBool = true;
        

    }

    public void PauseOn()
    {
        InGame = false;

    }

    public void StartAgain()
    {
        
        //GameObject retry = GameObject.FindGameObjectWithTag("Parameters");
        Retry.GetComponent<ParametersFIght>().StartAgain();
        GiantRobot.enabled = false;
        MainPanel.SetActive(false);
        PauseButton.SetActive(false);
        GameOverBool = false;
        currentTime = 0;
        currentRepetitions = 0;

    }
    public void PauseOff()
    {

        InGame = true;

    }
    public void EndGame()
    {

        MainPanel.SetActive(false);
        PauseButton.SetActive(false);
        ResultPanel.SetActive(true);

        //print("puntos, " + total + " totales " + score +" deberia ser "+((score/total)*100));
        StopAllCoroutines();
        GiantRobot.enabled = false;
        InGame = false;
        int result = Mathf.RoundToInt((score / total) * 100);
        results = ResultPanel.GetComponent<PutDataResults> ();
        results.updateData(result, 0);







    }

    void InvoqueGameOver() {
        InGame = false;
        GameOverBool = false;
        StartCoroutine(GameOver());

    }

    IEnumerator GameOver() {
        InGame = false;
        GiantRobot.Play("back_fall");
        yield return new WaitForSeconds(4);
        ResultPanel.SetActive(true);
        EndGame();

    }

    void ShowObjective() {

        StartCoroutine(Indicator());
        StartCoroutine(ObjectivesTime());
    }

    void InvoqueTIme() {

        StartCoroutine(ObjectivesTime());
    }

    IEnumerator ObjectivesTime() {

        yield return new WaitForSeconds(4f);

    }


    IEnumerator Indicator() {


       

        GameObject Temporary_Bullet_Handler;

        System.Random typeSelection = new System.Random(DateTime.Now.Millisecond);

        GiantRobot.Play("Punch");

        yield return new WaitForSeconds(4f);





        if (IsCombined) { 

        int type = typeSelection.Next(0, 100);

            if (type < 50)
            {
                System.Random positionZ = new System.Random();
                System.Random Angulo = new System.Random();
                System.Random handPunchSelection = new System.Random(DateTime.Now.Millisecond);
                int hand_selected = handPunchSelection.Next(0, 100);

                if (ShoulderSelection == 1) {
                    hand_selected = 70;
                }
                if (ShoulderSelection == 2) {
                    hand_selected = 30;
                }


                if (hand_selected < 50)
                {
                    //LEFT
                    double RandomAngle = (Minangle + Angulo.NextDouble() * (Maxangle - Minangle));


                    double posX = Math.Cos((RandomAngle + 90) * Math.PI / 180) * 2;
                    double posY = Math.Sin((RandomAngle + 90) * Math.PI / 180) * 2;

                    double posZ = 0;//positionZ.NextDouble() * (1.6 - 0.7) + 0.7;
                    // posZ debe variar de 0.7-1.7 para estar a distancia del jugador 
                    //posX debe variar de 25-180 y negativo 


                    var vector = new Vector3((float)(LeftShoulder.transform.position.x - posX), (float)(LeftShoulder.transform.position.y - posY), (float)(LeftShoulder.transform.position.z - posZ));//force-


                    Temporary_Bullet_Handler = Instantiate(ParticlePunchLeft, vector, PlayerCenter.transform.rotation) as GameObject;
                    Temporary_Bullet_Handler.transform.parent = ParticlesParent.transform;
                    Destroy(Temporary_Bullet_Handler, spawnTime);
                    total = total + 1;
                    ModifyRepetitions();

                }
                else
                {
                    //RIGHT
                    double RandomAngle = -((Minangle + Angulo.NextDouble() * (Maxangle - Minangle)));


                    double posX = Math.Cos((RandomAngle + 90) * Math.PI / 180) * 2;
                    double posY = Math.Sin((RandomAngle + 90) * Math.PI / 180) * 2;

                    double posZ = 0;//positionZ.NextDouble() * (1.6 - 0.7) + 0.7;

                    // posZ debe variar de 0.7-1.7 para estar a distancia del jugador 
                    //posX debe variar de 25-180 y negativo 


                    var vector = new Vector3((float)(RightShoulder.transform.position.x - posX), (float)(RightShoulder.transform.position.y - posY), (float)(RightShoulder.transform.position.z - posZ));//force-


                    Temporary_Bullet_Handler = Instantiate(ParticlePunchRight, vector, PlayerCenter.transform.rotation) as GameObject;
                    Temporary_Bullet_Handler.transform.parent = ParticlesParent.transform;


                    Destroy(Temporary_Bullet_Handler, spawnTime);
                    total = total + 1;
                    ModifyRepetitions();

                }






            }
            else
            {
                System.Random positionZ = new System.Random();
                System.Random Angulo = new System.Random();
                System.Random handSwordSelection = new System.Random(DateTime.Now.Millisecond);


                int hand_selected = handSwordSelection.Next(0, 100);
                if (ShoulderSelection == 1)
                {
                    hand_selected = 70;
                }
                if (ShoulderSelection == 2)
                {
                    hand_selected = 30;
                }


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
                    Destroy(Temporary_Bullet_Handler, spawnTime);
                    total = total + 1;
                    ModifyRepetitions();

                }
                else
                {
                    //RIGHT SWORD
                    double RandomAngle = -((Minangle + Angulo.NextDouble() * (Maxangle - Minangle)));


                    double posX = Math.Cos((RandomAngle + 90) * Math.PI / 180) * 2;
                    double posY = Math.Sin((RandomAngle + 90) * Math.PI / 180) * 2;

                    double posZ = 4;

                    // posZ debe variar de 0.7-1.7 para estar a distancia del jugador 
                    //posX debe variar de 25-180 y negativo 


                    var vector = new Vector3((float)(PlayerCenter.transform.position.x - posX), (float)(PlayerCenter.transform.position.y - posY), (float)(PlayerCenter.transform.position.z - posZ));//force-


                    Temporary_Bullet_Handler = Instantiate(ParticleSwordRight, vector, PlayerCenter.transform.rotation) as GameObject;
                    Temporary_Bullet_Handler.transform.parent = ParticlesParent.transform;
                    Destroy(Temporary_Bullet_Handler, spawnTime);
                    total = total + 1;
                    ModifyRepetitions();

                }

            }
            
        }

        if (IsFlexion)
        {
            int selection = typeSelection.Next(0, 100);
            System.Random positionZ = new System.Random();
            System.Random Angulo = new System.Random();
            System.Random handSwordSelection = new System.Random();

            if (ShoulderSelection == 1)
            {
                selection = 70;
            }
            if (ShoulderSelection == 2)
            {
                selection = 30;
            }

            if (selection < 50)
            {

                //LEFT FLEXION
                double RandomAngle = (Minangle + Angulo.NextDouble() * (Maxangle - Minangle));


                double posX = Math.Cos(( 25+ 90) * Math.PI / 180) * 2;
                double posY = Math.Sin((RandomAngle + 90) * Math.PI / 180) * 2;

                double posZ = Math.Cos((-RandomAngle + 90) * Math.PI / 180) * 2;//positionZ.NextDouble() * (1.6 - 0.7) + 0.7;

                // posZ debe variar de 0.7-1.7 para estar a distancia del jugador 
                //posX debe variar de 25-180 y negativo 


                var vector = new Vector3((float)(LeftShoulder.transform.position.x), (float)(LeftShoulder.transform.position.y - posY), (float)(LeftShoulder.transform.position.z - posZ));//force-


                Temporary_Bullet_Handler = Instantiate(ParticlePunchLeft, vector, PlayerCenter.transform.rotation) as GameObject;
                Temporary_Bullet_Handler.transform.parent = ParticlesParent.transform;
                Destroy(Temporary_Bullet_Handler, spawnTime);
                total = total + 1;
                ModifyRepetitions();

            }
            else
            {

                //RIGHT  FLEXION
                double RandomAngle = ((Minangle + Angulo.NextDouble() * (Maxangle - Minangle)));


                double posX = Math.Cos((-25 + 90) * Math.PI / 180) * 2;
                double posY = Math.Sin((RandomAngle + 90) * Math.PI / 180) * 2;

                double posZ = Math.Cos((-RandomAngle + 90) * Math.PI / 180) * 2;//positionZ.NextDouble() * (1.6 - 0.7) + 0.7;

                // posZ debe variar de 0.7-1.7 para estar a distancia del jugador 
                //posX debe variar de 25-180 y negativo 


                var vector = new Vector3((float)(RightShoulder.transform.position.x ), (float)(RightShoulder.transform.position.y - posY), (float)(RightShoulder.transform.position.z - posZ));//force-


                Temporary_Bullet_Handler = Instantiate(ParticlePunchRight, vector, PlayerCenter.transform.rotation) as GameObject;
                Temporary_Bullet_Handler.transform.parent = ParticlesParent.transform;
                Destroy(Temporary_Bullet_Handler, spawnTime);
                total = total + 1;
                ModifyRepetitions();

            }


        }
        if (IsExtension)
        {

            int selection = typeSelection.Next(0, 100);
            System.Random positionZ = new System.Random();
            System.Random Angulo = new System.Random();
            System.Random handSelection = new System.Random();

            if (ShoulderSelection == 1)
            {
                selection = 70;
            }
            if (ShoulderSelection == 2)
            {
                selection = 30;
            }

            if (selection < 50)
            {

                //LEFT EXTENSION
                double RandomAngle = (Minangle + Angulo.NextDouble() * (Maxangle - Minangle));


                double posX = Math.Cos((25 + 90) * Math.PI / 180) * 2;
                double posY = Math.Sin((RandomAngle + 90) * Math.PI / 180) * 2;

                double posZ = Math.Cos((RandomAngle + 90) * Math.PI / 180) * 2; //Math.Sin(( + 90) * Math.PI / 180) * 2; ;//positionZ.NextDouble() * (1.6 - 0.7) + 0.7;

                // posZ debe variar de 0.7-1.7 para estar a distancia del jugador 
                //posX debe variar de 25-180 y negativo 


                var vector = new Vector3((float)(LeftShoulder.transform.position.x ), (float)(LeftShoulder.transform.position.y - posY), (float)(LeftShoulder.transform.position.z - posZ));//force-


                Temporary_Bullet_Handler = Instantiate(ParticlePunchLeft, vector, PlayerCenter.transform.rotation) as GameObject;
                Temporary_Bullet_Handler.transform.parent = ParticlesParent.transform;
                Destroy(Temporary_Bullet_Handler, spawnTime);
                total = total + 1;
                ModifyRepetitions();

            }
            else
            {

                //RIGHT  EXTENSION  
                double RandomAngle = ((Minangle + Angulo.NextDouble() * (Maxangle - Minangle)));


                double posX = Math.Cos((-25 + 90) * Math.PI / 180) * 2;
                double posY = Math.Sin((RandomAngle + 90) * Math.PI / 180) * 2;

                double posZ = Math.Cos((RandomAngle + 90) * Math.PI / 180) * 2;//Math.Sin(( + 90) * Math.PI / 180) * 2;//positionZ.NextDouble() * (1.6 - 0.7) + 0.7;

                // posZ debe variar de 0.7-1.7 para estar a distancia del jugador 
                //posX debe variar de 25-180 y negativo 


                var vector = new Vector3((float)(RightShoulder.transform.position.x), (float)(RightShoulder.transform.position.y - posY), (float)(RightShoulder.transform.position.z - posZ));//force-


                Temporary_Bullet_Handler = Instantiate(ParticlePunchRight, vector, PlayerCenter.transform.rotation) as GameObject;
                Temporary_Bullet_Handler.transform.parent = ParticlesParent.transform;
                Destroy(Temporary_Bullet_Handler, spawnTime);
                total = total + 1;
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



