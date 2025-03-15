using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMovement : MonoBehaviour
{
    public Transform cart;
    public Transform[] wheels;
    public float wheelRatio = 0.4f;

    // Update is called once per frame
    void Update()
    {
        float distance = cart.position.z;
        float rotation = distance / (2 * Mathf.PI * wheelRatio) * 360;

        foreach (Transform wheel in wheels)
        {
            wheel.localRotation = Quaternion.Euler(0, rotation, 0);
        }
        
    }
}
