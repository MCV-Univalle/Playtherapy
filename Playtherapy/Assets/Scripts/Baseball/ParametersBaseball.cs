using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParametersBaseball : MonoBehaviour, IParametersManager
{

    private GameController gameController;
    public GameObject ParametersPanel;
    public GameObject TutorialPanel;
    
    public GameObject ResultPanel;



    // Parameters

    float _time_game;

    float _repetitions;

    public Text textCurrentTime;
    public Slider sliderCurrentTime;
    public Text sliderText;

    public Dropdown ArmSelection;


    public Dropdown numberRepetitions;
    public float currentRepetitions;
    public GameObject array_balls;




    public float time_default
    {

        get
        {
            if (numberRepetitions.value == 0)
            {
                return _time_game;

            }
            else
            {

                return _repetitions;

            }

        }
        set
        {

            if (numberRepetitions.value == 0)
            {

                _time_game = value;




                if (sliderText != null)
                {
                    sliderText.text = (((int)_time_game % 60).ToString("00") + ":" + ((int)_time_game / 60).ToString("00") + " mins");

                }
            }
            else
            {

                _repetitions = value;

                if (sliderText != null)
                {
                    sliderText.text = ("" + (int)_repetitions);

                }
            }

        }
    }




    float _velocity_game;
    public float velocity
    {

        get
        {
            return _velocity_game;
        }
        set
        {
            _velocity_game = value;

            if (current_velocity != null)
            {



                current_velocity.text = (((float)_velocity_game - 20) / 70 * 100).ToString("0") + "%";



            }
        }


    }


    public Slider slider_velocity;
    public Text current_velocity;



    public Slider slider_range;
    public Text current_range;

    public Slider sliderLeft;
    public Text angleLeft;

    public Slider sliderRight;
    public Text angleRight;

    public Slider sliderMinLeft;
    public Text angleMinLeft;

    public Slider sliderMinRight;
    public Text angleMinRight;

    float _range_game;

    float _angleMinRight;
    float _angleMinLeft;
    float radius;
    float time;


    public float range
    {

        get
        {
            return _range_game;
        }
        set
        {
            _range_game = value;

            if (current_range != null)
            {

                current_range.text = (((float)_range_game - 15) / 15 * 100).ToString("0") + "%";


            }
        }


    }



    public float MinRight
    {

        get
        {
            return _angleMinRight;
        }
        set
        {
            _angleMinRight = value;

            if (angleMinRight != null)
            {

                angleMinRight.text = ((int)_angleMinRight).ToString("") + "°";


            }
        }


    }

    public float MinLeft
    {

        get
        {
            return _angleMinLeft;
        }
        set
        {
            _angleMinLeft = value;

            if (angleMinLeft != null)
            {

                angleMinLeft.text = ((int)_angleMinLeft).ToString("") + "°";


            }
        }


    }



    float _angleRight;
    float _angleLeft;




    public float Right
    {

        get
        {
            return _angleRight;
        }
        set
        {
            _angleRight = value;

            if (angleRight != null)
            {

                angleRight.text = ((int)_angleRight).ToString("") + "°";


            }
        }


    }

    public float Left
    {

        get
        {
            return _angleLeft;
        }
        set
        {
            _angleLeft = value;

            if (angleLeft != null)
            {

                angleLeft.text = ((int)_angleLeft).ToString("") + "°";


            }
        }


    }




    public Toggle toggleX;
    public Text movlattext;
    public bool movimientoLateral;

    public Dropdown game_mode;

    public void OnArmChanged()
    {



    }
    public void OnGameModeChanged()
    {


        if (game_mode.value == 1)
        {



            sliderMinLeft.maxValue = 50;
            sliderLeft.maxValue = 50;
            current_range.text = "No Disponible";
            toggleX.enabled = false;
            toggleX.isOn = false;
            movlattext.text = "No Disponible";
            movimientoLateral = false;
            slider_range.enabled = false;


        }

        if (game_mode.value == 0)
        {


            sliderMinLeft.maxValue = 180;
            sliderLeft.maxValue = 180;
            current_range.text = ((int)_range_game).ToString("");
            toggleX.enabled = true;
            toggleX.isOn = true;
            movlattext.text = "Movimiento Lateral";

            slider_range.enabled = true;


        }
    }

    public void OnGameTypeChanged()
    {
        if (numberRepetitions.value == 0)
        {
            sliderCurrentTime.minValue = 1;
            sliderCurrentTime.maxValue = 30;

        }
        else
        {
            sliderCurrentTime.minValue = 1;
            sliderCurrentTime.maxValue = 30;

        }

        sliderCurrentTime.value = sliderCurrentTime.minValue;
        time_default = time_default;
    }

    public void OnGameArmChanged()
    {

    }
    public void TutorialPhase()
    {

        ParametersPanel.SetActive(false);
        TutorialPanel.SetActive(true);

    }
    public void EndTutorial()
    {
        ParametersPanel.SetActive(true);
        TutorialPanel.SetActive(false);
    }
    void Start() {
        _angleLeft = 0;
        _angleMinLeft = 0;
        _range_game = 0;
        _velocity_game = 0;

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {

            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {

            Debug.Log("Cannot find GameController script");
        }
    }
    public void SlideTime()
    {
        if (numberRepetitions.value == 0)
        {

            time = sliderCurrentTime.value * 30f;

        }
        if (numberRepetitions.value == 1)
        {

        }

        
    }

    public void StartGame()
	{
        _velocity_game = slider_velocity.value;
        _range_game = slider_range.value;
        _angleMinLeft = sliderMinLeft.value;
        _angleLeft = sliderLeft.value;


         gameController.StartGame(_velocity_game,_range_game,toggleX.isOn,numberRepetitions.value,time,_repetitions,_range_game,_angleMinLeft,_angleLeft,game_mode.value,ArmSelection.value);

       // print(_angleLeft +","+_angleMinLeft + "," + _velocity_game + "," + _range_game);

    }



}
