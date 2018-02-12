using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.Tween;
public class ManagerFL : MonoBehaviour {

	public static ManagerFL gm;
	public Text debug;
	public int modoPlay;
	public int rodillaOPie;
	public int plano;
	public int lados;
	public float anguloMin;
	public float anguloMax;
	public float tiempoRebote;
	public float valorPlay;
	public bool repeticiones;
	public bool rodilla;
	public bool pie;
	public bool izquierdo;
	public bool derecho;
	public bool frontal;
	public bool sagital;

	public int pag_tutorial;
	List<GameObject> tutorial_pages_array;
	public AudioSource golpePelota;
	public AudioSource golpeFallido;

	public int _puntos;
	public Text puntos_juego;
	public int puntos{

		get{ 
			return _puntos;
		}
		set{
		
			_puntos = value;
			if (puntos_juego!=null) {
				puntos_juego.text = "" + _puntos;
			}
		}

	}

	public Slider sliderTiempoActual;
	public Text tiempoActualText;
	public Text repeticionesText;
	public GameObject panelTiempo;
	public GameObject panelRepeticiones;
	public GameObject panelParametros;
	public GameObject panelResultados;
	public GameObject panelTutorial;
	public GameObject ball;
	GameObject tutorial_page_info;
	List<GameObject> array_arrows;

	public enum Movimientos { FrontalDerRodilla, FrontalIzqRodilla, SagitalDerRodilla, SagitalIzqRodilla, FrontalDerPie, FrontalIzqPie, SagitalDerPie, SagitalIzqPie };
	private Movimientos[] planoFrontal;
	private Movimientos[] planoSagital;
	private Movimientos[] ambosPlanos;
	public Movimientos movimientoActual;
	private System.Random random;
	public ParticleSystem rodillaDerParticles1;
	public ParticleSystem rodillaIzqParticles2;
	public ParticleSystem pieDerParticles1;
	public ParticleSystem pieIzqParticles2;
	public PlayerFL jugador;
	public PutValuesFL putValues;
	bool game_over;
	public bool hasStart;
	float timer_game=-1;
	float timeMillis;

	Random generador = new Random();

	public int repeticionesRestantes;
	public int repeticionesTotales;
	public int desempenio;

	PutDataResults resultados;
	finalAnimationFL animationFinal;
	// Use this for initialization
	void Start () {
		
		if (gm==null) {
			gm = this;
		}
		putValues = GameObject.FindObjectOfType<PutValuesFL> ();
		jugador = GameObject.FindObjectOfType<PlayerFL> ();
		resultados = FindObjectOfType<PutDataResults> ();
		animationFinal = FindObjectOfType<finalAnimationFL> ();
		panelResultados.SetActive (false);

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
		hasStart = false;
		game_over = false;
		panelTutorial.SetActive (false);
		random = new System.Random ();

	}
	void updateTimeText()
	{

		if (timer_game > 0)
		{

			timeMillis -= Time.deltaTime*1000;
			if (timeMillis<0) {
				timeMillis = 1000f;
			}

			tiempoActualText.text = (((int)timer_game) / 60).ToString("00") + ":"
				+ (((int)timer_game) % 60).ToString("00") + ":"
				+ ((int)(timeMillis * 60 / 1000)).ToString("00");




		} else {
			tiempoActualText.text= "00:00:00";
		}
	}
		
	// Update is called once per frame
	void Update () {
		if (hasStart == true) 
		{
			if (game_over==false) {
				if (repeticiones == false){
					panelTiempo.SetActive (true);
					panelRepeticiones.SetActive (false);
					if (timer_game > 0) {

						timer_game -= Time.deltaTime;

						sliderTiempoActual.value = (timer_game / (modoPlay * 60)) * 100;


					} else {
						timer_game = 0;
						sliderTiempoActual.value = 0;
						game_over = true;
						print ("acabo juego");
						panelResultados.SetActive (true);
						hasStart = false;
						ball.GetComponent<Rigidbody> ().useGravity = false;

					}
					updateTimeText ();


				}else{
					if (repeticionesRestantes < repeticionesTotales) {
						panelTiempo.SetActive (false);
						panelRepeticiones.SetActive (true);
						print ("Repeticiones restantes" + repeticionesRestantes);
						repeticionesText.text = repeticionesRestantes.ToString ()+"/"+repeticionesTotales.ToString();
					} else {
						ball.GetComponent<Rigidbody> ().useGravity = false;
						animationFinal.startAnimation ();
						TweenFinalAnimation ();

						hasStart = false;
					}
				}
			}
		}
	}
	private void TweenFinalAnimation()
	{
		
		this.gameObject.Tween("finalAnimation", 0, 0, 3.0f, TweenScaleFunctions.CubicEaseInOut, (t) =>
			{
				// progress

			}, (t) =>
			{
				finalizarJuego ();
				repeticionesRestantes = 0;
				repeticionesText.text = "0/0";
				game_over = true;


				print ("acabo juego");
				desempenio = (puntos / repeticionesTotales)*100;
			});
	}
	public void StartGame()	{

		modoPlay = putValues.modoPlay;
		valorPlay = putValues.valorPlay;
		rodillaOPie = putValues.rodillaOPie;
		plano = putValues.plano;
		lados = putValues.lados;
		anguloMin = putValues.anguloMin;
		anguloMax = putValues.anguloMax;
		tiempoRebote = putValues.tiempoRebote;
		repeticiones = putValues.repeticiones;
		rodilla = putValues.rodilla;
		pie = putValues.pie;
		izquierdo = putValues.izquierdo;
		derecho = putValues.derecho;
		frontal = putValues.frontal;
		sagital = putValues.sagital;
		puntos = 0;
		panelParametros.SetActive (false);

		Physics.gravity = Vector3.down * 5/tiempoRebote;
		ball.GetComponent<Rigidbody> ().useGravity = true;
		hasStart = true;

		if (!repeticiones) {
			timer_game = valorPlay * 60;
			sliderTiempoActual.value = 100;
			repeticionesRestantes = 0;
		} else {
			repeticionesRestantes = 0;
			repeticionesTotales = (int)valorPlay;
		}
		jugador.administrarTurno ();
	}

	public void reiniciarJuego(){
		puntos = 0;
		panelResultados.SetActive (false);
		panelParametros.SetActive (true);
		hasStart = false;
		game_over = false;
		animationFinal.cleanAnimation ();
	}

	public void iniciarTutorial(){
		pag_tutorial = 0;

		panelParametros.SetActive (false);
		panelTutorial.SetActive (true);
		putPageTutorial ();


	}

	public void cerrarTutorial(){
		
		panelParametros.SetActive (true);
		panelTutorial.SetActive (false);
	}

	public void putPageTutorial(){
		foreach (GameObject obj in tutorial_pages_array)
		{
			obj.SetActive (false);
		}


		if (pag_tutorial < tutorial_pages_array.Count) {
			tutorial_page_info = tutorial_pages_array [pag_tutorial];
			tutorial_page_info.SetActive (true);

			//PutRespectiveTextTutorial ();

		} else {
			cerrarTutorial ();
		}
		pag_tutorial++;
	}

	public void finalizarJuego(){
		guardarDatos ();
		int performance_loaded_BD = 0;
		panelResultados.SetActive (true);
		if (resultados!=null) {
			resultados.updateData (desempenio, performance_loaded_BD);
		}

		//hasStart = false;


	}

	public void guardarDatos(){
		string movimiento;
		if (rodilla == true && pie == true) {
			movimiento = "rodilla y pie";
		} else if (rodilla == true) {
			movimiento = "rodilla";
		} else {
			movimiento = "pie";
		}
		GameObject tre = GameObject.Find ("TherapySession");

		if (tre!=null)
		{
			TherapySessionObject objTherapy = tre.GetComponent<TherapySessionObject> ();

			if (objTherapy!=null) 
			{

				objTherapy.fillLastSession(puntos, repeticionesTotales, (int)valorPlay, "1");
				objTherapy.saveLastGameSession ();



				objTherapy.savePerformance((int)anguloMin, movimiento);



			}
		}
	}
}
