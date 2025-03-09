using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Leap.Unity;

public class GameControllerFrenesi : MonoBehaviour
{
    private RUISSkeletonManager skeletonManager;
    public GameObject kinectPlayer; // Modelo controlado por RUIS
    public GameObject Player; // Combinacion modelo mas carrito
    public GameObject mainCamera;
    public GameObject endGamePanel;
    public GameObject list;
    public GameObject timer;


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

    public bool InGame = true;
    public float currentTime = 60f;
    public float timeMillis = 1000f;
    public int numberRepetitions = 0;
    public Text textCurrentTime;
    public Slider sliderCurrentTime;
    private bool GameOver = false;





    void Start()
    {
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
        if (skeletonManager == null) return;

        // Moverse hacia adelante cuando se estiran los brazos hacia adelante
        if (AreArmsRaised())
        {
            MoveForward();
        }


        MoveSideways();
        //kinectPlayer.transform.position = new Vector3(Player.transform.position.x - 6.799438f, Player.transform.position.y, Player.transform.position.z - 17.80806f);
        if (!InGame || GameOver)
            return;

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

    void EndGame()
    {
        GameOver = true;
        InGame = false;
        list.SetActive(false);
        timer.SetActive(false);
        endGamePanel.SetActive(true);
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


}

