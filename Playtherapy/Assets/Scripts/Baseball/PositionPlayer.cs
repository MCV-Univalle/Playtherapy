using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionPlayer : MonoBehaviour {
    public Text HelpText;
    bool value;

    // Use this for initialization
    void Start()
    {
        // source = GetComponent<AudioSource>();
        value = false;


    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        HelpText.text = "Posición Incorrecta";

        //print("collition");

        if (other.tag == "Help")
        {
            //source = GetComponent<AudioSource>();
            //source.Play();

            HelpText.text = "Posición Correcta";
            value = true;
           



        }
        else
        {

            HelpText.text = "Posición Incorrecta";
            value = false;
        }


        
    }

    void OntriggerExit(Collider other) {

        if (other.tag == "Help") {

            HelpText.text = "Posición Incorrecta";
            value = false;

        }

    }

  
    
}


