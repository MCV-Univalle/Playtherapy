﻿using UnityEngine;
using System.Collections;

namespace MovementDetectionLibrary
{

    public class Shoot : MonoBehaviour
    {


        bool flag = true;
        float angle;
        GameAngles calc;
        bool flagSide = true;
        float  timeArrive ;
        private GameManagerAtrapalo gameM;
        private Vector3 pointFin;
		private float timeLaunch;
        private float vel;
        private bool side;
		public int type;
        private bool flagArrive;
        private Vector3 pointIni;
        private Rigidbody posNew; 


        // Use this for initialization
        void Start()
        {
            
			gameM = GameObject.Find("GameManager").GetComponent<GameManagerAtrapalo>();
			gameM.ballsAlive++;
			angle = (180*gameM.level)/6;
            calc = new GameAngles(angle, gameM.planeShootFront, gameM.planeShootLat);
            timeLaunch = gameM.launchTime;
            side = true;

            if (type == 3)
            {
                timeLaunch = timeLaunch / 2;
            }
            timeArrive = Time.time + timeLaunch;
            Vector3 pointIni = transform.position;
            flagArrive = false;
            //posNew = this.GetComponent<Rigidbody>();



        }

        // Update is called once per frame
        void Update()
        {

            if (flag)
            {               
                if (gameM.side)
                {
                    Debug.Log("lado der");
                    this.shootPosition("ShoulderRight", "HandRight", "right");
                    gameM.side = false;
                }
                else
                {
                    Debug.Log("lado izq");
                    this.shootPosition("ShoulderLeft", "HandLeft", "left");
                    gameM.side = true;
                }
                this.vel = (pointFin - gameObject.transform.position).magnitude / timeLaunch;
                this.vel = vel * Time.deltaTime;
                flag = false;
            }
            else {
                if (gameObject.transform.position != pointFin) {
                    
                    //posNew.MovePosition(pointIni+pointFin*Time.deltaTime);
                    this.transform.position = Vector3.MoveTowards(transform.position, pointFin, vel);
                }
                else
                {
                    if (!flagArrive)
                    {
                        pointFin = pointFin * 2;
                        flagArrive = true;
                    }
                    this.transform.position = Vector3.MoveTowards(transform.position, pointFin, vel);
                }
            }

                         
        }



        Vector3 calculateSpeedVector(float t, Vector3 point)
        {
            Vector3 initSpeed = new Vector3();

            float Vx = 0;
            float Vy = 0;
            float Vz = 0;
            float X0 = gameObject.transform.position.x;
            float Z0 = gameObject.transform.position.z;
            float Y0 = gameObject.transform.position.y;
            float Y1 = point.y;
            float X1 = point.x;
            float Z1 = point.z;

            Vx = (X1 - X0) / t;
            Vz = (Z1 - Z0) / t;

            Vy = (Y1-Y0)/t;


            initSpeed.x = Vx;
            initSpeed.y = Vy;
            initSpeed.z = Vz;

            return initSpeed;
        }


		public void shootPosition(string jointOneName, string jointTwoName, string side)
        {
            if (gameM.shootOpt==3)
            {
                counterPlane();
            }

            Debug.Log("plano"+gameM.plane);

            float angleRad = calc.setRamdomAngle(side, gameM.plane);
                
            Vector3 pointOne = GameObject.FindGameObjectWithTag(jointOneName).transform.position;
            Vector3 pointTwo = GameObject.FindGameObjectWithTag(jointTwoName).transform.position;


            this.pointFin = calc.getPosition(pointOne, calc.createPointTwoShoulderAF(pointOne, pointTwo), angleRad, 1.3f, gameM.plane);
            //gameObject.transform.position = pointFin;
            gameM.NewRepetition();      
			


        }

        void OnDestroy(){
			gameM.ballsAlive--;
		}

        // Function to count the balls shoot to a plane
        public void counterPlane()
        {
            if(gameM.countBallPlane != 2)
            {
                gameM.countBallPlane++;
            }else
            {
                gameM.countBallPlane = 1;
                gameM.changePlane();
            }
        }
    }
}
