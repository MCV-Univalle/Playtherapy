using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionWithEnemy : MonoBehaviour
{
    public float knockbackForce = 120f; // Fuerza del rebote
    public Transform enemy; // Referencia al NPC enemigo

    private Rigidbody rb;

    public GameControllerFrenesi gameControllerFrenesi;
    private bool timeReducted = false; // Para evitar restar varias veces seguidas
    private float timeLastReduction = 0f; // Controla el tiempo de la última resta

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemies"))
        {
            Debug.Log("Detecte la colision con el enemigo");
            //float laneWidth = 5.21f; // Ancho del carril
            float minX = -9.62f; // Límite izquierdo
            float maxX = -4.41f;  // Límite derecho
            Vector3 enemyPosition = other.transform.position;
            Vector3 playerPosition = transform.position;

            float direction = (playerPosition.x < enemyPosition.x) ? -1f : 1f; // Decide hacia dónde rebotar
            float newX = Mathf.Clamp(playerPosition.x + (direction * knockbackForce), minX, maxX); // Mantener en carriles

            // Aplicar rebote solo en X
            Vector3 knockback = new Vector3(newX - playerPosition.x, 0, 0) * knockbackForce;
            rb.AddForce(knockback, ForceMode.Impulse);

            

            

        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemies") && !timeReducted)
        {
            if (Time.time - timeLastReduction > 1.0f) // 1 segundo de cooldown
            {
                gameControllerFrenesi.ReduceTime();
                timeReducted = true;
                timeLastReduction = Time.time;
                StartCoroutine(RestoreDrag());
            }
        }
    }

    // Corrutina para restaurar el drag después de frenar
    IEnumerator RestoreDrag()
    {
        rb.drag = 10f; // Aumenta la fricción temporalmente para frenar rápido
        yield return new WaitForSeconds(0.5f); // Espera 0.5s para evitar que siga deslizándose
        rb.drag = 0f; // Restaura el drag a su estado normal
        timeReducted = false;
    }
}
