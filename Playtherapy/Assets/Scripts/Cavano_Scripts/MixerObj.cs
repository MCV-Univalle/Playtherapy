using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerObj : MonoBehaviour
{
	private int i = 0;
	public  Transform[] targets;
	public float speed;
	private bool run;
	GameManagerCavano manager;
	bool flag = false;
	// Use this for initialization
	void Start ()
	{
		run = false;
		manager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManagerCavano> ();


	}
	
	// Update is called once per frame
	void Update ()
	{
		float step = speed * Time.deltaTime;
		if (run) {
			transform.position = Vector3.MoveTowards (transform.position, targets [i].position, step);
			Debug.Log ("Mixer recorrido: " + i);
			//Debug.Log ("Distancia " + Vector3.Distance (transform.position, targets [i].position));

			if (Vector3.Distance (transform.position, targets [i].position) == 0.0f) {
				i++;
				if (i == (4) ) {
					i = 0;
					run = false;
					Debug.Log ("Deberia detener la moneda");
					StopCoin ();
				}

			}
		}

	}

	public void RunerCoin ()
	{
		if (flag) {
			float limite = transform.position.x;
			//Guia al jugete a la sexta de jugetes
			if (limite < -9.0f) {
				run = true;
			}
			flag = false;
		}
	}

	public void StopCoin ()
	{
		if (!flag) {
			i = 0;
			//Guia al jugete a la sexta de jugetes
			run = false;
			flag = true;
		}
	}

	IEnumerator WaitAndGo ()
	{
		yield return new WaitForSeconds (25);
	}
}
