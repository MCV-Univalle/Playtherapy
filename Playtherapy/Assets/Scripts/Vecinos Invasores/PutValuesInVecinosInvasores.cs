using Leap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PutValuesInVecinosInvasores : MonoBehaviour, IParametersManager
{
	[SerializeField] Dropdown jugabilidad;
    [SerializeField] Text txt_jugabilidad;
    [SerializeField] Slider slider_jugabilidad;

    [SerializeField] Slider slider_animales;
    [SerializeField] Text txt_animals;
    [SerializeField] Dropdown tipo_juego;
    [SerializeField] Dropdown modo_juego;

    [SerializeField] Slider tiempo_descanso;
    [SerializeField] Text txt_descanso;
    //[SerializeField] GameObject timerUI;

    [SerializeField] Slider slider_strength;
    [SerializeField] Text txt_strength;

    [SerializeField] GameObject right_hand_ui;
    [SerializeField] GameObject left_hand_ui;

    [SerializeField] GameObject right_hand_ui_color;
    [SerializeField] GameObject left_hand_ui_color;

    [SerializeField] GameObject thumb_left;
    [SerializeField] GameObject thumb_right;

    [SerializeField] GameObject panel_fuerza;
    [SerializeField] Toggle[] toggles;

    List<Finger.FingerType> fingerTypes;
    List<Material> fingers_materials;

	Button button_play;

    // Use this for initialization
    void Start ()
    {
        GameObject go_button_play = GameObject.Find("bt_play");        
        if (go_button_play != null)
            button_play = GameObject.Find("bt_play").GetComponent<Button>();

//		HoldParametersVecinosInvasores.use_time = (jugabilidad.value==0);
//
//		if (HoldParametersVecinosInvasores.use_time == false) {
//			slider_jugabilidad.minValue = HoldParametersVecinosInvasores.min_repeticiones;
//			slider_jugabilidad.maxValue = HoldParametersVecinosInvasores.max_repeticiones;
//		}
//		else 
//		{
//			slider_jugabilidad.minValue = HoldParametersVecinosInvasores.min_tiempo;
//			slider_jugabilidad.maxValue = HoldParametersVecinosInvasores.max_tiempo;
//		}

		slider_animales.minValue = HoldParametersVecinosInvasores.min_animals;
		slider_animales.maxValue = HoldParametersVecinosInvasores.max_animals;

		tiempo_descanso.minValue = HoldParametersVecinosInvasores.min_time_per_ship;
		tiempo_descanso.maxValue = HoldParametersVecinosInvasores.max_time_per_ship;

		fingers_materials = new List<Material> ();
		fingers_materials.Add((Material)Resources.Load("Materials/Vecinos Invasores/index_finger_material"));
		fingers_materials.Add((Material)Resources.Load("Materials/Vecinos Invasores/middle_finger_material"));
		fingers_materials.Add((Material)Resources.Load("Materials/Vecinos Invasores/ring_finger_material"));
		fingers_materials.Add((Material)Resources.Load("Materials/Vecinos Invasores/pinky_finger_material"));
		fingers_materials.Add((Material)Resources.Load("Materials/Vecinos Invasores/thumb_finger_material"));

        fingerTypes = new List<Finger.FingerType>();
        fingerTypes.Add(Finger.FingerType.TYPE_INDEX);
        fingerTypes.Add(Finger.FingerType.TYPE_MIDDLE);
        fingerTypes.Add(Finger.FingerType.TYPE_RING);
        fingerTypes.Add(Finger.FingerType.TYPE_PINKY);
        fingerTypes.Add(Finger.FingerType.TYPE_THUMB);
    }

    public void StartGame()
    {
        HoldParametersVecinosInvasores.use_time = (jugabilidad.value == 1);
        HoldParametersVecinosInvasores.type_game = tipo_juego.value;
        HoldParametersVecinosInvasores.mode_game = modo_juego.value;
        HoldParametersVecinosInvasores.select_jugabilidad = slider_jugabilidad.value;
        HoldParametersVecinosInvasores.select_animals = (int)slider_animales.value;
        HoldParametersVecinosInvasores.select_strenght_pinch = slider_strength.value / 100f;
        HoldParametersVecinosInvasores.select_time_per_ship = (int)tiempo_descanso.value;
        HoldParametersVecinosInvasores.fingerTypes = fingerTypes;

        if (ManagerVecinosInvasores.gm != null)
        {
            ManagerVecinosInvasores.gm.StartGame();
        }
    }

	void setSimpleFingersMaterials()
	{
		Color simple = Color.black;
		simple.a = 0.5f;

		foreach (var item in fingers_materials) {
			item.color=simple;
		}
	}

	void setColorFingersMaterials()
	{
		Color any_color;

		any_color= Color.red;
		any_color.a = 0.5f;
		fingers_materials [0].color=any_color;

		any_color= Color.blue;
		any_color.a = 0.5f;
		fingers_materials [1].color=any_color;

		any_color= new Color(255f/255f,135f/255f,0);//orange
		any_color.a = 0.5f;
		fingers_materials [2].color=any_color;

		any_color= new Color(255f/255f,0/255f,234f/255f);//orange
		any_color.a = 0.5f;
		fingers_materials [3].color=any_color;

	}

    public void SliderAnimals()
    {
        txt_animals.text = "" + (int)slider_animales.value;
    }

    public void SliderStrenght()
    {
        txt_strength.text = "" + slider_strength.value + "%";
    }

    public void SliderTimePerShip()
    {
        txt_descanso.text = "" + (int)tiempo_descanso.value + " seg";
    }

    public void TipoJuego()
    {
        panel_fuerza.SetActive(tipo_juego.value == HoldParametersVecinosInvasores.USE_PINCHS);
        thumb_left.SetActive(!(tipo_juego.value == HoldParametersVecinosInvasores.USE_PINCHS));
        thumb_right.SetActive(!(tipo_juego.value == HoldParametersVecinosInvasores.USE_PINCHS));

        if ((tipo_juego.value == 0 && fingerTypes.Count == 1) || (tipo_juego.value == 1 && fingerTypes.Count == 2))
        {
            foreach (Toggle t in toggles)
                if (t.isOn)
                    t.interactable = false;
        }
        else
        {
            foreach (Toggle t in toggles)
                t.interactable = true;
        }



        HoldParametersVecinosInvasores.type_game = tipo_juego.value;
    }

    public void ModoJuego()
    {
        if (modo_juego.value == HoldParametersVecinosInvasores.SIMPLE)
        {
            left_hand_ui_color.SetActive(false);
            right_hand_ui_color.SetActive(false);
            left_hand_ui.SetActive(true);
            right_hand_ui.SetActive(true);

            setSimpleFingersMaterials();
        }
        else
        {
            left_hand_ui_color.SetActive(true);
            right_hand_ui_color.SetActive(true);
            left_hand_ui.SetActive(false);
            right_hand_ui.SetActive(false);

            setColorFingersMaterials();
        }


        HoldParametersVecinosInvasores.mode_game = modo_juego.value;


    }

    public void Jugabilidad()
    {
        if (jugabilidad.value != 1)
        {
            slider_jugabilidad.minValue = HoldParametersVecinosInvasores.min_repeticiones;
            slider_jugabilidad.maxValue = HoldParametersVecinosInvasores.max_repeticiones;
            //timerUI.SetActive (false);

            txt_jugabilidad.text = "" + slider_jugabilidad.value + " rep";
            slider_animales.minValue = slider_jugabilidad.value;
        }
        else
        {
            //timerUI.SetActive (true);
            slider_jugabilidad.minValue = HoldParametersVecinosInvasores.min_tiempo;
            slider_jugabilidad.maxValue = HoldParametersVecinosInvasores.max_tiempo;

            string minutos_s = "";
            string segundos_s = "00";

            if (slider_jugabilidad.value < 10)
            {
                minutos_s = "0" + slider_jugabilidad.value;
            }
            else
            {
                minutos_s = "" + slider_jugabilidad.value;
            }

            minutos_s = minutos_s.Substring(0, 2);

            int min = int.Parse(minutos_s);
            float segundos = slider_jugabilidad.value - min;

            if (segundos > 0)
            {
                segundos = segundos * 60;
                if (segundos < 10)
                {
                    segundos_s = "0" + segundos;
                }
                else
                {
                    segundos_s = "" + segundos;
                }

                segundos_s = segundos_s.Substring(0, 2);
            }

            txt_jugabilidad.text = minutos_s + ":" + segundos_s + " min";
        }
    }

    public void ChangeToggleFinger(Toggle button)
    {
        Finger.FingerType type = button.GetComponent<toogle2buttons>().type;
        if (button.isOn == true)
        {
            fingerTypes.Add(type);
        }
        else if ((tipo_juego.value == 0 && fingerTypes.Count > 1) || (tipo_juego.value == 1 && fingerTypes.Count > 2))
        {
            fingerTypes.Remove(type);
        }

        if ((tipo_juego.value == 0 && fingerTypes.Count == 1) || (tipo_juego.value == 1 && fingerTypes.Count == 2))
        {
            foreach (Toggle t in toggles)
                if (t.isOn)
                    t.interactable = false;
        }
        else
        {
            foreach (Toggle t in toggles)
                t.interactable = true;
        }

        Debug.Log(fingerTypes.Count);
    }
}
