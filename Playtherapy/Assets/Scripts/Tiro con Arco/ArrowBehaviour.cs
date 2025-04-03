using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{

    Rigidbody rb;
    float minVelocity = 0.1f;
    private GameControllerTiro gameController;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameController = FindObjectOfType<GameControllerTiro>();
    }

    void Update()
    {
        if (rb.velocity.magnitude <= minVelocity)
            return;

        transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity, Vector3.up);

        if (Input.GetKeyDown(KeyCode.E)) // Simular eliminación con la tecla "E"
        {
            SimulateEnemyElimination("WarriorOrc"); // Puedes cambiar el enemigo
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        transform.parent = collision.transform;


        // Obtener el Animator del objeto colisionado
        Animator enemyAnimator = collision.transform.GetComponent<Animator>();

        // Si el objeto tiene el tag "Enemy" y un Animator, activar la animación
        if (collision.transform.CompareTag("Enemy"))
        {
            if (enemyAnimator != null)
            {
                enemyAnimator.SetTrigger("Dying");
            }

            float points = GetScoreForEnemy(collision.gameObject.name);

            if (gameController != null)
            {
                gameController.updateScore(points);
            }
        }

        Destroy(rb);
        Destroy(this);
    }

    private float GetScoreForEnemy(string enemyName)
    {
        switch (enemyName)
        {
            case "WarriorOrc":
                return 200f;
            case "ShamanGoblin":
                return 150f;
            case "WarriorGoblin":
                return 100f;
            case "BlasterOrc":
                return 250f;
            default:
                return 50f;
        }
    }

    public void SimulateEnemyElimination(string enemyName)
    {
        Debug.Log("I simulate eliminating");
        float points = GetScoreForEnemy(enemyName);
        gameController.updateScore(points);
        Debug.Log($"Eliminaste un {enemyName}. Puntaje: {points}");
    }
}