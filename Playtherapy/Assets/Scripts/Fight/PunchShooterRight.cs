using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchShooterRight : MonoBehaviour {



    public GameObject Proyectile;
    public GameObject[] array;
    public GameObject Objective;
    public GameObject mark;
    GameObject Right;


    // Use this for initialization
    void Start()
    {


        array = GameObject.FindGameObjectsWithTag("Robot");
        Objective = array[0];

        Right = GameObject.FindGameObjectsWithTag("RightHand")[0];

        Destroy(Instantiate(mark, Right.transform.position, Right.transform.rotation) as GameObject, 1.0f);

    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "RightHand")
        {
            GameObject Temporary_Bullet_Handler;
            Temporary_Bullet_Handler = Instantiate(Proyectile, this.transform.position, this.transform.rotation) as GameObject;
            var vector = new Vector3(-(float)(other.transform.position.x - Objective.transform.position.x), -(float)(other.transform.position.y - Objective.transform.position.y), (float)(-Objective.transform.position.z)).normalized * 50;//force

            Temporary_Bullet_Handler.GetComponent<Rigidbody>().velocity = vector;
            Destroy(gameObject);
        }

    }
   
}
