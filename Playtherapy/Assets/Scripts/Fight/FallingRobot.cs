using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRobot : MonoBehaviour {
    private AudioSource source;
    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();
    }

    
    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Floor")
        {
            source = GetComponent<AudioSource>();
            source.Play();


        }


    }
}
