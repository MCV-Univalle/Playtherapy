using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PutValuesFL : MonoBehaviour, IParametersManager {

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

	public Dropdown modo_juego_dropdown;
	public Dropdown modo_parte_juego;
	public Dropdown modo_lado;
	public Dropdown modo_parte_cuerpo;
	public Slider valor_modo_juego;
	public Slider valor_angulo_min;
	public Slider valor_angulo_max;
	public Slider valor_tiempo_rebote;
	public Text modo_juego;
	public Text angulo_max;
	public Text angulo_min;
	public Text tiempo_rebote;




	public void StartGame()
	{

        capturarModoPlay();
        capturarLado();
        capturarPartecuerpo();
        capturarPlano();
        capturarAnguloMin();
        capturarAnguloMax();
        capturarValorModoJuego();
        capturarTiempoRebote();
        if (ManagerFL.gm!=null) {
            //StartGame();
            ManagerFL.gm.StartGame(modoPlay, repeticiones, valorPlay, rodillaOPie, plano, lados, anguloMin, anguloMax, tiempoRebote, rodilla, pie, izquierdo
        , derecho, frontal, sagital);

        }



	}
	// Use this for initialization
	void Start () {
		repeticiones = true;
		derecho = true;
		izquierdo = true;
		capturarModoPlay ();
		capturarLado ();
		capturarPartecuerpo ();
		capturarPlano ();
		capturarAnguloMin ();
		capturarAnguloMax ();
		capturarValorModoJuego ();
		capturarTiempoRebote ();
	}
	
	// Update is called once per frame
	/*void Update () {
		
	}*/

	public void capturarModoPlay(){
		modoPlay = modo_juego_dropdown.value;
		if (modoPlay == 0) {
			repeticiones = true;
		} else {
			repeticiones = false;
		}
		capturarValorModoJuego ();
	}

	public void capturarPartecuerpo(){
		rodillaOPie = modo_parte_cuerpo.value;
		switch (rodillaOPie) {
		case 0:
			rodilla = true;
			pie = true;
			break;
		case 1:
			rodilla = true;
			pie = false;
			break;
		case 2:
			rodilla = false;
			pie = true;
			break;
		}
	}

	public void capturarLado(){
		lados = modo_lado.value;
		switch (lados) {
		case 0:
			izquierdo = true;
			derecho = true;
			break;
		case 1:
			izquierdo = true;
			derecho = false;
			break;
		case 2:
			izquierdo = false;
			derecho = true;
			break;
		}
	}

	public void capturarPlano(){
		plano = modo_lado.value;
		switch (plano) {
		case 0:
			frontal = true;
			sagital = true;
			break;
		case 1:
			frontal = true;
			sagital = false;
			break;
		case 2:
			frontal = false;
			sagital = true;
			break;
		}
	}

	public void capturarAnguloMin(){
		anguloMin = valor_angulo_min.value;
		angulo_min.text = anguloMin + "°";
	}

	public void capturarAnguloMax(){
		anguloMax = valor_angulo_max.value;
		angulo_max.text = anguloMax + "°";
	}

	public void capturarTiempoRebote(){
		tiempoRebote = valor_tiempo_rebote.value;
		tiempo_rebote.text = tiempoRebote + " seg";
	}		
	public void capturarValorModoJuego(){
		valorPlay = valor_modo_juego.value;
		if (modoPlay == 0) {
			modo_juego.text = valorPlay + " rep";
		} else {
			modo_juego.text = valorPlay + "  min";
		}
	}
}
