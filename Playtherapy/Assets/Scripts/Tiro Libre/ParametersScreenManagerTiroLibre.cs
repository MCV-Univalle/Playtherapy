using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ParametersScreenManagerTiroLibre : MonoBehaviour, IParametersManager
{
    public GameObject parametersPanel;
    public Dropdown dropdownGameType;
    public Slider sliderGameType;
    public Text labelGameType;
    public Toggle toggleFront;
    public InputField inputFrontAngle1;
    public InputField inputFrontAngle2;
    public InputField inputFrontAngle3;
    public Toggle toggleBack;
    public InputField inputBackAngle1;
    public InputField inputBackAngle2;
    public InputField inputBackAngle3;
    public Toggle toggleShifts;
    public Slider sliderShiftsFrequency;
    public Text labelShiftsFrequency;
    public Slider sliderTimeBetweenTargets;
    public Text labelTimeBetweenTargets;
    public Toggle toggleSustained;
    public Slider sliderSustained;
    public Toggle toggleChangeMovement;
    public Toggle toggleLow;
    public Toggle toggleMedium;
    public Toggle toggleHigh;
    public GameObject GameSessionController;

    public void StartGame()
    {
        bool withTime = false;
        float time = 0;
        int repetitions = 0;
        float timeBetweenTargets = sliderTimeBetweenTargets.value * 0.5f;
        bool frontPlane = toggleFront.isOn;
        bool backPlane = toggleBack.isOn;
        bool shifts = toggleShifts.isOn;
        float shiftsFrequency = sliderShiftsFrequency.value * 10;
        bool changeMovement = toggleChangeMovement.isOn;
        //bool sustained = toggleSustained.isOn;
        bool useLow = toggleLow.isOn;
        bool useMedium = toggleMedium.isOn;
        bool useHigh = toggleHigh.isOn;

        if (dropdownGameType.value == 1)
        {
            withTime = true;
            time = sliderGameType.value * 30;
        }
        else
        {
            repetitions = (int)sliderGameType.value;
        }

        if (GameManagerTiroLibre.gm)
        {
            GameManagerTiroLibre.gm.StartGame(withTime, time, repetitions, timeBetweenTargets, frontPlane,
                float.Parse(inputFrontAngle1.text), float.Parse(inputFrontAngle2.text), float.Parse(inputFrontAngle3.text),
                backPlane, float.Parse(inputBackAngle1.text), float.Parse(inputBackAngle2.text), float.Parse(inputBackAngle3.text),
                shifts, shiftsFrequency, changeMovement, useLow, useMedium, useHigh);
        }

        if (parametersPanel != null)
            parametersPanel.SetActive(false);
    }

    public void OnGameTypeChanged()
    {
        if (dropdownGameType.value == 1)
        {
            sliderGameType.minValue = 1f;
            sliderGameType.maxValue = 20f;
        }
        else
        {
            sliderGameType.minValue = 5;
            sliderGameType.maxValue = 80;
        }

        sliderGameType.value = sliderGameType.minValue;
    }

    public void OnGameTypeSliderValueChanged()
    {
        if (dropdownGameType.value == 1)
        {
            float time = sliderGameType.value * 30f;
            labelGameType.text = ((int)time / 60).ToString("00") + ":" + ((int)time % 60).ToString("00") + " mins";
        }
        else
        {
            labelGameType.text = sliderGameType.value.ToString();
        }
    }

    public void OnFrontToggleValueChanged()
    {
        inputFrontAngle1.interactable = toggleFront.isOn && toggleLow.isOn;
        inputFrontAngle2.interactable = toggleFront.isOn && toggleMedium.isOn;
        inputFrontAngle3.interactable = toggleFront.isOn && toggleHigh.isOn;
    }

    public void OnBackToggleValueChanged()
    {
        inputBackAngle1.interactable = toggleBack.isOn && toggleLow.isOn;
        inputBackAngle2.interactable = toggleBack.isOn && toggleMedium.isOn;
        inputBackAngle3.interactable = toggleBack.isOn && toggleHigh.isOn;
    }

    public void OnShiftsFrequencySliderValueChanged()
    {
        labelShiftsFrequency.text = (sliderShiftsFrequency.value * 10) + "%";
    }

    public void OnTimeBetweenTargetsSliderValueChanged()
    {
        labelTimeBetweenTargets.text = (sliderTimeBetweenTargets.value * 0.5f) + " segs";
    }

    public void OnLowToggleValueChanged()
    {
        inputFrontAngle1.interactable = toggleLow.isOn && toggleFront.isOn;
        inputBackAngle1.interactable = toggleLow.isOn && toggleBack.isOn;
    }

    public void OnMediumToggleValueChanged()
    {
        inputFrontAngle2.interactable = toggleMedium.isOn && toggleFront.isOn;
        inputBackAngle2.interactable = toggleMedium.isOn && toggleBack.isOn;
    }

    public void OnHighToggleValueChanged()
    {
        inputFrontAngle3.interactable = toggleHigh.isOn && toggleFront.isOn;
        inputBackAngle3.interactable = toggleHigh.isOn && toggleBack.isOn;
    }

    public void SendGame(int result, float time, float repetitions, int score, string minigame)
    {

        string date = date = DateTime.Now.ToString("yyyy-MM-dd");
        GameSessionController gameCtrl = new GameSessionController();
        gameCtrl.addGameSession(result, repetitions, time, score, minigame);



    }
}
