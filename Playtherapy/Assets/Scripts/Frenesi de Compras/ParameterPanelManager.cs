using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parameters : MonoBehaviour
{
    public GameObject ParametersPanel;
    public GameObject TutorialPanel;

    // Tiempo de juego
    public Slider timeSlider;
    public Text timeText;
    private int _timeGame = 2;

    // Velocidad de los compradores
    public Dropdown speedDropdown;
    private string _speedGame = "Medio";

    // Elementos en la lista
    public Slider itemCountSlider;
    public Text itemCountText;
    private int _itemCount = 5;

    // Mostrar lista
    public Toggle showListToggle;
    private bool _showListAlways = true;

    // Rango de movimiento del tronco
    public Slider trunkSlider;
    public Text trunkText;
    private int _trunk = 20;

    // Rango de extension de los brazos
    public Slider armExtensionSlider;
    public Text armExtensionText;
    private int _armExtension = 45;

    // Rango de abduccion del hombro
    public Slider shoulderAbductionSlider;
    public Text shoulderAbductionText;
    private int _shoulderAbduction = 20;

    // Rango de inclinacion del tronco
    public Slider trunkInclinationSlider;
    public Text trunkInclinationText;
    private int _trunkInclination = 20;

    private void Start()
    {
        //Inicializar valores
        timeSlider.minValue = 2;
        timeSlider.maxValue = 10;
        timeSlider.value = _timeGame;
        timeText.text = _timeGame + " min";

        speedDropdown.options.Clear();
        speedDropdown.options.Add(new Dropdown.OptionData("Paso lento"));
        speedDropdown.options.Add(new Dropdown.OptionData("Paso medio"));
        speedDropdown.options.Add(new Dropdown.OptionData("Paso rápido"));
        speedDropdown.value = 1;

        itemCountSlider.minValue = 5;
        itemCountSlider.maxValue = 15;
        itemCountSlider.value = _itemCount;
        itemCountText.text = _itemCount.ToString();

        showListToggle.isOn = _showListAlways;

        trunkSlider.minValue = 20;
        trunkSlider.maxValue = 60;
        trunkSlider.value = _trunk;
        trunkText.text = _trunk + "°";

        armExtensionSlider.minValue = 45;
        armExtensionSlider.maxValue = 135;
        armExtensionSlider.value = _armExtension;
        armExtensionText.text = _armExtension + "°";

        shoulderAbductionSlider.minValue = 20;
        shoulderAbductionSlider.maxValue = 90;
        shoulderAbductionSlider.value = _shoulderAbduction;
        shoulderAbductionText.text = _shoulderAbduction + "°";

        trunkInclinationSlider.minValue = 20;
        trunkInclinationSlider.maxValue = 90;
        trunkInclinationSlider.value = _trunkInclination;
        trunkInclinationText.text = _trunkInclination + "°";
    }

    // Métodos para actualizar valores en tiempo real
    public void UpdateTime()
    {
        _timeGame = (int)timeSlider.value;
        timeText.text = _timeGame + " min";
    }

    public void UpdateSpeed()
    {
        _speedGame = speedDropdown.options[speedDropdown.value].text;
    }

    public void UpdateItemCount()
    {
        _itemCount = (int)itemCountSlider.value;
        itemCountText.text = _itemCount.ToString();
    }

    public void ToggleShowList()
    {
        _showListAlways = showListToggle.isOn;
    }

    public void UpdateTrunk()
    {
        _trunk = (int)trunkSlider.value;
        trunkText.text = _trunk + "°";
    }

    public void UpdateArmExtension()
    {
        _armExtension = (int)armExtensionSlider.value;
        armExtensionText.text = _armExtension + "°";
    }

    public void UpdateShoulderAbduction()
    {
        _shoulderAbduction = (int)shoulderAbductionSlider.value;
        shoulderAbductionText.text = _shoulderAbduction + "°";
    }

    public void UpdateTrunkInclination()
    {
        _trunkInclination = (int)trunkInclinationSlider.value;
        trunkInclinationText.text = _trunkInclination + "°";
    }

    public void TutorialPhase()
    {

        ParametersPanel.SetActive(false);
        TutorialPanel.SetActive(true);

    }

    public void StartGame()
    {
        Debug.Log("Iniciando el minijuego con los siguientes parámetros:");
        Debug.Log("Tiempo: " + _timeGame + " min");
        Debug.Log("Velocidad: " + _speedGame);
        Debug.Log("Cantidad de objetos en la lista: " + _itemCount);
        Debug.Log("Lista de compras siempre visible: " + _showListAlways);
        Debug.Log("Tronco: 20° a " + _trunk + "°");
        Debug.Log("Extensión de brazos: " + _armExtension + "°");
        Debug.Log("Abducción  del hombro: " + _shoulderAbduction + "°");
    }
}
