using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using LeapAPI;

namespace CompleteProject
{

	public class PlayerController : MonoBehaviour
	{
		public bool UsoLeapMotion = false;
		public bool ManoDerecha = false;
		public float speed = 7;
		private float rotacion = 2 * Mathf.PI;
		public float sensitiveRotate = 1.5f;
		private bool stop = false;
		Vector3 movement;
		Animator anim;
		int idleHash;
		int runStateHash;
		int floorMask;
		// A layer mask so that a ray can be cast just at gameobjects on the floor layer.
		float camRayLength = 100f;
		Vector3 angulo;
		LeapMotionController controladorLeapMotion;

		private Rigidbody playerRigidbody;
		//public Camera camera_gameplay;
		float moveHorizontal;
		float moveVertical;
		bool flag = false;
		/*Scripts para detectar la orientacion de la mano y convertirla en un vector3 de movimientos*/
		public GameObject middle_L;
		//Dedo Medio Izquierda
		public GameObject middle_R;
		//Dedo Medio Deracha
		private Transform transformMiddle_L;
		private Transform transformMiddle_R;
		private Movements leapMotion;
		private StatusGame statusGame;

		void Awake ()
		{
			anim = GetComponent<Animator> ();
			idleHash = Animator.StringToHash ("Idle_B");
			runStateHash = Animator.StringToHash ("Run");
			playerRigidbody = GetComponent<Rigidbody> ();
			floorMask = LayerMask.GetMask ("Floor");
			statusGame = GameObject.Find ("StatusGame").GetComponent<StatusGame> ();


			//camera_gameplay = GameObject.Find ("Camera").GEt;
		}

		void Start ()
		{
			SetStop (false);
			transformMiddle_L = middle_L.GetComponent<Transform> ();
			transformMiddle_R = middle_R.GetComponent<Transform> ();
			leapMotion = new Movements ();
		}

		void FixedUpdate ()
		{
			if (UsoLeapMotion) {
				/*Decide si usa leapmotion o Intercambia con teclado*/
				/*Uso Con LeapMotion*/
				if (statusGame.isRight()) {
					/*Decide que Mano Utilizara*/
					moveHorizontal = InputLeapMotionLeftHorizontal ();
					moveVertical = InputLeapMotionRightVertical ();
				} else {
					moveHorizontal = InputLeapMotionLeftHorizontal ();
					moveVertical = InputLeapMotionLeftVertical ();
				}

			} else {
				/*Uso Con KeyBoard*/
				moveHorizontal = CrossPlatformInputManager.GetAxisRaw ("Horizontal");
				moveVertical = CrossPlatformInputManager.GetAxisRaw ("Vertical");
			}
			//Aplicacion de fuerza
			//Detiene el movimiento del personaje para recolectar la moneda
			Move (moveHorizontal, moveVertical);
			// Turn the player to face the mouse cursor.
			Turning ();
			// Animate the player.
			Animating (moveHorizontal, moveVertical);
		}

		void Move (float h, float v)
		{
			// Set the movement vector based on the axis input.
			movement.Set (h, 0f, v);
			// Normalise the movement vector and make it proportional to the speed per second.
			movement = movement.normalized * speed * Time.deltaTime;
			// Move the player to it's current position plus the movement.
			playerRigidbody.MovePosition (transform.position + movement);
		}

		void Animating (float h, float v)
		{        // Ejecuta la Animacion de correr y detencion
			if (h == 0 && v == 0) {
				anim.Play (idleHash);
			} else {
				anim.Play (runStateHash);
			}
		}

		void Turning ()
		{
			// rotacion a la Derecha.
			float movimientoAngular = moveHorizontal * 3;
			angulo = new Vector3 (0, movimientoAngular, 0);
			if (moveHorizontal > 0 && playerRigidbody.rotation.y < 0.5f) {
				flag = true;
				Rotar ();
			}
			// rotacion ala Izquierda.
			if (moveHorizontal < 0 && playerRigidbody.rotation.y > -0.5f) {
				flag = true;
				Rotar ();
			}

			if (moveVertical == 1 && playerRigidbody.rotation.y != 0.0f && playerRigidbody.rotation.y > 0.0f) {
				flag = true;
				movimientoAngular = -2.0f;
				angulo = new Vector3 (0, movimientoAngular, 0);
				Rotar ();
			}
			if (moveVertical == 1 && playerRigidbody.rotation.y != 0.0f && playerRigidbody.rotation.y < 0.0f) {
				movimientoAngular = 2.0f;
				flag = true;
				angulo = new Vector3 (0, movimientoAngular, 0);
				Rotar ();
			}
			
		}

		void Rotar ()
		{
		
			Quaternion newRotatation = Quaternion.Euler (angulo);
			// Set the player's rotation to this new rotation.
			playerRigidbody.MoveRotation (playerRigidbody.rotation * newRotatation);
		}

		float InputLeapMotionRightHorizontal ()
		{
			/*Esta funcion detecta la orientacion del dedo
		medio y calcula asia que lado se esta desviando Para Movimiento Horizontal*/
			float movimiento = 0.0f;
			Vector3 dedoMedioPosition = transformMiddle_R.transform.localPosition;
			float desplasamientoDedo = dedoMedioPosition.x;
			if (desplasamientoDedo < 0.0f) {
				movimiento = -1.0f;
			}

			if (desplasamientoDedo > 0.0f) {
				movimiento = 1.0f;
			}
			return movimiento;
		}
		/**************************************************************Controlador de Leapmotion ***********************************************************************/
		float InputLeapMotionLeftHorizontal ()
		{
		
			/*Esta funcion detecta la orientacion del dedo
		medio y calcula asia que lado se testa desviando para Movimiento Horizontal*/
			float movimiento = 0.0f;
			Vector3 dedoMedioPosition = transformMiddle_L.transform.localPosition;
			float desplasamientoDedo = dedoMedioPosition.x;
			if (desplasamientoDedo < 0.0f) {
				Debug.Log ("Izquierda");
				movimiento = -1.0f;
			}

			if (desplasamientoDedo > 0.0f) {
				Debug.Log ("Derecha ");
				movimiento = 1.0f;
			}
			return movimiento;
		}

		float InputLeapMotionRightVertical ()
		{
			/*Esta funcion detecta la orientacion del dedo
		medio y calcula asia que lado se esta desviando Para Movimiento Vertical*/
			float movimiento = 0.0f;
			Vector3 dedoMedioPosition = transformMiddle_R.transform.localPosition;
			float desplasamientoDedo = dedoMedioPosition.y;
			if (desplasamientoDedo < 0.2f) {

				movimiento = -1.0f;
			}

			if (desplasamientoDedo > 0.2f) {
				movimiento = 1.0f;
			}
			return movimiento;
		}
		/*Controlador de Leapmotion */
		float InputLeapMotionLeftVertical ()
		{

			/*Esta funcion detecta la orientacion del dedo
		medio y calcula asia que lado se testa desviando para Movimiento Veritical*/
			float movimiento = 0.0f;
			Vector3 dedoMedioPosition = transformMiddle_L.transform.localPosition;
			Debug.Log ("Dedo Posicion z " + dedoMedioPosition);
			float desplasamientoDedo = dedoMedioPosition.y;
			if (desplasamientoDedo < 0.2f) {
				Debug.Log ("Down");
				movimiento = -1.0f;
			}

			if (desplasamientoDedo > 0.2f) {
				Debug.Log ("Up");
				movimiento = 1.0f;
			}
			return movimiento;
		}

		public void SetStop (bool into)
		{
			this.stop = into;
		}

		public bool GetStop (bool into)
		{
			return stop;
		}
	}

}
