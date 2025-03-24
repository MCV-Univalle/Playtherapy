using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemySpawnerTiro : MonoBehaviour
{
    public GameObject[] meleeEnemies;  // 2 tipos de melee
    public GameObject[] rangedEnemies; // 2 tipos de rango

    public Transform spawnPoint1, spawnPoint2; // Puntos de spawn
    public Transform destinationMelee1, destinationMelee2; // Destinos melee
    public Transform destinationRanged3, destinationRanged4; // Destinos rango

    public float baseSpawnRate = 3f; // Tiempo base de spawn
    private float currentSpawnRate;

    private int maxRangedEnemies = 8; // Límite total de enemigos de rango
    private int rangedEnemiesInGame = 0; // Contador de enemigos de rango en el mapa
    private List<GameObject> rangedEnemiesList = new List<GameObject>(); // Lista para seguimiento

    void Start()
    {
        currentSpawnRate = baseSpawnRate;
        InvokeRepeating("SpawnEnemy", 0f, currentSpawnRate);
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = Random.Range(0, 2) == 0 ? spawnPoint1 : spawnPoint2;
        bool isMelee = Random.Range(0, 2) == 0;

        GameObject enemyPrefab;
        Transform targetDestination;

        if (isMelee)
        {
            enemyPrefab = meleeEnemies[Random.Range(0, meleeEnemies.Length)];
            targetDestination = spawnPoint == spawnPoint1 ? destinationMelee1 : destinationMelee2;
        }
        else
        {
            if (rangedEnemiesInGame >= maxRangedEnemies) return; // No spawnea más si se alcanzó el límite

            enemyPrefab = rangedEnemies[Random.Range(0, rangedEnemies.Length)];
            targetDestination = spawnPoint == spawnPoint1 ? destinationRanged3 : destinationRanged4;
            rangedEnemiesInGame++;
        }

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        EnemyAITiro enemyAI = newEnemy.GetComponent<EnemyAITiro>();
        if (enemyAI != null)
        {
            enemyAI.SetTarget(targetDestination, isMelee, this);
        }

        if (!isMelee)
        {
            rangedEnemiesList.Add(newEnemy);
        }

        AdjustSpawnRate();
    }

    void AdjustSpawnRate()
    {
        currentSpawnRate = Mathf.Clamp(baseSpawnRate - (rangedEnemiesInGame * 0.2f), 1.5f, baseSpawnRate);
        CancelInvoke();
        InvokeRepeating("SpawnEnemy", currentSpawnRate, currentSpawnRate);
    }

    public void EnemyDied(GameObject enemy, bool isRanged)
    {
        if (isRanged)
        {
            rangedEnemiesInGame--;
            rangedEnemiesList.Remove(enemy);
        }
    }
}