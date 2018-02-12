using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using MovementDetectionLibrary;
using UnityEngine.UI;
// for your own scripts make sure to add the following line:
using DigitalRuby.Tween;
public class PlaneController : MonoBehaviour {

    public float speedForward = 90;
    public float speed = 20;

    public float distanceMovementX = 10;
    public float initialY;
    public bool useRestitution = false;
    Rigidbody rig;
    //connection with the kinect

    BodyFrameReader reader;
    KinectSensor sensor;
    MovementsCollection bodyMovements;
    Dictionary<BodyParts, BodyPoint> bodyPointsCollection;
    KinectTwoAdapter adapter;

	BodyPointPosition positionSpineBase_init;

    //parametros de angulos de la cadera
	double minAngle;
	double maxAngle;

	Text txt_prueba;
	GameObject prueba;
	Vector3 movement;
    // Use this for initialization
    void Start() {
        rig = GetComponent<Rigidbody>();
        initialY = transform.position.y;
		//prueba = GameObject.Find ("angle_test");
		//prueba.SetActive (false);
		//txt_prueba = prueba.GetComponent<Text>();
        connectWithSensor();
		resetData ();

    }
	public void resetData()
	{
		minAngle = HoldParametersGreatJourney.select_angle_min;
		maxAngle = HoldParametersGreatJourney.select_angle_max;
		this.gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, speedForward);

	}
	public void resetPositionsSpineBase()
	{
		if (bodyPointsCollection!=null) {
			positionSpineBase_init =bodyPointsCollection [BodyParts.SpineBase].getCurrentPosition ();
		}

	}
    void connectWithSensor()
    {
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
			resetPositionsSpineBase ();
        }
    }
    MovementAxis moveWithKinect(double hAxis,double vAxis)
    {
		
		
        // se actualiza cada parte del cuerpo 
        for (int i = 0; i < (int)BodyParts.ThumbRight; i++)
        {
            BodyPointPosition position = adapter.ReturnPosition((BodyParts)i);
            bodyPointsCollection[(BodyParts)i].setPosition(position);
        }
        bodyMovements.setBodyPointsCollection(bodyPointsCollection);

		double angleMovementX=0;
		double angleMovementY=0;
        //transform.position += transform.forward * Time.deltaTime * speedForward;
       // print("select movimiento: "+ HoldParametersGreatJourney.select_movimiento);
		switch (HoldParametersGreatJourney.select_movimiento)
		{

		case HoldParametersGreatJourney.MOVIMIENTO_MIEMBROS_INFERIORES:
			hAxis = 0;




			angleMovementX = bodyMovements.hipLeftAbMovement();

			if (angleMovementX >= minAngle) {
				//print ("movio a la izquierda" + angleMovement);
				hAxis = -(float)((angleMovementX-minAngle) / (maxAngle - minAngle));


			}
			else {
				hAxis = 0;
				angleMovementX = bodyMovements.hipRigthAbMovement();
				if (angleMovementX >= minAngle) {
					//print("movio a la derecha" + angleMovement);
					hAxis = (float)((angleMovementX-minAngle) / (maxAngle - minAngle));
					
				}
				// falta hacia abajo (probar)

			}

			if (HoldParametersGreatJourney.lados_involucrados == HoldParametersGreatJourney.LADO_TODOS || HoldParametersGreatJourney.lados_involucrados == HoldParametersGreatJourney.LADO_ABAJO) 
			{
				vAxis = 0;
				// make a squat to move down;	
				BodyPointPosition currentSpineBasePos = bodyPointsCollection [BodyParts.SpineBase].getCurrentPosition ();

				if (currentSpineBasePos.y<positionSpineBase_init.y && positionSpineBase_init.y - currentSpineBasePos.y >0.07) {
					vAxis =- Mathf.Abs ((float)(positionSpineBase_init.y - currentSpineBasePos.y));


				}

			}


			break;
		case HoldParametersGreatJourney.MOVIMIENTO_TRONCO:


			hAxis = 0;
			float sign;

			angleMovementX = bodyMovements.spineLatMovement ();
			minAngle = (double) HoldParametersGreatJourney.select_angle_min;


			if (Mathf.Abs((float)angleMovementX)>minAngle) {
				sign = -Mathf.Sign ((float)angleMovementX);
				hAxis = (float)(sign * (Mathf.Abs((float)(angleMovementX))-minAngle) / (maxAngle - minAngle));
				if (Mathf.Abs((float)hAxis)>1) {
					hAxis = sign * 1;
				}

			}




			if (HoldParametersGreatJourney.lados_involucrados == HoldParametersGreatJourney.LADO_TODOS || HoldParametersGreatJourney.lados_involucrados == HoldParametersGreatJourney.LADO_ABAJO) {
				vAxis = 0;
				minAngle = (double) HoldParametersGreatJourney.select_angle_min_frontal;
				// make a an anterior spine flexion to move down;
				angleMovementY = bodyMovements.spineIncMovement ();
				//print ("trunck angleY:"+angleMovementY);
				////if (angleMovementY> minAngle && angleMovementY-minAngle>14) {
				if (angleMovementY > minAngle + (maxAngle - minAngle) * 0.8) {
					vAxis = -(float)((angleMovementY - minAngle) / (maxAngle - minAngle));

					sign = Mathf.Sign ((float)vAxis);
					if (Mathf.Abs ((float)vAxis) > 1) {
						vAxis = sign * 1;
					}
				}
			}


		break;

//			case HoldParametersGreatJourney.MOVI:
//				angleMovementX = bodyMovements.spineIncMovement ();
//				break;
		default:
			break;
		}

            
            
		//txt_prueba.text = "angleX: " +Mathf.RoundToInt((float)angleMovementX)+"º"+ " , angleY:"+Mathf.RoundToInt((float)angleMovementY)+"º"+ ", VAxis:"+vAxis;
           
		switch (HoldParametersGreatJourney.select_movimiento) {
		case HoldParametersGreatJourney.MOVIMIENTO_MIEMBROS_INFERIORES:
			if (angleMovementX > HoldParametersGreatJourney.best_angle_left) {
				HoldParametersGreatJourney.best_angle_left = angleMovementX;
			}
			break;

		}

        
        return new MovementAxis(hAxis,vAxis);
    }
    // Update is called once per frame
    void FixedUpdate () {
		Quaternion newRotation;

		bool can_move = true;
		float vAxis = Input.GetAxis ("Vertical");
		float hAxis = Input.GetAxis ("Horizontal");
 
        // de estar conectado el kinect vera el moviento y retornara lo que ha movido
        // de no ser asi pasara los valores como estan
		//comentar cuando se prueba sin kinect
		if (sensor!=null) {
			if (sensor.IsOpen!=null && sensor.IsOpen) {
				MovementAxis movement_axis = moveWithKinect (hAxis, vAxis);
				hAxis = (float)movement_axis.xAxis;
				vAxis = (float)movement_axis.yAxis;
			}
		}


		//movement = new Vector3 (hAxis, vAxis, 0)*speed;
		movement = new Vector3 (hAxis, vAxis, 0)*speed*Time.deltaTime;
		if (rig.transform.position.x + movement.x > distanceMovementX || rig.transform.position.x + movement.x < -distanceMovementX) {
			hAxis = 0;
			movement = new Vector3 (hAxis, vAxis, 0)*speed;

		}
		if (rig.transform.position.y + movement.y > initialY || rig.transform.position.y + movement.y < 24) 
		{		

			vAxis = 0;
			movement = new Vector3 (hAxis, vAxis, 0)*speed;

		}

		//if (can_move) {
		rig.MovePosition (transform.position + movement);
		//}

		//rig.AddForce ( movement);
		//rig.velo


        //esto es para controlar ña rotacion apenas se mueva el avion en cualquiera de las dos direcciones (X,y)
        //if (hAxis == 0)
		if (hAxis == 0 || vAxis == 0)
		{
            transform.Rotate(-vAxis * 0.5f, 0, -hAxis * 4);

        }

        if (useRestitution) {
			if (hAxis == 0 && vAxis == 0) {

                Vector3 startPos = transform.position;
                Vector3 endPos = new Vector3(0, initialY, startPos.z);
                transform.position=Vector3.Lerp(startPos, endPos, 0.05f);
			}
		}
        
        newRotation = Quaternion.AngleAxis(0, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, .05f);

        


		  
	}

    class MovementAxis
    {
        public double xAxis;
        public double yAxis;
        
        public MovementAxis(double x,double y)
        {
            xAxis = x;
            yAxis = y;
        }
        
    }
}
