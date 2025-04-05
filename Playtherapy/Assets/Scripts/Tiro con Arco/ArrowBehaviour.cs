using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{

    Rigidbody rb;
    float minVelocity = 0.1f;
    private GameControllerTiro gameController;

    //Sounds for hitting enemies
    public AudioClip GoblinSound;
    public AudioClip OrcSound;
    public AudioClip HitSound;

    AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameController = FindObjectOfType<GameControllerTiro>();
        audioSource = GetComponent<AudioSource>();
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
        if (collision.transform.CompareTag("Enemy"))
        {
            audioSource.PlayOneShot(HitSound);

            if (enemyAnimator != null)
            {
                enemyAnimator.SetTrigger("Dying");
                PlayImpactSound(collision.gameObject.name);
            }

            float points = GetScoreForEnemy(collision.gameObject.name);

            if (gameController != null)
            {
                gameController.updateScore(points);
                gameController.IncrementDefeatedEnemies();
            }
        }

        Destroy(rb);
        Destroy(this);
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