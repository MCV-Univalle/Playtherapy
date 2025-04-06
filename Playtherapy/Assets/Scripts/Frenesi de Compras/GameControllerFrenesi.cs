using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Leap.Unity;
using OpenNI;

public class GameControllerFrenesi : MonoBehaviour
{
    private RUISSkeletonManager skeletonManager;
    public GameObject kinectPlayer; // Modelo controlado por RUIS
    public GameObject Player; // Combinacion modelo mas carrito
    public GameObject mainCamera;
    public GameObject parametersPanel;
    public GameObject memoryPanel;
    public GameObject endGamePanel;
    public GameObject list;
    public GameObject timer;
    public GameObject BuyerNPCPrefab;
    public GameObject ShoppingListUI;
    public GameObject GenerateHallways;

    //public Button startGameButton;
    //public Button confirmListButton;
    public Toggle memorizeListToggle;
    //private bool isPreviewingList = false;

    public GameObject LeftShoulder;
    public GameObject RightShoulder;
    public GameObject Spine;
    public GameObject LeftHand;
    public GameObject RightHand;



    public float forwardSpeed = 5.0f; // Velocidad hacia adelante
    public float lateralSpeed = 7.0f; // Velocidad hacia los lados
    public float maxLeftPosition = -3.62f;// Distancia en x maxima a la que se puede mover hacia la izquierda
    public float maxRightPosition = 3.134f; // Distancia en x maxima a la que se puede mover hacia la derecha

    //public float rotationThreshold = 20f; // Umbral para la rotacion del torso

    static public float trunkRotationAngle;
    static public float trunkInclinationAngle;
    static public float shoulderFlexionAngle;
    static public string shoulderAbductionValue;

    private float calibrationOffset = 10f;

    public Vector3 offset; // Desplazamiento de la cámara respecto al jugador

    public bool InGame = false;


    public float currentTime = 120f;
    private float totalGameTime;
    public float timeMillis = 1000f;
    public int numberRepetitions = 0;
    public Text textCurrentTime;
    public Slider sliderCurrentTime;
    private bool GameOver = false;
    public float speed = 5f;
    public float itemCount = 8f;
    public bool memorizeGamemode = false;

    public GameObject FinalHallwayPrefab;

    public EnemySpawner enemySpawner;

    public GeneratingMap generatingMap;

    public AudioSource movementSound;


    static public GameControllerFrenesi gcf;
    PutDataResults dataResults;

    void Start()
    {
        gcf = gameObject.GetComponent<GameControllerFrenesi>();
        //enemyMovement = FindObjectOfType<EnemyMovement>();
        parametersPanel.SetActive(true);
        endGamePanel.SetActive(false);
        //list.SetActive(false);
        timer.SetActive(false);
        memoryPanel.SetActive(false);

        generatingMap = GenerateHallways.GetComponent<GeneratingMap>();
        if (generatingMap != null)
        {
            generatingMap.enabled = false;
        }

            Invoke("HideList", 0.001f);

        forwardSpeed = 0f;
        lateralSpeed = 0f;
        totalGameTime = currentTime;

        if (enemySpawner != null)
        {
            enemySpawner.enabled = false;
        }

        //startGameButton.onClick.AddListener(OnStartGameButtonPressed);
        //confirmListButton.onClick.AddListener(StartGame);
        //confirmListButton.onClick.AddListener(() => StartGame(currentTime, speed, itemCount, memorizeGamemode, trunkRotationAngle, trunkInclinationAngle, shoulderFlexionAngle, shoulderAbductionAngle));
        // Buscar el Skeleton Manager en la escena
        skeletonManager = FindObjectOfType<RUISSkeletonManager>();
        if (skeletonManager == null)
        {
            Debug.LogError("Falta el script RUISSkeletonManager en la escena.");
            return;
        }

        // Obtener control del esqueleto
        RUISSkeletonController[] kinectControllers = kinectPlayer.GetComponentsInChildren<RUISSkeletonController>();
        if (kinectControllers.Length > 0)
        {
            kinectControllers[0].updateRootPosition = false;
        }
        dataResults = endGamePanel.GetComponent<PutDataResults>();

    }

    void Update()
    {

        if (!InGame || GameOver) return;
        if (skeletonManager == null) return;

        // Moverse hacia adelante cuando se estiran los brazos hacia adelante
        if (AreArmsRaised())
        {
            MoveForward();
        }
        else
        {
            // Si el carrito se detiene, se detiene el sonido
            if (movementSound.isPlaying)
            {
                movementSound.Stop();
            }
        }



        MoveSideways();
        //UpdateTimer();
        //kinectPlayer.transform.position = new Vector3(Player.transform.position.x - 6.799438f, Player.transform.position.y, Player.transform.position.z - 17.80806f);

        if (numberRepetitions == 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime > 0)
            {
                timeMillis -= Time.deltaTime * 1000;
                if (timeMillis < 0)
                    timeMillis = 1000f;

                textCurrentTime.text = string.Format("{0:00}:{1:00}:{2:00}",
                    Mathf.FloorToInt(currentTime / 60),
                    Mathf.FloorToInt(currentTime % 60),
                    Mathf.FloorToInt(timeMillis * 60 / 1000));

                sliderCurrentTime.value = currentTime * 100 / 60f;

            }
            else
            {
                textCurrentTime.text = "00:00:00";
                EndGame();
            }
        }
        mainCamera.transform.position = Player.transform.position + offset;
        dataResults = endGamePanel.GetComponent<PutDataResults>();

    }

    //void OnStartGameButtonPressed()
    //{
    //    if (memorizeListToggle.isOn)
    //    {
    //        parametersPanel.SetActive(false);

    //        memoryPanel.SetActive(true);
    //        isPreviewingList = true;
    //    }
    //    else
    //    {

    //        StartGame(currentTime,speed,itemCount,memorizeGamemode,trunkRotationAngle,trunkInclinationAngle,shoulderFlexionAngle,shoulderAbductionAngle);

    //    }
    //}

    public void StartGame(float _timeGame, float enemySpeed, float _itemCount, bool _showListAlways, float _trunk, float _trunkInclination, float _shoulderFlexion, string _shoulderAbduction)
    {
        Debug.Log("¡Juego iniciado!");

        currentTime = _timeGame * 60;
        speed = enemySpeed;
        itemCount = _itemCount;
        memorizeGamemode = _showListAlways;
        trunkRotationAngle = _trunk;
        trunkInclinationAngle = _trunkInclination;
        shoulderFlexionAngle = _shoulderFlexion;
        shoulderAbductionValue = _shoulderAbduction;

        Debug.Log("la inclinacion del tronco pasada es " + trunkInclinationAngle);

        EnemyMovement enemyMovement = BuyerNPCPrefab.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.setEnemySpeed(speed);
            Debug.Log("Velocidad del prefab NPCBuyer cambiada a: " + speed);
        }

        else
        {
            Debug.LogError("No se encontró el script EnemyMovement en el prefab NPCBuyer.");
        }


        GenerateShoppingListContent generateShoppingListContent = ShoppingListUI.GetComponent<GenerateShoppingListContent>();
        if (generateShoppingListContent != null)
        {
            generateShoppingListContent.UpdateNumberOfProducts(itemCount);
            Debug.Log("Cantidad de productos cambiada a : " + itemCount);
        }

        else
        {
            Debug.LogError("No se encontró el script GenerateShoppingListContent en el prefab ShoppingListUI.");
        }

        //GeneratingMap generatingMap = GenerateHallways.GetComponent<GeneratingMap>();
        if (generatingMap != null)
        {
            generatingMap.setShoulderAbductionValue(shoulderAbductionValue);
            Debug.Log("Valor de abduccion de hombro cambiada a: " + shoulderAbductionValue);
        }
        else
        {
            Debug.LogError("No se encontró el script GeneratingMap en el prefab GenerateHallways.");
        }


        Parameters parameterPanelManager = parametersPanel.GetComponent<Parameters>();

        if (memorizeGamemode && !memoryPanel.activeSelf)
        {
            parametersPanel.SetActive(false);
            memoryPanel.SetActive(true);
            return;
        }

        if (Parameters.isPreviewingList)
        {
            memoryPanel.SetActive(false);
            parameterPanelManager.setIsPreviewingList(false);
            parametersPanel.SetActive(false);
        }
        else
        {
            parametersPanel.SetActive(false); // Ocultar pantalla de parámetros
        }

        InGame = true; //  Permitir que `Update()` funcione
        forwardSpeed = 5.0f; //  Restaurar la velocidad de movimiento
        lateralSpeed = 7.0f;

        if (!memorizeGamemode)
        {
            list.SetActive(true);
        }

        timer.SetActive(true);

        //FindObjectOfType<BackgroundMusic>().PlayBackgroundMusic();

        if (enemySpawner != null)
        {
            enemySpawner.enabled = true;
        }

        if (generatingMap != null)
        {
            generatingMap.enabled = true;
        }

        }

    public void EndGame()
    {
        GameOver = true;
        InGame = false;
        list.SetActive(false);
        timer.SetActive(false);

        forwardSpeed = 0f;
        lateralSpeed = 0f;
        movementSound.Stop();

        enemySpawner.StopSpawning(); // Detiene el InvokeRepeating
        Debug.Log("Juego terminado. Se detuvo el spawn de enemigos.");
        GameObject FakeHallway = GameObject.Find("FakeHallwayPrefab(Clone)");
        Destroy(FakeHallway);
        GameObject BuyerNPC = GameObject.Find("BuyerNPC(Clone)");
        Destroy(BuyerNPC);
        // Instanciar el prefab final

        SpawnEndPrefab();

        // Mover al jugador al centro
        StartCoroutine(EndGameSequence());

        // Moverlo hasta el final
        
        


    }

    void UpdateTimer()
    {
        totalGameTime -= Time.deltaTime;
        if (totalGameTime > 0)
        {
            timeMillis -= Time.deltaTime * 1000;
            if (timeMillis < 0)
                timeMillis = 1000f;

            textCurrentTime.text = string.Format("{0:00}:{1:00}:{2:00}",
                Mathf.FloorToInt(totalGameTime / 60),
                Mathf.FloorToInt(totalGameTime % 60),
                Mathf.FloorToInt(timeMillis * 60 / 1000));

            sliderCurrentTime.value = totalGameTime * 100 / currentTime;
        }
        else
        {
            textCurrentTime.text = "00:00:00";
            EndGame();
        }
    }

    void HideList()
    {
        list.SetActive(false);
    }

    public void ReduceTime()
    {
        currentTime -= 10f;
        if (currentTime < 0)
            currentTime = 0;
    }

    /*
    Explicacion de la relacion entre los angulos de abduccion de hombro y elevacion de brazo:

        - Ajustando el ángulo de abducción del hombro, se decide a qué altura se quieren detectar los brazos levantados.
        - Ajustando la elevación del brazo, se define cuánta tolerancia se permite para que el jugador pueda moverse.

    */

    private bool AreArmsRaised()
    {
        Vector3 leftShoulderPos = LeftShoulder.transform.position;
        Vector3 rightShoulderPos = RightShoulder.transform.position;
        Vector3 leftHandPos = LeftHand.transform.position;
        Vector3 rightHandPos = RightHand.transform.position;

        // Direcciones de los brazos
        Vector3 leftArmDir = leftHandPos - leftShoulderPos;
        Vector3 rightArmDir = rightHandPos - rightShoulderPos;

        // Eje lateral de los hombros (para proyectar correctamente)
        Vector3 leftLateral = LeftShoulder.transform.right;
        Vector3 rightLateral = RightShoulder.transform.right;

        // Proyección en el plano frontal del personaje (plano X-Z)
        Vector3 leftProjection = Vector3.ProjectOnPlane(leftArmDir, leftLateral);
        Vector3 rightProjection = Vector3.ProjectOnPlane(rightArmDir, rightLateral);

        // Obtener el ángulo desde abajo (0°) hasta arriba (180°)
        float leftAngle = Vector3.Angle(Vector3.down, leftProjection);
        float rightAngle = Vector3.Angle(Vector3.down, rightProjection);

        float shoulderFlexion = shoulderFlexionAngle;

        // Tolerancia para la detección
        float tolerance = 15f;

        // Comprobar si los brazos están dentro del rango configurado por shoulderFlexion y la tolerancia
        //float paraElDebug1 = Mathf.Abs(rightAngle - shoulderFlexion);

        //Debug.Log("El angulo derecho esta siendo: " + rightAngle + " El cual menos el angulo que mande por parametros " + "("+ shoulderFlexion  +")" + "da en valor absoluto: " 
        //    + paraElDebug1 + " Y este valor debe ser menor a la tolerancia : " + tolerance);

        return Mathf.Abs(leftAngle - shoulderFlexion) < tolerance ||
               Mathf.Abs(rightAngle - shoulderFlexion) < tolerance;
    }

    private void MoveForward()
    {

        Player.transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
        if (!movementSound.isPlaying)
        {
            movementSound.Play();
        }
        //mainCamera.transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
    }

    /*
    Para que el movimiento funcione los angulos deben:
            1. el angulo de inclinacion debe cumplirse o superarse
            2. dependiendo de la direccion de la rotacion del torso se, el personaje se mueve hacia la derecha o hacia la izquierda
    */

    private void MoveSideways()
    {
        // Obtener la rotación actual del tronco en ángulos de Euler
        Vector3 spineEulerAngles = Spine.transform.rotation.eulerAngles;

        // Normalizar el ángulo de inclinación en X al rango -180° a 180°
        float torsoInclinationX = spineEulerAngles.x;
        if (torsoInclinationX > 180f) torsoInclinationX -= 360f;


        float toleranceInclination = 10f; // Margen de error de 10 grados
        // Calcular la inclinación ajustada: 0° es posición recta, valores positivos indican inclinación hacia adelante
        float adjustedInclination = torsoInclinationX + toleranceInclination;

        // Obtener la rotación en Y del tronco y normalizarla al rango -180° a 180°
        float torsoRotationY = spineEulerAngles.y;
        if (torsoRotationY > 180f) torsoRotationY -= 360f;

        float toleranceRotation = 10f; // Margen de error de 10 grados

        // Umbral de inclinación y rotación desde la interfaz
        float inclinationThreshold = trunkInclinationAngle; // Valores entre 0 y 30 grados
        float rotationThreshold = trunkRotationAngle; // Umbral para detectar rotación lateral


        float movementDelta = 0f;
        //float paraElDebug1 = Mathf.Abs(adjustedInclination - inclinationThreshold);

        //Debug.Log("El angulo de inclinacion del tronco esta siendo: " + adjustedInclination + " el cual debe ser mayor al valor pasado por parametros : " + inclinationThreshold);
         Debug.Log("El angulo de rotacion del tronco esta siendo: " + torsoRotationY + " el cual debe ser mayor al valor pasado por parametros : " + rotationThreshold);
        // Verificar si la inclinación cumple el umbral
        if (adjustedInclination >= 0f && adjustedInclination >= inclinationThreshold)
        {
            

            // Determinar dirección solo con la rotación en Y
            if (torsoRotationY + toleranceRotation > rotationThreshold)
                {
                    movementDelta = lateralSpeed * Time.deltaTime; // Mover a la derecha
                }
                else if (torsoRotationY - toleranceRotation < -rotationThreshold)
                {
                    movementDelta = -lateralSpeed * Time.deltaTime; // Mover a la izquierda
                }

        }


        // Aplicar movimiento con restricciones
        float newX = Mathf.Clamp(Player.transform.position.x + movementDelta, maxLeftPosition, maxRightPosition);
        Player.transform.position = new Vector3(newX, Player.transform.position.y, Player.transform.position.z);
    }



    void SpawnEndPrefab()
    {
        // Encontrar el último pasillo generado
        GeneratingMap generateMap = FindObjectOfType<GeneratingMap>();

        if (generateMap == null)
        {
            Debug.LogError("No se encontró GeneratingMap en la escena.");
            return;
        }

        GameObject lastHallway = generateMap.GetLastHallway();
        if (lastHallway == null)
        {
            Debug.Log("no se encontro el ultimo pasillo;");
            return;
        }



        float sectionLength = generateMap.GetSectionLength();
        Vector3 spawnPos = lastHallway.transform.position + new Vector3(-6.66f, -0.3f, (sectionLength / 2) - 0.1574f);
        GameObject InstanceFinalHallwayPrefab = Instantiate(FinalHallwayPrefab, spawnPos, Quaternion.identity);
        FindObjectOfType<GeneratingMap>().enabled = false;
        //StartCoroutine(MoveToEnd(InstanceFinalHallwayPrefab.transform.position, 5f));
    }

    IEnumerator MovePlayerToCenter(float speed)
    {
        Vector3 start = Player.transform.position;
        Vector3 end = new Vector3(-6.87f, start.y, start.z); // Posición centrada

        while (Mathf.Abs(Player.transform.position.x - end.x) > 0.01f)
        {
            //Debug.Log(Player.transform.position);
            float newX = Mathf.MoveTowards(Player.transform.position.x, end.x, speed * Time.deltaTime);
            Player.transform.position = new Vector3(newX, Player.transform.position.y, Player.transform.position.z);
            mainCamera.transform.position = Player.transform.position + offset;
            yield return null;
        }

        Player.transform.position = end;
    }

    IEnumerator MoveToEnd(GameObject finalHallway, float speed)
    {
        // Calcula un punto dentro del FinalHallway para que el jugador vaya hacia allí
        Vector3 targetPosition = finalHallway.transform.position;
        targetPosition.x = -6.87f;
        targetPosition.y = 1.34f;

        Debug.Log("Moviéndose hacia: " + targetPosition);

        while (Vector3.Distance(Player.transform.position, targetPosition) > 0.1f)
        {
            float newX = Mathf.MoveTowards(Player.transform.position.x, targetPosition.x, speed * Time.deltaTime);
            float newZ = Mathf.MoveTowards(Player.transform.position.z, targetPosition.z, speed * Time.deltaTime);

            Player.transform.position = new Vector3(newX, Player.transform.position.y, newZ);
            mainCamera.transform.position = Player.transform.position + offset;
            yield return null;
        }

        Debug.Log("Llegó al final");
        yield return new WaitForSeconds(2f);
        FindObjectOfType<BackgroundMusic>().PlayGameOverMusic();
        endGamePanel.SetActive(true);
        string idMinigame = "12";
        //PutDataResults dataResults = endGamePanel.GetComponent<PutDataResults>();

        float totalProducts = itemCount; // Total de productos en la lista de compras
        int collectedProducts = HandCollissionWithProducts.productosRecolectados.Count; // Productos obtenidos

        // Calcular el porcentaje de rendimiento basado en los productos recolectados
        int performance = Mathf.RoundToInt(((float)collectedProducts / totalProducts) * 100);
        Debug.Log("Soy el performance " + performance);
        //GameSessionController gameCtrl = new GameSessionController();
        //gameCtrl.addGameSession(collectedProducts, this.numberRepetitions, this.totalGameTime, performance, idMinigame);

        // Actualizar los datos en la UI del panel de resultados
        PutDataResults dataResults = endGamePanel.GetComponent<PutDataResults>();
        dataResults.Minigame = idMinigame;
        dataResults.updateData(performance, 0);


        if (PlaylistManager.pm != null && PlaylistManager.pm.active)

        {
            PlaylistManager.pm.NextGame();
        }
    }

    IEnumerator EndGameSequence()
    {
        // Mover al jugador al centro primero
        yield return StartCoroutine(MovePlayerToCenter(7f));

        // Ahora sí, mover al jugador hacia el pasillo final
        GameObject finalPrefab = GameObject.Find("FinalHallway(Clone)"); // Encuentra el prefab final

        Debug.Log("encontre el pasillo final es (antes del if): " + finalPrefab.name);
        if (finalPrefab != null)
        {
            Debug.Log("encontre el pasillo final es: " + finalPrefab.name);
            yield return StartCoroutine(MoveToEnd(finalPrefab, 20f));
        }
        else
        {
            Debug.LogError("No se encontró el prefab final.");
        }

        Debug.Log("termine mi trabajo, terminaron la corrutinas lolololo");
    }

    //public void UpdateTimeFromSlider(float value)
    //{
    //    Debug.Log("soy el valor recibido del slider de tiempo: " + value);
    //    currentTime = value * 60;
    //}

    //public void UpdateTrunkRotationAngle(float value)
    //{
    //    Debug.Log("soy el valor recibido del slider de rotacion de tronco: " + value);
    //    int intValue = Mathf.RoundToInt(value);
    //    trunkRotationAngle = intValue;

    //}

    //public void UpdateTrunkInclinationAngle(float value)
    //{
    //    Debug.Log("soy el valor recibido del slider de inclinacion de tronco: " + value);
    //    int intValue = Mathf.RoundToInt(value);
    //    trunkInclinationAngle = intValue;

    //}

    //public void UpdateshoulderFlexionAngle(float value)
    //{
    //    Debug.Log("soy el valor recibido del slider de elevacion der brazo: " + value);
    //    int intValue = Mathf.RoundToInt(value);
    //    shoulderFlexionAngle = intValue;

    //}

    //public void UpdateShoulderAbductionAngle(float value)
    //{
    //    Debug.Log("soy el valor recibido del slider de abduccion de hombro: " + value);
    //    int intValue = Mathf.RoundToInt(value);
    //    shoulderAbductionAngle = intValue;

    //}

    //public void StartGame()
    //{

    //}




}

