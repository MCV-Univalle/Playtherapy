using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{

    Rigidbody rb;
    float minVelocity = 0.1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb.velocity.magnitude <= minVelocity)
            return;

        transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity, Vector3.up);
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