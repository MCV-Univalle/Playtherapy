using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollissionWithProducts : MonoBehaviour
{
    // Arreglo para almacenar los productos recogidos
    private Dictionary<string, int> productosRecolectados = new Dictionary<string, int>();
    private GenerateShoppingListContent shoppingList;
    public int totalProductosEnEscena;
    // Start is called before the first frame update
    void Start()
    {
        shoppingList = FindObjectOfType<GenerateShoppingListContent>();
        totalProductosEnEscena = GameObject.FindGameObjectsWithTag("Producto").Length;
        Debug.Log("Total de productos en escena: " + totalProductosEnEscena);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Simular recolección con la tecla E
        {
            string productoSimulado = "Fresas"; // Cambia por cualquier producto de la lista
            Debug.Log($"[SIMULACIÓN] Recolectado: {productoSimulado}");

            if (!productosRecolectados.ContainsKey(productoSimulado))
            {
                productosRecolectados[productoSimulado] = 1;
            }
            else
            {
                productosRecolectados[productoSimulado]++;
            }

            // Notificar a la lista de compras
            if (shoppingList != null)
            {
                shoppingList.MarkProductAsCollected(productoSimulado);
            }
        }
    }

    // Método para detectar colisiones con productos
    private void OnTriggerEnter(Collider other)
    {
      
        if (other.CompareTag("Producto"))
        {
            string nombreProducto = other.gameObject.name.Replace("(Clone)", "").Trim();

            // Actualizar la cantidad del producto en el diccionario
            if (productosRecolectados.ContainsKey(nombreProducto))
            {
                productosRecolectados[nombreProducto]++;
            }
            else
            {
                productosRecolectados[nombreProducto] = 1;
            }

            Debug.Log($"Recolectado: {nombreProducto}");

            if (shoppingList != null)
            {
                shoppingList.MarkProductAsCollected(nombreProducto);
            }

            string diccionarioFormateado = "Productos recolectados:\n";
            foreach (var item in productosRecolectados)
            {
                diccionarioFormateado += $"{item.Key}: {item.Value}\n";
            }

            // Mostrar en consola
            Debug.Log(diccionarioFormateado);
            // Destruir el objeto recolectado
            Destroy(other.gameObject);
        }
    }
}
