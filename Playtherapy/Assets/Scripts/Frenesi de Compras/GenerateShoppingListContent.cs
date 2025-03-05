using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateShoppingListContent : MonoBehaviour
{
    public GameObject entryPrefab; // Prefab del renglón
    public Transform contentParent; // Contenedor de la lista

    private Dictionary<string, Sprite> productIcons = new Dictionary<string, Sprite>(); // Diccionario para los íconos
    private Dictionary<string, GameObject> productEntries = new Dictionary<string, GameObject>();

    private List<string> selectedProducts;

    void Start()
    {
        // Lista completa de 20 productos con su respectivo ícono
        List<string> allProducts = new List<string>
        {
            "Bananos", "Berenjenas", "Fresas", "Gaseosa morada", "Gaseosa naranja", "Gaseosa verde", "Leche descremada", "Leche deslactosada", "Leche entera", "Limones",
            "Manzanas rojas", "Manzanas verdes", "Naranjas", "Papas azul claro", "Papas azul oscuro", "Papas moradas", "Papas naranjas", "Piña", "Rabanos", "Tomates"
        };

        LoadIcons(); // Cargar los íconos desde la carpeta

        // Seleccionar 8 productos aleatorios
        selectedProducts = GetRandomProducts(allProducts, 8);

        // Generar la lista en la UI
        foreach (string product in selectedProducts)
        {
            GameObject entry = Instantiate(entryPrefab, contentParent);
            entry.transform.Find("ProductText").GetComponent<Text>().text = product;

            // Asigna el ícono correspondiente
            Image icon = entry.transform.Find("ProductIcon").GetComponent<Image>();
            if (productIcons.ContainsKey(product))
            {
                icon.sprite = productIcons[product];
            }

            productEntries[product] = entry;
        }
    }

    // Marcar los productos recolectados

    public void MarkProductAsCollected(string productName)
    {
        if (productEntries.ContainsKey(productName))
        {
            // Marcar con un check
            productEntries[productName].transform.Find("ProductText").GetComponent<Text>().text = "(Obtenido) " + productName;

            // Eliminando el producto de la lista
            // Destroy(productEntries[productName]);
            // productEntries.Remove(productName);
        }
    }

    // Cargar íconos desde la carpeta "Resources/Icons"
    void LoadIcons()
    {
        foreach (string product in new string[]
        {
            "Bananos", "Berenjenas", "Fresas", "Gaseosa morada", "Gaseosa naranja", "Gaseosa verde", "Leche descremada", "Leche deslactosada", "Leche entera", "Limones",
            "Manzanas rojas", "Manzanas verdes", "Naranjas", "Papas azul claro", "Papas azul oscuro", "Papas moradas", "Papas naranjas", "Piña", "Rabanos", "Tomates"
        })
        {
            Sprite icon = Resources.Load<Sprite>($"Sprites/Frenesi de Compras/Product icons/{product}");
            Debug.Log("Supuestamente encontre sprites");
            if (icon != null)
            {
                Debug.Log("no encontre sprite");
                productIcons[product] = icon;
            }
        }
    }

    // Devuelve una lista de elementos aleatorios sin repetición
    List<string> GetRandomProducts(List<string> sourceList, int count)
    {
        List<string> tempList = new List<string>(sourceList);
        List<string> selectedItems = new List<string>();

        for (int i = 0; i < count && tempList.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            selectedItems.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
        }

        return selectedItems;
    }
}
