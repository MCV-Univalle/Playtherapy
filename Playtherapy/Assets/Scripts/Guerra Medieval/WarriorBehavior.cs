
using UnityEngine;

namespace GuerraMedieval
{
    public class WarriorBehavior : MonoBehaviour
    {
        private Animator animator;
        private Rigidbody m_rigidbody;
        private Renderer render;

        public float minWalkTime = 5;
        public float maxWalkTime = 20;
        public float velocity = 2;

        private float walkTime;
        private float savedTime;

        // Use this for initialization
        void Start()
        {
            render = gameObject.transform.GetChild(1).GetComponent<Renderer>();
            m_rigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            walkTime = Random.Range(minWalkTime, maxWalkTime);
        }

        // Update is called once per frame
        void Update()
        {
            if(Time.time - savedTime > walkTime)
            {
                m_rigidbody.velocity = Vector3.zero;
            }
            if(Vector3.Magnitude(m_rigidbody.velocity) != 0)
            {
                animator.Play("WK_heavy_infantry_06_combat_walk", 0);
            }
            else
            {
                animator.Play("WK_heavy_infantry_05_combat_idle", 0);
            }
        }

        private void OnEnable()
        {
            savedTime = Time.time;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Airballoon")
            {
                if (!GameManagerMedieval.gmm.WithGrab)
                {
                    CannonPlayerController.cpc.Shoot();
                }

                render.material.color = new Color(1f, 0.5f, 0.5f);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Airballoon")
            {
                render.material.color = Color.white;
            }
        }
    }
}
