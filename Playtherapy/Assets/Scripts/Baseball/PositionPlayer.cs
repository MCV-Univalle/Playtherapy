using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionPlayer : MonoBehaviour {
    public Text HelpText;


    // Use this for initialization
    void Start()
    {
        // source = GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {

        print("collition");

        if (other.tag == "Help")
        {
            //source = GetComponent<AudioSource>();
            //source.Play();

            HelpText.text = "Posición Correcta";
           



        }
        else
        {

            HelpText.text = "Posición Incorrecta";
        }

        

    }

    
}


