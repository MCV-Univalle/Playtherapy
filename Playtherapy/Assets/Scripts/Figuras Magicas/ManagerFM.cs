using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// for your own scripts make sure to add the following line:
using DigitalRuby.Tween;
public class ManagerFM : MonoBehaviour {



    public static ManagerFM gm;
	public List<int> list_gestures_index;
	public List<Toggle> list_gestures_used;

	int _modo_juego;
	float jugabilidad_number;
	float _timeBetweenEnemies;
	float _percentFigureMin;

	//para la pantalla de parametros
	Text time_enemies;
	Text txt_jugabilidad;
	Text txt_scaleMin;
	Button bt_play;


	//canvas usados
	GameObject results_canvas;
	GameObject parameters_canvas;
	GameObject tutorial_canvas;
	List<GameObject> tutorial_pages_array;
	GameObject tutorial_page_info;
	Slider timeSlider;

	float timer_game=-1;
	float timeMillis;
	int tutorial_page=0;
	Text txt_time;

	bool game_over;
	bool hasStart;
	//objetos del juego:
	SpawnnerFM spawnnerEnemies;
	GameObject gestureManager;
	ScoreHandlerFM score_script;
	PutDataResults results_script;
	public GesturesShapeManager managerShapes;

	bool _change_togle=true;
	public bool changeAnyToggle{

		get{return _change_togle; }
		set{ 
			_change_togle = value;
			if (bt_play!=null) {
				bt_play.interactable = atLeastATooggleActive () && _modo_juego!=0;
			}


		}

	}

	void Awake()
	{
		managerShapes = FindObjectOfType<GesturesShapeManager> ();
		results_script=FindObjectOfType<PutDataResults> ();
		score_script=FindObjectOfType<ScoreHandlerFM> ();
		txt_scaleMin= GameObject.Find ("txt_scaleMinFM").GetComponent<Text>();
		time_enemies = GameObject.Find ("txt_enemies_timeFM").GetComponent<Text>();
		txt_jugabilidad = GameObject.Find ("txt_jugabilidadFM").GetComponent<Text>();
		bt_play =  GameObject.Find ("bt_playFM").GetComponent<Button>();
		results_canvas = GameObject.Find ("results_canvas");
		parameters_canvas = GameObject.Find ("parameters_canvasFM");
		tutorial_canvas = GameObject.Find ("tutorial_canvas");
		timeSlider = GameObject.Find ("slideTimeUI").GetComponent<Slider>();
		spawnnerEnemies = GameObject.Find ("Spawnner").GetComponent<SpawnnerFM>();
		txt_time = GameObject.Find ("txt_timer").GetComponent<Text> ();


		results_canvas.transform.localScale = Vector3.zero;
		tutorial_canvas.transform.localScale = Vector3.zero;
		tutorial_pages_array = new List<GameObject> ();



		int contador=0;


		do {


			contador++;
			tutorial_page_info = GameObject.Find("tutorial_page"+contador);

			if (tutorial_page_info!=null) {
				tutorial_pages_array.Add(tutorial_page_info);
				tutorial_page_info.SetActive (false);
			}

		} while (tutorial_page_info!=null);


		jugabilidad_number = 1;
		_timeBetweenEnemies = 1;





	}


	void Start () {
        if (gm == null)
        {
            gm = this;
        }
        gestureManager = GameObject.Find ("GesturesManager");
		gestureManager.SetActive (false);

        TweenShowParameters ();
       

	}




	bool atLeastATooggleActive()
	{
		int contadorToggleActivos = 0;

		foreach (var item in list_gestures_used) {
			if (item.isOn) {
				contadorToggleActivos++;
			}
		}

		return contadorToggleActivos > 0;

	}

	void Update () {
		if (hasStart == true) 
		{
			if (game_over==false) 
			{
				if (modo_juego == 2)
				{
					if (timer_game > 0) {

						timer_game -= Time.deltaTime;

						timeSlider.value = (timer_game / (select_jugabilidad * 60)) * 100;


					} 
					else 
					{
						timer_game = 0;
						timeSlider.value = 0;
						spawnnerEnemies.reset ();

						game_over = true;
						TweenFinishGame();


					}
					updateTimeText ();


				} 
				else 
				{
					
					if (spawnnerEnemies.enemiesRemainingToSpawn==0  &&spawnnerEnemies.enemiesRemainingAlive==0 ) 
					{
						game_over = true;
						//print ("termino gameplay");
						TweenFinishGame();

					}
				}
			}
		}
	}
	void updateTimeText()
	{

		if (timer_game > 0)
		{

			timeMillis -= Time.deltaTime*1000;
			if (timeMillis<0) {
				timeMillis = 1000f;
			}

			txt_time.text = (((int)timer_game) / 60).ToString("00") + ":"
				+ (((int)timer_game) % 60).ToString("00") + ":"
				+ ((int)(timeMillis * 60 / 1000)).ToString("00");




		} else {
			txt_time.text= "00:00:00";
		}


	}
	public void StartGame()
	{
        /*
		list_gestures_index= new List<int>();

		for (int i = 0; i < list_gestures_used.Count; i++) {

			if (list_gestures_used[i].isOn) {
				list_gestures_index.Add (i);
			}
		}
      
		
        */
        spawnnerEnemies.gestures_index_used = list_gestures_index;

        timer_game = -1;
		score_script.reset ();


		TweenSmallPlayer ();
		//si el modo de juego es por tiempo
		if (modo_juego==2) {
			timer_game = select_jugabilidad * 60;

		}


		TweenHideParameters ();
	}

	public void startGame(int mj= 1,float jugabilidad=3,float time_enemies=3)
	{
		
		modo_juego = mj;
		select_jugabilidad = jugabilidad;
		timeBetweenEnemies = time_enemies;

		StartGame ();

	}


	public void EndGame()
	{
		saveData ();

		int performance_game = Mathf.RoundToInt (((float)score_script.score_obtain / (float)score_script.score_max) * 100);
		int performance_loaded_BD = 0;
		results_script.updateData (performance_game, performance_loaded_BD);
		gestureManager.SetActive(false);
		spawnnerEnemies.can_spawn = false;
		hasStart = false;
		//paramenters_canvas.SetActive (true);
		FinalAnimation ();

	}
	public void RetryGame()
	{
		TweenHideResults ();
		TweenShowParameters ();



	}
	private void CloseTutorial()
	{
		TweenHideTutorial ();
		TweenShowParameters ();


	}
	public void OpenTutorial()
	{

		tutorial_page = 0;
		putPageTutorial ();
		TweenShowTutorial ();
		TweenHideParameters ();
	}
	public void putPageTutorial()
	{

		foreach (GameObject obj in tutorial_pages_array)
		{
			obj.SetActive (false);
		}


		if (tutorial_page < tutorial_pages_array.Count) {
			tutorial_page_info = tutorial_pages_array [tutorial_page];
			tutorial_page_info.SetActive (true);

		} else {
			CloseTutorial ();
		}
		tutorial_page++;

	}
	private void TweenShowTutorial()
	{
		tutorial_canvas.transform.localScale = Vector3.zero;
		this.gameObject.Tween("ShowTutorial", Vector3.zero, Vector3.one, 0.75f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
			{
				// progress
				tutorial_canvas.transform.localScale =t.CurrentValue;

			}, (t) =>
			{
				//complete
			});



	}
	private void TweenHideTutorial()
	{
		tutorial_canvas.transform.localScale = Vector3.one;
		this.gameObject.Tween("HideTutorial", Vector3.one, Vector3.zero, 0.75f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
			{
				// progress
				tutorial_canvas.transform.localScale =t.CurrentValue;

			}, (t) =>
			{
				//complete
			});



	}
	private void saveData()
	{
		GameObject tre = GameObject.Find ("TherapySession");

		if (tre!=null)
		{
			TherapySessionObject objTherapy = tre.GetComponent<TherapySessionObject> ();

			if (objTherapy!=null) 
			{

				objTherapy.fillLastSession(score_script.score_obtain, score_script.score_max, (int)0, "1");
				objTherapy.saveLastGameSession ();

				//objTherapy.savePerformance((int)HoldParametersGreatJourney.best_angle_left, "14");



			}
		}




		//		TherapySessionObject objTherapy = GameObject.Find ("TherapySession").GetComponent<TherapySessionObject> ();
		//		objTherapy.fillLastSession(score, currentReps, (int)totalTime, level.ToString());
		//		objTherapy.saveLastGameSession ();
		//
		//		objTherapy.savePerformance((int)bestTotalLeftShoulderAngle, "4");
		//		objTherapy.savePerformance((int)bestTotalRightShoulderAngle, "5");

	}
	private void FinalAnimation()
	{
		GameObject player = GameObject.FindWithTag ("Player");

		player.transform.position = new Vector3 (0, 0, 70);
		TweenBigPlayer ();
//		txt_rubies.text = "x" + score_script.rubies_caught;
//		txt_dodge.text = "x" + score_script.airplanes_dodge;
//		TweenShowCountObjets ();
//		TweenRotatePlane ();
//		TweenRotateCamera ();

	}
	private void TweenSmallPlayer()
	{
		GameObject player = GameObject.FindWithTag ("Player");

		this.gameObject.Tween("SmallPlayer2", player.transform.position, Vector3.up*10, 3f, TweenScaleFunctions.QuadraticEaseOut, (t2) =>
			{
				// progress
				player.transform.position =t2.CurrentValue;

			}, (t2) =>
			{
				game_over = false;
				hasStart = true;
				gestureManager.SetActive (true);
				spawnnerEnemies.reset ();
				spawnnerEnemies.can_spawn=true;
				spawnnerEnemies.NextWave ();

			});

		this.gameObject.Tween("SmallPlayer", player.transform.localScale, Vector3.one*5, 0.25f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
			{
				// progress
				player.transform.localScale =t.CurrentValue;
				//player.transform.position=Vector3.forward*60;

			}, (t) =>
			{



			});
		


	}
	private void TweenBigPlayer()
	{
		GameObject player = GameObject.FindWithTag ("Player");


		this.gameObject.Tween("BigPlayer2", player.transform.position, new Vector3(0,-45,90), 0.75f, TweenScaleFunctions.QuadraticEaseOut, (t2) =>
			{
				// progress
				player.transform.position =t2.CurrentValue;

			}, (t2) =>
			{


			});

		this.gameObject.Tween("BigPlayer", player.transform.localScale, Vector3.one*35, 3f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
			{
				// progress
				player.transform.localScale =t.CurrentValue;

			}, (t) =>
			{
				TweenShowResults();

			});



	}
	private void TweenShowResults()
	{
		results_canvas.transform.localScale = Vector3.zero;
		this.gameObject.Tween("ShowResults", Vector3.zero, Vector3.one, 0.75f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
			{
				// progress
				results_canvas.transform.localScale =t.CurrentValue;

			}, (t) =>
			{
				// esto para verificar si hay una playlist y reproducir el siguiente juego
				if (PlaylistManager.pm != null && PlaylistManager.pm.active)
					PlaylistManager.pm.NextGame();

			});



	}
	private void TweenHideResults()
	{
		results_canvas.transform.localScale = Vector3.one;
		this.gameObject.Tween("HideResults", Vector3.one, Vector3.zero, 0.75f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
			{
				// progress
				results_canvas.transform.localScale =t.CurrentValue;

			}, (t) =>
			{
				//complete
			});



	}
	private void TweenShowParameters()
	{
		parameters_canvas.transform.localScale = Vector3.zero;
		this.gameObject.Tween("ShowParameters", Vector3.zero, Vector3.one, 0.75f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
			{
				// progress
				parameters_canvas.transform.localScale =t.CurrentValue;

			}, (t) =>
			{
				//complete
			});



	}
	private void TweenHideParameters()
	{
		parameters_canvas.transform.localScale = Vector3.one;
		this.gameObject.Tween("HideParameters", Vector3.one, Vector3.zero, 0.75f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
			{
				// progress
				parameters_canvas.transform.localScale =t.CurrentValue;

			}, (t) =>
			{
				//complete
			});



	}
	private void TweenFinishGame(float time=2f)
	{
		this.gameObject.Tween("FinishGameJourney", this.transform.position.x, this.transform.position.x, time, TweenScaleFunctions.QuadraticEaseOut, (t) =>
			{
				// progress


			}, (t) =>
			{
				//complete time
				EndGame();
			});
	}
	public int modo_juego {

		get{
			return _modo_juego; 

		}
		set{
			
			_modo_juego = value;

			select_jugabilidad = select_jugabilidad;
			bt_play.interactable = _modo_juego != 0;
			timeSlider.transform.parent.gameObject.SetActive (modo_juego==2);
		}

	}
	public float select_jugabilidad {

		get{
			return jugabilidad_number;

		}
		set{
			jugabilidad_number = value;
			switch (modo_juego) {
			case 0:// sin definir modod de juego
				txt_jugabilidad.text="";
				break;
			case 1:// repeticiones
				
				txt_jugabilidad.text = value.ToString ("00") ;

				if (spawnnerEnemies!=null) {
					spawnnerEnemies.waves [0].enemyCount = (int)jugabilidad_number;
				}

				break;
			case 2://tiempo
				txt_jugabilidad.text = value.ToString ("00") + " min";
				if (spawnnerEnemies!=null) {
					spawnnerEnemies.waves [0].enemyCount = 999;
				}
				break;
			default:
				break;
			}

		}

	}
	public float timeBetweenEnemies {

		get{
			return _timeBetweenEnemies;

		}
		set{
			_timeBetweenEnemies= value;

			if (time_enemies!=null) {
				time_enemies.text = _timeBetweenEnemies.ToString("00")+" s";
			}
			if (spawnnerEnemies!=null) {
				spawnnerEnemies.waves [0].timeBetweenSpawn = _timeBetweenEnemies;
			}
		}

	}
	public float percentFigureMin {

		get{
			return _percentFigureMin;

		}
		set{
			_percentFigureMin= value;

			if (txt_scaleMin!=null) {


				txt_scaleMin.text = _percentFigureMin.ToString("00")+" %";
			}
			if (managerShapes!=null) {
				managerShapes.minScaleFigure = _percentFigureMin / 100;
			}

		}

	}
}
