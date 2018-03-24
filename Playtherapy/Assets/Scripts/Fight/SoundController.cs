using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    private AudioSource source;


    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "PunchParticle")
        {
            source = GetComponent<AudioSource>();
            source.Play();


        }


    }
}
