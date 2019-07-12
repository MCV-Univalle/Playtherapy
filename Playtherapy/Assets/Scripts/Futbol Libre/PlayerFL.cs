using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using MovementDetectionLibrary;
using UnityEngine.UI;

public class PlayerFL : MonoBehaviour {

	MovementsCollection bodyMovements;
	Dictionary<BodyParts, BodyPoint> bodyPointsCollection;
	KinectTwoAdapter adapter;
	KinectSensor sensor;
	ControllerBall controlPelota;
	int turno;
	double minAngle;
	double maxAngle;
	bool puede_patear_der;
	bool puede_patear_izq;

	//bool puede_patear;
	ManagerFL manager;
	// Use this for initialization
	void Start () {
		//puede_patear = true;
		puede_patear_der = false;
		puede_patear_izq = false;
		connectWithSensor ();
		controlPelota = GameObject.FindObjectOfType<ControllerBall>();
		manager = GameObject.FindObjectOfType<ManagerFL> ();
	}

	void Update () {
		for (int i = 0; i < (int)BodyParts.ThumbRight; i++)
		{
			BodyPointPosition position = adapter.ReturnPosition((BodyParts)i);
			bodyPointsCollection[(BodyParts)i].setPosition(position);
		}
		bodyMovements.setBodyPointsCollection(bodyPointsCollection);

		if(manager.hasStart == true){
//			administrarTurno ();
			if (manager.derecho == true && manager.rodilla == true) {
				////print ("sensando rodilla derecha");
				sensarRodillaDer ();
			}
			if ( manager.izquierdo == true && manager.rodilla == true) {
				////print ("sensando rodilla izquierda");
				sensarRodillaIzq ();
			}
			if ( manager.derecho == true && manager.pie == true) {
				////print ("sensando pie derecho");
				sensarPieDerecho ();
			}
			if ( manager.izquierdo == true && manager.pie == true) {
				////print ("sensando pie izquierdo");
				sensarPieIzquierdo ();
			}
		}
	}

	public void administrarTurno(){
		if (manager.rodilla==true && manager.pie==true) {
			int numero = UnityEngine.Random.Range (1, 101);
			if (numero % 2 == 0) {
				moverRodilla ();
			} else {
				moverPie ();
			}
		} else if (manager.rodilla==true) {
			moverRodilla ();
		} else {
			moverPie ();
		}
	}

	public void moverRodilla(){
		if (manager.izquierdo == true && manager.derecho == true) {
			int numero = UnityEngine.Random.Range (1, 101);
			if (numero % 2==0) {
				manager.rodillaDerParticles1.Play ();
				manager.rodillaIzqParticles2.Stop ();
				manager.pieDerParticles1.Stop ();
				manager.pieIzqParticles2.Stop ();
				//print ("sensando rodilla derecha");
				sensarRodillaDer ();
			} else {
				manager.rodillaDerParticles1.Stop ();
				manager.rodillaIzqParticles2.Play ();
				manager.pieDerParticles1.Stop ();
				manager.pieIzqParticles2.Stop ();
				//print ("sensando rodilla izquierda");
				sensarRodillaIzq ();
			}
		} else if (manager.izquierdo==true) {
			manager.rodillaDerParticles1.Stop ();
			manager.rodillaIzqParticles2.Play ();
			manager.pieDerParticles1.Stop ();
			manager.pieIzqParticles2.Stop ();
			//print ("sensando rodilla izquierda");
			sensarRodillaIzq ();
		} else {
			manager.rodillaDerParticles1.Play ();
			manager.rodillaIzqParticles2.Stop ();
			manager.pieDerParticles1.Stop ();
			manager.pieIzqParticles2.Stop ();
			//print ("sensando rodilla derecha");
			sensarRodillaDer ();
		}
	}


	public void moverPie(){
		if (manager.izquierdo == true && manager.derecho == true) {
			int numero = UnityEngine.Random.Range (1, 101);
			if (numero % 2==0) {
				manager.rodillaDerParticles1.Stop ();
				manager.rodillaIzqParticles2.Stop ();
				manager.pieDerParticles1.Play ();
				manager.pieIzqParticles2.Stop ();
				//print ("sensando pie derecho");
				sensarPieDerecho ();
			} else {
				manager.rodillaDerParticles1.Stop ();
				manager.rodillaIzqParticles2.Stop ();
				manager.pieDerParticles1.Stop ();
				manager.pieIzqParticles2.Play ();
				//print ("sensando pie izquierdo");
				sensarPieIzquierdo ();
			}
		} else if (manager.izquierdo==true) {
			manager.rodillaDerParticles1.Stop ();
			manager.rodillaIzqParticles2.Stop ();
			manager.pieDerParticles1.Stop ();
			manager.pieIzqParticles2.Play ();
			////print ("sensando pie izquierdo");
			sensarPieIzquierdo ();
		} else {
			manager.rodillaDerParticles1.Stop ();
			manager.rodillaIzqParticles2.Stop ();
			manager.pieDerParticles1.Play ();
			manager.pieIzqParticles2.Stop ();
			////print ("sensando pie derecho");
			sensarPieDerecho ();
		}
	}

	public void sensarRodillaDer(){
		////print ("rodilla derecha");
		manager.debug.text = bodyMovements.kneeRigthMovement().ToString();
//			//manager.anguloMin.ToString();
		if (bodyMovements.kneeRigthMovement()<manager.anguloMin) {
			puede_patear_der = true;
			//print ("aqui no puede:"+controlPelota.puede_levantarse);
		}
		if (bodyMovements.kneeRigthMovement()>manager.anguloMax && puede_patear_der==true && controlPelota.puede_levantarse==true) {
			//print ("aqui puede:"+controlPelota.puede_levantarse);
			puede_patear_der = false;
			manager.repeticionesRestantes++;
			manager.puntos++;
			administrarTurno ();
			controlPelota.patear ();
			manager.golpePelota.Play ();
		}
	}

	public void sensarRodillaIzq(){
		////print ("rodilla izquierda");
		manager.debug.text = bodyMovements.kneeRigthMovement().ToString();
		if (bodyMovements.kneeLeftMovement()<manager.anguloMin) {
			puede_patear_izq = true;
		}
		if (bodyMovements.kneeLeftMovement()>manager.anguloMax && puede_patear_izq==true && controlPelota.puede_levantarse==true) {
			manager.repeticionesRestantes++;
			manager.puntos++;
			administrarTurno ();
			puede_patear_izq = false;
			controlPelota.patear ();
			manager.golpePelota.Play ();
		}
	}

	public void sensarPieDerecho(){
		////print ("pie derecho");
		manager.debug.text = bodyMovements.kneeRigthMovement().ToString();
		//manager.anguloMin.ToString();
		////print("angulo: "+bodyMovements.hipRigthFlexMovement());
		if (bodyMovements.hipRigthFlexMovement () < manager.anguloMin) {
			puede_patear_der = true;
		}
		if(bodyMovements.hipRigthFlexMovement() > manager.anguloMax && puede_patear_der == true && controlPelota.puede_levantarse==true){
			manager.repeticionesRestantes++;
			manager.puntos++;
			administrarTurno ();
			puede_patear_der = false;
			controlPelota.patear ();
			manager.golpePelota.Play ();
		}
	}

	public void sensarPieIzquierdo(){
		////print ("pie izquierdo");
		manager.debug.text = bodyMovements.kneeRigthMovement().ToString();
		if (bodyMovements.hipLeftFlexMovement () < manager.anguloMin) {
			puede_patear_izq = true;
		}
		if(bodyMovements.hipLeftFlexMovement() > manager.anguloMax && puede_patear_izq == true && controlPelota.puede_levantarse==true){
			manager.repeticionesRestantes++;
			manager.puntos++;
			administrarTurno ();
			puede_patear_izq = false;
			controlPelota.patear ();
			manager.golpePelota.Play ();
		}
	}
		
	void connectWithSensor(){
		adapter = gameObject.AddComponent<KinectTwoAdapter>();
		sensor = KinectSensor.GetDefault();

		if (sensor != null)
		{

			if (!sensor.IsOpen)
			{
				//prueba.SetActive (true);
				sensor.Open();
			}
			bodyMovements = new MovementsCollection();
			bodyPointsCollection = new Dictionary<BodyParts, BodyPoint>();
			for (int i = 0; i < (int)BodyParts.ThumbRight; i++)
			{
				bodyPointsCollection.Add(((BodyParts)i), new BodyPoint((BodyParts)i));
			}
			//resetPositionsSpineBase ();
		}
	}
}
