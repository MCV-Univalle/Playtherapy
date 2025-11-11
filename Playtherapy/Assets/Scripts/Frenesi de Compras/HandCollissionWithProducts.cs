using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;


public class HandCollissionWithProducts : MonoBehaviour
{
    // Arreglo para almacenar los productos recogidos
    public static Dictionary<string, int> productosRecolectados = new Dictionary<string, int>();
    //private GenerateShoppingListContent shoppingList;

    public GameObject endGamePanel;
    public GameObject list;
    public GameObject player;
    public GameObject cart; // Referencia al carrito
    public Transform spawnPoint; // Punto donde aparecerán los productos en el carrito
    public Text WarningMessage;
    public Text ReducedTime;
    public Text AlreadyCollectedProductWarningMessage;
    public CanvasGroup reducedTimeCanvasGroup;

    public GameObject[] productModels; // Arreglo con los modelos de los productos que caeran en el carrito

    private AudioSource audioSource;
    public AudioClip correctPickupSound;
    public AudioClip wrongPickupSound;

 
    // Start is called before the first frame update
    void Start()
    {
        ////shoppingList = FindObjectOfType<GenerateShoppingListContent>();
        //Debug.Log("soy la shopping list: " + shoppingList);
        endGamePanel.SetActive(false);
        list.SetActive(true);

        if (WarningMessage != null)
        {
            WarningMessage.gameObject.SetActive(false);
        }

        if (ReducedTime != null)
        {
            ReducedTime.gameObject.SetActive(false);
        }

        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // Simular recolección de todos los productos
        {
            SimularRecoleccionCompleta();
        }
    }

    // Método para detectar colisiones con productos
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Producto"))
        {
            string nombreObjeto = other.gameObject.name.Replace("(Clone)", "").Trim();
            //Debug.Log("soy el nombre objeto: " + nombreObjeto);

            if (GenerateShoppingListContent.selectedProducts.ContainsKey(nombreObjeto))
            {
                string nombreLista = GenerateShoppingListContent.selectedProducts[nombreObjeto];
                Debug.Log("soy el nombre lista/producto: " + nombreLista);
                if (productosRecolectados.ContainsKey(nombreLista))
                {
                    Debug.Log("entre al ++" + nombreLista);
                    //productosRecolectados[nombreLista]++;
                    Destroy(other.gameObject);

                    if (audioSource != null && wrongPickupSound != null)
                    {
                        audioSource.PlayOneShot(wrongPickupSound);
                    }

                    if (AlreadyCollectedProductWarningMessage != null)
                    {
                        AlreadyCollectedProductWarningMessage.text = $"Ya recolectaste este producto!";
                        WarningMessage.gameObject.SetActive(false);
                        AlreadyCollectedProductWarningMessage.gameObject.SetActive(true);
                        StartCoroutine(HideMessage());
                    }
                    return;
                }
                else
                {
                    Debug.Log("entre al = 1" + nombreLista);
                    productosRecolectados[nombreLista] = 1;
                }

                // Debug.Log($"Recolectado: {nombreLista}");

                GenerateShoppingListContent.MarkProductAsCollected(nombreObjeto);

                foreach (var item in productosRecolectados)
                {
                    Debug.Log($"Producto: {item.Key}, Cantidad: {item.Value}");
                }

                try
                {
                    CheckIfGameFinished();
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error en CheckIfGameFinished(): {e.Message}");
                }


                Destroy(other.gameObject);

                AddProductToCart(nombreLista);

                if (audioSource != null && correctPickupSound != null)
                {
                    audioSource.PlayOneShot(correctPickupSound);
                }

                // Verificar si se han recogido todos los elementos

            }
            else
            {
                // Producto incorrecto: penalizar tiempo
                Debug.Log($"{nombreObjeto} no esta en la lista!...");

                GameControllerFrenesi gameController = FindObjectOfType<GameControllerFrenesi>();
                if (gameController != null)
                {
                    gameController.ReduceTime();
                    gameController.wrongItems += 1;
                }

                if (WarningMessage != null)
                {
                    WarningMessage.text = $"Cuidado, este producto no esta en la lista!";
                    AlreadyCollectedProductWarningMessage.gameObject.SetActive(false);
                    WarningMessage.gameObject.SetActive(true);
                    StartCoroutine(HideMessage());
                }

                if (ReducedTime != null)
                {
                    ReducedTime.text = "-10 segundos";
                    ReducedTime.gameObject.SetActive(true);
                    StartCoroutine(AnimateAndHideReducedTime());
                    //StartCoroutine(HideMessage());
                }

                if (audioSource != null && wrongPickupSound != null)
                {
                    audioSource.PlayOneShot(wrongPickupSound);
                }

                // Opcional: destruir el producto incorrecto
                Destroy(other.gameObject);
            }
        }
    }

    void AddProductToCart(string nombreProducto)
    {
        GameObject modeloEncontrado = null;
        foreach (GameObject modelo in productModels)
        {
            //Debug.Log("soy el nombre modelo: " + modelo.name.ToLower());
            if (nombreProducto.ToLower().Contains(modelo.name.ToLower()))
            {
                modeloEncontrado = modelo;
                break;
            }
        }



        if (modeloEncontrado == null)
        {
            Debug.LogError($"No se encontró un modelo en modelosCarrito que coincida con: {nombreProducto}");
            return;
        }

        Vector3 spawnPosition = spawnPoint.position + new Vector3(
        Random.Range(-0.3f, 0.3f),
        0,
        Random.Range(-0.7f, 0.7f)
    );

        GameObject productoEnCarrito = Instantiate(modeloEncontrado, spawnPoint.position, Quaternion.identity);
        if (!(nombreProducto.ToLower().Contains("leche")))
        {
            productoEnCarrito.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            productoEnCarrito.transform.localRotation = Quaternion.Euler(-180, -180, -90);
        }

        if ((nombreProducto.ToLower().Contains("leche")))
        {
            productoEnCarrito.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            productoEnCarrito.transform.localRotation = Quaternion.Euler(-180, -180, -90);
        }



        Rigidbody rb = productoEnCarrito.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = productoEnCarrito.AddComponent<Rigidbody>();
        }
        rb.useGravity = true;
        //rb.constraints = RigidbodyConstraints.FreezeRotation;

        BoxCollider collider = productoEnCarrito.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = productoEnCarrito.AddComponent<BoxCollider>();
        }

        collider.size *= 0.8f;

        productoEnCarrito.transform.SetParent(cart.transform, true);
    }

    IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(2);
        if (WarningMessage != null)
        {
            WarningMessage.gameObject.SetActive(false);
        }


        if (AlreadyCollectedProductWarningMessage != null)
        {
            AlreadyCollectedProductWarningMessage.gameObject.SetActive(false);
        }
        

        //if (ReducedTime != null)
        //{
        //    ReducedTime.gameObject.SetActive(false);
        //}
    }

    void CheckIfGameFinished()
    {
        Debug.Log("soy el tamano del diccionario productos recolectados: " + productosRecolectados.Count);
        if (productosRecolectados.Count >= GenerateShoppingListContent.selectedProducts.Count)
        {
            Debug.Log("Todos los productos han sido recolectados! Fin del juego");
            GameControllerFrenesi gameController = FindObjectOfType<GameControllerFrenesi>();

            if (gameController != null)
            {
                gameController.EndGame();
            }
            else
            {
                Debug.LogError("GameControllerFrenesi no encontrado en la escena.");
            }
        }
    }

    void EndGame()
    {
        endGamePanel.SetActive(true);
        list.SetActive(false);
        if (player != null)
        {
            player.GetComponent<GameControllerFrenesi>().enabled = false;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void SimularRecoleccionCompleta()
    {
        Debug.Log("[SIMULACIÓN] Recolectando todos los productos...");

        foreach (var producto in GenerateShoppingListContent.selectedProducts.Keys)
        {
            string nombreLista = GenerateShoppingListContent.selectedProducts[producto];

            if (!productosRecolectados.ContainsKey(nombreLista))
            {
                productosRecolectados[nombreLista] = 1;
            }

            // Notificar a la lista de compras
            GenerateShoppingListContent.MarkProductAsCollected(producto);
        }

        // Verificar si se han recogido todos los elementos
        CheckIfGameFinished();
    }

    IEnumerator AnimateAndHideReducedTime()
    {
        reducedTimeCanvasGroup.alpha = 1;
        RectTransform rt = ReducedTime.GetComponent<RectTransform>();

        // Escala inicial
        rt.localScale = Vector3.zero;

        // Animación de "rebote" de escala (como punch)
        float duration = 0.3f;
        float elapsed = 0f;
        Vector3 targetScale = Vector3.one * 1.2f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            rt.localScale = Vector3.Lerp(Vector3.zero, targetScale, Mathf.Sin(t * Mathf.PI));
            elapsed += Time.deltaTime;
            yield return null;
        }

        rt.localScale = Vector3.one;

        // Esperar un momento visible
        yield return new WaitForSeconds(1f);

        // Fade out
        float fadeDuration = 0.5f;
        elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            reducedTimeCanvasGroup.alpha = Mathf.Lerp(1, 0, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        reducedTimeCanvasGroup.alpha = 0;
        ReducedTime.gameObject.SetActive(false);
    }


}
