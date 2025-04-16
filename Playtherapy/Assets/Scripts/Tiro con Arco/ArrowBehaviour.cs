using UnityEngine;
using UnityEngine.AI;

public class ArrowBehaviour : MonoBehaviour
{

    Rigidbody rb;
    float minVelocity = 0.1f;
    private GameControllerTiro gameController;

    //Sounds for hitting enemies
    public AudioClip GoblinSound;
    public AudioClip OrcSound;
    public AudioClip HitSound;
    private bool hasHit = false;

    AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameController = FindObjectOfType<GameControllerTiro>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (hasHit || rb == null)
            return;

        if (rb.velocity.magnitude <= minVelocity)
            return;

        transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity, Vector3.up);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;  
        hasHit = true;
        transform.parent = collision.transform;

        // Obtener el Animator del objeto colisionado
        Animator enemyAnimator = collision.transform.GetComponent<Animator>();
        NavMeshAgent enemyAgent = collision.transform.GetComponent<NavMeshAgent>();
        Rigidbody enemyRb = collision.transform.GetComponent<Rigidbody>();
        Collider enemyCol = collision.transform.GetComponent<Collider>();

        // Si el objeto tiene el tag "Enemy" y un Animator, activar la animación
        if (collision.transform.CompareTag("Enemy"))
        {
            if (enemyAnimator != null)
            {
                enemyAnimator.SetTrigger("Dying");
                PlayImpactSound(collision.gameObject.name);
            }

            audioSource.PlayOneShot(HitSound);

            // Detener al enemigo
            if (enemyAgent != null)
            {
                enemyAgent.isStopped = true;
                enemyAgent.velocity = Vector3.zero;
            }          

            float points = GetScoreForEnemy(collision.gameObject.name);

            if (gameController != null)
            {
                gameController.updateScore(points);
                
                gameController.IncrementDefeatedEnemies();
            }

            if (enemyRb != null) Destroy(enemyRb);
            if (enemyCol != null) Destroy(enemyCol);

            // Destruir Rigidbody y Collider de la flecha para que no siga afectando físicas
            Rigidbody rb = GetComponent<Rigidbody>();
            Collider col = GetComponent<Collider>();    
            if (rb != null) Destroy(rb);
            if (col != null) Destroy(col);
            Destroy(collision.gameObject, 10f);
        }

        //Destroy(rb);
        //Destroy(this);

    }

    private float GetScoreForEnemy(string enemyName)
    {
        switch (enemyName)
        {
            case "WarriorOrc": return 175f;
            case "ShamanGoblin": return 125f;
            case "WarriorGoblin": return 100f;
            case "BlasterOrc": return 200f;
            default: return 50f;
        }
    }

    private void PlayImpactSound(string enemyName)
    {
        AudioClip clipToPlay = GoblinSound;

        switch (enemyName)
        {
            case "WarriorOrc":
                clipToPlay = OrcSound;
                break;
            case "ShamanGoblin":
                clipToPlay = GoblinSound;
                break;
            case "WarriorGoblin":
                clipToPlay = GoblinSound;
                break;
            case "BlasterOrc":
                clipToPlay = OrcSound;
                break;
        }

        audioSource.PlayOneShot(clipToPlay);

    }

}