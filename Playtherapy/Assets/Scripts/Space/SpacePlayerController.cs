using UnityEngine;
using LeapAPI;
using System;

namespace GameSpace
{
    [System.Serializable]
    public class Boundary
    {
        public float xMin = -6;
        public float xMax = 6;
    }
    /// <summary>
    /// Script used for controll the movements of the ship
    /// </summary>
    public class SpacePlayerController : MonoBehaviour
    {
        public float minAngle = 10f;
        public float maxAngle = 90f;

        public float horizontalSpeed = 10f;                     // Velocity of the horizontal move
        public float tilt = 5f;                                 // Max rotation of the ship
        public float rotateSpeed = 5f;                          // Velocity of the rotation

        public Boundary boundary;                               // Boundaries of the ship movement

        //public bool withKeyboard = true;                            // If the game is controlled by keyboard


        private Rigidbody m_rigidbody;                          // Rigidbody of the Ship
        private float horizontalMove;                           // Amount of horizontal movement

        private bool destroyed;


        void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            destroyed = false;
        }

        void FixedUpdate()
        {
            if (GameManagerSpace.gms.IsPlaying())
            {
                CalculateHorizontalMove();
                HorizontalMove();
                CalculateRotation();
                CalculateBoundary();
                CalculateShoot();
            }

            if (GameManagerSpace.gms.IsPlaying() && destroyed)
            {
                destroyed = false;
            }

            if (GameManagerSpace.gms.IsGameOver() && !destroyed)
            {
                ResetObject();
                destroyed = true;
            }
        }

        /// <summary>
        /// Calculates the horizontal move of the ship
        /// </summary>
        public void HorizontalMove()
        {
            

            m_rigidbody.velocity = new Vector3(horizontalMove * horizontalSpeed, 0.0f, m_rigidbody.velocity.z);
        }

        /// <summary>
        /// Calculates the effect of the ship rotation
        /// </summary>
        public void CalculateRotation()
        {
            transform.rotation = Quaternion.Slerp(m_rigidbody.rotation,
                Quaternion.Euler(m_rigidbody.rotation.x, m_rigidbody.rotation.y, m_rigidbody.velocity.x * -tilt),
                rotateSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Limist the border of the ship movement
        /// </summary>
        public void CalculateBoundary()
        {
            m_rigidbody.position = new Vector3
                (
                Mathf.Clamp(m_rigidbody.position.x, boundary.xMin, boundary.xMax),
                m_rigidbody.position.y,
                m_rigidbody.position.z
                );
        }

        public void ResetObject()
        {
            m_rigidbody.velocity = new Vector3(0f, 0f, 0f);
            m_rigidbody.angularVelocity = new Vector3(0f, 0f, 0f);
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }

        public void CalculateHorizontalMove()
        {
            if (GameManagerSpace.gms.withKeyboard)
            {
                horizontalMove = Input.GetAxis("Horizontal");
            }
            else
            {
                float angle = (float)new Movements().FlexoExtention();

                if (Math.Abs(angle) > minAngle)
                {
                    angle = Math.Min(Math.Abs(angle), maxAngle) * Math.Sign(angle);
                    horizontalMove = angle / maxAngle;
                }
                else
                {
                    horizontalMove = 0;
                }
                    
            }
        }

        public void CalculateShoot()
        {
            if (GameManagerSpace.gms.withKeyboard)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    BulletBehavior.bbh.Fire();
                }
            }
            else
            {
                if (new Movements().Grab() > 0.5f)
                {
                    BulletBehavior.bbh.Fire();
                }
            }
            
        }
    }
}
