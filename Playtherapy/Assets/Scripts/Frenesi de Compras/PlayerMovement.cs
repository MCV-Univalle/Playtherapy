using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidad = 5f; // Velocidad de movimiento

    void Update()
    {
        // Mueve el jugador hacia adelante en el eje Z
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
    }
}