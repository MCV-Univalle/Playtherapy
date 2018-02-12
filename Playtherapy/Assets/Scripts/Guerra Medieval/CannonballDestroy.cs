using UnityEngine;

namespace GuerraMedieval
{
    public class CannonballDestroy : MonoBehaviour
    {

        public GameObject explosionParticle;
        public GameObject point;

        private Rigidbody m_rigidbody;


        // Use this for initialization
        void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                if (explosionParticle != null)
                {
                    Instantiate(explosionParticle, transform.position, Quaternion.identity);
                    Instantiate(point, transform.position + new Vector3(-1f, 2f, 0f), Quaternion.identity);
                }
                ResetObject();
                GameManagerMedieval.gmm.UpdateScore(1);
            }
            else if (other.gameObject.tag == "Terrain")
            {
                if (explosionParticle != null)
                {
                    Instantiate(explosionParticle, transform.position, Quaternion.identity);
                }
                ResetObject();
            }else if(other.gameObject.tag == "EndWall")
            {
                ResetObject();
            }
        }

        public void ResetObject()
        {
            m_rigidbody.velocity = new Vector3(0f, 0f, 0f);
            m_rigidbody.angularVelocity = new Vector3(0f, 0f, 0f);
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            gameObject.SetActive(false);
        }
    }
}