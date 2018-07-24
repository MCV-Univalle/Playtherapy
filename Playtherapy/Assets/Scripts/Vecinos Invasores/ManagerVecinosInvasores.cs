using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// for your own scripts make sure to add the following line:
using DigitalRuby.Tween;
using Leap;
public class ManagerVecinosInvasores : MonoBehaviour {


	public static ManagerVecinosInvasores gm;

	
//	Animator tutorial_movements;
	GameObject parameters_canvas;
	GameObject results_canvas;
	GameObject tutorial_canvas;
    GameObject pause_menu;
	GameObject count_objects_canvas;
	GameObject tutorial_page_info;
	GameObject cam;
	GameObject timerUI;

    TinyPauseScript pausa;

    Slider timeSlider;

	Vector3 cam_initial_pos;
	Quaternion cam_initial_rot;

	Text txt_rubies;
	Text txt_dodge;
	Text txt_time;
	SpannerShipsEnemies spanner;
	PutDataResults results_script;
	ScoreHandlerVecinosInvasores score_script;
	SpannerAnimals spannerAnimals;


	List<MonoBehaviour> array_scrips_disabled;
	List<GameObject> tutorial_pages_array;
	List<GameObject> array_arrows;


	bool game_over;
	bool hasStart;
	public float timer_game=-1;
	int tutorial_page=0;
	float timeMillis;
	// Use this for initialization
	void Start () {


		if (gm==null) {
			gm = this;
		}

        pausa = FindObjectOfType<TinyPauseScript>();

        hasStart = false;
		game_over = false;

		timeMillis = 1000f;		

		spanner = FindObjectOfType<SpannerShipsEnemies> ();
		spannerAnimals = FindObjectOfType<SpannerAnimals> ();
		score_script = FindObjectOfType<ScoreHandlerVecinosInvasores> ();
		results_script = FindObjectOfType<PutDataResults> ();


		array_scrips_disabled = new List<MonoBehaviour> ();
		array_scrips_disabled.Add (spanner);
		array_scrips_disabled.Add (spannerAnimals);


		parameters_canvas = GameObject.Find ("parameters_canvas");
		results_canvas = GameObject.Find ("results_canvas");
		tutorial_canvas= GameObject.Find ("tutorial_canvas");
        pause_menu = GameObject.Find("pause_data");
		count_objects_canvas = GameObject.Find ("count_objects_canvas");
		timeSlider = GameObject.Find ("slideTimeUI").GetComponent<Slider>();
        timerUI = GameObject.Find("timerUI");
		cam = GameObject.Find("Camera");

		cam_initial_pos = cam.transform.position;
		cam_initial_rot = cam.transform.rotation;

		txt_rubies = GameObject.Find ("txt_rubies").GetComponent<Text> ();
		txt_dodge = GameObject.Find ("txt_dodge").GetComponent<Text> ();
		txt_time = GameObject.Find ("txt_timer").GetComponent<Text> ();
        //		tutorial_movements = GameObject.Find ("anim_moves").GetComponent<Animator> ();

        //		array_arrows = new List<GameObject> ();
        //
        //		array_arrows.Add(GameObject.Find("left_img"));
        //		array_arrows.Add(GameObject.Find("right_img"));
        //		array_arrows.Add(GameObject.Find("down_img"));


        //TweenShowParameters ();
        parameters_canvas.transform.localScale = Vector3.zero;
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

		// esto para activar el panel de parámetros en caso de que no se esté en playlist
		if (PlaylistManager.pm == null || (PlaylistManager.pm != null && !PlaylistManager.pm.active))
		{	
			TweenShowParameters ();
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
	void Update () {


		if (hasStart == true) 
		{
			if (game_over==false) 
			{
				if (HoldParametersVecinosInvasores.use_time == true)
				{
					if (timer_game > 0) {

						timer_game -= Time.deltaTime;

						timeSlider.value = (timer_game / (HoldParametersVecinosInvasores.select_jugabilidad * 60)) * 100;
					} else {


						timer_game = 0;
						timeSlider.value = 0;
						bool response = spanner.isActiveAnyShip ();
						if (response==false) {
							game_over = true;
							EndGame ();
						}

					}
					updateTimeText ();

				} 
				else 
				{
					bool response = spanner.isActiveAnyShip ();
					//print ("repeticiones_restantes:"+HoldParametersVecinosInvasores.repeticiones_restantes +", response: "+ response);
					if (HoldParametersVecinosInvasores.repeticiones_restantes==0 && response==false) 
					{
						game_over = true;
						//print ("termino gameplay");
						TweenFinishGame ();

					}
				}
			}
		}

	}
	public void EndGame()
	{
		//saveData ();
		int performance_game=0;
		if (score_script.score_max>0) {
			performance_game = Mathf.RoundToInt (((float)score_script.score_obtain / (float)score_script.score_max) * 100);
		}


		//print ("my performance was:"+performance_game+", score_obtain:"+score_script.score_obtain+ ", score_max:"+score_script.score_max);
		int performance_loaded_BD = 0;
		results_script.updateData (performance_game, performance_loaded_BD);


		hasStart = false;
		//paramenters_canvas.SetActive (true);
		foreach (MonoBehaviour behaviour in array_scrips_disabled)
		{
			behaviour.enabled = false;   
		}
		FinalAnimation ();

	}
	public void RetryGame()
	{
		TweenHideResults ();
		TweenShowParameters ();



	}

	public void StartGame()
	{
		timer_game = -1;
		game_over = false;
		hasStart = true;
		timeSlider.value = 100;
		score_script.reset ();
		spannerAnimals.crearAnimales(HoldParametersVecinosInvasores.select_animals);
		spanner.LIMIT_TIME_TO_SPAWN = (int)HoldParametersVecinosInvasores.select_time_per_ship;
		spanner.timer_spawn = spanner.LIMIT_TIME_TO_SPAWN;
		//		spannerAnimals.resetData ();
		//		spannerAnimals.resetPositionsSpineBase ();
		//paramenters_canvas.SetActive (false);

		foreach (MonoBehaviour behaviour in array_scrips_disabled)
		{
			behaviour.enabled = true;   
		}

		if (HoldParametersVecinosInvasores.use_time == true) {
            timerUI.SetActive(true);
			timer_game = HoldParametersVecinosInvasores.select_jugabilidad * 60;

		} else
		{
            timerUI.SetActive(false);
			HoldParametersVecinosInvasores.repeticiones_restantes = (int)HoldParametersVecinosInvasores.select_jugabilidad;

		}
        if (HoldParametersVecinosInvasores.type_game == HoldParametersVecinosInvasores.USE_PINCHS)
        {
            HoldParametersVecinosInvasores.fingerTypes.Remove(Finger.FingerType.TYPE_THUMB);
        }
		cam.transform.localPosition = Vector3.zero;
		cam.transform.localRotation = Quaternion.Euler (Vector3.zero);
		//cam.transform.LookAt (spannerAnimals.transform.position);
		cam.transform.position = cam_initial_pos;
		cam.transform.rotation = cam_initial_rot;
		//		spanner.setup ();
		TweenHideParameters ();
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

				objTherapy.savePerformance((int)HoldParametersVecinosInvasores.best_score, "33");



			}
		}
	}
	
	

	private void FinalAnimation()
	{
//		txt_rubies.text = "x" + score_script.rubies_caught;
//		txt_dodge.text = "x" + score_script.airplanes_dodge;
		//TweenShowCountObjets ();



		TweenRotateCamera ();

	}
	private void TweenRotateCamera()
	{
		GameObject[] animals = GameObject.FindGameObjectsWithTag ("Animals");
		cam.gameObject.Tween("RotateCamera", 0, 100, 4.0f, TweenScaleFunctions.CubicEaseInOut, (t) =>
			{
				

				foreach (var item in animals) {
					item.GetComponent<AnimalData> ().jumpAction ();
				}

				// progress
				cam.transform.RotateAround (spannerAnimals.transform.position, Vector3.up, 1f);
				//cam.transform.rotation = Quaternion.identity;
				//cam.transform.Rotate(spannerAnimals.transform.forward, t.CurrentValue);
			}, (t) =>
			{
				TweenShowResults();
				//completion
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
	private void TweenShowCountObjets()
	{
		count_objects_canvas.transform.localScale = Vector3.zero;
		this.gameObject.Tween("ShowCountObjets", Vector3.zero, Vector3.one, 0.75f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
			{
				// progress
				count_objects_canvas.transform.localScale =t.CurrentValue;

			}, (t) =>
			{
				//complete
			});



	}
	private void TweenHideCountObjets()
	{
		count_objects_canvas.transform.localScale = Vector3.one;
		this.gameObject.Tween("HideCountObjets", Vector3.one, Vector3.zero, 0.75f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
			{
				// progress
				count_objects_canvas.transform.localScale =t.CurrentValue;

			}, (t) =>
			{
				//complete
			});



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

			PutRespectiveTextTutorial ();

		} else {
			CloseTutorial ();
		}
		tutorial_page++;

	}
    
    private void PutRespectiveTextTutorial()
	{
		if (tutorial_page==1) {

			Text txt_mover = GameObject.Find("txt_mover").GetComponent<Text>();




			switch (HoldParametersVecinosInvasores.type_game) {
			case HoldParametersVecinosInvasores.USE_FINGERS:
				switch (HoldParametersVecinosInvasores.mode_game) {

				case HoldParametersVecinosInvasores.SIMPLE:

					txt_mover.text="Usa tus dedos tocando las naves para destruirlas";

					break;
				case HoldParametersVecinosInvasores.COLORS:

					txt_mover.text="Usa tus dedos de colores para destruir la nave que sea de ese color.";
					break;
				default:
					txt_mover.text="Usa tus dedos tocando las naves para destruirlas";
					break;
				}
				break;
			case HoldParametersVecinosInvasores.USE_PINCHS:
				switch (HoldParametersVecinosInvasores.mode_game) {

				case HoldParametersVecinosInvasores.SIMPLE:

					txt_mover.text="Usa una pinza encima de las naves para destruirlas";

					break;
				case HoldParametersVecinosInvasores.COLORS:

					txt_mover.text="Usa una pinza de la combinacion de los colores correcta para destruir las naves";
					break;
				default:
					txt_mover.text="Usa una pinza encima de las naves para destruirlas";
					break;
				}

				break;
			
			default:
				txt_mover.text="Usa tus dedos tocando las naves para destruirlas";
				break;
			
			}


		}

	}
    private void CloseTutorial()
    {
        if (hasStart == false)
        {
            TweenHideTutorial();
            TweenShowParameters();
        }
        else
        {
            tutorial_canvas.transform.localScale = Vector3.zero;
            pausa.gameObject.SetActive(true);
        }



    }
    public void OpenTutorial()
    {

        tutorial_page = 0;
        putPageTutorial();

        if (hasStart == false)
        {
            TweenShowTutorial();
            TweenHideParameters();
        }
        else
        {
            tutorial_canvas.transform.localScale = Vector3.one;
            pausa.gameObject.SetActive(false);
        }

    }

    /*
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
	}*/
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
		this.gameObject.Tween("FinishGameVecinosInvasores", this.transform.position.x, this.transform.position.x, time, TweenScaleFunctions.QuadraticEaseOut, (t) =>
			{
				// progress


			}, (t) =>
			{
				//complete time
				EndGame();
			});
	}

}

