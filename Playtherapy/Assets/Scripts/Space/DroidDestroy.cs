using UnityEngine;

namespace GameSpace
{
    public class DroidDestroy : MonoBehaviour
    {
        public GameObject explosionParticle;
        private Rigidbody m_rigidbody;

        // Use this for initialization
        void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Ball")
            {
                if (explosionParticle != null)
                {
                    Instantiate(explosionParticle, transform.position, Quaternion.identity);
                }
                ResetObject();
            }
        }

        public void ResetObject()
        {
            m_rigidbody.velocity = Vector3.zero;
            m_rigidbody.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            gameObject.SetActive(false);
        }
    }
}