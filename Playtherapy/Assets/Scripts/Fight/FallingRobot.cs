using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRobot : MonoBehaviour {
    private AudioSource source;
    public GameObject SmokeParticles;
    public GameObject floorParticles;
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


            GameObject Temporary_Bullet_Handler;
            GameObject Temporary_Bullet_Handler2;
            Temporary_Bullet_Handler = Instantiate(SmokeParticles, this.transform.position, this.transform.rotation) as GameObject;
            Temporary_Bullet_Handler2 = Instantiate(floorParticles, this.transform.position, this.transform.rotation) as GameObject;
            //var vector = new Vector3(-(float)(other.transform.position.x - Objective.transform.position.x), -(float)(other.transform.position.y - Objective.transform.position.y), (float)(-Objective.transform.position.z)).normalized * 50;//force

            //Temporary_Bullet_Handler.GetComponent<Rigidbody>().velocity = vector;

            Destroy(Temporary_Bullet_Handler,1.5f);
            Destroy(Temporary_Bullet_Handler2, 1.5f);


        }


    }
}
