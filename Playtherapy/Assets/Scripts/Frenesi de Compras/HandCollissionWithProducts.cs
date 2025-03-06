using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollissionWithProducts : MonoBehaviour
{
    // Arreglo para almacenar los productos recogidos
    private Dictionary<string, int> productosRecolectados = new Dictionary<string, int>();
    private GenerateShoppingListContent shoppingList;
    public GameObject endGamePanel;
    public GameObject list;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        shoppingList = FindObjectOfType<GenerateShoppingListContent>();
        endGamePanel.SetActive(false);
        list.SetActive(true);

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

            if (shoppingList.selectedProducts.ContainsKey(nombreObjeto))
            {
                string nombreLista = shoppingList.selectedProducts[nombreObjeto];

                if (productosRecolectados.ContainsKey(nombreLista))
                {
                    productosRecolectados[nombreLista]++;
                }
                else
                {
                    productosRecolectados[nombreLista] = 1;
                }

                Debug.Log($"Recolectado: {nombreLista}");

                shoppingList.MarkProductAsCollected(nombreObjeto);

                Destroy(other.gameObject);

                // Verificar si se han recogido todos los elementos
                CheckIfGameFinished();
            }
            else
            {
                // Producto incorrecto: penalizar tiempo
                Debug.Log($"¡Producto incorrecto recogido: {nombreObjeto}! Penalizando tiempo...");

                GameControllerFrenesi gameController = FindObjectOfType<GameControllerFrenesi>();
                if (gameController != null)
                {
                    gameController.ReduceTime();
                }

                // Opcional: destruir el producto incorrecto
                Destroy(other.gameObject);
            }
        }
    }

    void CheckIfGameFinished()
    {
        if (productosRecolectados.Count >= shoppingList.selectedProducts.Count)
        {
            Debug.Log("Todos los productos han sido recolectados! Fin del juego");
            EndGame();
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

        foreach (var producto in shoppingList.selectedProducts.Keys)
        {
            string nombreLista = shoppingList.selectedProducts[producto];

            if (!productosRecolectados.ContainsKey(nombreLista))
            {
                productosRecolectados[nombreLista] = 1;
            }

            // Notificar a la lista de compras
            shoppingList.MarkProductAsCollected(producto);
        }

        // Verificar si se han recogido todos los elementos
        CheckIfGameFinished();
    }
}
