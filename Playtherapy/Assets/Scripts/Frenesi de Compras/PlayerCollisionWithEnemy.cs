using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollisionWithEnemy : MonoBehaviour
{
    public float knockbackForce = 3f; // Fuerza del rebote
    public Transform enemy; // Referencia al NPC enemigo

    private Rigidbody rb;

    public GameControllerFrenesi gameControllerFrenesi;
    private bool timeReducted = false; // Para evitar restar varias veces seguidas
    private float timeLastReduction = 0f; // Controla el tiempo de la última resta
    public AudioSource collisionSound;
    public Text ReducedTime;
    public CanvasGroup reducedTimeCanvasGroup;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (ReducedTime != null)
        {
            ReducedTime.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemies"))
        {
            Debug.Log("Detecté la colisión con el enemigo");

            if (collisionSound != null)
            {
                collisionSound.Play();
            }

            float minX = -9.62f; // Límite izquierdo
            float maxX = -4.41f; // Límite derecho
            Vector3 playerPosition = transform.position;

            // Determinar la dirección del knockback según el borde más cercano
            float distanceToLeft = Mathf.Abs(playerPosition.x - minX);
            float distanceToRight = Mathf.Abs(playerPosition.x - maxX);
            float direction = (distanceToLeft < distanceToRight) ? 1f : -1f; // Si está más cerca del borde izquierdo, se mueve a la derecha

            // Calcular la nueva posición
            float newX = Mathf.Clamp(playerPosition.x + (direction * knockbackForce), minX, maxX);

            // Aplicar el knockback solo en X
            Vector3 knockback = new Vector3(newX - playerPosition.x, 0, 0) * knockbackForce;
            rb.AddForce(knockback, ForceMode.VelocityChange);

            if (ReducedTime != null)
            {
                ReducedTime.text = "-10 segundos";
                ReducedTime.gameObject.SetActive(true);
                StartCoroutine(AnimateAndHideReducedTime());
                // StartCoroutine(HideMessage());
            }
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
                StartCoroutine(StopKnockback());
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

    IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(0.2f); // Espera un poco después del empuje
        rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z); // Elimina la velocidad en X
    }

    //IEnumerator HideMessage()
    //{
    //    yield return new WaitForSeconds(2);
    //    if (ReducedTime != null)
    //    {
    //        ReducedTime.gameObject.SetActive(false);
    //    }
    //}
    IEnumerator AnimateAndHideReducedTime()
    {
        reducedTimeCanvasGroup.alpha = 1;
        RectTransform rt = ReducedTime.GetComponent<RectTransform>();

        // Escala inicial
        rt.localScale = Vector3.zero;

        // Animación de "rebote" de escala (como punch)
        float duration = 0.3f;
        float elapsed = 0f;
        Vector3 targetScale = Vector3.one * 1.2f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            rt.localScale = Vector3.Lerp(Vector3.zero, targetScale, Mathf.Sin(t * Mathf.PI));
            elapsed += Time.deltaTime;
            yield return null;
        }

        rt.localScale = Vector3.one;

        // Esperar un momento visible
        yield return new WaitForSeconds(1f);

        // Fade out
        float fadeDuration = 0.5f;
        elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            reducedTimeCanvasGroup.alpha = Mathf.Lerp(1, 0, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        reducedTimeCanvasGroup.alpha = 0;
        ReducedTime.gameObject.SetActive(false);
    }
}
