using UnityEngine;

namespace GameSpace
{
    public class DroidBehavior : MonoBehaviour
    {

        public float horizontalSpeed = 10f;                     // Velocity of the horizontal move
        public float tilt = 5f;                                 // Max rotation of the droid
        public float rotateSpeed = 5f;                          // Velocity of the rotation
        public float timeBetweenMove = 2f;                      //
        public float timeBetweenFire = 2f;                      //
        private float savedTime;
        private float movementTime;
        private float randomMove;

        public Boundary boundary;                               // Boundaries of the ship movement

        private Rigidbody m_rigidbody;                          // Rigidbody of the Ship

        private bool destroyed;


        // Use this for initialization
        void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            ResetMovement();

            destroyed = false;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (GameManagerSpace.gms.IsPlaying())
            {
                if (Time.time - savedTime >= timeBetweenMove)
                {
                    HorizontalMove(randomMove);
                    if (Time.time - savedTime >= timeBetweenMove + movementTime)
                    {
                        ResetMovement();
                    }
                }
                CalculateRotation();
                CalculateBoundary();
            }
            if (GameManagerSpace.gms.IsPlaying() && destroyed)
            {
                destroyed = false;
            }
            if (GameManagerSpace.gms.IsGameOver() && !destroyed)
            {
                GetComponent<AsteroidDestroy>().ResetObject();
                destroyed = true;
            }
        }

        /// <summary>
        /// Calculates the horizontal move of the ship
        /// </summary>
        public void HorizontalMove(float moveHorizontal)
        {
            //moveHorizontal = Input.GetAxis("Horizontal");
            m_rigidbody.velocity = new Vector3(moveHorizontal * horizontalSpeed, 0.0f, m_rigidbody.velocity.z);
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

        private void OnEnable()
        {
            savedTime = Time.time;
        }

        public void ResetMovement()
        {
            savedTime = Time.time;
            movementTime = Random.Range(0f, 1f);
            randomMove = Random.Range(-1f, 1f);
            //if (randomMove == 0)
            //{
            //    randomMove = Random.Range(-1, 2);
            //}else if(randomMove == -1)
            //{
            //    randomMove = 1;
            //}
            //else
            //{
            //    randomMove = -1;
            //}

            HorizontalMove(0);
        }
    }

}