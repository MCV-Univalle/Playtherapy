using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;
    private Transform jugador;
    //public Dropdown speedDropdown;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        //if (speedDropdown != null)
        //{
        //    // Suscribirse al evento cuando el dropdown cambia
        //    speedDropdown.onValueChanged.AddListener(SetEnemySpeed);
        //}
    }

    void Update()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;

        // Si el enemigo pasó completamente al jugador, lo destruimos
        if (transform.position.z < jugador.position.z - 7) // -2 es un margen para asegurarse que el npc está fuera de la vista
        {
            Destroy(gameObject);
        }
    }

    public void SetEnemySpeed(int optionIndex)
    {
        switch (optionIndex)
        {
            case 0: // Paso lento
                speed = 2f;
                break;
            case 1: // Paso medio
                speed = 5f;
                break;
            case 2: // Paso rápido
                speed = 8f;
                break;
        }

        Debug.Log("Velocidad de enemigos establecida en: " + speed);
    }
}
