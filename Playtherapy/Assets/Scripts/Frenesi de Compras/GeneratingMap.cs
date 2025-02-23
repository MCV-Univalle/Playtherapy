using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Purchasing;
using System.Security.Cryptography;

public class GenerationMap : MonoBehaviour
{
    public GameObject pasilloPrefab;
    public GameObject fakePasilloPrefab;
    public GameObject[] shelfPrefabs;
    public GameObject[] canastaProducts;
    public GameObject[] frituraLecheProducts;
    public Vector3 escalaLeche = new Vector3(0.13f, 0.13f, 0.13f);
    public Vector3 escalaProductos = new Vector3(0.6f, 0.6f, 0.6f);
 
    public int cantidadTramos = 5;
    public Transform jugador;
    public float spaceBetweenShelves = 1.0f; // Espacio entre estantes

    private Queue<GameObject> pasillos = new Queue<GameObject>();
    private Dictionary<GameObject, List<GameObject>> pasilloEstantes = new Dictionary<GameObject, List<GameObject>>(); // Para almacenar los estantes de cada pasillo
    private float longitudTramo;
    private GameObject fakePasillo;

    private float[] shelfHeights = { 2.11f, 1.14f, 0.09f };

    void Start()
    {
        Renderer renderer = pasilloPrefab.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            longitudTramo = renderer.bounds.size.z;
        }
        else
        {
            Debug.LogError("No se encontró Renderer en el prefab del pasillo.");
            return;
        }

        for (int i = 0; i < cantidadTramos; i++)
        {
            GameObject tramo = Instantiate(pasilloPrefab, new Vector3(0, 0, i * longitudTramo), Quaternion.identity);
            pasillos.Enqueue(tramo);
            SpawnShelves(tramo);
        }

        Vector3 ultimaPosicion = new Vector3(-7, 2, (cantidadTramos * longitudTramo) - longitudTramo);
        fakePasillo = Instantiate(fakePasilloPrefab, ultimaPosicion, Quaternion.identity);
        fakePasillo.transform.rotation = Quaternion.Euler(90, 180, 0);
    }

    void Update()
    {
        if (pasillos.Count == 0) return;

        GameObject primerTramo = pasillos.Peek();
        if (jugador.position.z > primerTramo.transform.position.z + longitudTramo / 2)
        {
            // Mover el pasillo al final
            pasillos.Dequeue();
            primerTramo.transform.position += Vector3.forward * longitudTramo * cantidadTramos;
            pasillos.Enqueue(primerTramo);

            // Reposicionar fakePasillo
            Vector3 nuevaPosicion = new Vector3(-7, 2, pasillos.Last().transform.position.z);
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

        float pasilloStartZ = pasillo.transform.position.z - longitudTramo; // Punto inicial del pasillo
        float pasilloEndZ = pasilloStartZ + longitudTramo; // Punto final del pasillo

        // Seleccionar 3 estantes para cada lado
        List<GameObject> leftShelves = shelfPrefabs.OrderBy(x => Random.value).Take(3).ToList();
        List<GameObject> rightShelves = shelfPrefabs.OrderBy(x => Random.value).Take(3).ToList();

        // Calcular espacio total disponible para los estantes (sin contar los bordes)
        float availableLength = longitudTramo - 2.0f; // Dejamos 1 unidad de margen en cada extremo

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

        if (shelfName == "ShelfMedium")
        {
            products = frituraLecheProducts.ToList();
            TrySpawnProduct(shelf, new Vector3(-0.7f, -0.7f, 0), isRightShelf);
            TrySpawnProduct(shelf, new Vector3(-0.7f, 0.7f, 0), isRightShelf);
        }
        else if (shelfName == "ShelfThreeLevelLarge")
        {
            products = canastaProducts.ToList();
            foreach (float height in shelfHeights)
            {
                TrySpawnProduct(shelf, new Vector3(-0.7f, -1.05f, height), isRightShelf);
                TrySpawnProduct(shelf, new Vector3(-0.7f, 1.05f, height), isRightShelf);
                //Debug.Log("Soy la altura imprimida " + height);
            }
        }
        else if (shelfName == "ShelfThreeLevelMedium")
        {
            products = canastaProducts.ToList();
            foreach (float height in shelfHeights)
            {
                TrySpawnProduct(shelf, new Vector3(-0.7f, 0, height), isRightShelf);
            }
        }
        else if (shelfName == "ShelfThreeFloorSmall")
        {
            products = frituraLecheProducts.ToList();
            foreach (float height in shelfHeights)
            {
                TrySpawnProduct(shelf, new Vector3(-0.7f, 0, height), isRightShelf);
            }
        }
    }

    void TrySpawnProduct(GameObject shelf, Vector3 localOffset, bool isRightShelf)
    {
        // 50% de probabilidad de spawnear un producto
        //if (Random.value > 0.8f) return;

        // Selecciona un producto aleatorio
        GameObject[] productList = (shelf.name.Contains("ThreeLevel")) ? canastaProducts : frituraLecheProducts;
        GameObject productPrefab = productList[Random.Range(0, productList.Length)];
        GameObject product = Instantiate(productPrefab, shelf.transform);
        product.transform.localScale = escalaProductos;
        // Ajusta posición relativa dentro del estante
        product.transform.localPosition = localOffset;


        product.transform.localRotation = Quaternion.Euler(-180, -180, -90);
        //bool isFrituraLeche = frituraLecheProducts.Contains(productPrefab);
        if (product.name.ToLower().Contains("leche"))
        {
            // Hacer algo específico para la leche
            product.transform.localRotation = Quaternion.Euler(0, -180, -90);
            product.transform.localScale = escalaLeche;
            Vector3 nuevaPosicion = product.transform.position;
            nuevaPosicion.y += 0.4f;
            if (isRightShelf)
            {
                nuevaPosicion.x -= 0.3f;
            }
            else
            {
                nuevaPosicion.x += 0.3f;
            }
            
            product.transform.position = nuevaPosicion;
        }

        if (product.name.ToLower().Contains("bolsa"))
        {
            // Hacer algo específico para la leche
            product.transform.localRotation = Quaternion.Euler(180, -180, 180);
            Vector3 nuevaPosicion = product.transform.position;
            nuevaPosicion.y += 0.1f;
            product.transform.position = nuevaPosicion;

        }
    }


}
