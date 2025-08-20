using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Purchasing;
using System.Security.Cryptography;

public class GeneratingMap : MonoBehaviour
{
    public GameObject pasilloPrefab;
    public GameObject fakePasilloPrefab;
    public GameObject[] shelfPrefabs;
    public GameObject[] canastaProducts;
    public GameObject[] frituraLecheProducts;
    public Vector3 escalaLeche = new Vector3(0.6f, 0.6f, 0.6f);
    public Vector3 escalaPapitas = new Vector3(1.1f, 1.1f, 1.1f);
    public Vector3 escalaGaseosas = new Vector3(0.9f, 0.9f, 0.9f);
    public Vector3 escalaProductos = new Vector3(0.6f, 0.6f, 0.6f);

    public int cantidadTramos = 5;
    public Transform jugador;
    public float spaceBetweenShelves = 1.0f; // Espacio entre estantes

    private Queue<GameObject> hallways = new Queue<GameObject>();
    private Dictionary<GameObject, List<GameObject>> pasilloEstantes = new Dictionary<GameObject, List<GameObject>>(); // Para almacenar los estantes de cada pasillo
    private float sectionLength;
    private GameObject fakePasillo;

    private float[] shelfHeights = {  0.09f, 1.14f, 2.11f };

    static public string shoulderAbductionValue;

    void Start()
    {
        Renderer renderer = pasilloPrefab.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            sectionLength = renderer.bounds.size.z;
        }
        else
        {
            Debug.LogError("No se encontró Renderer en el prefab del pasillo.");
            return;
        }

        for (int i = 0; i < cantidadTramos; i++)
        {
            GameObject tramo = Instantiate(pasilloPrefab, new Vector3(0, 0, i * sectionLength), Quaternion.identity);
            hallways.Enqueue(tramo);
            SpawnShelves(tramo);
        }

        Vector3 ultimaPosicion = new Vector3(-7, 2, (cantidadTramos * sectionLength) - sectionLength);
        fakePasillo = Instantiate(fakePasilloPrefab, ultimaPosicion, Quaternion.identity);
        fakePasillo.transform.rotation = Quaternion.Euler(90, 180, 0);
    }

    void Update()
    {
        if (hallways.Count == 0) return;

        GameObject primerTramo = hallways.Peek();
        if (jugador.position.z > primerTramo.transform.position.z + sectionLength / 2)
        {
            // Mover el pasillo al final
            hallways.Dequeue();
            primerTramo.transform.position += Vector3.forward * sectionLength * cantidadTramos;
            hallways.Enqueue(primerTramo);

            // Reposicionar fakePasillo
            Vector3 nuevaPosicion = new Vector3(-7, 2, hallways.Last().transform.position.z);
            fakePasillo.transform.position = nuevaPosicion;

            // Eliminar los estantes anteriores y generar nuevos
            if (pasilloEstantes.ContainsKey(primerTramo))
            {
                foreach (GameObject shelf in pasilloEstantes[primerTramo])
                {
                    Destroy(shelf);
                }
                pasilloEstantes.Remove(primerTramo);
            }
            SpawnShelves(primerTramo);
        }
    }


    void SpawnShelves(GameObject pasillo)
    {
        List<GameObject> estantes = new List<GameObject>();

        float pasilloStartZ = pasillo.transform.position.z - sectionLength; // Punto inicial del pasillo
        float pasilloEndZ = pasilloStartZ + sectionLength; // Punto final del pasillo

        // Seleccionar 3 estantes para cada lado
        List<GameObject> leftShelves = shelfPrefabs.OrderBy(x => Random.value).Take(3).ToList();
        List<GameObject> rightShelves = shelfPrefabs.OrderBy(x => Random.value).Take(3).ToList();

        // Calcular espacio total disponible para los estantes (sin contar los bordes)
        float availableLength = sectionLength - 2.0f; // Dejamos 1 unidad de margen en cada extremo

        // Calcular la suma total de las profundidades de los estantes seleccionados
        float totalShelfDepthLeft = leftShelves.Sum(s => s.GetComponent<Renderer>().bounds.size.z);
        float totalShelfDepthRight = rightShelves.Sum(s => s.GetComponent<Renderer>().bounds.size.z);

        // Calcular el espaciado necesario para distribuirlos uniformemente
        float shelfSpacingLeft = (availableLength - totalShelfDepthLeft) / 2.0f;
        float shelfSpacingRight = (availableLength - totalShelfDepthRight) / 2.0f;

        // Asegurar que el espacio sea al menos un valor mínimo
        shelfSpacingLeft = Mathf.Max(shelfSpacingLeft, 0.5f);
        shelfSpacingRight = Mathf.Max(shelfSpacingRight, 0.5f);

        // Posición inicial de los estantes (dejamos margen)
        float startZLeft = pasilloStartZ + shelfSpacingLeft;
        float startZRight = pasilloStartZ + shelfSpacingRight;

        // Generar estantes de la izquierda
        foreach (GameObject shelfPrefab in leftShelves)
        {
            GameObject shelf = Instantiate(shelfPrefab);
            float shelfDepth = shelf.GetComponent<Renderer>().bounds.size.z;

            float extraYOffset = (shelfPrefab.name == "ShelfMedium") ? 1f : 0f;
            shelf.transform.SetPositionAndRotation(new Vector3(-13.4f, extraYOffset, startZLeft), Quaternion.Euler(-90, 180, 0));

            estantes.Add(shelf);
            startZLeft += shelfDepth + shelfSpacingLeft;

            // Ahora se spawnean los productos
            SpawnProducts(shelf, false);
        }

        // Generar estantes de la derecha
        foreach (GameObject shelfPrefab in rightShelves)
        {
            GameObject shelf = Instantiate(shelfPrefab);
            float shelfDepth = shelf.GetComponent<Renderer>().bounds.size.z;

            float extraYOffset = (shelfPrefab.name == "ShelfMedium") ? 1f : 0f;
            shelf.transform.position = new Vector3(-0.8f, extraYOffset, startZRight);

            estantes.Add(shelf);
            startZRight += shelfDepth + shelfSpacingRight;

            // Ahora se spawnean los productos
            SpawnProducts(shelf, true);
        }

        pasilloEstantes[pasillo] = estantes;
    }

    void SpawnProducts(GameObject shelf, bool isRightShelf)
    {
        string shelfName = shelf.name.Replace("(Clone)", "").Trim();
        List<GameObject> products = new List<GameObject>();

        // Determinar los productos según el tipo de estante
        if (shelfName == "ShelfMedium" || shelfName == "ShelfThreeFloorSmall")
        {
            products = frituraLecheProducts.ToList();
        }
        else if (shelfName == "ShelfThreeLevelLarge" || shelfName == "ShelfThreeLevelMedium")
        {
            products = canastaProducts.ToList();
        }


        // Determinar en qué niveles se generarán productos según la selección del usuario
        int maxLevel = shelfHeights.Length - 1; // Por defecto, todos los niveles (hasta el más alto)
        if (shoulderAbductionValue == "Solo primer piso (30°)")
        {
            maxLevel = 0; // Solo el primer nivel
        }
        else if (shoulderAbductionValue == "Primer y segundo piso (60°)")
        {
            maxLevel = 1; // Hasta el segundo nivel
        }
        // Si es "Todos los pisos (90°)", maxLevel se mantiene en 2 (o lo que sea shelfHeights.Length - 1)

        // Generar productos en los niveles seleccionados
        bool shelfMediumHandled = false;
        for (int i = 0; i <= maxLevel; i++)
        {
            float height = shelfHeights[i];

            if (shelfName == "ShelfThreeFloorSmall")
            {
                TrySpawnProduct(shelf, new Vector3(-0.7f, 0, height), isRightShelf);
            }
            else if (shelfName == "ShelfThreeLevelLarge")
            {
                TrySpawnProduct(shelf, new Vector3(-0.7f, -1.05f, height), isRightShelf);
                TrySpawnProduct(shelf, new Vector3(-0.7f, 1.05f, height), isRightShelf);
            }
            else if (shelfName == "ShelfThreeLevelMedium")
            {
                TrySpawnProduct(shelf, new Vector3(-0.7f, 0, height), isRightShelf);
            }

            if (shelfName == "ShelfMedium" && i >= 1 && !shelfMediumHandled)
            {
                TrySpawnProduct(shelf, new Vector3(-0.7f, -0.7f, 0), isRightShelf);
                TrySpawnProduct(shelf, new Vector3(-0.7f, 0.7f, 0), isRightShelf);
                shelfMediumHandled = true;
            }
        }
    }

    void TrySpawnProduct(GameObject shelf, Vector3 localOffset, bool isRightShelf)
    {

        if (shoulderAbductionValue == "Solo primer piso (30°)")
        {
            // 100% de probabilidad de spawnear un producto

        }
        else if (shoulderAbductionValue == "Primer y segundo piso (60°)")
        {
            // 70% de probabilidad de spawnear un producto
            if (Random.value > 0.7f) return;
        }
        else if (shoulderAbductionValue == "Todos los pisos (90°)")
        {
            // 50% de probabilidad de spawnear un producto
            if (Random.value > 0.5f) return;
        }


        // Selecciona un producto aleatorio
        GameObject[] productList = (shelf.name.Contains("ThreeLevel")) ? canastaProducts : frituraLecheProducts;
        GameObject productPrefab = productList[Random.Range(0, productList.Length)];
        GameObject product = Instantiate(productPrefab, shelf.transform);
        product.transform.localScale = escalaProductos;
        // Ajusta posición relativa dentro del estante
        product.transform.localPosition = localOffset;

        product.tag = "Producto";

        if (product.GetComponent<BoxCollider>() == null)
        {
            BoxCollider collider = product.AddComponent<BoxCollider>();

            collider.isTrigger = true;
            // Se hacen el collider un 50% mas grande de los productos pequeños
            //if (product.name.ToLower().Contains("leche") || product.name.ToLower().Contains("bolsa") || product.name.ToLower().Contains("botella"))
            //{
            //    collider.size *= 1.5f;
            //}
        }

        product.transform.localRotation = Quaternion.Euler(0, -180, -90);
        //bool isFrituraLeche = frituraLecheProducts.Contains(productPrefab);
        if (product.name.ToLower().Contains("botella"))
        {
            
            product.transform.localScale = escalaGaseosas;

        }

        if (product.name.ToLower().Contains("leche"))
        {
            // Hacer algo específico para la leche
            product.transform.localRotation = Quaternion.Euler(0, 90, 270);
            product.transform.localScale = escalaLeche;
            Vector3 nuevaPosicion = product.transform.position;
            nuevaPosicion.y += 0.4f;

            product.transform.position = nuevaPosicion;
        }

        if (product.name.ToLower().Contains("bolsa"))
        {
            // Hacer algo específico para la leche
            product.transform.localRotation = Quaternion.Euler(0, 180, 180);
            product.transform.localScale = escalaPapitas;
            Vector3 nuevaPosicion = product.transform.position;
            product.transform.position = nuevaPosicion;

        }

        
    }

    public GameObject GetLastHallway()
    {
        Debug.Log("Este es el ultimo pasillo: " + hallways.Last());
        return hallways.Count > 0 ? hallways.Last() : null;
    }

    public float GetSectionLength()
    {
        return sectionLength;
    }

    public void setShoulderAbductionValue(string shoulderAbduction)
    {
        shoulderAbductionValue = shoulderAbduction;
        Debug.Log("Entre a cambiar el valor de la abduccion de hombro en el generatingMap: " + shoulderAbduction);
    }


}
