using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotPosition : MonoBehaviour {

    bool value;
    private GameController gameController;

    // Use this for initialization
    void Start()
    {
        // source = GetComponent<AudioSource>();
        value = false;
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

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        //print("collition");


        if (other.tag == "IPosition")
        {
            //source = GetComponent<AudioSource>();
            //source.Play();

            //HelpText.text = "Posición Correcta";
            value = true;

            GameController.gc.indicatorplayer = false;

        }
        else
        {

         


        }



    }


}
