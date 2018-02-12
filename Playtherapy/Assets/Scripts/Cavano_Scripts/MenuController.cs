using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public Slider rangoSlider;
	public Slider tiempoSlider;

	public Text rangoText;
	public Text tiempoText;

	public Dropdown manoSelect;
	GameManagerCavano manager;
	// Use this for initialization
	void Start ()
	{
		manager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManagerCavano> ();
		rangoSlider.minValue = 35;
		rangoSlider.maxValue = 80;
		tiempoSlider.minValue = 2;
		tiempoSlider.maxValue = 15;

	}

	// Update is called once per frame
	void Update ()
	{

		SlideUpdate ();
	}

	void SlideUpdate ()
	{
		rangoText.text = ":" + ((int)rangoSlider.value).ToString () + "º Grados";
		tiempoText.text = ":" + ((int)tiempoSlider.value).ToString () + " Minutos";


	}

	public void StartGameControllerPlay ()
	{
		float tiempo = tiempoSlider.value;
		float rango = rangoSlider.value;
		//int repeticiones = (int)repeticioneSlider.value;
		int repeticiones = 1;
		int mano = manoSelect.value;
		/*Para le seleccion e de una mano el valor de 0 izquierdad el valor 1 derecha*/
		manager.StratGame (tiempo, rango, repeticiones, mano);
	}

}
