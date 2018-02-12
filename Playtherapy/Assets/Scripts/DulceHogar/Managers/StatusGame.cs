using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompleteProject
{
	public class StatusGame : MonoBehaviour
	{
		public StatusGame status;
		public float timer_game;
		// Tiempo Global del Juego.
		public float healthPlayer;
		// Salud Global del Juego.
		public float tiempoInicial = 5;
		public string indicador;
		public float velocidaEnemigos = 1;
		public int scoreGlobal;
		private int rango;
		private string nameScene;
		private int timon;



		// Use this for initialization
		void Awake ()
		{
			/*Crea una instancia del GameObject que no se destruye entre escenas*/	



			if (status == null) {
				/*Verifica que No exista previamente Otro StatusGame
				Antes de Crearse.*/
				DontDestroyOnLoad (gameObject);

			} else if (status != this) {
				Destroy (gameObject);
			}

		}

		void Start ()
		{
			scoreGlobal = 0;
			healthPlayer = 100;
		}
		
		// Update is called once per frame
		void Update ()
		{
			
		}

		public int NumberStar ()
		{
			
			int salida = (int)tiempoInicial * 10 + scoreGlobal;
			return salida;
		}
		//Resive la informacion del Minijuego Para su ejecucion
		public void SetInformationGame (float time, int rango, int timon)
		{
			timer_game = (float)time * 60.0f; // Asigna el valor del Slider para ser llevado a otras escenas.
			tiempoInicial = time;
			this.rango = rango;
			this.timon = timon;
		}

		public int GetTimon ()
		{
			return this.timon;
		}

		public int GetRango ()
		{
			return this.rango;
		}

		public bool isRight(){
			bool salida = false;
			if(timon != 0){
				salida = true;
			}

				return salida;
		}
	}
}