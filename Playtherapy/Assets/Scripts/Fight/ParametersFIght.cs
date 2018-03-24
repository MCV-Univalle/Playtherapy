using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ParametersFIght : MonoBehaviour {

    private GameControllerFight gameController;
    public GameObject ParametersPanel;
    public GameObject ResultsPanel;
    public GameObject TutorialPanel;
    public GameObject Eraser;
    public Dropdown Shoulder;

    float angleMin;
    public float AngleMin {

        get
        {
            return angleMin;
        }
        set
        {
            angleMin = value;

            if (angleMin_Text != null) {



                angleMin_Text.text = ((int)angleMin).ToString("")+"°";



            }
        }


    }


    public Slider angleMinSlider;
    public Text angleMin_Text;
    float angleMax;
    public float AngleMax
    {

        get
        {
            return angleMax;
        }
        set
        {
            angleMax = value;

            if (angleMax_Text != null)
            {



                angleMax_Text.text = ((int)angleMax).ToString("")+"°";



            }
        }


    }


    public Slider angleMaxSlider;
    public Text angleMax_Text;

    public Slider slider_velocity;
    public Text current_velocity;

    float _velocity_game;
    public float velocity
    {

        get
        {
            return _velocity_game;
        }
        set
        {
            _velocity_game = slider_velocity.value;

            if (current_velocity != null)
            {



                current_velocity.text = ((float)_velocity_game / 10 * 100).ToString("0") + "%";



            }
        }


    }

   

    // Use this for initialization
    void Start() {

        toggleF.isOn = false;
        toggleFA.isOn = true;
        toggleEX.isOn = false;
        _velocity_game = slider_velocity.minValue;
        ResultsPanel.SetActive(true);

        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameControllerFight");
        if (gameControllerObject != null)
        {

            gameController = gameControllerObject.GetComponent<GameControllerFight>();


        }


        if (gameController == null)

        {

            Debug.Log("Cannot find GameController script");
        }
        ResultsPanel.SetActive(false);

    }
    public void StartAgain()
    {

        GameObject ParticlesParent;
        Start();
        ParametersPanel.SetActive(true);
        ResultsPanel.SetActive(false);
        Eraser.SetActive(true);
        gameController.InGame = false;




    }

    


    public Toggle toggleFA;
    public Text FlexionAbduction;
    public bool FelxionAb;

    public Toggle toggleF;
    public Text Flexion;
    public bool FelxionOnly;

    public Toggle toggleEX;
    public Text Extension;


    public Text textCurrentTime;
    public Slider sliderCurrentTime;
    public Text sliderText;
    public Dropdown numberRepetitions;
    public float currentRepetitions;
    float _time_game;
    float _repetitions;


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

    public void OnGameSelectionChanged()
    {
        
    }

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

    public void TutorialButton() {

        ParametersPanel.SetActive(false);
        TutorialPanel.SetActive(true);


    }
    public void ExitTutorialButton() {

        ParametersPanel.SetActive(true);
        TutorialPanel.SetActive(false);

    }



    public void StartGameButton() {

        status = toggleF.isOn;
        statusEX = toggleEX.isOn;
        statusFA = toggleFA.isOn;


        if (angleMin > angleMax) {

            if (numberRepetitions.value == 0)
            {

                gameController.StartGame(angleMin, angleMin + 1, numberRepetitions.value, _time_game * 60, _velocity_game, status, statusEX, statusFA,Shoulder.value);
            }
            else
            {
                gameController.StartGame(angleMin, angleMin + 1, numberRepetitions.value, _repetitions, _velocity_game, status, statusEX, statusFA,Shoulder.value);
            }

        }
        else {

            if (numberRepetitions.value == 0)
            {

                gameController.StartGame(angleMin, angleMax, numberRepetitions.value, _time_game * 60, _velocity_game, status, statusEX, statusFA,Shoulder.value);
            }
            else
            {
                gameController.StartGame(angleMin, angleMax, numberRepetitions.value, _repetitions, _velocity_game, status, statusEX, statusFA,Shoulder.value);
            }


        }


        //print(status + "," + statusEX + "," + statusFA);
        ParametersPanel.SetActive(false);


    }

    public bool status;
    public bool GameModeChange {

        get {

            status = toggleF.isOn;

            return status;
        }
        set
        {

            if (toggleF.isOn == true)
            {

                toggleFA.isOn = false;
                toggleEX.isOn = false;
                angleMinSlider.maxValue = 180;
                angleMaxSlider.maxValue = 180;
            }
            
        }


    }
    public bool statusFA;
    public bool GameModeChangeFA
    {

        get
        {
            statusFA = toggleFA.isOn;

            return statusFA;
        }
        set
        {

            if (toggleFA.isOn == true)
            {

                toggleF.isOn = false;
                toggleEX.isOn = false;
                angleMinSlider.maxValue = 180;
                angleMaxSlider.maxValue = 180;

            }
            
        }


    }
    public bool statusEX;
    public bool GameModeChangeEX
    {

        get
        {
            statusEX = toggleEX.isOn;

            return statusEX;
        }
        set
        {

            if (toggleEX.isOn == true)
            {

                toggleF.isOn = false;
                toggleFA.isOn = false;
                angleMinSlider.maxValue = 50;
                angleMaxSlider.maxValue = 50;
            }
            
        }


    }





}
