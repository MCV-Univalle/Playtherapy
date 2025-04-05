using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParametersController : MonoBehaviour, IParametersManager
{

    public GameObject ParametersPanel;
    public GameObject TutorialPanel;
    public GameControllerTiro gameControllerTiro;
    public Button startGameButton;
    //public Button confirmListButton;

    // Tiempo de juego
    public Slider timeSlider;
    public Text timeText;
    static private int _timeGame = 2;

    // Velocidad de movimiento de los enemigos
    public Dropdown speedDropdown;
    static private string _speedGame = "Paso medio";
    // Velocidad de moviemiento de los enemigos numerica 
    static public float enemySpeed = 5f;

    // Velocidad de aparicion de los enemigos
    public Dropdown spawnDropdown;
    static private string _spawnrate = "Moderado";
    // Velocidad de moviemiento de los enemigos numerica 
    static public float enemySpawnRate = 8f;

    // Rango de inclinacion de la cabeza
    public Slider headInclinationSlider;
    public Text headInclinationText;
    static private int _headInclination = 35;

    // Rango de abduccion del hombro
    public Slider shoulderRotationSlider;
    public Text shoulderRotationText;
    static private int _shoulderRotation = 40;

   
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Entre a ejecutar el start del parameter panel");
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {

            gameControllerTiro = gameControllerObject.GetComponent<GameControllerTiro>();
        }
        if (gameControllerTiro == null)
        {

            Debug.Log("Cannot find GameController script");
        }

        if (GameControllerTiro.gct != null)
        {
            startGameButton.onClick.AddListener(OnStartGameButtonPressed);

        }





        //Inicializar valores
        timeSlider.minValue = 1;
        timeSlider.maxValue = 5;
        timeSlider.value = _timeGame;
        timeText.text = _timeGame + " min";

        speedDropdown.options.Clear();
        speedDropdown.options.Add(new Dropdown.OptionData("Paso lento"));
        speedDropdown.options.Add(new Dropdown.OptionData("Paso medio"));
        speedDropdown.options.Add(new Dropdown.OptionData("Paso rápido"));
        speedDropdown.value = 1;

        spawnDropdown.options.Clear();
        spawnDropdown.options.Add(new Dropdown.OptionData("Lento"));
        spawnDropdown.options.Add(new Dropdown.OptionData("Moderado"));
        spawnDropdown.options.Add(new Dropdown.OptionData("Rápido"));
        spawnDropdown.value = 1;

        headInclinationSlider.minValue = 35;
        headInclinationSlider.maxValue = 45;
        headInclinationSlider.value = _headInclination;
        headInclinationText.text = _headInclination + "°";

        shoulderRotationSlider.minValue = 20;
        shoulderRotationSlider.maxValue = 60;
        shoulderRotationSlider.value = _shoulderRotation;
        shoulderRotationText.text = _shoulderRotation + "°";
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
        if (_speedGame == "Paso lento")
        {
            enemySpeed = 3f;
        }
        else if (_speedGame == "Paso medio")
        {
            enemySpeed = 5f;
        }
        else
        {
            enemySpeed = 8f;
        }
        Debug.Log("Se cambio a la velocidad: " + _speedGame + "en numerico: " + enemySpeed);
    }

    public void UpdateSpawn()
    {
        _spawnrate = spawnDropdown.options[spawnDropdown.value].text;
        if (_spawnrate == "Lento")
        {
            enemySpawnRate = 12f;
        }
        else if (_spawnrate == "Moderado")
        {
            enemySpawnRate = 8f;
        }
        else
        {
            enemySpawnRate = 5f;
        }
        Debug.Log("Se cambio a la velocidad de aparicion: " + _spawnrate + "en numerico: " + enemySpawnRate);
    }

    public void UpdateHeadInclination()
    {
        _headInclination = (int)headInclinationSlider.value;
        headInclinationText.text = _headInclination + "°";
    }

    public void UpdateShoulderRotation()
    {
        _shoulderRotation = (int)shoulderRotationSlider.value;
        shoulderRotationText.text = _shoulderRotation + "°";
    }

    public void TutorialPhase()
    {

        ParametersPanel.SetActive(false);
        TutorialPanel.SetActive(true);

    }

    public void StartGame()
    {
        Debug.Log("Ejecute el startgame, se deberian quitar las interfaces");
        GameControllerTiro.gct.StartGame(_timeGame, enemySpeed, enemySpawnRate, _headInclination, _shoulderRotation);
    }

    public void OnStartGameButtonPressed()
    {
       GameControllerTiro.gct.StartGame(_timeGame, enemySpeed, enemySpawnRate, _headInclination, _shoulderRotation);
    }
}
