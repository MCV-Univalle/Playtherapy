using System.Collections.Generic;
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
    private Animator animator;
    public GameObject magicProjectilePrefab; // 🔥 Prefab de bola de magia
    public GameObject cannonBallPrefab; // 🔥 Prefab de bola de cañón
    public Transform firePoint; // 🔥 Lugar donde se dispara el proyectil
    public float fireRate = 2f; // 🔥 Tiempo entre disparos
    private bool isShooting = false;
    private List<GameObject> activeProjectiles = new List<GameObject>();
    private GameControllerTiro gameController;
    static public float enemySpeed;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemySpeed;
        cameraTransform = Camera.main.transform;
        animator = GetComponent<Animator>();
        gameController = FindObjectOfType<GameControllerTiro>();

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
        float distanceToTarget = agent.remainingDistance;

        if (!hasReachedDestination)
        {

            // Si el enemigo está cerca del destino, reduce la velocidad
            if (distanceToTarget <= 4)
            {
                agent.speed = Mathf.Lerp(agent.speed, 0, Time.deltaTime * 3); // Detiene suavemente
            }

            if (distanceToTarget <= 4)
            {
                hasReachedDestination = true;
                agent.isStopped = true;
                agent.updateRotation = false;

                if (!isMelee)
                {

                    RotateToFaceCamera();
                }
                else
                {
                    gameController.updateScore(-200f);
                    Destroy(gameObject);
                    
                }
            }
        }
        else if (distanceToTarget == 0)
        {
            if (!isMelee)
            {

                animator.SetTrigger("Shoot");
                //StartShooting();
            }
        }

    }

    //void StartShooting()
    //{
    //    if (!isShooting)
    //    {
    //        isShooting = true;
    //        InvokeRepeating(nameof(Shoot), 0f, fireRate); // Dispara en intervalos
    //    }
    //}

    void Shoot()
    {
        GameObject projectilePrefab = gameObject.name.Contains("Goblin") ? magicProjectilePrefab : cannonBallPrefab; // 🔄 Selecciona el proyectil correcto
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        MoveProjectile movement = projectile.AddComponent<MoveProjectile>(); // Se agrega script para moverlo
        movement.Initialize(cameraTransform.position, 20f); // Se le pasa el objetivo y la velocidad
        activeProjectiles.Add(projectile); // Se guarda el proyectil en la lista
    }

    void AdjustPositionToAvoidOverlap()
    {
        float detectionRadius = 10f;
        float separationDistance = 20f;

        Collider[] colliders = Physics.OverlapSphere(target.position, detectionRadius);
        int nearbyEnemies = 0;

        foreach (var col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                nearbyEnemies++;
            }
        }

        // Genera un desplazamiento aleatorio en un rango determinado
        Vector3 randomOffset = new Vector3(Random.Range(-separationDistance, separationDistance), 0, Random.Range(-separationDistance, separationDistance));
        Vector3 newPos = target.position + randomOffset;

        Debug.Log($"📍 Nueva posición generada: {newPos}");

        // Validar si `newPos` está en el NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(newPos, out hit, separationDistance, NavMesh.AllAreas))
        {
            newPos = hit.position; // Asegura que el punto sea válido
            Debug.Log($"✅ Posición válida en NavMesh: {newPos}");
            agent.SetDestination(newPos);
        }
        else
        {
            Debug.LogWarning("⚠️ No se encontró una posición válida en el NavMesh cerca de " + newPos);
        }
    }

    void RotateToFaceCamera()
    {
        Vector3 directionToCamera = cameraTransform.position - transform.position;
        directionToCamera.y = 0; // Bloquea la rotación en X y Z para que solo gire en Y

        Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
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


    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(target.position, 0.2f); // Destino original

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(agent.destination, 0.2f); // Destino actual del agente
    //}

    public void setEnemySpeed(float speed)
    {
        enemySpeed = speed;
    }

    private void OnDestroy()
    {
        if (!isMelee && spawner != null)
        {
            spawner.EnemyDied(gameObject, true);
        }
    }
}