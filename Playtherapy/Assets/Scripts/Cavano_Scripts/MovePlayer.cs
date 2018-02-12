using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeapAPI;
using UnityEngine.UI;

public class MovePlayer : MonoBehaviour
{
	private Rigidbody playerRigidbody;
	public float speed;
	Vector3 movement;
	float moveHorizontal;
	float moveVertical;
	private Movements leapMotion;
	public GameObject playerCava;
	private int statusAngle = 0;
	private float limitex;
	public Text test;
	private double anguloSencible = 35;

	GameManagerCavano manager;
	// Use this for initialization
	void Start ()
	{	
		//Correcion de error de objeto no asignado
		manager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManagerCavano> ();
		anguloSencible = manager.GetRangoMano ();
		playerRigidbody = GetComponent<Rigidbody> ();
		leapMotion = new Movements ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		LimitesCava ();
		/**Area de Prueba*/
		//test.text = " Supra y Prono: " + Mathf.Round (((float)leapMotion.PronoSupra ()));

		/***/
		//Codicional que verifica si la mano que se configuro en el meno es la mismas que se esta usando para mover el player
		if (manager.isRightController () && !leapMotion.IsLeft ()) {
			moveHorizontal = (float)leapMotion.UlnarRadial () * 2.0f;
			moveVertical = 0;
			Move (moveHorizontal, moveVertical);
		}
		if (manager.isLeftController () && leapMotion.IsLeft ()) {
			moveHorizontal = (float)leapMotion.UlnarRadial () * 2.0f;
			moveVertical = 0;
			Move (moveHorizontal, moveVertical);
		}
		//-----------------------------------------------------------------
		/*Caso neutro*/
		/*Caso Mano Derecha*/
		if (statusAngle != 0 && leapMotion.PronoSupra () < anguloSencible) {/*Reinicia la Ubicion del Objecto*/
			
			if (statusAngle == 1) {
				playerCava.transform.Rotate (0, 0, -60);
				statusAngle = 0;
			}
			if (statusAngle == -1) {
				playerCava.transform.Rotate (0, 0, 60);
				statusAngle = 0;
			}
			manager.SetStatusCava (false);

		}
		/*Caso Mano Izquierda*/
		if (statusAngle != 0 && -leapMotion.PronoSupra () < anguloSencible) {/*Reinicia la Ubicion del Objecto*/

			if (statusAngle == 1) {
				playerCava.transform.Rotate (0, 0, -60);
				statusAngle = 0;
			}
			if (statusAngle == -1) {
				playerCava.transform.Rotate (0, 0, 60);
				statusAngle = 0;
			}
			manager.SetStatusCava (false);

		}
		/*Supra y Prono Mano Derecha*/
		if (!leapMotion.IsLeft () && statusAngle == 0 && -leapMotion.PronoSupra () > anguloSencible) {/*Gira el objeto a la derecha*/

			playerCava.transform.Rotate (0, 0, 60);
			statusAngle = 1;

			manager.SetStatusCava (true);

		}
		/*Supra y Prono Mano izquierda*/
		if (leapMotion.IsLeft () && statusAngle == 0 && leapMotion.PronoSupra () > anguloSencible) {
			//Gira el objeto a la izquierda
			playerCava.transform.Rotate (0, 0, -60);
			statusAngle = -1;
			manager.SetStatusCava (true);
		}
	}

	void Move (float h, float v)
	{
		if (manager.PauseStatus ()) {//Estado Juego Pausado
			// Set the movement vector based on the axis input.
			movement.Set (h, 0f, v);
			// Normalise the movement vector and make it proportional to the speed per second.
			movement = movement.normalized * speed * Time.deltaTime;
			// Move the player to it's current position plus the movement.
			playerRigidbody.MovePosition (transform.position + movement);
		}
	}

	void LimitesCava ()
	{
		//
		float limite = transform.position.x;
		if (limite > 5.9f) {
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (5.8f, transform.position.y, transform.position.z), 5);
		}
		if (limite < -10.24f) {
			transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-10.0f, transform.position.y, transform.position.z), 5);
		}

	}
}
