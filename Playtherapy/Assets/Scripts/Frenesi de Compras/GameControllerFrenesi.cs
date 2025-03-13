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
    public Button startGameButton;
    public Button confirmListButton;
    public Toggle memorizeListToggle;
    private bool isPreviewingList = false;

    public GameObject LeftShoulder;
    public GameObject RightShoulder;
    public GameObject Spine;
    public GameObject LeftHand;
    public GameObject RightHand;



    public float forwardSpeed = 5.0f; // Velocidad hacia adelante
    public float lateralSpeed = 7.0f; // Velocidad hacia los lados
    public float maxLeftPosition = -3.62f;// Distancia en x maxima a la que se puede mover hacia la izquierda
    public float maxRightPosition = 3.134f; // Distancia en x maxima a la que se puede mover hacia la derecha
    public float rotationThreshold = 20f; // Umbral para la rotacion del torso
    public Vector3 offset; // Desplazamiento de la cámara respecto al jugador

    public bool InGame = false;


    public float currentTime = 120f;
    private float totalGameTime;
    public float timeMillis = 1000f;
    public int numberRepetitions = 0;
    public Text textCurrentTime;
    public Slider sliderCurrentTime;
    private bool GameOver = false;

    public GameObject FinalHallwayPrefab;

    public EnemySpawner enemySpawner;

    public AudioSource movementSound;

    void Start()
    {
        parametersPanel.SetActive(true);
        endGamePanel.SetActive(false);
        //list.SetActive(false);
        timer.SetActive(false);
        memoryPanel.SetActive(false);

        Invoke("HideList", 0.001f);

        forwardSpeed = 0f;
        lateralSpeed = 0f;
        totalGameTime = currentTime;

        if (enemySpawner != null)
        {
            enemySpawner.enabled = false;
        }

        startGameButton.onClick.AddListener(OnStartGameButtonPressed);
        confirmListButton.onClick.AddListener(StartGame);
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


    }

    void OnStartGameButtonPressed()
    {
        if (memorizeListToggle.isOn)
        {
            parametersPanel.SetActive(false);
            memoryPanel.SetActive(true);
            isPreviewingList = true;
        }
        else
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        Debug.Log("¡Juego iniciado!");

        if (isPreviewingList)
        {
            memoryPanel.SetActive(false);
            isPreviewingList = false;
        }
        else
        {
            parametersPanel.SetActive(false); // Ocultar pantalla de parámetros
        }

        InGame = true; //  Permitir que `Update()` funcione
        forwardSpeed = 5.0f; //  Restaurar la velocidad de movimiento
        lateralSpeed = 7.0f;

        if (!memorizeListToggle.isOn)
        {
            list.SetActive(true);
        }

        timer.SetActive(true);

        //FindObjectOfType<BackgroundMusic>().PlayBackgroundMusic();

        if (enemySpawner != null)
        {
            enemySpawner.enabled = true;
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
        GameObject FakeHallway = GameObject.Find("FakeHallway(Clone)");
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

    private bool AreArmsRaised()
    {
        Vector3 leftShoulderPos = LeftShoulder.transform.position;
        Vector3 rightShoulderPos = RightShoulder.transform.position;
        Vector3 leftHandPos = LeftHand.transform.position;
        Vector3 rightHandPos = RightHand.transform.position;

        float leftAngle = Vector3.Angle(Vector3.up, leftHandPos - leftShoulderPos);
        float rightAngle = Vector3.Angle(Vector3.up, rightHandPos - rightShoulderPos);

        return Mathf.Abs(leftAngle - 110) < 20 || Mathf.Abs(rightAngle - 110) < 20;
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

    private void MoveSideways()
    {
        float torsoRotationY = Spine.transform.rotation.eulerAngles.y;
        if (torsoRotationY > 180) torsoRotationY -= 360; // Normalize rotation (-180 to 180)

        float movementDelta = 0;

        if (torsoRotationY > rotationThreshold)
        {
            movementDelta = lateralSpeed * Time.deltaTime;
        }
        else if (torsoRotationY < -rotationThreshold)
        {
            movementDelta = -lateralSpeed * Time.deltaTime;
        }

        // Aplicar movimiento de manera uniforme al GameObject completo
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

    public void UpdateTimeFromSlider(float value)
    {
        Debug.Log("soy el valor recibido del slider: " + value);
        currentTime = value * 60;
    }




}

