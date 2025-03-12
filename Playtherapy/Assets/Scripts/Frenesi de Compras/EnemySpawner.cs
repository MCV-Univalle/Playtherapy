using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Asigna el prefab del personaje
    public Transform jugador; // Asigna el transform del jugador
    public float spawnRate = 2f; // Frecuencia de aparición
    public float spawnDistance = 43.5f; // Qué tan atrás aparecen
    public Vector3[] spawnPositions; // Posiciones en los carriles
    private bool isSpawning = true;
    public AudioClip enemySound;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnRate);
    }


    void SpawnEnemy()
    {
        spawnPositions = new Vector3[] {
            new (-9.62f, 0, 0), // Carril izquierdo
            new (-6.87f, 0),  // Carril medio
            new (-4.41f, 0, 0)   // Carril derecho
        };
        if (spawnPositions.Length == 0) return;

        // Escoger un carril aleatorio
        Vector3 spawnPos = spawnPositions[Random.Range(0, spawnPositions.Length)];

        // Ajustar la posición en Z para que spawneen más atrás del jugador
        spawnPos.z = jugador.position.z + spawnDistance;

        // Instanciar el enemigo
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.Euler(0, 180, 0));

        AudioSource enemyAudio = newEnemy.GetComponent<AudioSource>();
        if (enemyAudio != null && enemySound != null)
        {
            //enemyAudio.clip = enemySound;
            enemyAudio.PlayOneShot(enemySound);
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        CancelInvoke(nameof(SpawnEnemy));
    }
}