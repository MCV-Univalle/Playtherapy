using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Si la velocidad es suficiente, actualizar la rotación de la flecha
        if (rb.velocity.sqrMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity.normalized);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        transform.parent = collision.transform;


        // Obtener el Animator del objeto colisionado
        Animator enemyAnimator = collision.transform.GetComponent<Animator>();

        // Si el objeto tiene el tag "Enemy" y un Animator, activar la animación
        if (collision.transform.CompareTag("Enemy") && enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("Dying");
        }

        Destroy(rb);
        Destroy(this);
    }
}