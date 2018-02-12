using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsControler : MonoBehaviour
{
	public MixerObj coinGuia;
	private int coinCollider;
	public GameObject[] ubicaciones;
	public GameObject coin;
	private GameObject antes, player;
	public GameObject[] toys;
	GameManagerCavano manager;
	private bool stop = false;
	// Use this for initialization
	void Start ()
	{
		ShowAToy ();
		player = GameObject.FindGameObjectWithTag ("Player");
		coinCollider = 0;
		antes = ubicaciones [0];
		manager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManagerCavano> ();
		coin.transform.position = antes.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
			
		if (!manager.GetStatusCava ()) {
			FrezzerCoin ();
			coinGuia.StopCoin ();

		} else if (manager.GetStatusCava ()) {
			stop = false;
			coinGuia.RunerCoin ();

		}
	}
	/*Si la variable coinCollider es mayor a 1 significa que cayo en la taza */
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) {
			//Debug.Log ("Debe Pausarse ------------");
			player = other.gameObject;
			stop = true;//Detiene la moneda al tocar 
			coinCollider++;
		}

		if (other.gameObject.CompareTag ("Container")) {
			coin.gameObject.SetActive (false);
			if (coinCollider > 0) {
				//Asigna Puntos por cada Moneda recolecctada

				//manager.SetScore (puntosCoin);
				coinCollider = 0;
				manager.SetStatusCava (false);
			}
			RamdonLugar ();
		}

		if (other.gameObject.CompareTag ("Base")) {
			coin.gameObject.SetActive (false);
			RamdonLugar ();
			coinCollider = 0;
		}

	}

	void RamdonLugar ()
	{
		antes.GetComponent<Renderer> ().material.color = Color.black;
		coin.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		coin.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		//Correcion de Bug de solapamiento
		coin.transform.position = new Vector3 (antes.transform.position.x, 22.0f, antes.transform.position.z);
		//coinGuia.StopCoin ();
		coin.gameObject.SetActive (true);
		coin.GetComponent<Rigidbody> ().Sleep ();
		coin.GetComponent<Rigidbody> ().Sleep ();
		antes = ubicaciones [Random.Range (0, ubicaciones.Length)];
		//Correcions de bug de caida de juegete en las flechas

		ChagenColor ();
	}

	void ChagenColor ()
	{
		ShowAToy ();
		antes.GetComponent<Renderer> ().material.color = Color.green;
	}

	void FrezzerCoin ()
	{
		
		if (stop) {
			coin.transform.position = player.transform.position;
		}
	}
	//Oculta todos los jugetes
	void HideAllToys ()
	{
		for (int i = 0; i < toys.Length; i++) {
			toys [i].SetActive (false); 
		}
	}

	//Muestra un Juegete aleatoriamente
	void ShowAToy ()
	{
		HideAllToys ();
		toys [Random.Range (0, toys.Length)].SetActive (true);

	}
}
