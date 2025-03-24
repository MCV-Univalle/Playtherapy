using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveProjectile : MonoBehaviour
{
    private Vector3 targetDirection;
    private float speed;
    private GameControllerTiro gameController;


    private void Start()
    {
        gameController = FindObjectOfType<GameControllerTiro>();
    }
    public void Initialize(Vector3 target, float speed)
    {
        this.speed = speed;
        targetDirection = (target - transform.position).normalized; // Dirección normalizada
    }

    void Update()
    {
        transform.position += targetDirection * speed * Time.deltaTime; // Movimiento manual

        // Se comprueba si el proyectil paso la camara
        Vector3 toProjectile = transform.position - Camera.main.transform.position;

        if (Vector3.Dot(Camera.main.transform.forward, toProjectile) < 0) // Si está detrás de la cámara
        {
            gameController.updateScore(-200f);
            Destroy(gameObject);
        }
    }
}