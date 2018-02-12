using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraFinal : MonoBehaviour
{

	// Use this for initialization
	public Transform target;
	public float speed;
	GameManagerCavano manager;

	void Start ()
	{
		manager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManagerCavano> ();
		
	}

	void Update ()
	{
		if (manager.GetEndGame () && manager.StarGame()) {
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, target.position, step);
		}
	}
}
