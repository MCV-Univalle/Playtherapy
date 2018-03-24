using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleSwords : MonoBehaviour {


    public GameObject RightSword;
    public GameObject LeftSword;

	// Use this for initialization
	void Start () {
		
	}

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "LeftHand")
        {
            if (RightSword.activeSelf && LeftSword.activeSelf)
            {
                RightSword.SetActive(false);
                LeftSword.SetActive(false);

            }
            else {

                RightSword.SetActive(true);
                LeftSword.SetActive(true);

            }

        


        }
    }
}
