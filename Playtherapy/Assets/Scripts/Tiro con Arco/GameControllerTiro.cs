using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Leap;
using OpenNI;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class GameControllerTiro : MonoBehaviour
{
    private RUISSkeletonManager skeletonManager;
    public GameObject kinectPlayer; // Modelo controlado por RUIS
    public GameObject timer;
    public GameObject parametersPanel;
    public Text textCurrentTime;
    public Slider sliderCurrentTime;

    public GameObject BowHead;
    ProjectileThrow projectileThrow;
    public Transform Player;
    public Transform cameraTransform;
    public Transform ArrowFather;

    public Transform leftHand;
    public Transform rightHand;
    private bool clapDetected = false; // Bandera para controlar la detección
    private string clapTurn = "First clap"; // Bandera para el tensado de la cuerda
    private bool _isTensioning;
    public bool isTensioning
    {
        get { return _isTensioning; }
        set
        {
            if (_isTensioning != value) 
            {
                _isTensioning = value;
                projectileThrow.updateIsTensioning(_isTensioning);
            }
        }
    }
    public bool shootArrow = false;
    public AnimationClip bowTensioningClip;
    public float arrowStartZ = 0f; // Posición inicial en Z de la flecha
    public float arrowEndZ = -0.5f; // Posición final en Z de la flecha al tensar completamente

   // public GameObject trajectoryObject; //Diseño de la preview de la trayectoria de la flecha


    public Animator bowAnimator;
    private bool animationStarted = false;
    private bool ShotFired = false;
    private bool shotEventFire = false;

    private Vector3 cameraOffset;

    private float totalGameTime;
    public float timeMillis = 1000f;
    public GameObject score;
    public Text textCurrentScore;
    public Slider sliderCurrentScore;
    static public float gameScore = 0;
    public Text floatingScorePrefab; // Prefab del texto animado 

    public bool InGame = false;
    private bool GameOver = false;

    //Variables del panel de parametros
    public float currentTime = 600f;
    public float speed;
    public float spawnRate;
    static public float headInclination;
    static public float shoulderRotation;

    //Script que spawnea enemigos
    public GameObject enemySpawner;
    EnemySpawnerTiro enemySpawnerTiro;

    //Prefabs de los enemigos
    public GameObject[] enemyPrefabs;


    //Variable para hacer referencia el gameController
    static public GameControllerTiro gct;

    // Start is called before the first frame update
    void Start()
    {
        gct = gameObject.GetComponent<GameControllerTiro>();
        timer.SetActive(false);
        score.SetActive(false);
        parametersPanel.SetActive(true);
        totalGameTime = currentTime;


        enemySpawnerTiro = enemySpawner.GetComponent<EnemySpawnerTiro>();
        projectileThrow = BowHead.GetComponent<ProjectileThrow>();

        //Esto no se hace porque este gameObject ya viene desactivado en la escena
        //if (enemySpawner != null)
        //{
        //    enemySpawner.setActive(false);
        //}

        // Calcula y almacena el desplazamiento inicial de la cámara respecto al Player
        cameraOffset = new Vector3(cameraTransform.localPosition.x, 0.5f, cameraTransform.localPosition.z - 1f);

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

    // Update is called once per frame
    void Update()
    {
        if (!InGame || GameOver) return;
        //if (isTensioning)
        //{
        //    ShowTrajectoryPreview();
        //}
        if (shootArrow)
        {
            projectileThrow.ThrowObject();
        }
        //if (isTensioning)
        //{
        //    projectileThrow.updateIsTensioning(isTensioning);
        //}
        AdjustCameraPosition();
        DetectClap();

        // UpdateBowAnimation();

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

        textCurrentScore.text = gameScore.ToString();

        if (Input.GetKeyDown(KeyCode.E)) // Simular eliminación con la tecla "E"
        {
            SimulateEnemyElimination("WarriorOrc"); // Puedes cambiar el enemigo
        }
        //sliderCurrentScore.value = gameScore;

    }

    public void StartGame(float _timeGame, float enemySpeed, float enemySpawnRate, float _headInclination, float _shoulderRotation)
    {
        Debug.Log("¡Juego iniciado!");

        currentTime = _timeGame * 60;
        speed = enemySpeed;
        spawnRate = enemySpawnRate;
        headInclination = _headInclination;
        shoulderRotation = _shoulderRotation;



        foreach (GameObject prefab in enemyPrefabs)
        {
            EnemyAITiro enemyAITiro = prefab.GetComponent<EnemyAITiro>();
            if (enemyAITiro != null)
            {
                enemyAITiro.setEnemySpeed(enemySpeed);
            }
            else
            {
                Debug.LogError($"No se encontró el script EnemyAITiro en el prefab {prefab.name}.");
            }
        }
        Debug.Log("Velocidad de los NPC's cambiada a: " + speed);

        if (enemySpawnerTiro != null)
        {
            enemySpawnerTiro.setEnemySpawnRate(spawnRate);
            enemySpawner.SetActive(true);
            Debug.Log("Valor de spawnRate cambiado a : " + spawnRate);
        }
        else
        {
            Debug.LogError("No se encontró el script enemySpawnerTiro en el prefab EnemySpawner.");
        }
        parametersPanel.SetActive(false);
        timer.SetActive(true);
        score.SetActive(true);



        InGame = true; //  Permitir que `Update()` funcione

    }

    public void updateScore(float score)
    {
        gameScore += score;
        ShowFloatingScore(score);
        Debug.Log("Puntaje actualizado: " + score);
    }

    void ShowFloatingScore(float score)
    {
        if (floatingScorePrefab == null) return;

        Vector3 offset = new Vector3(100f, -70f, 0f);
        Vector3 spawnPosition = textCurrentScore.transform.position + offset;
        Text floatingText = Instantiate(floatingScorePrefab, spawnPosition, Quaternion.identity, textCurrentScore.transform.parent);
        floatingText.text = (score > 0 ? "+" : "") + score.ToString();
        floatingText.color = score > 0 ? Color.green : Color.red; // Verde si es positivo, rojo si es negativo

        StartCoroutine(AnimateFloatingText(floatingText));
    }

    IEnumerator AnimateFloatingText(Text floatingText)
    {
        float duration = 1.5f;
        float elapsedTime = 0f;
        Vector3 startScale = Vector3.one * 1.2f; // Comienza un poco más grande
        Vector3 endScale = Vector3.one; // Regresa al tamaño normal
        Vector3 startPos = floatingText.transform.position;
        Vector3 endPos = startPos + new Vector3(0, 50, 0); // Se mueve hacia arriba
        Color startColor = floatingText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0); // Se desvanece

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            floatingText.transform.position = Vector3.Lerp(startPos, endPos, t);
            floatingText.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            floatingText.color = Color.Lerp(startColor, endColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(floatingText.gameObject); // 🗑️ Se destruye tras la animación
    }
    void AdjustCameraPosition()
    {
        // Verifica si el Kinect ha detectado el cuerpo
        if (skeletonManager == null)
        {
            return; // Si no hay detección, no mover la cámara
        }

        // 🔹 Calcula la posición correcta de la cámara con un offset rotado alrededor del arco
        Vector3 cameraLocalOffset = Quaternion.Euler(0, BowHead.transform.eulerAngles.y, 0) * cameraOffset;
        cameraTransform.position = BowHead.transform.position + cameraLocalOffset;

        // 🔹 Obtenemos la rotación en Y del arco sin afectar X ni Z
        Quaternion targetRotation = Quaternion.Euler(0, BowHead.transform.eulerAngles.y, 0);

        // 🔹 Aplica la rotación corregida, agregando un ajuste para inclinar la cámara si es necesario
        cameraTransform.rotation = targetRotation;

        // 🔹 Opcional: Pequeña rotación extra para ver la cuerda del arco más claramente
        cameraTransform.Rotate(0, 0, 0); // Ajusta estos valores si es necesario
    }


    void DetectClap()
    {
        shootArrow = false;
        Vector3 leftHandPos = leftHand.position;
        Vector3 rightHandPos = rightHand.position;
        float distance = Vector3.Distance(leftHandPos, rightHandPos);
        Debug.Log("La distancia que hay entres ambas manos es: " + distance);
        //float distanceX = Mathf.Abs(leftHandPos.x - rightHandPos.x);
        if (distance < 0.7f && !clapDetected) // Umbral de distancia y verificación de la bandera
        {
            clapDetected = true; // Marcar que el aplauso ha sido detectado  

            Debug.Log("¡Aplauso detectado!");
            //Debug.Log("La distancia que hay entres ambas manos es: " + distance);
            Debug.Log("El valor del clapTurn es: " + clapTurn + "Al segundo: " + currentTime);
            if (clapTurn == "First clap")
            {
                OnBowTensioning();
                isTensioning = true;
                clapTurn = "Second clap"; 
                Debug.Log("Se detecto la primer palmada");
            }
            else if (clapTurn == "Second clap")
            {
                isTensioning = false;
                shootArrow = true;
                OnBowShooting();     
                clapTurn = "First clap";
                Debug.Log("Se detecto la segunda palmada");
            }


        }
        else if (distance >= 1f && clapDetected)
        {
            Debug.Log("Detecte que se alejo de umbral especificado, ya no hay palmada");
            clapDetected = false;
        }
    }

    void OnBowTensioning()
    {
        bowAnimator.SetTrigger("StartBowTensioning");
    }

    //void ShowTrajectoryPreview()
    //{
    //    float angle = BowHead.transform.eulerAngles.x * Mathf.Deg2Rad; // Convertir a radianes
    //    angle = -angle; //Se invierte para que alzar lam mirada sea aumentar la distancia y viceversa
    //    //Vector3 launchDirection = BowHead.transform.forward; // Dirección en la que apunta el arco
    //    Vector3 launchDirection = BowHead.transform.forward; // Dirección en la que apunta el arco
    //    launchDirection.x = 0;
    //    launchDirection.y = 0;
    //    //launchDirection = -launchDirection;
    //    float launchSpeed = 5f; // Velocidad de lanzamiento ajustable
    //    float gravity = 9.81f; // Aceleración debido a la gravedad en m/s²
    //    Vector3 offset = new Vector3(0.2f, 0f,0.2f);
    //    // Componentes de la velocidad inicial
    //    Vector3 velocity = launchDirection * launchSpeed;
    //    velocity.y = Mathf.Sin(angle) * launchSpeed;

    //    List<Vector3> trajectoryPoints = new List<Vector3>();
    //    Vector3 startPosition = BowHead.transform.position + offset; // Posición del arco
    //    float timeStep = 0.05f; // Intervalo de tiempo entre puntos
    //    float totalTime = (2 * velocity.y) / gravity; // Tiempo total de vuelo
    //    int numPoints = Mathf.CeilToInt(totalTime / timeStep);

    //    for (int i = 0; i <= numPoints; i++)
    //    {
    //        float t = i * timeStep;
    //        float x = startPosition.x + velocity.x * t;
    //        float y = startPosition.y + velocity.y * t - 0.5f * gravity * t * t;
    //        float z = startPosition.z + velocity.z * t;
    //        trajectoryPoints.Add(new Vector3(x, y, z));
    //    }

    //    LineRenderer lineRenderer = trajectoryObject.GetComponent<LineRenderer>();
    //    lineRenderer.alignment = LineAlignment.TransformZ;
    //    Material lineMaterial = new Material(Shader.Find("Sprites/Default"));
    //    lineRenderer.material = lineMaterial;
    //    lineRenderer.positionCount = trajectoryPoints.Count;
    //    lineRenderer.SetPositions(trajectoryPoints.ToArray());
    //}



    void OnBowShooting()
    {
        bowAnimator.SetTrigger("StartBowShooting");
    }

    public void SimulateEnemyElimination(string enemyName)
    {
        Debug.Log("Simulando eliminación de enemigo...");
        float points = GetScoreForEnemy(enemyName);
        updateScore(points);
        Debug.Log($"Eliminaste un {enemyName}. Puntaje: {points}");
    }

    private float GetScoreForEnemy(string enemyName)
    {
        switch (enemyName)
        {
            case "WarriorOrc": return 175f;
            case "ShamanGoblin": return 125f;
            case "WarriorGoblin": return 100f;
            case "BlasterOrc": return 200f;
            default: return 50f;
        }
    }

}
