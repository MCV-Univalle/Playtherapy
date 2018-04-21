using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {

    private AudioSource source;
    private GameController gameController;


    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {

            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {

            Debug.Log("Cannot find GameController script");
        }
    }
    void OnTriggerStay(Collider other)
    {

        if (other.tag == "Help")
        {

            GameController.gc.HandInPosition = true;
        }
    }
    void OnTriggerExit(Collider other) {

        if(other.tag == "Help"){

            GameController.gc.HandInPosition = false;

        }


    }



        // Update is called once per frame
        void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Ball")
        {
            source = GetComponent<AudioSource>();
            source.Play();

      
        }

        
        else
        {
            GameController.gc.HandInPosition = false;
        }


    }
    }
