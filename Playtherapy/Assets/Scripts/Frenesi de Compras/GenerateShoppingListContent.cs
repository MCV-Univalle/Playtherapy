using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateShoppingListContent : MonoBehaviour
{
    public GameObject entryPrefab; // Prefab del renglón
    public Transform contentParent; // Contenedor de la lista

    private Dictionary<string, Sprite> productIcons = new Dictionary<string, Sprite>(); // Diccionario para los íconos
    private Dictionary<string, GameObject> productEntries = new Dictionary<string, GameObject>();

    // Diccionario con equivalencias entre el nombre del objeto en la escena y el nombre en la lista
    private Dictionary<string, string> allProducts = new Dictionary<string, string>()
    {
        { "Canasta de fresas", "Fresas" },
        { "Bolsa de papas rosadas", "Papas moradas" },
        { "Leche deslactosada", "Leche deslactosada" },
        { "Canasta de manzanas rojas", "Manzanas rojas" },
        { "Canasta de remolacha", "Rabanos" },
        { "Canasta de bananos", "Bananos" },
        { "Canasta de berenjenas", "Berenjenas" },
        { "Botella rosada", "Gaseosa rosada" },
        { "Botella naranja", "Gaseosa naranja" },
        { "Botella verde claro", "Gaseosa verde" },
        { "Leche descremada", "Leche descremada" },
        { "Leche entera", "Leche entera" },
        { "Canasta de limones", "Limones" },
        { "Canasta manzanas verdes", "Manzanas verdes" },
        { "Canasta de naranjas", "Naranjas" },
        { "Bolsa de papas azul claro", "Papas azul claro" },
        { "Bolsa de papas azul oscuro", "Papas azul oscuro" },
        { "Bolsa de papas de pollo", "Papas de pollo" },
        { "Canasta de piñas", "Piña" },
        { "Canasta de tomates", "Tomates" }
    };
    public Dictionary<string, string> selectedProducts;

    void Start()
    {
        // Lista completa de 20 productos con su respectivo ícono
        //List<string> allProducts = new List<string>
        //{
        //    "Bananos", "Berenjenas", "Fresas", "Gaseosa morada", "Gaseosa naranja", "Gaseosa verde", "Leche descremada", "Leche deslactosada", "Leche entera", "Limones",
        //    "Manzanas rojas", "Manzanas verdes", "Naranjas", "Papas azul claro", "Papas azul oscuro", "Papas moradas", "Papas naranjas", "Piña", "Remolachas", "Tomates"
        //};

        LoadIcons(); // Cargar los íconos desde la carpeta

        // Seleccionar 8 productos aleatorios
        selectedProducts = GetRandomProducts(allProducts, 8);

        // Generar la lista en la UI
        foreach (var product in selectedProducts)
        {
            GameObject entry = Instantiate(entryPrefab, contentParent);
            entry.transform.Find("ProductText").GetComponent<Text>().text = product.Value;

            // Asigna el ícono correspondiente
            Image icon = entry.transform.Find("ProductIcon").GetComponent<Image>();
            if (productIcons.ContainsKey(product.Value))
            {
                icon.sprite = productIcons[product.Value];
            }

            productEntries[product.Key] = entry;
        }
    }

    // Marcar los productos recolectados

    public void MarkProductAsCollected(string productName)
    {
        if (productEntries.ContainsKey(productName))
        {
            // Marcar con un check
            //productEntries[productName].transform.Find("ProductText").GetComponent<Text>().text = "(Obtenido) " + selectedProducts[productName];
            //productEntries[productName].transform.Find("ProductText").GetComponent<Text>().text = "----------------------";
            //productEntries[productName].transform.Find("ProductText").GetComponent<Text>().text = "<s>" + selectedProducts[productName] + "</s>";
            productEntries[productName].transform.Find("StrikethroughLine").gameObject.SetActive(true);

            // Eliminando el producto de la lista
            // Destroy(productEntries[productName]);
            // productEntries.Remove(productName);
        }
    }

    // Cargar íconos desde la carpeta "Resources/Icons"
    void LoadIcons()
    {
        //foreach (string product in new string[]
        //{
        //    "Bananos", "Berenjenas", "Fresas", "Gaseosa morada", "Gaseosa naranja", "Gaseosa verde", "Leche descremada", "Leche deslactosada", "Leche entera", "Limones",
        //    "Manzanas rojas", "Manzanas verdes", "Naranjas", "Papas azul claro", "Papas azul oscuro", "Papas moradas", "Papas naranjas", "Piña", "Rabanos", "Tomates"
        //})
        //{
        //    Sprite icon = Resources.Load<Sprite>($"Sprites/Frenesi de Compras/Product icons/{product}");
        //    Debug.Log("Supuestamente encontre sprites");
        //    if (icon != null)
        //    {
        //        Debug.Log("no encontre sprite");
        //        productIcons[product] = icon;
        //    }
        //}
        foreach (var product in allProducts.Values) // Cargar íconos con los nombres de la lista
        {
            Sprite icon = Resources.Load<Sprite>($"Sprites/Frenesi de Compras/Product icons/{product}");
            if (icon != null)
            {
                productIcons[product] = icon;
            }
        }
    }

    // Devuelve una lista de elementos aleatorios sin repetición
    //List<string> GetRandomProducts(List<string> sourceList, int count)
    //{
    //    List<string> tempList = new List<string>(sourceList);
    //    List<string> selectedItems = new List<string>();

    //    for (int i = 0; i < count && tempList.Count > 0; i++)
    //    {
    //        int randomIndex = Random.Range(0, tempList.Count);
    //        selectedItems.Add(tempList[randomIndex]);
    //        tempList.RemoveAt(randomIndex);
    //    }

    //    return selectedItems;
    //}
    Dictionary<string, string> GetRandomProducts(Dictionary<string, string> sourceDict, int count)
    {
        List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>(sourceDict);
        Dictionary<string, string> selectedItems = new Dictionary<string, string>();

        for (int i = 0; i < count && tempList.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            selectedItems.Add(tempList[randomIndex].Key, tempList[randomIndex].Value);
            tempList.RemoveAt(randomIndex);
        }

        return selectedItems;
    }
}
