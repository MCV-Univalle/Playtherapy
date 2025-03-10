using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;
    private Transform jugador;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    void Update()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;

        // Si el enemigo pasó completamente al jugador, lo destruimos
        if (transform.position.z < jugador.position.z - 7) // -2 es un margen para asegurarnos que está fuera de la vista
        {
            Destroy(gameObject);
        }
    }
}
