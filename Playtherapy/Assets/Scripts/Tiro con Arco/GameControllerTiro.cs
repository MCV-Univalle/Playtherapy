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

    public Transform BowHead;
    public Transform Player;
    public Transform cameraTransform;
    public Transform ArrowFather;

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
        //Esto no se hace porque este gameObject ya viene desactivado en la escena
        //if (enemySpawner != null)
        //{
        //    enemySpawner.setActive(false);
        //}

        // Calcula y almacena el desplazamiento inicial de la cámara respecto al Player
        cameraOffset = new Vector3(-0.3f, 0.1f, -0.7f);

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

        AdjustCameraPosition();

        UpdateBowAnimation();

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



        InGame = true; //  Permitir que `Update()` funcione

    }

    public void updateScore(float score)
    {
        gameScore += score;
        ShowFloatingScore(score);
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
        Vector3 cameraLocalOffset = Quaternion.Euler(0, BowHead.eulerAngles.y, 0) * cameraOffset;
        cameraTransform.position = BowHead.position + cameraLocalOffset;

        // 🔹 Obtenemos la rotación en Y del arco sin afectar X ni Z
        Quaternion targetRotation = Quaternion.Euler(0, BowHead.eulerAngles.y, 0);

        // 🔹 Aplica la rotación corregida, agregando un ajuste para inclinar la cámara si es necesario
        cameraTransform.rotation = targetRotation;

        // 🔹 Opcional: Pequeña rotación extra para ver la cuerda del arco más claramente
        cameraTransform.Rotate(0, 0, 0); // Ajusta estos valores si es necesario
    }


    void UpdateBowAnimation()
    {
        if (skeletonManager == null)
        {
            return; // Si no hay detección, no mover la cámara
        }

        // Verificar si la animación no ha sido iniciada
        if (!animationStarted)
        {
            Debug.Log("Entre a empezar la animacion");
            // Activar el trigger para iniciar la animación
            bowAnimator.SetTrigger("StartBowAnimation");
            animationStarted = true;
        }

        // Verificar si la animación ha alcanzado el frame 43
        AnimatorStateInfo state = bowAnimator.GetCurrentAnimatorStateInfo(0);

       

        if (state.IsName("BowAnimation"))
        {
            // Calcular el progreso de la animación hasta el frame 43
            float animationProgress = Mathf.Clamp01(state.normalizedTime / 0.43f);
            Debug.Log("Soy el progreso de la animacion" + animationProgress);
            // Definir la posición inicial y final de la flecha en el eje Z
            float initialZ = 0f; // Posición inicial de la flecha
            float finalZ = -0.8f; // Posición final de la flecha al tensar completamente

            // Interpolar la posición de la flecha en Z según el progreso de la animación
            float currentZ = Mathf.Lerp(initialZ, finalZ, animationProgress);
            Debug.Log("Soy el valor devuelto por math.lerp" + currentZ);
            // Aplicar la nueva posición a la flecha
            Vector3 arrowPosition = ArrowFather.localPosition;
            arrowPosition.z = currentZ;
            ArrowFather.localPosition = arrowPosition;
            // Debug.Log("Cambie la posicion de la flecha");

            if (state.normalizedTime >= 0.43f && !ShotFired)
            {
                // Pausar la animación en el frame 43
                bowAnimator.speed = 0f;

            }


            // Detectar el evento de disparo (reemplaza "Fire1" con tu input de disparo)
            if (shotEventFire && state.normalizedTime >= 0.43f)
            {
                // Realizar el disparo (implementa tu lógica aquí)
                Shoot();

                // Reanudar la animación
                bowAnimator.speed = 1f;

                // Reiniciar el state de la animación
                animationStarted = false;
                ShotFired = false;
            }
        }

        if (state.normalizedTime >= 1f)
        {
            // Reiniciar la animación
            bowAnimator.SetTrigger("StartBowAnimation");
        }
    }

    void Shoot()
    {
        // Implementa aquí la lógica de disparo
        // Por ejemplo, instanciar un proyectil, aplicar daño, etc.
        Debug.Log("Disparo realizado");
    }


}
