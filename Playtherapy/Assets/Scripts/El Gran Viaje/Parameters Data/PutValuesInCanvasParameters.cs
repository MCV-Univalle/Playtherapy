using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PutValuesInCanvasParameters : MonoBehaviour, IParametersManager {


	private int _type_game;
	private int _lados_involucrados;
	private float _select_jugabilidad;
	private int _select_movimientos;
	private float _select_descanso;
	private float _select_sostener;
	private float _angle_min;
	private float _angle_min_frontal;
	private float _angle_max;

	public int type_game{

		get{ 
		
			return _type_game;
		
		}
		set{

			_type_game = value;
			if ((value==1) == false) {
				slider_jugabilidad.minValue = HoldParametersGreatJourney.min_repeticiones;
				slider_jugabilidad.maxValue = HoldParametersGreatJourney.max_repeticiones;
				/*if (timerUI!=null) {
					timerUI.SetActive (false);
				}*/

			}

			else 
			{
				slider_jugabilidad.minValue = HoldParametersGreatJourney.min_tiempo;
				slider_jugabilidad.maxValue = HoldParametersGreatJourney.max_tiempo;
				/*if (timerUI!=null) {
					timerUI.SetActive (true);
				}*/

			}

			updateTextTime (value);


		}


	}
	public int lados_involucrados{


		get{ 
		
			return _lados_involucrados;
		
		
		}


		set{ 
		
			_lados_involucrados = value;
		}




	}
	public int select_movimiento{

		get{ 
		
			return _select_movimientos;
		
		}
		set{ 
			_select_movimientos = value;
           // print("select: "+ _select_movimientos);
		}



	}
    
	public float select_jugabilidad
	{
		get{ 
		
			return _select_jugabilidad;
		
		}
		set{ 
		
			_select_jugabilidad = value;
			updateTextTime (type_game);

		}


	}
	public float angle_min_frontal{

		get{ 
			return _angle_min_frontal;
		}


		set{
			_angle_min_frontal = value;
			txt_nivel_min_frontal.text = "" + _angle_min_frontal+"º";

			if (_angle_min_frontal>= angle_max) {
				angle_max = _angle_min_frontal;
			}


		}
			

	}
	public float angle_min{


		get{ 
			return _angle_min;
		}


		set{
			_angle_min = value;
			txt_nivel_min.text = "" + _angle_min+"º";

			if (_angle_min>= angle_max) {
				angle_max = _angle_min;
			}
				

		}



	}
	public float angle_max
	{
		get{ 
			return _angle_max;
		}
		set{
		

			_angle_max = value;

			if (_angle_max>=angle_min && _angle_max>=angle_min_frontal) {
				

				txt_nivel_max.text = "" + _angle_max+"º";
			}


		}

	}
	public float select_descanso{

		get{ 
			return _select_descanso;
		}
		set{
			_select_descanso = value;

			txt_descanso.text = "" + _select_descanso+" seg";

		}



	}
	public float select_sostener{

		get{ 
			return _select_sostener;
		}
		set{
			_select_sostener = value;

			txt_sostener.text = "" + _select_sostener+" seg";
		}



	}
	public Dropdown jugabilidad;
	public Text txt_jugabilidad;
	public Slider slider_jugabilidad;

	public Slider angulo_dificultad_min;
	public Text txt_nivel_min;

	public Slider angulo_dificultad_min_frontal;
	public Text txt_nivel_min_frontal;

	public Slider angulo_dificultad_max;
	public Text txt_nivel_max;

	public Slider sostener_movimiento;
	public Text txt_sostener;

	public Slider tiempo_descanso;
	public Text txt_descanso;


	public Dropdown lados_utilizar;
	public Dropdown movimientos_posibles;
	

    public void StartGame()
    {
		HoldParametersGreatJourney.use_time = type_game == 1;
		HoldParametersGreatJourney.select_jugabilidad = select_jugabilidad;
		HoldParametersGreatJourney.select_movimiento = select_movimiento;

		HoldParametersGreatJourney.lados_involucrados = lados_involucrados;
		HoldParametersGreatJourney.select_angle_min_frontal = angle_min_frontal;
		HoldParametersGreatJourney.select_angle_min = angle_min;
		HoldParametersGreatJourney.select_angle_max = angle_max;
		HoldParametersGreatJourney.select_descanso = select_descanso;
		HoldParametersGreatJourney.select_sostener = select_sostener;


		if (ManagerGreatJourney.gm != null)
		{
			ManagerGreatJourney.gm.StartGame();
		}

    }


	void updateTextTime(int type_game=0)
	{


		if (type_game==1) {
			string minutos_s = "";
			string segundos_s = "00";

			if (select_jugabilidad < 10) {
				minutos_s = "0" + select_jugabilidad;
			} else {
				minutos_s = "" + select_jugabilidad;
			}


			minutos_s = minutos_s.Substring (0, 2);

			int min = int.Parse (minutos_s);
			float segundos = select_jugabilidad - min;


			if (segundos > 0) {
				segundos = segundos * 60;
				if (segundos < 10) {
					segundos_s = "0" + segundos;
				} else {
					segundos_s = "" + segundos;
				}

				segundos_s = segundos_s.Substring (0, 2);
			} 


			txt_jugabilidad.text = minutos_s + ":" + segundos_s + " min";
		} else {
			txt_jugabilidad.text = select_jugabilidad+" rep";
		}

	}

    // Use this for initialization
    void Start () {
		

		angulo_dificultad_min.minValue = HoldParametersGreatJourney.min_angle;
		angulo_dificultad_min.maxValue = HoldParametersGreatJourney.max_angle-1;

		angulo_dificultad_min_frontal.minValue = HoldParametersGreatJourney.min_angle;
		angulo_dificultad_min_frontal.maxValue = HoldParametersGreatJourney.max_angle-1;

		angulo_dificultad_max.minValue = HoldParametersGreatJourney.min_angle+1;
		angulo_dificultad_max.maxValue = HoldParametersGreatJourney.max_angle;



		sostener_movimiento.minValue = HoldParametersGreatJourney.min_sostener;
		sostener_movimiento.maxValue = HoldParametersGreatJourney.max_sostener;

		tiempo_descanso.minValue = HoldParametersGreatJourney.min_descanso;
		tiempo_descanso.maxValue = HoldParametersGreatJourney.max_descanso;
	
		type_game = 0;
		angle_min_frontal = 20;
		angle_min = 15;
		angle_max = 30;
        

    }
}
