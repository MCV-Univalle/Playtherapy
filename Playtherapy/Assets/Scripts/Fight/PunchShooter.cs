using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchShooter : MonoBehaviour {


    public GameObject Proyectile;
    public GameObject Objective;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "LeftHand") {
            GameObject Temporary_Bullet_Handler;
            Temporary_Bullet_Handler = Instantiate(Proyectile, other.transform.position, other.transform.rotation) as GameObject;
            var vector = new Vector3(-(float)(other.transform.position.x - Objective.transform.position.x), -(float)(other.transform.position.y - Objective.transform.position.y), (float)(-Objective.transform.position.z)).normalized * 50;//force

            Temporary_Bullet_Handler.GetComponent<Rigidbody>().velocity = vector;
        }

        if (other.tag == "RightHand")
        {
            GameObject Temporary_Bullet_Handler;
            Temporary_Bullet_Handler = Instantiate(Proyectile, other.transform.position, other.transform.rotation) as GameObject;
            var vector = new Vector3((float)(other.transform.position.x - Objective.transform.position.x), (float)(other.transform.position.y - Objective.transform.position.y), (float)(other.transform.position.z - Objective.transform.position.z)) * 10;//force

            Temporary_Bullet_Handler.GetComponent<Rigidbody>().velocity = vector;
        }




    }
}
