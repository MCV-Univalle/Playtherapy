using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollissionWithProducts : MonoBehaviour
{
    // Arreglo para almacenar los productos recogidos
    private Dictionary<string, int> productosRecolectados = new Dictionary<string, int>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
