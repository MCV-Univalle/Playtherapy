using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PutValuesFM : MonoBehaviour, IParametersManager
{


    int _modo_juego;
    float jugabilidad_number;
    float _timeBetweenEnemies;
    float _percentFigureMin;

    public Text time_enemies;
    public Text txt_jugabilidad;
    public Text txt_scaleMin;

    public Slider slide_jugabilidad;
    public Slider slide_tiempo_entre_enemigos;
    public Slider slide_escala_min;
    public Dropdown drop_modo_juego;
    public List<int> list_gestures_index;
    public List<Toggle> list_gestures_used;
    public Button bt_play;



    bool _change_togle = true;
    public bool changeAnyToggle
    {

        get { return _change_togle; }
        set
        {
            _change_togle = value;
            if (bt_play != null)
            {
                bt_play.interactable = atLeastATooggleActive() && _modo_juego != 0;
            }


        }

    }
    bool atLeastATooggleActive()
    {
        int contadorToggleActivos = 0;

        foreach (var item in list_gestures_used)
        {
            if (item.isOn)
            {
                contadorToggleActivos++;
            }
        }

        return contadorToggleActivos > 0;

    }
    public void updateJugabilidadNumber()
    {

        if (_modo_juego==1)
        {
            jugabilidad_number = slide_jugabilidad.value;
            txt_jugabilidad.text = "" + jugabilidad_number.ToString("00") ;
        }
        if (_modo_juego==2)
        {
            jugabilidad_number = slide_jugabilidad.value;
            txt_jugabilidad.text = "" + jugabilidad_number.ToString("00") + " s";
        }

        
    }
    public void updateTiempoEntreEnemigos()
    {
        _timeBetweenEnemies = slide_tiempo_entre_enemigos.value;
        time_enemies.text = "" + _timeBetweenEnemies.ToString("00") + " s";

    }
    public void updateEscalaMin()
    {
        _percentFigureMin = slide_escala_min.value;
        txt_scaleMin.text = "" + _percentFigureMin.ToString("00")+" %";


    }
    public void updateModoJuego()
    {

        _modo_juego = drop_modo_juego.value;


    }
    // Use this for initialization
    void Start()
    {
        list_gestures_index = new List<int>();
        changeAnyToggle=false;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void StartGame()
    {
        list_gestures_index = new List<int>();

        for (int i = 0; i < list_gestures_used.Count; i++)
        {

            if (list_gestures_used[i].isOn)
            {
                list_gestures_index.Add(i);
            }
        }

        ManagerFM.gm.list_gestures_index = list_gestures_index;
        ManagerFM.gm.managerShapes.loadGestures(list_gestures_index);
       // ManagerFM.gm.spawnnerEnemies.gestures_index_used = list_gestures_index;

        ManagerFM.gm.startGame(_modo_juego, jugabilidad_number, _timeBetweenEnemies);

    }

}
