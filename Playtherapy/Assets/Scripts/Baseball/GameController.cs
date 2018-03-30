using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    GameObject particulas;
    public static GameController gc;
    Vector3 initialposition;

    GameObject target;
    //
    float numberRepetitions;
    float game_mode;
    float ArmSelection;
    public GameObject LeftShoulder;
    public GameObject RightShoulder;

    public GameObject danceUnityChan;
    public GameObject danceTaichi;
    public GameObject Camera;

    public Text textCurrentTime;
    public Slider sliderCurrentTime;
    public Text sliderText;


    public GameObject ParametersPanel;
    public GameObject TutorialPanel;
    public GameObject MainPanel;
    public GameObject ResultPanel;
    public GameObject pausa;

    public Button boton;

    public bool InGame;
    public bool GameOver;
    public bool progress;

    public GameObject Cannon;

    public GameObject Ball;

    public Animator pitcher;

    public GameObject test;

    public GameObject catcher;

    public GameObject PlayerCenter;
    public GameObject RealPlayerCenter;
    public GameObject RealPlayerLeft;
    public GameObject RealPlayerRight;

    public GameObject PhantomRight;
    public GameObject PhantomLeft;
    public GameObject catcherLefthand;
    public GameObject catcherRighthand;

    private RUISSkeletonManager skeletonManager;

    public float rate = 0f;

    float force = 50;

    float shootTime = 5.2f;

    float _time_game;

    float _repetitions;

    public float currentRepetitions;
    public GameObject array_balls;




    




    float _velocity_game;
    

    

    public GameObject rightHandPraticles;
    public GameObject positionParticles;
    public int selectArm;

    float _range_game;

    float _angleMinRight;
    float _angleMinLeft;
    float radius;


   



    float _angleRight;
    float _angleLeft;




    




    
    public bool movimientoLateral;

    private float time;


    private float currentTime;
    private float timeMillis;
    public float totalTime;

    PutDataResults results;
    public Text finalResult;
    public float lanzamiento;
    public Text total;

    public AudioClip catcher_sound;


    float maxTime = 2;

    public Text scoretext;

    private int score;

    public bool pivote;
    public bool send;



    

    public GameObject kinectPlayer;
    public bool left = false;
    public bool right = false;

    

	void Reset(){

		target = catcher;
	}

	void realRetry(){
		
		Reset ();
		Start ();
	}

	

	// Use this for initialization
	void Start () {
        RUISSkeletonController[] kinectPlayer1 = kinectPlayer.GetComponentsInChildren<RUISSkeletonController>();
        kinectPlayer1[0].updateRootPosition = true;

        initialposition = kinectPlayer.transform.position;

		gc = gameObject.GetComponent<GameController> ();


		if (skeletonManager == null)
		{
			skeletonManager = FindObjectOfType(typeof(RUISSkeletonManager)) as RUISSkeletonManager;
			if (!skeletonManager)
				Debug.Log("The scene is missing " + typeof(RUISSkeletonManager) + " script!");


		}

		danceTaichi.SetActive (false);
		danceUnityChan.SetActive (false);
		Camera.transform.position = new Vector3(35f, 31.83f,450.62f);

		//Camera.transform.position = new Vector3(0f, 31.83f,134.62f);



		score= 0;
		UpdateScore();
        InGame = false;
        MainPanel.SetActive(false);
        pausa.SetActive(false);
        ResultPanel.SetActive (false);
		movimientoLateral = false;
		force = 20;
        /*if(_angleMinLeft == 0){

			_angleMinLeft = 25;
		}
		if(_angleLeft == 0){

			_angleLeft = 25;
		}*/
		if(_range_game == 0){

			_range_game = 25;
		}
        movimientoLateral = true;
		
		results = ResultPanel.GetComponent<PutDataResults> ();

		//results = FindObjectOfType<PutDataResults> ();
		//Button btn = boton.GetComponent<Button> ();
		//btn.onClick.AddListener(StartGame);
		
		//sliderCurrentTime.onValueChanged.AddListener(delegate {SlideTime(); });
    }

	public void retry(){
	
		gc = gameObject.GetComponent<GameController> ();
        RUISSkeletonController[] kinectPlayer1 = kinectPlayer.GetComponentsInChildren<RUISSkeletonController>();
        kinectPlayer1[0].updateRootPosition = true;

        if (skeletonManager == null)
		{
			skeletonManager = FindObjectOfType(typeof(RUISSkeletonManager)) as RUISSkeletonManager;
			if (!skeletonManager)
				Debug.Log("The scene is missing " + typeof(RUISSkeletonManager) + " script!");
		}
		danceTaichi.SetActive (false);
		danceUnityChan.SetActive (false);
		Camera.transform.position = new Vector3(35f, 31.83f,450.62f);
		//Camera.transform.Translate(0f, 0.83f,0 - 10.62f);
		
		score= 0;
		UpdateScore();
		InGame = false;
		MainPanel.SetActive(false);
        pausa.SetActive(false);
        ResultPanel.SetActive (false);
		ParametersPanel.SetActive (true);
		movimientoLateral = false;
		force = 20;
        /*if(_angleMinLeft == 0){

			_angleMinLeft = 25;
		}
		if(_angleLeft == 0){

			_angleLeft = 25;
		}*/
		if(_range_game == 0){

			_range_game = 25;
		}
        results = ResultPanel.GetComponent<PutDataResults> ();
		lanzamiento = 0;
		movimientoLateral = true;
		//results = FindObjectOfType<PutDataResults> ();
		//Button btn = boton.GetComponent<Button> ();
		//btn.onClick.AddListener(StartGame);
	}






	

	// Update is called once per frame
	void Update () {

		if (InGame)
		{

			if (numberRepetitions == 0) {

				currentTime -= Time.deltaTime;
				if (currentTime > 0 && numberRepetitions == 0) {
					timeMillis -= Time.deltaTime * 1000;
					if (timeMillis < 0)
						timeMillis = 1000f;
					textCurrentTime.text = (((int)currentTime) / 60).ToString ("00") + ":"
						+ (((int)currentTime) % 60).ToString ("00") + ":"
						+ ((int)(timeMillis * 60 / 1000)).ToString ("00");
					sliderCurrentTime.value = currentTime * 100 / totalTime;

				
				} else {


					textCurrentTime.text = "00:00:00";
				}
			}

			if (numberRepetitions == 1) {

				textCurrentTime.text = currentRepetitions +" Restante";


			}




			Time.timeScale = 1;
			if (numberRepetitions == 1) {
				if (currentRepetitions <= 0 && array_balls.transform.childCount==0 && !progress) {

					danceTaichi.SetActive (true);
					danceUnityChan.SetActive (true);
                    GameOver = true;

					faseFinal ();
                    
				
				}
			} else {

				if (currentTime <= 0  && array_balls.transform.childCount==0 && !progress) {
					danceTaichi.SetActive (true);
					danceUnityChan.SetActive (true);
                    GameOver = true;
                    faseFinal ();
                   

				}
			}
			if (Time.time > shootTime  && !GameOver && !progress) {

                if (array_balls.transform.childCount == 0) {
                    InvokeRepeating("Lanzar", 0f, 0f);
                }
				
				shootTime = shootTime + rate;

			}


             
                


            } else {

			Time.timeScale = 0;
		}


		
	}
	public void EndGame()
	{
        /*_angleMinLeft = 0;
        sliderMinLeft.value = 0;
        sliderLeft.value = 0;
        _angleLeft = 0;*/
        
		MainPanel.SetActive (false);
        pausa.SetActive(false);
        ResultPanel.SetActive (true);
		danceTaichi.SetActive (false);
		danceUnityChan.SetActive (false);

		StopAllCoroutines();
		InGame = false;
		int result = Mathf.RoundToInt ((score / lanzamiento) * 100);
		movimientoLateral = false;
		results = ResultPanel.GetComponent<PutDataResults> ();
		results.updateData (result, 0);
	






	}

	void faseFinal(){
		Camera.transform.position = new Vector3(53f, 70f,30.62f);
		movimientoLateral = false;
		RUISSkeletonController [] kinectPlayer1 = kinectPlayer.GetComponentsInChildren<RUISSkeletonController> ();
		kinectPlayer1[0].updateRootPosition = movimientoLateral;

		StartCoroutine (animacionFinal());
	}

	IEnumerator animacionFinal(){
		

		yield return new WaitForSeconds(5f);


		EndGame ();

	}


	

	public void UpdateSlide(){
		
		//Debug.Log(sliderCurrentTime.value);
	}


    public void TutorialPhase() {

        ParametersPanel.SetActive(false);
        TutorialPanel.SetActive(true);

    }

    public void PauseOn()
    {
        InGame = false;
        

    }

    public void StartAgain() {

        pausa.SetActive(false);

     

        retry();
    }
    public void PauseOff() {

        InGame = true;

    }


    public void EndTutorial() {
        ParametersPanel.SetActive(true);
        TutorialPanel.SetActive(false);
    }

	public void StartGame(float Velocity, float rangeParam, bool lateralmovement, float numberrepetitions,float timegame,float repetitions, float forces ,float anglemin,float anglemax,float gamemode,float armselection)
	{
		InGame = true;
        GameOver = false;
		force = Velocity;
		_range_game = rangeParam;
        game_mode = gamemode;
        ArmSelection = armselection;

		movimientoLateral = lateralmovement;
		GameOver = false;
		MainPanel.SetActive (true);
        pausa.SetActive(true);
        ParametersPanel.SetActive (false);
        numberRepetitions = numberrepetitions;


		if (numberrepetitions == 0) {

			currentTime = timegame * 60;
			
		}
		if(numberrepetitions == 1){
		
			currentRepetitions = repetitions;
            
		}

		if (forces == 0) {
		
			force = 20;
		}
        //if (numberRepetitions.value == 0 
        if (numberrepetitions == 0 && currentTime == 0) {
		
			currentTime = 60;
			
			//currentRepetitions = 1;
		}  
		if (numberrepetitions == 1 && currentRepetitions == 0) {

			currentRepetitions = 1;
			//currentTime = 90000000000;
		}

        _angleMinLeft = anglemin;

        _angleLeft = anglemax;




        if (_angleMinLeft > _angleLeft) {

			_angleLeft = _angleMinLeft + 1;
		}


		RUISSkeletonController [] kinectPlayer1 = kinectPlayer.GetComponentsInChildren<RUISSkeletonController> ();
		kinectPlayer1[0].updateRootPosition = movimientoLateral;



	}

    void Lanzar()
    {


		if (InGame) {
			
		}
		if (numberRepetitions == 0) {

			if (currentTime > 0) {
				StartCoroutine (Disparo ());
			}
		} 
		else 
		{
			
			if (currentRepetitions>0) {
				
				StartCoroutine(Disparo());

			}
		}



        
    }

	IEnumerator Disparo(){
        progress = true;
        pitcher.Play("Throw");
        yield return new WaitForSeconds(2.7f);

        GameObject Temporary_Bullet_Handler;
		Temporary_Bullet_Handler = Instantiate (Ball, Cannon.transform.position, Cannon.transform.rotation) as GameObject;
		Temporary_Bullet_Handler.transform.parent = array_balls.transform;
		Temporary_Bullet_Handler.transform.Rotate (Vector3.left * 90);
		System.Random rany = new System.Random();
		System.Random ranx = new System.Random();
        

        //int pivy = rany.Next(80.0, 80.23);

        if (game_mode == 1){



			double posX = 0;
			double posY = 0;
            double posZ = 0;
            double posXpart = 0;
			double posYpart = 0;
            double posZpart = 0;
            double angleRandom = 0;
            System.Random ranz = new System.Random();
            System.Random ranxy = new System.Random();
            System.Random ranxx = new System.Random();
            angleRandom = (_angleMinLeft + ranxx.NextDouble () * (_angleLeft - _angleMinLeft));

            selectArm = ranz.Next(1, 100);

            if (ArmSelection == 1)
            {

                //Derecho
                selectArm = 30;

            }
            if (ArmSelection == 2)
            {
                //Izquierdo
                selectArm = 80;

            }

            if (selectArm <= 50)
            {

                //angleRandom = (115+ _angleMinRight) + ranyy.NextDouble ()*((_angleRight+115) - (115+_angleMinRight));






                //angleRandom = -(_angleMinLeft + ranxy.NextDouble() * (_angleLeft - _angleMinLeft));


                Destroy(Instantiate(rightHandPraticles, catcherRighthand.transform.position, Quaternion.identity), 2.0f);
                posX = Math.Cos((angleRandom + 90) * Math.PI / 180) * 35;
                posY = Math.Sin((angleRandom + 90) * Math.PI / 180) * 35;
                posXpart = Math.Cos((angleRandom + 90) * Math.PI / 180) * ((radius / 10) + 7);
                posYpart = Math.Sin((angleRandom + 90) * Math.PI / 180) * ((radius / 10) + 7);

                var vectortest = new Vector3((float)RightShoulder.transform.position.x, (float)(RightShoulder.transform.position.y - posYpart), (float)(RightShoulder.transform.position.z -posX ));
                var vector = new Vector3((float)(vectortest.x - Cannon.transform.position.x), (float)(vectortest.y - Cannon.transform.position.y), (float)(vectortest.z - Cannon.transform.position.z)).normalized;//force

                particulas = Instantiate(positionParticles, new Vector3((float)RightShoulder.transform.position.x, (float)(RightShoulder.transform.position.y - posYpart), (float)(RightShoulder.transform.position.z - posXpart)), Quaternion.identity) as GameObject;
                //Destroy (Instantiate (positionParticles,new Vector3 ((float)(RealPlayerCenter.transform.position.x - posXpart), (float)(RealPlayerCenter.transform.position.y-posYpart), (float)RealPlayerCenter.transform.position.z), Quaternion.identity),4.0f);

                Temporary_Bullet_Handler.GetComponent<Rigidbody>().velocity = vector * force;
                Destroy(particulas, 4);






            }
            else
            {






                Destroy(Instantiate(rightHandPraticles, catcherLefthand.transform.position, Quaternion.identity), 2.0f);
                posX = Math.Cos((angleRandom + 90) * Math.PI / 180) * 35;
                posY = Math.Sin((angleRandom + 90) * Math.PI / 180) * 35;
                posXpart = Math.Cos((angleRandom + 90) * Math.PI / 180) * ((radius / 10) + 7);
                posYpart = Math.Sin((angleRandom + 90) * Math.PI / 180) *  ((radius / 10) + 7);

                var vectortest = new Vector3((float)LeftShoulder.transform.position.x, (float)(LeftShoulder.transform.position.y - posYpart), (float)(LeftShoulder.transform.position.z-posX));
                var vector = new Vector3((float)(vectortest.x - Cannon.transform.position.x), (float)(vectortest.y - Cannon.transform.position.y), (float)(vectortest.z - Cannon.transform.position.z)).normalized;//force

                particulas = Instantiate(positionParticles, new Vector3((float)LeftShoulder.transform.position.x, (float)(LeftShoulder.transform.position.y - posYpart), (float)(LeftShoulder.transform.position.z- posXpart)), Quaternion.identity) as GameObject;
                //Destroy (Instantiate (positionParticles,new Vector3 ((float)(RealPlayerCenter.transform.position.x - posXpart), (float)(RealPlayerCenter.transform.position.y-posYpart), (float)RealPlayerCenter.transform.position.z), Quaternion.identity),4.0f);

                Temporary_Bullet_Handler.GetComponent<Rigidbody>().velocity = vector * force;
                Destroy(particulas, 4);

            }

            
			

			

			

            

			lanzamiento = lanzamiento + 1;
			DecrementRepetitions ();
		}

		



		if (game_mode == 0) {
			radius = _range_game;

			double posX = 0;
			double posY = 0;
			double posXpart = 0;
			double posYpart = 0;
 			float posZ = 0;

			System.Random sel = new System.Random ();

			int pos = 0;

			if (movimientoLateral) {
				System.Random rand = new System.Random ();

                pos = rand.Next(0, 2);

                if (ArmSelection == 2)
                {

                    //Derecho
                    pos = 0;
                    selectArm = 30;

                }
                if (ArmSelection == 1)
                {
                    //Izquierdo
                    pos = 1;
                    selectArm = 80;

                }
                


            } else {
				pos = 2;
			}
			


	
			double angleRandom = 0;

			if (pos == 0) {
                selectArm = 30;

                System.Random ranyy = new System.Random ();

				angleRandom = _angleMinLeft + ranyy.NextDouble () * (_angleLeft - _angleMinLeft);

                posX = Math.Cos((angleRandom + 90) * Math.PI / 180) * radius;
                posY = Math.Sin((angleRandom + 90) * Math.PI / 180) * radius;
                posXpart = Math.Cos((angleRandom + 90) * Math.PI / 180) * (radius / 4);
                posYpart = Math.Sin((angleRandom + 90) * Math.PI / 180) * (radius / 4);
                particulas = Instantiate(positionParticles, new Vector3((float)(RealPlayerLeft.transform.position.x - posXpart), (float)(RealPlayerLeft.transform.position.y - posYpart), (float)RealPlayerLeft.transform.position.z), Quaternion.identity) as GameObject;
                selectArm = 70;
                Destroy (Instantiate (rightHandPraticles, catcherLefthand.transform.position, Quaternion.identity), 2.0f);
                //virtual rehab revisar 
                var vectortest = new Vector3((float)(RealPlayerLeft.transform.position.x - posXpart), (float)(RealPlayerLeft.transform.position.y - posYpart), (float)RealPlayerLeft.transform.position.z);
                var vector = new Vector3((float)(vectortest.x - Cannon.transform.position.x), (float)(vectortest.y - Cannon.transform.position.y), (float)(RealPlayerRight.transform.position.z - Cannon.transform.position.z)).normalized;//force

                
				//Destroy (Instantiate (positionParticles,new Vector3 ((float)(RealPlayerCenter.transform.position.x - posXpart), (float)(RealPlayerCenter.transform.position.y-posYpart), (float)RealPlayerCenter.transform.position.z), Quaternion.identity),4.0f);
				Destroy(particulas,4.0f);
				Temporary_Bullet_Handler.GetComponent<Rigidbody> ().velocity = vector * force;
				lanzamiento = lanzamiento + 1;

			}
            if (pos == 1) {//desplaz
                selectArm = 80;
                System.Random ranxx = new System.Random();
                angleRandom = -(_angleMinLeft + ranxx.NextDouble() * (_angleLeft - _angleMinLeft));

                selectArm = 25;

                Destroy(Instantiate(rightHandPraticles, catcherRighthand.transform.position, Quaternion.identity), 2.0f);

                posX = Math.Cos((angleRandom + 90) * Math.PI / 180) * radius;
                posY = Math.Sin((angleRandom + 90) * Math.PI / 180) * radius;
                posXpart = Math.Cos((angleRandom + 90) * Math.PI / 180) * (radius / 4);
                posYpart = Math.Sin((angleRandom + 90) * Math.PI / 180) * (radius / 4);
                particulas = Instantiate(positionParticles, new Vector3((float)(RealPlayerRight.transform.position.x -posXpart), (float)(RealPlayerRight.transform.position.y - posYpart), (float)RealPlayerRight.transform.position.z), Quaternion.identity) as GameObject;

                //var vectortest = new Vector3((float)(RightShoulder.transform.position.x ), (float)(RightShoulder.transform.position.y ), (float)RightShoulder.transform.position.z);
               // var vector = new Vector3((float)(vectortest.x - Cannon.transform.position.x), (float)(vectortest.y - Cannon.transform.position.y), (float)vectortest.z).normalized * force;//force

                var vectortest = new Vector3((float)(RealPlayerRight.transform.position.x - posXpart), (float)(RealPlayerRight.transform.position.y -posYpart ), (float)RealPlayerRight.transform.position.z);
                var vector = new Vector3((float)(vectortest.x - Cannon.transform.position.x), (float)(vectortest.y - Cannon.transform.position.y), (float)(RealPlayerRight.transform.position.z - Cannon.transform.position.z)).normalized;//force

                //Destroy (Instantiate (positionParticles,new Vector3 ((float)(RealPlayerCenter.transform.position.x - posXpart), (float)(RealPlayerCenter.transform.position.y-posYpart), (float)RealPlayerCenter.transform.position.z), Quaternion.identity),4.0f);
                Destroy(particulas,4.0f);
				Temporary_Bullet_Handler.GetComponent<Rigidbody> ().velocity = vector *force;
				lanzamiento = lanzamiento + 1;
			}
			if (pos == 2) {//tirar al centro
                System.Random selection = new System.Random ();
				System.Random ranyy = new System.Random ();
				System.Random ranxy = new System.Random ();
				System.Random ranz = new System.Random ();

				int select = ranz.Next (1, 100);


                if (ArmSelection == 1) {

                    //Derecho
                    select = 30;
                    selectArm = 30;

                }
                if (ArmSelection  == 2) {
                    //Izquierdo
                    select = 80;
                    selectArm = 80;

                }


                if (select <= 50) {

                    //angleRandom = (115+ _angleMinRight) + ranyy.NextDouble ()*((_angleRight+115) - (115+_angleMinRight));


                    selectArm = select;



                    angleRandom = -(_angleMinLeft + ranxy.NextDouble() * (_angleLeft - _angleMinLeft));


                    Destroy(Instantiate(rightHandPraticles, catcherRighthand.transform.position, Quaternion.identity), 2.0f);
                    posX = Math.Cos((angleRandom + 90) * Math.PI / 180) * radius;
                    posY = Math.Sin((angleRandom + 90) * Math.PI / 180) * radius;
                    posXpart = Math.Cos((angleRandom + 90) * Math.PI / 180) * (radius / 4);
                    posYpart = Math.Sin((angleRandom + 90) * Math.PI / 180) * (radius / 4);
                    var vectortest = new Vector3((float)(RightShoulder.transform.position.x - posXpart), (float)(RightShoulder.transform.position.y - posYpart), (float)RightShoulder.transform.position.z);
                    var vector = new Vector3((float)(vectortest.x - Cannon.transform.position.x), (float)(vectortest.y - Cannon.transform.position.y), (float)(RightShoulder.transform.position.z - Cannon.transform.position.z)).normalized;//force

                    particulas = Instantiate(positionParticles, new Vector3((float)(RightShoulder.transform.position.x - posXpart), (float)(RightShoulder.transform.position.y - posYpart), (float)RightShoulder.transform.position.z), Quaternion.identity) as GameObject;
                    //Destroy (Instantiate (positionParticles,new Vector3 ((float)(RealPlayerCenter.transform.position.x - posXpart), (float)(RealPlayerCenter.transform.position.y-posYpart), (float)RealPlayerCenter.transform.position.z), Quaternion.identity),4.0f);

                    Temporary_Bullet_Handler.GetComponent<Rigidbody>().velocity = vector * force;
                    Destroy(particulas, 4);


                }
                else {

                    selectArm = select;

                    angleRandom = (_angleMinLeft + ranxy.NextDouble() * (_angleLeft - _angleMinLeft));



                    Destroy(Instantiate(rightHandPraticles, catcherLefthand.transform.position, Quaternion.identity), 2.0f);


                    posX = Math.Cos((angleRandom + 90) * Math.PI / 180) * radius;
                    posY = Math.Sin((angleRandom + 90) * Math.PI / 180) * radius;
                    posXpart = Math.Cos((angleRandom + 90) * Math.PI / 180) * (radius / 4);
                    posYpart = Math.Sin((angleRandom + 90) * Math.PI / 180) * (radius / 4);
                    //particulas = Instantiate(positionParticles, new Vector3((float)(LeftShoulder.transform.position.x - posXpart), (float)(LeftShoulder.transform.position.y - posYpart), (float)LeftShoulder.transform.position.z), Quaternion.identity) as gameObject;
                    var vectortest = new Vector3((float)(LeftShoulder.transform.position.x - posXpart), (float)(LeftShoulder.transform.position.y - posYpart), (float)LeftShoulder.transform.position.z);
                    var vector = new Vector3((float)(vectortest.x - Cannon.transform.position.x), (float)(vectortest.y - Cannon.transform.position.y), (float)(LeftShoulder.transform.position.z - Cannon.transform.position.z)).normalized;//force

                    particulas = Instantiate(positionParticles, new Vector3((float)(LeftShoulder.transform.position.x - posXpart), (float)(LeftShoulder.transform.position.y - posYpart), (float)LeftShoulder.transform.position.z), Quaternion.identity) as GameObject;
                    //Destroy (Instantiate (positionParticles,new Vector3 ((float)(RealPlayerCenter.transform.position.x - posXpart), (float)(RealPlayerCenter.transform.position.y-posYpart), (float)RealPlayerCenter.transform.position.z), Quaternion.identity),4.0f);
                    
                    Temporary_Bullet_Handler.GetComponent<Rigidbody>().velocity = vector * force;
                    Destroy(particulas, 4);
                    
                }
				//angleRandom = 0;
				

				//Instantiate (rightHandPraticles, new Vector3 ((float) (RealPlayerCenter.transform.position.x-posX), (float) (RealPlayerCenter.transform.position.y-posY),(float) RealPlayerCenter.transform.position.z), Quaternion.identity), 2.0f);
				//Instantiate (rightHandPraticles, new Vector3 ((float) (RealPlayerCenter.transform.position.x-posX), (float) (RealPlayerCenter.transform.position.y-posY),(float) RealPlayerCenter.transform.position.z), Quaternion.identity);

				/*var vector = new Vector3 ((float)(PlayerCenter.transform.position.x - posX), (float)(PlayerCenter.transform.position.y - posY), (float)PlayerCenter.transform.position.z).normalized * force;//force

				particulas = Instantiate (positionParticles,new Vector3 ((float)(RealPlayerCenter.transform.position.x - posXpart), (float)(RealPlayerCenter.transform.position.y-posYpart), (float)RealPlayerCenter.transform.position.z), Quaternion.identity) as GameObject;
				//Destroy (Instantiate (positionParticles,new Vector3 ((float)(RealPlayerCenter.transform.position.x - posXpart), (float)(RealPlayerCenter.transform.position.y-posYpart), (float)RealPlayerCenter.transform.position.z), Quaternion.identity),4.0f);
				Destroy(particulas,4.0f);
				Temporary_Bullet_Handler.GetComponent<Rigidbody> ().velocity = vector;*/

			

			}
            
            lanzamiento = lanzamiento + 1;
			DecrementRepetitions ();

		}

        progress = false;

	}


	public void AddScore(int newscore)
	{
        score += newscore;
		UpdateScore();
	}
	void UpdateScore() {

        scoretext.text = ""+ score;
	}

	public void DecrementRepetitions(){
	

			currentRepetitions = currentRepetitions - 1; 

	}

}
