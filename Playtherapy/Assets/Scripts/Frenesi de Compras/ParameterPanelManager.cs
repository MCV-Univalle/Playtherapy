using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parameters : MonoBehaviour, IParametersManager
{
    public GameObject ParametersPanel;
    public GameObject TutorialPanel;
    public GameControllerFrenesi gameControllerFrenesi;
    public Button startGameButton;
    public Button confirmListButton;
    
    static public bool isPreviewingList = false;
    public GameObject memoryPanel;
    //public GenerateShoppingListContent generateShoppingListContent;

    // Tiempo de juego
    public Slider timeSlider;
    public Text timeText;
    static private int _timeGame = 2;

    // Velocidad de los compradores
    public Dropdown speedDropdown;
    private string _speedGame = "Paso medio";

    //Velocidad de los compradores numerica 
    public float enemySpeed = 5f;

    // Elementos en la lista
    public Slider itemCountSlider;
    public Text itemCountText;
    static private int _itemCount = 6;

    // Mostrar lista
    public Toggle showListToggle;
    private bool _showListAlways = true;

    // Rango de movimiento del tronco
    public Slider trunkSlider;
    public Text trunkText;
    static private int _trunk = 20;

    // Rango de extension de los brazos
    public Slider shoulderFlexionSlider;
    public Text shoulderFlexionText;
    static private int _shoulderFlexion = 90;

    // Rango de abduccion del hombro
    public Dropdown shoulderAbductionDropdown;
    private string _shoulderAbduction = "Primer y segundo piso (60°)";

    // Rango de inclinacion del tronco
    public Slider trunkInclinationSlider;
    public Text trunkInclinationText;
    static private int _trunkInclination = 0;

    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {

            gameControllerFrenesi = gameControllerObject.GetComponent<GameControllerFrenesi>();
        }
        if (gameControllerFrenesi == null)
        {

            Debug.Log("Cannot find GameController script");
        }

        if (GameControllerFrenesi.gcf != null)
        {
            startGameButton.onClick.AddListener(OnStartGameButtonPressed);
            confirmListButton.onClick.AddListener(() => GameControllerFrenesi.gcf.StartGame(_timeGame, enemySpeed, _itemCount, _showListAlways, _trunk, _trunkInclination, _shoulderFlexion, _shoulderAbduction));

        }


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

        itemCountSlider.minValue = 2;
        itemCountSlider.maxValue = 8;
        itemCountSlider.value = _itemCount;
        itemCountText.text = _itemCount.ToString();

        showListToggle.isOn = _showListAlways;

        trunkSlider.minValue = 20;
        trunkSlider.maxValue = 50;
        trunkSlider.value = _trunk;
        trunkText.text = _trunk + "°";

        shoulderFlexionSlider.minValue = 0;
        shoulderFlexionSlider.maxValue = 180;
        shoulderFlexionSlider.value = _shoulderFlexion;
        shoulderFlexionText.text = _shoulderFlexion + "°";

        shoulderAbductionDropdown.options.Clear();
        shoulderAbductionDropdown.options.Add(new Dropdown.OptionData("Todos los pisos (90°)"));
        shoulderAbductionDropdown.options.Add(new Dropdown.OptionData("Primer y segundo piso (60°)"));
        shoulderAbductionDropdown.options.Add(new Dropdown.OptionData("Solo primer piso (30°)"));
        shoulderAbductionDropdown.value = 1;

        trunkInclinationSlider.minValue = 0;
        trunkInclinationSlider.maxValue = 30;
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
        if (_speedGame == "Paso lento")
        {
            enemySpeed = 2f;
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

    public void UpdateItemCount()
    {
        _itemCount = (int)itemCountSlider.value;
        itemCountText.text = _itemCount.ToString();
        //GameObject shoppingListUIObject = GameObject.FindWithTag("ShoppingListUI");
        //if (shoppingListUIObject != null)
        //{

        //    generateShoppingListContent = shoppingListUIObject.GetComponent<GenerateShoppingListContent>();
        //}
        //if (generateShoppingListContent == null)
        //{

        //    Debug.Log("Cannot find GenerateShoppingContentScript script");
        //}

        //if (GenerateShoppingListContent.gsc != null)
        //{
        //    Debug.Log("Ejecute el update list, se deberian actualizar dinamicamente la lista luego de iniciar");
        //    GenerateShoppingListContent.gsc.UpdateProductList();
        //}

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

    public void UpdateShoulderFlexion()
    {
        _shoulderFlexion = (int)shoulderFlexionSlider.value;
        shoulderFlexionText.text = _shoulderFlexion + "°";
    }

    public void UpdateShoulderAbduction()
    {

       
        _shoulderAbduction = shoulderAbductionDropdown.options[shoulderAbductionDropdown.value].text;
        Debug.Log("Abducción  del hombro (la de la funcion): " + _shoulderAbduction);
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
        Debug.Log("Velocidad: " + _speedGame + "con velocidad: " + enemySpeed);
        Debug.Log("Cantidad de objetos en la lista: " + _itemCount);
        Debug.Log("Lista de compras siempre visible: " + _showListAlways);
        Debug.Log("Tronco: 20° a " + _trunk + "°");
        Debug.Log("Extensión de brazos: " + _shoulderFlexion + "°");
        Debug.Log("Abducción  del hombro: " + _shoulderAbduction);
            


        //GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        //if (gameControllerObject != null)
        //{

        //    gameControllerFrenesi = gameControllerObject.GetComponent<GameControllerFrenesi>();
        //}
        //if (gameControllerFrenesi == null)
        //{

        //    Debug.Log("Cannot find GameController script");
        //}

        //if (GameControllerFrenesi.gcf != null)
        //{
        //}
        Debug.Log("Ejecute el startgame, se deberian quitar las interfaces");
        if (showListToggle.isOn)
        {
            //ParametersPanel.SetActive(false);

            //memoryPanel.SetActive(true);
            isPreviewingList = true;
            GameControllerFrenesi.gcf.StartGame(_timeGame, enemySpeed, _itemCount, _showListAlways, _trunk, _trunkInclination, _shoulderFlexion, _shoulderAbduction);
        }
        else
        {
            GameControllerFrenesi.gcf.StartGame(_timeGame, enemySpeed, _itemCount, _showListAlways, _trunk, _trunkInclination, _shoulderFlexion, _shoulderAbduction);
        }
    }

    public void OnStartGameButtonPressed()
    {
        if (showListToggle.isOn)
        {
            //ParametersPanel.SetActive(false);

            //memoryPanel.SetActive(true);
            isPreviewingList = true;
            GameControllerFrenesi.gcf.StartGame(_timeGame, enemySpeed, _itemCount, _showListAlways, _trunk, _trunkInclination, _shoulderFlexion, _shoulderAbduction);
        }
        else
        {

            GameControllerFrenesi.gcf.StartGame(_timeGame, enemySpeed, _itemCount, _showListAlways, _trunk, _trunkInclination, _shoulderFlexion, _shoulderAbduction);

        }
    }

    public void setIsPreviewingList(bool isPreviewing)
    {
        isPreviewingList = isPreviewing;
    }






}
