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

	public GameObject danceUnityChan;
	public GameObject danceTaichi;
	public GameObject Camera;


    
    public GameObject ParametersPanel;
    public GameObject TutorialPanel;
    public GameObject MainPanel;
	public GameObject ResultPanel;

	public Button boton;

    public  bool InGame;
	public  bool GameOver;

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

	float shootTime=5.2f;

	float _time_game;

	float _repetitions;

	public Text textCurrentTime;
	public Slider sliderCurrentTime;
	public Text sliderText;
	public Dropdown numberRepetitions;
	public float currentRepetitions;
	public GameObject array_balls; 

	


	public float time_default {

		get
		{
			if (numberRepetitions.value == 0) {
				return _time_game;

			}
			else {
			
				return _repetitions; 

			}

		}
		set
		{

			if (numberRepetitions.value == 0) {
				
				_time_game = value;




				if (sliderText!=null) {
					sliderText.text = ( ((int)_time_game % 60).ToString("00") + ":" + ((int)_time_game / 60).ToString("00") + " mins");

				}
			}
			else {

				_repetitions = value;

				if (sliderText!=null) {
					sliderText.text = ("" +(int)_repetitions);

				}
			}

		}
		}




	float _velocity_game;
	public float velocity {

		get
		{
			return _velocity_game ;
		}
		set
		{
			_velocity_game = value;

			if (current_velocity!=null) {



				current_velocity.text = (((float)_velocity_game-20)/30 *100).ToString("0")+ "%" ;
		


			}
		}


	}


	public Slider slider_velocity;
	public Text current_velocity;



	public Slider slider_range;
	public Text current_range;

	public Slider sliderLeft;
	public Text angleLeft;

	public Slider sliderRight;
	public Text angleRight;

	public Slider sliderMinLeft;
	public Text angleMinLeft;

	public Slider sliderMinRight;
	public Text angleMinRight;

	public GameObject rightHandPraticles;
	public GameObject positionParticles;
	public int selectArm;

	float _range_game;

	float _angleMinRight;
	float _angleMinLeft;
	float radius;


	public float range {

		get
		{
			return _range_game ;
		}
		set
		{
			_range_game = value;

			if (current_range!=null) {

				current_range.text = ((int)_range_game).ToString("");


			}
		}


	}


	
	public float MinRight {

		get
		{
			return _angleMinRight ;
		}
		set
		{
			_angleMinRight = value;

			if (angleMinRight!=null) {

				angleMinRight.text = ((int)_angleMinRight).ToString("") + "°";


			}
		}


	}

	public float MinLeft {

		get
		{
			return _angleMinLeft;
		}
		set
		{
			_angleMinLeft = value;

			if (angleMinLeft!=null) {

				angleMinLeft.text = ((int)_angleMinLeft).ToString("") + "°";


			}
		}


	}



	float _angleRight;
	float _angleLeft;




	public float Right {

		get
		{
			return _angleRight ;
		}
		set
		{
			_angleRight = value;

			if (angleRight!=null) {

				angleRight.text = ((int)_angleRight).ToString("") + "°";


			}
		}


	}

	public float Left {

		get
		{
			return _angleLeft;
		}
		set
		{
			_angleLeft = value;

			if (angleLeft!=null) {

				angleLeft.text = ((int)_angleLeft).ToString("") + "°";


			}
		}


	}




	public Toggle toggleX;
	public Text movlattext;
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
	
   
	
	public Dropdown game_mode;

	public GameObject kinectPlayer;
	public bool left = false;
	public bool right = false;
	public void OnGameModeChanged()
	{
		

		if(game_mode.value == 1){
			
			if(right){
				kinectPlayer.transform.Rotate(0,-180,0);
				right = false;
				left = true;
			}else{
					kinectPlayer.transform.Rotate(0,-90,0);
					left = true;
			}

			sliderMinLeft.maxValue = 75;
			sliderLeft.maxValue = 75;
			current_range.text = "No Disponible";
			toggleX.enabled = false;
			toggleX.isOn = false;
			movlattext.text = "No Disponible";
			movimientoLateral = false;
			slider_range.enabled = false;

			
		}
		if(game_mode.value == 2){

			if(left){
				kinectPlayer.transform.Rotate(0,180,0);
				right = true;
				left = false;
			}else{
					kinectPlayer.transform.Rotate(0,90,0);
					right = true;
			}

			sliderMinLeft.maxValue = 75;
			sliderLeft.maxValue = 75;
			current_range.text = "No Disponible";
			toggleX.enabled = false;
			toggleX.isOn = false;
			movlattext.text = "No Disponible";
			movimientoLateral = false;
			slider_range.enabled = false;

		}
		if (game_mode.value == 0){
			
			if(right){
				kinectPlayer.transform.Rotate(0,-90,0);
				right = false;
			}
			if(left){
				kinectPlayer.transform.Rotate(0,90,0);
				left = false;
			}

			sliderMinLeft.maxValue = 180;
			sliderLeft.maxValue = 180;
			current_range.text = ((int)_range_game).ToString("");
			toggleX.enabled = true;
			toggleX.isOn = true;
			movlattext.text = "Movimiento Lateral";

			slider_range.enabled = true;


		}
	}

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
		ResultPanel.SetActive (false);
		movimientoLateral = false;
		force = 20;
		if(_angleMinLeft == 0){

			_angleMinLeft = 25;
		}
		if(_angleLeft == 0){

			_angleLeft = 25;
		}
		if(_range_game == 0){

			_range_game = 25;
		}
		toggleX.isOn = true;
		
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
		ResultPanel.SetActive (false);
		ParametersPanel.SetActive (true);
		movimientoLateral = false;
		force = 20;
		if(_angleMinLeft == 0){

			_angleMinLeft = 25;
		}
		if(_angleLeft == 0){

			_angleLeft = 25;
		}
		if(_range_game == 0){

			_range_game = 25;
		}
		results = ResultPanel.GetComponent<PutDataResults> ();
		lanzamiento = 0;
		toggleX.isOn = true;
		//results = FindObjectOfType<PutDataResults> ();
		//Button btn = boton.GetComponent<Button> ();
		//btn.onClick.AddListener(StartGame);
	}
	public void OnGameTypeChanged()
	{
		if (numberRepetitions.value == 0)
		{
			sliderCurrentTime.minValue = 1;
			sliderCurrentTime.maxValue = 30;

		}
		else
		{
			sliderCurrentTime.minValue = 1;
			sliderCurrentTime.maxValue = 30;

		}

		sliderCurrentTime.value = sliderCurrentTime.minValue;
		time_default = time_default;
	}

	public void OnGameTypeGameChanged()
	{
		
	}


	// Update is called once per frame
	void Update () {

		if (InGame)
		{

			if (numberRepetitions.value == 0) {

				currentTime -= Time.deltaTime;
				if (currentTime > 0 && numberRepetitions.value == 0) {
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

			if (numberRepetitions.value == 1) {

				textCurrentTime.text = currentRepetitions +" Restante";


			}




			Time.timeScale = 1;
			if (numberRepetitions.value == 1) {
				if (currentRepetitions <= 0 && array_balls.transform.childCount==0) {

					danceTaichi.SetActive (true);
					danceUnityChan.SetActive (true);

					faseFinal ();
				
				}
			} else {

				if (currentTime <= 0  && array_balls.transform.childCount==0) {
					danceTaichi.SetActive (true);
					danceUnityChan.SetActive (true);

					faseFinal ();

				}
			}
			if (Time.time > shootTime ) {

				InvokeRepeating ("Lanzar", 0f, 0f);
				shootTime = shootTime + rate;

			}


		} else {

			Time.timeScale = 0;
		}


		
	}
	public void EndGame()
	{
		
		MainPanel.SetActive (false);
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


	public void SlideTime(){
		if (numberRepetitions.value == 0) {

			time = sliderCurrentTime.value * 30f;

		}
		if (numberRepetitions.value == 1) {

		}


		UpdateSlide ();
	}

	public void UpdateSlide(){
		
		//Debug.Log(sliderCurrentTime.value);
	}


    public void TutorialPhase() {

        ParametersPanel.SetActive(false);
        TutorialPanel.SetActive(true);

    }

    public void EndTutorial() {
        ParametersPanel.SetActive(true);
        TutorialPanel.SetActive(false);
    }

	public void StartGame()
	{
		InGame = true;
		force = _velocity_game;
		_range_game = range ;

		movimientoLateral = toggleX.isOn;
		GameOver = false;
		MainPanel.SetActive (true);
		ParametersPanel.SetActive (false);


		if (numberRepetitions.value == 0) {

			currentTime = _time_game * 60;
			
		}
		if(numberRepetitions.value == 1){
		
			currentRepetitions = _repetitions;
		}

		if (force == 0) {
		
			force = 20;
		}

		if (numberRepetitions.value == 0 && currentTime == 0) {
		
			currentTime = 60;
			
			//currentRepetitions = 1;
		}  
		if (numberRepetitions.value == 1 && currentRepetitions == 0) {

			currentRepetitions = 1;
			//currentTime = 90000000000;
		}

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
		if (numberRepetitions.value == 0) {

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

        pitcher.Play("Throw");
        yield return new WaitForSeconds(2.7f);

        GameObject Temporary_Bullet_Handler;
		Temporary_Bullet_Handler = Instantiate (Ball, Cannon.transform.position, Cannon.transform.rotation) as GameObject;
		Temporary_Bullet_Handler.transform.parent = array_balls.transform;
		Temporary_Bullet_Handler.transform.Rotate (Vector3.left * 90);
		System.Random rany = new System.Random();
		System.Random ranx = new System.Random();

		//int pivy = rany.Next(80.0, 80.23);

		if(game_mode.value == 1){

			double posX = 0;
			double posY = 0;
			double posXpart = 0;
			double posYpart = 0;

			double angleRandom = 0;
			System.Random ranxx = new System.Random ();

			angleRandom = -(_angleMinLeft + ranxx.NextDouble () * (_angleLeft - _angleMinLeft));

			posX = Math.Cos ((angleRandom + 90) * Math.PI / 180) * 35;
			posY = Math.Sin ((angleRandom + 90) * Math.PI / 180) * 35;
			posXpart = Math.Cos ((angleRandom + 90) * Math.PI / 180)*((radius/10)+7);
			posYpart = Math.Sin ((angleRandom + 90) * Math.PI / 180) *((radius/10)+7);

			selectArm = 40;

			Destroy (Instantiate (rightHandPraticles, catcherRighthand.transform.position, Quaternion.identity), 2.0f);
			//virtual rehab revisar 
			particulas = Instantiate (positionParticles,new Vector3 ((float)(RealPlayerCenter.transform.position.x - posXpart), (float)(RealPlayerCenter.transform.position.y-posYpart), (float)RealPlayerCenter.transform.position.z), Quaternion.identity) as GameObject;
			//Destroy (Instantiate (positionParticles,new Vector3 ((float)(RealPlayerCenter.transform.position.x - posXpart), (float)(RealPlayerCenter.transform.position.y-posYpart), (float)RealPlayerCenter.transform.position.z), Quaternion.identity),4.0f);
			Destroy(particulas,4.0f);
			var vector = new Vector3 ((float)(PlayerCenter.transform.position.x - posX), (float)(PlayerCenter.transform.position.y - posY), (float)PlayerCenter.transform.position.z).normalized * force;//force

			Temporary_Bullet_Handler.GetComponent<Rigidbody> ().velocity = vector;

			lanzamiento = lanzamiento + 1;
			DecrementRepetitions ();
		}

		if(game_mode.value == 2){

			double posX = 0;
			double posY = 0;
			double posXpart = 0;
			double posYpart = 0;

			double angleRandom = 0;
			System.Random ranyy = new System.Random ();

			angleRandom = _angleMinLeft + ranyy.NextDouble () * (_angleLeft - _angleMinLeft);

			posX = Math.Cos ((angleRandom + 90) * Math.PI / 180) * 35;
			posY = Math.Sin ((angleRandom + 90) * Math.PI / 180) * 35;
			posXpart = Math.Cos ((angleRandom + 90) * Math.PI / 180)*((radius/10)+7);
			posYpart = Math.Sin ((angleRandom + 90) * Math.PI / 180) *((radius/10)+7);

			selectArm = 70;

			Destroy (Instantiate (rightHandPraticles, catcherLefthand.transform.position, Quaternion.identity), 2.0f);
			//virtual rehab revisar 
			particulas = Instantiate (positionParticles,new Vector3 ((float)(RealPlayerCenter.transform.position.x - posXpart), (float)(RealPlayerCenter.transform.position.y-posYpart), (float)RealPlayerCenter.transform.position.z), Quaternion.identity) as GameObject;
			//Destroy (Instantiate (positionParticles,new Vector3 ((float)(RealPlayerCenter.transform.position.x - posXpart), (float)(RealPlayerCenter.transform.position.y-posYpart), (float)RealPlayerCenter.transform.position.z), Quaternion.identity),4.0f);
			Destroy(particulas,4.0f);
			var vector = new Vector3 ((float)(PlayerCenter.transform.position.x - posX), (float)(PlayerCenter.transform.position.y - posY), (float)PlayerCenter.transform.position.z).normalized * force;//force

			Temporary_Bullet_Handler.GetComponent<Rigidbody> ().velocity = vector;

			lanzamiento = lanzamiento + 1;
			DecrementRepetitions ();

		}



		if (game_mode.value == 0) {
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
				pos = rand.Next (0, 3);
				 

			} else {
				pos = 2;
			}
			


	
			double angleRandom = 0;

			if (pos == 0) {// desplazamineot hacia la derecha

				System.Random ranyy = new System.Random ();

				angleRandom = _angleMinLeft + ranyy.NextDouble () * (_angleLeft - _angleMinLeft);

				posX = Math.Cos ((angleRandom + 90) * Math.PI / 180) * radius;
				posY = Math.Sin ((angleRandom + 90) * Math.PI / 180) * radius;
                posXpart = Math.Cos((angleRandom + 90) * Math.PI / 180) * (radius / 4);
                posYpart = Math.Sin((angleRandom + 90) * Math.PI / 180) * (radius / 4);

                selectArm = 70;
			
				Destroy (Instantiate (rightHandPraticles, catcherLefthand.transform.position, Quaternion.identity), 2.0f);
				//virtual rehab revisar 
				var vector = new Vector3 ((float)(PhantomRight.transform.position.x - posX), (float)(PhantomRight.transform.position.y - posY), (float)PhantomRight.transform.position.z).normalized * force;//force
				particulas = Instantiate (positionParticles,new Vector3 ((float)(RealPlayerLeft.transform.position.x - posXpart), (float)(RealPlayerLeft.transform.position.y-posYpart), (float)RealPlayerLeft.transform.position.z), Quaternion.identity) as GameObject;
				//Destroy (Instantiate (positionParticles,new Vector3 ((float)(RealPlayerCenter.transform.position.x - posXpart), (float)(RealPlayerCenter.transform.position.y-posYpart), (float)RealPlayerCenter.transform.position.z), Quaternion.identity),4.0f);
				Destroy(particulas,4.0f);
				Temporary_Bullet_Handler.GetComponent<Rigidbody> ().velocity = vector;
				lanzamiento = lanzamiento + 1;

			}
			if (pos == 1) {
				System.Random ranxx = new System.Random ();
				angleRandom = -(_angleMinLeft + ranxx.NextDouble () * (_angleLeft - _angleMinLeft));

				selectArm = 25;

				Destroy (Instantiate (rightHandPraticles, catcherRighthand.transform.position, Quaternion.identity), 2.0f);

				posX = Math.Cos ((angleRandom + 90) * Math.PI / 180) * radius;
				posY = Math.Sin ((angleRandom + 90) * Math.PI / 180) * radius;
                posXpart = Math.Cos((angleRandom + 90) * Math.PI / 180) * (radius / 4);
                posYpart = Math.Sin((angleRandom + 90) * Math.PI / 180) * (radius / 4);

                var vector = new Vector3 ((float)(PhantomLeft.transform.position.x - posX), (float)(PhantomLeft.transform.position.y - posY), (float)PhantomLeft.transform.position.z).normalized * force;//force
				particulas = Instantiate (positionParticles,new Vector3 ((float)(RealPlayerRight.transform.position.x - posXpart), (float)(RealPlayerRight.transform.position.y-posYpart), (float)RealPlayerRight.transform.position.z), Quaternion.identity) as GameObject;
				//Destroy (Instantiate (positionParticles,new Vector3 ((float)(RealPlayerCenter.transform.position.x - posXpart), (float)(RealPlayerCenter.transform.position.y-posYpart), (float)RealPlayerCenter.transform.position.z), Quaternion.identity),4.0f);
				Destroy(particulas,4.0f);
				Temporary_Bullet_Handler.GetComponent<Rigidbody> ().velocity = vector;
				lanzamiento = lanzamiento + 1;
			}
			if (pos == 2) {//tirar al centro

				System.Random ranyy = new System.Random ();
				System.Random ranxy = new System.Random ();
				System.Random ranz = new System.Random ();

				selectArm = ranz.Next (1, 100);
		


				if (selectArm <= 50) {

					//angleRandom = (115+ _angleMinRight) + ranyy.NextDouble ()*((_angleRight+115) - (115+_angleMinRight));

					angleRandom = -(_angleMinLeft + ranxy.NextDouble () * (_angleLeft - _angleMinLeft));
			

					Destroy (Instantiate (rightHandPraticles, catcherRighthand.transform.position, Quaternion.identity), 2.0f);
				

				}
				if (selectArm > 50) {

					angleRandom = _angleMinLeft + ranxy.NextDouble () * (_angleLeft - _angleMinLeft);

		
			
					Destroy (Instantiate (rightHandPraticles, catcherLefthand.transform.position, Quaternion.identity), 2.0f);

				


				}
				//angleRandom = 0;
				posX = Math.Cos ((angleRandom + 90) * Math.PI / 180) * radius;
				posY = Math.Sin ((angleRandom + 90) * Math.PI / 180) * radius;
				posXpart = Math.Cos ((angleRandom + 90) * Math.PI / 180)*(radius/4);
				posYpart = Math.Sin ((angleRandom + 90) * Math.PI / 180) *(radius/4);

				//Instantiate (rightHandPraticles, new Vector3 ((float) (RealPlayerCenter.transform.position.x-posX), (float) (RealPlayerCenter.transform.position.y-posY),(float) RealPlayerCenter.transform.position.z), Quaternion.identity), 2.0f);
				//Instantiate (rightHandPraticles, new Vector3 ((float) (RealPlayerCenter.transform.position.x-posX), (float) (RealPlayerCenter.transform.position.y-posY),(float) RealPlayerCenter.transform.position.z), Quaternion.identity);

				var vector = new Vector3 ((float)(PlayerCenter.transform.position.x - posX), (float)(PlayerCenter.transform.position.y - posY), (float)PlayerCenter.transform.position.z).normalized * force;//force

				particulas = Instantiate (positionParticles,new Vector3 ((float)(RealPlayerCenter.transform.position.x - posXpart), (float)(RealPlayerCenter.transform.position.y-posYpart), (float)RealPlayerCenter.transform.position.z), Quaternion.identity) as GameObject;
				//Destroy (Instantiate (positionParticles,new Vector3 ((float)(RealPlayerCenter.transform.position.x - posXpart), (float)(RealPlayerCenter.transform.position.y-posYpart), (float)RealPlayerCenter.transform.position.z), Quaternion.identity),4.0f);
				Destroy(particulas,4.0f);
				Temporary_Bullet_Handler.GetComponent<Rigidbody> ().velocity = vector;

			

			}

			lanzamiento = lanzamiento + 1;
			DecrementRepetitions ();

		}


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
