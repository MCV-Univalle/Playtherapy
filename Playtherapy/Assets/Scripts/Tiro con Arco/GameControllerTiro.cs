using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Xml.Linq;
using Leap;
using Leap.Unity.Attributes;
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
    public GameObject endGamePanel;
    public Text textCurrentTime;
    public Slider sliderCurrentTime;

    public GameObject BowHead;
    public GameObject Head;
    ProjectileThrow projectileThrow;
    public Transform Player;
    public Transform cameraTransform;
    public Transform ArrowFather;

    private float rotationSpeed = 15f;
    public float maxLeftAngle = -60f;// Distancia en x maxima a la que se puede mover hacia la izquierda
    public float maxRightAngle = 60f; // Distancia en x maxima a la que se puede mover hacia la derecha

    private float flexionSpeed = 15f;
    public float maxBackwardAngle = -40f;// Distancia en x maxima a la que se puede mover hacia la izquierda
    public float maxForwardAngle = 40f; // Distancia en x maxima a la que se puede mover hacia la derecha

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

    //Efectos de sonido
    public AudioSource bowTensioningAudioSource;  // Loop de tensión
    public AudioSource bowShootingAudioSource;    // Disparo único


    //Variable para hacer referencia el gameController
    static public GameControllerTiro gct;
    PutDataResults dataResults;
    private int totalEnemiesSpawned = 0; // Contador de enemigos generados
    private int totalEnemiesDefeated = 0; // Contador de enemigos eliminados
    public float maximumPossibleScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        gct = gameObject.GetComponent<GameControllerTiro>();
        timer.SetActive(false);
        score.SetActive(false);
        endGamePanel.SetActive(false);
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
        cameraOffset = new Vector3(cameraTransform.localPosition.x, 0.5f, -6f);

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

        UpdateBowHeadFlexion();
        UpdateBowHeadInclination();

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
        else
        {
            EndGame();
        }

        textCurrentScore.text = gameScore.ToString();

        if (Input.GetKeyDown(KeyCode.E)) // Simular eliminación con la tecla "E"
        {
            IncrementDefeatedEnemies();
            SimulateEnemyElimination("WarriorOrc"); // Puedes cambiar el enemigo
        }

        if (Input.GetKeyDown(KeyCode.T)) // Simular eliminación con la tecla "E"
        {
            currentTime = 1;
        }
        //sliderCurrentScore.value = gameScore;
        dataResults = endGamePanel.GetComponent<PutDataResults>();

    }

    public void StartGame(float _timeGame, float enemySpeed, float enemySpawnRate, float _headInclination)
    {
        Debug.Log("¡Juego iniciado!");

        currentTime = _timeGame * 60;
        speed = enemySpeed;
        spawnRate = enemySpawnRate;
        headInclination = _headInclination;



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

    public void EndGame()
    {
        Debug.Log("Juego Terminado");
        timer.SetActive(false);
        score.SetActive(false);
        FindObjectOfType<BackgroundMusic>().PlayGameOverMusic();
        endGamePanel.SetActive(true);

        // Detener el spawn de enemigos
        if (enemySpawnerTiro != null)
        {
            enemySpawner.SetActive(false);
        }

        // Eliminar a todos los enemigos en la escena
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        string idMinigame = "13";
        Debug.Log("Spawnearon " + totalEnemiesSpawned + " enemigos");
        Debug.Log("Acabe con " + totalEnemiesDefeated + " enemigos");

        int performance = Mathf.RoundToInt((totalEnemiesDefeated / (float)totalEnemiesSpawned) * 100);
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

        // Marcar el juego como finalizado
        InGame = false;
        GameOver = true;
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
        //Debug.Log("La distancia que hay entres ambas manos es: " + distance);
        //float distanceX = Mathf.Abs(leftHandPos.x - rightHandPos.x);
        if ((distance < 0.7f || Input.GetMouseButtonDown(0)) && !clapDetected) // Umbral de distancia y verificación de la bandera
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
        else if ((distance >= 1f || Input.GetMouseButtonDown(0)) && clapDetected)
        {
            Debug.Log("Detecte que se alejo de umbral especificado, ya no hay palmada");
            clapDetected = false;
        }
    }

    void OnBowTensioning()
    {
        bowAnimator.SetTrigger("StartBowTensioning");
        if (!bowTensioningAudioSource.isPlaying)
        {
            bowTensioningAudioSource.loop = true;
            bowTensioningAudioSource.Play();
        }
    }


    void OnBowShooting()
    {
        bowAnimator.SetTrigger("StartBowShooting");
        if (bowTensioningAudioSource.isPlaying)
        {
            bowTensioningAudioSource.Stop();
        }

        if (bowShootingAudioSource != null)
        {
            bowShootingAudioSource.loop = false;
            bowShootingAudioSource.PlayOneShot(bowShootingAudioSource.clip);
        }
    }

    //public void UpdateBowHeadFlexion()
    //{
    //    Transform HeadTransform = Head.transform;
    //    Transform BowHeadTransform = BowHead.transform;
    //    // Captura la rotación local en X del Head
    //    float flexionAngle = HeadTransform.localEulerAngles.x;
    //    // Ajuste para evitar el salto de 360 a 0
    //    if (flexionAngle > 180f)
    //        flexionAngle -= 360f;

    //    // Aplica ese ángulo a la rotación en X del BowHead (manteniendo Y y Z)
    //    Vector3 bowRotation = BowHeadTransform.localEulerAngles;
    //    bowRotation.x = flexionAngle;
    //    BowHeadTransform.localEulerAngles = bowRotation;
    //}

    public void UpdateBowHeadFlexion()
    {
        Transform HeadTransform = Head.transform;
        Transform BowHeadTransform = BowHead.transform;
        // Captura la rotación local en X del Head
        float flexionAngle = HeadTransform.localEulerAngles.x;
        // Ajuste para evitar el salto de 360 a 0
        if (flexionAngle > 180f)
            flexionAngle -= 360f;

        float delta = flexionSpeed * Time.deltaTime;
        // Aplica ese ángulo a la rotación en X del BowHead (manteniendo Y y Z)
        float currentX = BowHeadTransform.localEulerAngles.x;
        if (currentX > 180f) currentX -= 360f;

        float flexionThresholdForward = 15f;
        float flexionThresholdBackward = 1f;

        Debug.Log("El angulo actual de la inclinacion de la cabeza en X es: " + flexionAngle);

        Debug.Log("Entre al UpdateBowHeadFlexion, el valor de flexion angle es: " + flexionAngle + " y tambien el currentX" + currentX + " y el valor maximo hacia adelante es: " + maxForwardAngle);
        //Flexion hacia adelante
        if (flexionAngle > flexionThresholdForward && currentX < maxForwardAngle)
        {
            BowHeadTransform.localEulerAngles = new Vector3(
                currentX + delta,
                BowHeadTransform.localEulerAngles.y,
                BowHeadTransform.localEulerAngles.z
            );
        }
        //Flexion hacia atras
        else if (flexionAngle < -flexionThresholdBackward && currentX > maxBackwardAngle)
        {
            BowHeadTransform.localEulerAngles = new Vector3(
                currentX - delta,
                BowHeadTransform.localEulerAngles.y,
                BowHeadTransform.localEulerAngles.z
            );
        }
    }

    public void UpdateBowHeadInclination()
    {

        Transform HeadTransform = Head.transform;
        Transform BowHeadTransform = BowHead.transform;
        // Captura la inclinación lateral en Z del Head
        float headInclinationZ = HeadTransform.localEulerAngles.z;
        if (headInclinationZ > 180f)
            headInclinationZ -= 360f;



        //Parametro
        float inclinationThreshold = headInclination;

        float tolerance = 5f; // Pequeño margen de error

        float currentY = BowHeadTransform.localEulerAngles.y;
        if (currentY > 180f) currentY -= 360f;

        float delta = rotationSpeed * Time.deltaTime;

        if (headInclinationZ < -inclinationThreshold && currentY < maxRightAngle)
        {
            BowHeadTransform.localEulerAngles = new Vector3(
                BowHeadTransform.localEulerAngles.x,
                currentY + delta,
                BowHeadTransform.localEulerAngles.z
            );
        }
        // Inclinación hacia la izquierda
        else if (headInclinationZ > inclinationThreshold && currentY > maxLeftAngle)
        {
            BowHeadTransform.localEulerAngles = new Vector3(
                BowHeadTransform.localEulerAngles.x,
                currentY - delta,
                BowHeadTransform.localEulerAngles.z
            );
        }

    }

    //public void ApplyHeadTiltToBowDirection(Transform head, Transform bowHead)
    //{
    //    float headZ = head.localEulerAngles.z;

    //    // Corregimos valores mayores a 180°
    //    if (headZ > 180) headZ -= 360;

    //    // Aplicamos el valor de inclinación en Z a la rotación Y del BowHead
    //    float bowY = headZ; // Podés multiplicar por un factor si querés suavizar o amplificar

    //    bowHead.localRotation = Quaternion.Euler(bowHead.localEulerAngles.x, bowY, bowHead.localEulerAngles.z);
    //}

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

    public void IncrementSpawnedEnemies()
    {
        totalEnemiesSpawned++;
    }

    public void IncrementDefeatedEnemies()
    {
        totalEnemiesDefeated++;
    }


    public void changeMaximumPossibleScore(float score)
    {
      maximumPossibleScore += score;
    }
}
