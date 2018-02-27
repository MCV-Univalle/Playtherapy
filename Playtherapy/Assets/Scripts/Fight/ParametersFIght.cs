using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ParametersFIght : MonoBehaviour {

    private GameControllerFight gameController;
    public GameObject ParametersPanel;

    float angleMin;
	public float AngleMin {

		get
		{
			return angleMin ;
		}
		set
		{
			angleMin = value;

			if (angleMin_Text!=null) {



				angleMin_Text.text = ((int)angleMin).ToString("");



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



                angleMax_Text.text = ((int)angleMax).ToString("");



            }
        }


    }


    public Slider angleMaxSlider;
    public Text angleMax_Text;

    // Use this for initialization
    void Start() {


        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameControllerFight");
        if (gameControllerObject != null)
        {

            gameController = gameControllerObject.GetComponent<GameControllerFight>();

      
        }

        print(gameController.name);
        if (gameController == null)
            
        {

            Debug.Log("Cannot find GameController script");
        }

    }


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



    public void StartGameButton() {


        

        if (angleMin == 0) {

            angleMin = 25;
        }
        if (angleMax == 0) {

            angleMax = 25;
        }

        if (angleMin > angleMax) {

            if (numberRepetitions.value == 0)
            {

                gameController.StartGame(angleMin, angleMin + 1, numberRepetitions.value, _time_game * 60);
            }
            else
            {
                gameController.StartGame(angleMin, angleMin + 1, numberRepetitions.value, _repetitions);
            }
            
        }
        else {

            if (numberRepetitions.value == 0)
            {

                gameController.StartGame(angleMin, angleMax, numberRepetitions.value, _time_game * 60);
            }
            else
            {
                gameController.StartGame(angleMin, angleMax,numberRepetitions.value, _repetitions);
            }
            

        }

            

            ParametersPanel.SetActive(false);
        
        
        }

   



    
	
	
}
