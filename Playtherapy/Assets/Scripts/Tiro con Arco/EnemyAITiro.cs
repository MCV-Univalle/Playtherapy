using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAITiro : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform target;
    private bool isMelee;
    private EnemySpawnerTiro spawner;
    private bool hasReachedDestination = false;
    private Transform cameraTransform;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cameraTransform = Camera.main.transform;

    }

    public void SetTarget(Transform destination, bool melee, EnemySpawnerTiro enemySpawnerTiro)
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>(); // 👈 Reasigna si se perdió la referencia
            if (agent == null)
            {
                Debug.LogError("⚠️ NavMeshAgent sigue siendo null en " + gameObject.name);
                return;
            }
        }

        target = destination;
        isMelee = melee;
        spawner = enemySpawnerTiro;

        if (!isMelee)
        {
            AdjustPositionToAvoidOverlap(); 
        }
        else
        {
            agent.SetDestination(target.position); // Solo los melee van al destino fijo
        }
    }

    void Update()
    {
        if (!hasReachedDestination && agent.remainingDistance <= agent.stoppingDistance)
        {
            hasReachedDestination = true;
            agent.isStopped = true;   // Detiene el NavMeshAgent para evitar movimientos bruscos
            agent.updateRotation = false; // Evita que el NavMeshAgent siga ajustando la rotación
            if (!isMelee)
            {
                RotateToFaceCamera();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void AdjustPositionToAvoidOverlap()
    {
        float detectionRadius = 3f; // Radio de detección más realista
        float separationDistance = 2f; // Distancia mínima entre enemigos

        Collider[] colliders = Physics.OverlapSphere(target.position, detectionRadius);
        int nearbyEnemies = 0;

        foreach (var col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                nearbyEnemies++;
            }
        }

        // Separación controlada con un ángulo aleatorio en un radio específico
        Vector3 randomOffset = new Vector3(Random.Range(-separationDistance, separationDistance), 0, Random.Range(-separationDistance, separationDistance));
        Vector3 newPos = target.position + randomOffset;

        agent.SetDestination(newPos);
    }

    void RotateToFaceCamera()
    {
        Quaternion targetRotation = Quaternion.LookRotation(cameraTransform.position - transform.position);
        StartCoroutine(SmoothRotate(targetRotation));
    }

    System.Collections.IEnumerator SmoothRotate(Quaternion targetRotation)
    {
        float duration = 1.5f; // Duración de la animación en segundos
        float elapsed = 0f;
        Quaternion startRotation = transform.rotation;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation; // Asegurar que termine exactamente en la rotación correcta
    }

    private void OnDestroy()
    {
        if (!isMelee && spawner != null)
        {
            spawner.EnemyDied(gameObject, true);
        }
    }
}