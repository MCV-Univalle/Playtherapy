using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerCavano : MonoBehaviour
{
	private bool statusCava;
	// Determina si la tasa esta neutra o inclinada TRUE = Inclinada FALSE = Neutra
	public Text scoreUi;
	public int score = 0;
	float rango = 0;
	float temporizador;
	float tiempoGlobal;
//Almacena cuanto debe durar una repeticion
	int totalRepetitions = 0;
	public Slider timerSlider;
	private bool endGame = false;
	//Repeticiones
	//public GameObject panelRepeticiones;
	//public Text MensajeRepeticion;

	private bool pausePlay = false;
	private int mano = 0;

	// Panels used in the scene
	public GameObject mainPanel;
	public GameObject parametrersPanel;
	public GameObject resultsPanel;
	public bool StartG = false;

	public Text resultsScoreText;
	public Text resultsBestScoreText;
	public Sprite starOn;
	public Sprite starOff;
	public Image star1;
	public Image star2;
	public Image star3;

	// Use this for initialization
	void Start ()
	{
		temporizador = 2;
		statusCava = false;
		score = 0;
		timerSlider.maxValue = 100;
	}

	public bool StarGame ()
	{
		return StartG;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Inicia el timer del juego cuando se llama la funcion StartGame()
		if (StarGame ()) {
			Timer ();
		}
	}

	public bool GetStatusCava ()
	{
		return statusCava;
	}

	public void SetStatusCava (bool status)
	{
		statusCava = status;
	}

	public void SetScore (int puntos)
	{
		score += puntos;
		scoreUi.text = score.ToString ();
	}

	public int GetScore ()
	{
		return score;
	}

	public float GetRangoMano ()
	{
		return this.rango;
	}

	public bool PauseStatus ()
	{
		return pausePlay;
	}

	public void Pause ()
	{
		pausePlay = false;
	}

	public void Play ()
	{
		pausePlay = true;
	}

	void Timer ()
	{
		temporizador -= Time.deltaTime;
		timerSlider.value = temporizador;


		if (temporizador < 0) {
			if (totalRepetitions == 0) {
				endGame = true;
				SaveAndShowResults ();
			} else {
				
			/*	panelRepeticiones.SetActive (true);
				MensajeRepeticion.text = "Faltan " + totalRepetitions + " Repeticones ";
				StartCoroutine (WaitAndGo (5));
				panelRepeticiones.SetActive (false);
				Replay ();// Reinicia el tiempo para una nueva repeticion*/
			}
		}
	}

	public bool GetEndGame ()
	{
		return endGame;
	}

	public void SetEndGame (bool intro)
	{
		endGame = intro;
	}
	//+++++++++++++++++++++++++++++++++++++++++  Retorna la orientacion de las manos  +++++++++++++++++++++++++++++++++++++++++++++++++
	public int GetMano ()
	{
		return this.mano;
	}

	public void SetMano (int intro)
	{
		this.mano = intro;
	}
	//Funcion que inica el Juego
	public void StratGame (float Tiempo, float Rango, int Repeticiones, int mano)
	{
		float tiempoJuego = Tiempo * 59.0f;
		this.rango = Rango;
		this.temporizador = tiempoJuego;
		timerSlider.maxValue = tiempoJuego;
		timerSlider.value = tiempoJuego;
		this.tiempoGlobal = tiempoJuego;
		this.totalRepetitions = Repeticiones;
		this.mano = mano;
		StartG = true;
		Play ();
		
	
	}

	public void SaveAndShowResults ()
	{
		TherapySessionObject objTherapy = TherapySessionObject.tso;

		//if (objTherapy != null)
		//{
		//    objTherapy.fillLastSession(score, totalRepetitions, (int)totalTime, level.ToString());
		//    objTherapy.saveLastGameSession();
		//}

		int finalScore;
		if (totalRepetitions == 0) {
			finalScore = 0;
		} else {
			finalScore = (int)(((float)score / totalRepetitions) * 100.0f);
		}
		resultsScoreText.text = "Desempeño: " + finalScore + "%";

		if (objTherapy != null)
			resultsBestScoreText.text = "Mejor: " + objTherapy.getGameRecord () + "%";

		if (finalScore <= 60) {
			//resultMessage.GetComponent<TextMesh>().text = "¡Muy bien!";
			star1.sprite = starOn;
			star2.sprite = starOff;
			star3.sprite = starOff;
		} else if (finalScore <= 90) {
			//resultMessage.GetComponent<TextMesh>().text = "¡Grandioso!";
			star1.sprite = starOn;
			star2.sprite = starOn;
			star3.sprite = starOff;
		} else if (finalScore <= 100) {
			//resultMessage.GetComponent<TextMesh>().text = "¡Increíble!";
			star1.sprite = starOn;
			star2.sprite = starOn;
			star3.sprite = starOn;
		}

		resultsPanel.SetActive (true);
	}
	//Indica que mano se selecciono cpmpo timosn del juego
	public bool isLeftController ()
	{
		bool salida = false; 
		if (mano == 0) {
			salida = true;
		}
		return salida;
	}

	public  bool isRightController ()
	{
		bool salida = false; 
		if (mano == 1) {
			salida = true;
		}
		return salida;
	}


	public void Replay ()
	{
		totalRepetitions--;
		temporizador = tiempoGlobal;
	}

	IEnumerator WaitAndGo (int sleep)
	{
		yield return new WaitForSeconds (sleep);
	}
}
