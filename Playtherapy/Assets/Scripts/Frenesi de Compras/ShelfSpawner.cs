using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShelfSpawner : MonoBehaviour
{
    public GameObject[] shelfPrefabs; 
    public Transform leftSide;  
    public Transform rightSide; 
    public float spaceBetweenShelves = 1.0f; 
    public float aisleLength = 10.0f; 

    void Start()
    {
        SpawnShelves(leftSide, -1);  // -1 porque los estantes van a la izquierda
        SpawnShelves(rightSide, 1);  // 1 porque los estantes van a la derecha
    }

    void SpawnShelves(Transform startPoint, int direction)
    {
        // Seleccionamos aleatoriamente 3 estantes diferentes
        List<GameObject> selectedShelves = shelfPrefabs.OrderBy(x => Random.value).Take(3).ToList();

        float currentZ = startPoint.position.z; // Posición inicial en el eje Z

        foreach (GameObject shelfPrefab in selectedShelves)
        {
            GameObject shelf = Instantiate(shelfPrefab, startPoint.position, Quaternion.identity);
            float shelfWidth = shelf.GetComponent<Renderer>().bounds.size.y; // Obtener ancho del estante

            // Posicionar el estante en el lado correcto
            shelf.transform.position = new Vector3(startPoint.position.x + (shelfWidth / 2 * direction), startPoint.position.y, currentZ);

            // Avanzar en el eje Z para el siguiente estante
            currentZ += shelf.GetComponent<Renderer>().bounds.size.z + spaceBetweenShelves;
        }
    }
}
