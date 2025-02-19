using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class GeneratingMap : MonoBehaviour
{
    public GameObject pasilloPrefab;
    public GameObject fakePasilloPrefab;
    public int cantidadTramos = 5;
    public Transform jugador;

    private Queue<GameObject> pasillos = new Queue<GameObject>();
    private float longitudTramo;
    private GameObject fakePasillo;

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
            pasillos.Dequeue();
            primerTramo.transform.position += Vector3.forward * longitudTramo * cantidadTramos;
            pasillos.Enqueue(primerTramo);

            Vector3 nuevaPosicion = new Vector3(-7, 2, pasillos.Last().transform.position.z);
            fakePasillo.transform.position = nuevaPosicion;
        }
    }
}