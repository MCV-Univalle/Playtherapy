using LeapAPI;
using System;
using UnityEngine;

namespace GuerraMedieval
{
    [System.Serializable]
    public static class Boundary
    {
        public static float xMin = -12;
        public static float xMax = 12;
    }

    public class CannonPlayerController : MonoBehaviour
    {
        public static CannonPlayerController cpc;

        public float minAngleHorizontal = 10f;
        public float maxAngleHorizontal = 90f;

        public float minAngleVertical = 5f;
        public float maxAngleVertical = 60f;



        public float horizontalSpeed = 10f;                     // Velocity of the horizontal move
        public float tilt = 5f;                                 // Max rotation of the ship
        public float rotateSpeed = 5f;                          // Velocity of the rotation



        

        //public Vector2 boundary = new Vector2(-6, 6);

        private float horizontalMove;                           // Amount of horizontal movement
        private float verticalMove;

        private bool destroyed;

        public GameObject canonStructure;
        public GameObject realCanon;
        public GameObject canon;
        public GameObject cannoballBehavior;
        public GameObject plane;

        public AudioSource canonRecoil;
        public float canonBallVelocityMagnitude = 10f;
        public Vector3 canonBallVelocity = new Vector3(0, 0, 0);
        private Vector3 canonBallPosition;
        private Vector3 relativeCanonBallPosition = new Vector3(0, 0f, 3f);
        float trayectoryTime = 0;
        float temp;


        private GameObject[] trayectoryBalls;

        // Use this for initialization
        void Start()
        {
            if(cpc == null)
            {
                cpc = this.gameObject.GetComponent<CannonPlayerController>();
            }

            trayectoryBalls = GameObject.FindGameObjectsWithTag("Airballoon");
            temp = 0.1f;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            switch (GameManagerMedieval.gmm.GetGameState())
            {
                case GameManagerMedieval.GameState.PLAYING:
                    {
                        
                        CalculateHorizontalMove();
                        CalculateVerticalMove();
                        CalculateCanonBallPosition();
                        Move();
                        CalculateBoundary();
                        CalculateVelocity();
                        if (GameManagerMedieval.gmm.WithGrab)
                        {
                            CalculateShoot();
                        }
                        CalculateTrayectory();
                    }
                    break;
                case GameManagerMedieval.GameState.GAMEOVER:
                    {

                    }
                    break;
            }
        }

        /// <summary>
        /// Calculates the cannon movement
        /// </summary>
        public void Move()
        {
            if (GameManagerMedieval.gmm.WithFlexionExtension)
            {
                transform.Rotate(0f, horizontalMove, 0f);
            }
            if(GameManagerMedieval.gmm.WithPronation)
            {
                transform.Translate(horizontalMove / 10f, 0f, 0f);
            }

            
            if (!GameManagerMedieval.gmm.WithFlexionExtension && !GameManagerMedieval.gmm.WithPronation) {
                

                if (realCanon.transform.position.x == 12)
                {
                    temp = -0.1f;


                }
                if (realCanon.transform.position.x == -12)
                {
                    temp = 0.1f;

                }
                transform.Translate(temp, 0f, 0f);


            }
            canon.transform.Rotate(verticalMove, 0f, 0f);
        }

        /// <summary>
        /// Limit the cannon rotation
        /// </summary>
        public void CalculateBoundary()
        {
            float angleY = transform.eulerAngles.y;
            if (GameManagerMedieval.gmm.WithFlexionExtension)
            {
                if (angleY > 180)
                {
                    angleY = angleY - 360;
                }
                angleY = Mathf.Clamp(angleY, -30, 30);
                if (angleY < 0)
                {
                    angleY += 360;
                }
                transform.eulerAngles = new Vector3
                    (
                    transform.eulerAngles.x,
                    angleY,
                    transform.eulerAngles.z
                    );
            }
            else
            {
                transform.position = new Vector3
                    (
                    Mathf.Clamp(transform.position.x, Boundary.xMin, Boundary.xMax),
                    transform.position.y,
                    transform.position.z
                    );
            }
            


            float angleX = canon.transform.localEulerAngles.x;
            if (angleX > 180)
            {
                angleX = angleY - 360;
            }
            angleX = Mathf.Clamp(angleX, 0, 45);
            if (angleX < 0)
            {
                angleX += 360;
            }
            canon.transform.localEulerAngles = new Vector3
                (
                angleX,
                canon.transform.localEulerAngles.y,
                canon.transform.localEulerAngles.z
                );
        }

        public void CalculateHorizontalMove()
        {
            if (GameManagerMedieval.gmm.withKeyboard)
            {
                horizontalMove = Input.GetAxis("Horizontal");
                if(horizontalMove == 0)
                {
                    if (canonRecoil && canonRecoil.isPlaying)
                        canonRecoil.Stop();
                }
                else
                {
                    if (canonRecoil && !canonRecoil.isPlaying)
                        canonRecoil.Play();
                }
            }
            else
            {

                if (GameManagerMedieval.gmm.WithFlexionExtension && GameManagerMedieval.gmm.WithPronation == false)
                { }
                else
                {
                    float angle = (float)new Movements().UlnarRadial();

                    if (Math.Abs(angle) > minAngleHorizontal)
                    {
                        angle = Math.Min(Math.Abs(angle), maxAngleHorizontal) * Math.Sign(angle);
                        horizontalMove = angle / maxAngleHorizontal;
                        if (canonRecoil && canonRecoil.isPlaying)
                            canonRecoil.Play();
                    }
                    else
                    {
                        horizontalMove = 0;
                        if (canonRecoil && !canonRecoil.isPlaying)
                            canonRecoil.Stop();
                    }
                }
            }
        }

        public void CalculateVerticalMove()
        {
            if (GameManagerMedieval.gmm.withKeyboard)
            {
                verticalMove = Input.GetAxis("Vertical");
            }
            else
            {
                float angle = (float)new Movements().FlexoExtention();

                if (Math.Abs(angle) > minAngleVertical)
                {
                    angle = Math.Min(Math.Abs(angle), maxAngleVertical) * Math.Sign(angle);
                    verticalMove = angle / maxAngleVertical;
                }
                else
                {
                    verticalMove = 0;
                }
            }
        }

        public void CalculateShoot()
        {
            if (GameManagerMedieval.gmm.withKeyboard)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                }
            }
            else
            {
                if (new Movements().Grab() > 0.5f)
                {
                    Shoot();
                }
            }
        }

        public void CalculateTrayectory()
        {
            float step;
            float gravity;
            if (GameManagerMedieval.gmm.WithFlexionExtension)
            {
                step = CalculateTime(canonBallVelocity.y, canonBallPosition) / (trayectoryBalls.Length - 1);
                gravity = Physics.gravity.y;
            }
            else
            {
                step = (plane.transform.position.z / canonBallVelocity.magnitude) / (trayectoryBalls.Length - 1);
                gravity = 0;
            }

            if (trayectoryTime < step)
            {
                trayectoryTime += 0.001f;
            }
            else
            {
                trayectoryTime = 0;
            }

            float t = trayectoryTime;            
            
            foreach (GameObject obj in trayectoryBalls)
            {
                float x = ParabolicMovement(t, canonBallVelocity.x, canonBallPosition.x, 0);
                float y = ParabolicMovement(t, canonBallVelocity.y, canonBallPosition.y, gravity);
                float z = ParabolicMovement(t, canonBallVelocity.z, canonBallPosition.z, 0);
                
                obj.transform.position = new Vector3(x, y, z);
                t += step;
            }
        }

        public float ParabolicMovement(float t, float velocity, float position, float acceleration)
        {
            return position + velocity * t + (acceleration/2) * (float)Math.Pow(t, 2);
        }

        public float CalculateTime(float velocity, Vector3 position)
        {
            float acceleration = (Physics.gravity.y / 2f);
            return (float)(-velocity - Math.Sqrt(Math.Pow(velocity, 2) - 4f * acceleration * position.y)) / (2 * acceleration);
        }

        public void CalculateVelocity()
        {
            float angleA = canon.transform.localRotation.eulerAngles.x;
            float angleB = transform.localRotation.eulerAngles.y;

            canonBallVelocity.y = canonBallVelocityMagnitude * (float)Math.Sin(angleA * (Math.PI / 180.0));
            float elevationForce = canonBallVelocityMagnitude * (float)Math.Cos(angleA * (Math.PI / 180.0));

            canonBallVelocity.x = elevationForce * (float)Math.Sin(angleB * (Math.PI / 180.0));
            canonBallVelocity.z = elevationForce * (float)Math.Cos(angleB * (Math.PI / 180.0));
        }

        public void CalculateCanonBallPosition()
        {
            float angleA = canon.transform.localRotation.eulerAngles.x;
            float angleB = transform.localRotation.eulerAngles.y;
            Vector3 tempPosition = Vector3.zero;

            float magnitude = Vector3.Magnitude(relativeCanonBallPosition);
            tempPosition.y = (magnitude * (float)Math.Sin(angleA * (Math.PI / 180.0)));
            float elevationForce = magnitude * (float)Math.Cos(angleA * (Math.PI / 180.0));

            tempPosition.x = elevationForce * (float)Math.Sin(angleB * (Math.PI / 180.0));
            tempPosition.z = elevationForce * (float)Math.Cos(angleB * (Math.PI / 180.0));

            canonBallPosition =  tempPosition + canon.transform.position;
        }

        public void Shoot()
        {
            cannoballBehavior.GetComponent<CannonballBehavior>().Fire(canonBallVelocity, canonBallPosition);
        }
    }

}
